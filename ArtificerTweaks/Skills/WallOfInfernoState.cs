using EntityStates;
using HAT;
using HIFUArtificerTweaks.Projectiles;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace HIFUArtificerTweaks.Skills
{
    public class WallOfInfernoState : BaseSkillState
    {
        public float duration = 2f;
        public float timer;
        public float interval = 0.25f;
        public float speedMultiplier = Main.flamewallSpeed.Value;
        private Vector3 idealDirection;
        public GameObject wallPrefab = WallOfInfernoProjectile.prefab;
        public SkillLocator sl;

        public override void OnEnter()
        {
            base.OnEnter();
            sl = characterBody.GetComponent<SkillLocator>();
            if (isAuthority)
            {
                if (inputBank)
                {
                    idealDirection = inputBank.aimDirection;
                    idealDirection.y = 0f;
                }
                if (sl)
                {
                    if (sl.special.skillNameToken == "MAGE_SPECIAL_FIRE_NAME")
                    {
                        sl.special.skillDef.canceledFromSprinting = false;
                        sl.special.skillDef.cancelSprintingOnActivation = false;
                    }
                }
                UpdateDirection();
            }

            if (modelLocator) modelLocator.normalizeToFloor = true;
            if (characterDirection) characterDirection.forward = idealDirection;

            characterBody.SetAimTimer(duration + 1f);
            PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", duration);
            AkSoundEngine.PostEvent(2855368448, gameObject); // Play_item_use_fireballDash_start
        }

        private void UpdateDirection()
        {
            if (inputBank)
            {
                Vector2 vector = Util.Vector3XZToVector2XY(inputBank.moveVector);
                if (vector != Vector2.zero)
                {
                    vector.Normalize();
                    idealDirection = new Vector3(vector.x, 0f, vector.y).normalized;
                }
            }
        }

        private Vector3 GetIdealVelocity()
        {
            return characterDirection.forward * characterBody.moveSpeed * speedMultiplier;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;
            if (isAuthority)
            {
                if (characterBody)
                {
                    characterBody.isSprinting = true;
                }
                if (characterDirection)
                {
                    characterDirection.moveVector = idealDirection;
                    if (characterMotor && !characterMotor.disableAirControlUntilCollision)
                    {
                        characterMotor.rootMotion += GetIdealVelocity() * Time.fixedDeltaTime;
                    }
                }
                UpdateDirection();

                if (fixedAge >= duration)
                {
                    outer.SetNextStateToMain();
                }
                else if (timer >= interval)
                {
                    var vector = Vector3.up;

                    FireProjectileInfo info = new()
                    {
                        projectilePrefab = wallPrefab,
                        position = characterBody.corePosition,
                        rotation = Util.QuaternionSafeLookRotation(vector),
                        owner = gameObject,
                        damage = damageStat,
                        force = 0f,
                        damageColorIndex = DamageColorIndex.Default,
                        crit = Util.CheckRoll(critStat, characterBody.master)
                    };

                    ProjectileManager.instance.FireProjectile(info);

                    Util.PlaySound(EntityStates.Mage.Weapon.Flamethrower.endAttackSoundString, gameObject);

                    timer = 0f;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!outer.destroying && characterBody)
            {
                PlayAnimation("Gesture, Additive", "FireWall");
                characterBody.isSprinting = false;
            }
            if (characterMotor && !characterMotor.disableAirControlUntilCollision)
            {
                characterMotor.velocity += GetIdealVelocity();
            }
            if (modelLocator)
            {
                modelLocator.normalizeToFloor = false;
            }
            if (isAuthority && sl)
            {
                if (sl.special.skillNameToken == "MAGE_SPECIAL_FIRE_NAME")
                {
                    sl.special.skillDef.canceledFromSprinting = true;
                    sl.special.skillDef.cancelSprintingOnActivation = true;
                }
            }
            AkSoundEngine.PostEvent(561188827, gameObject); // Play_item_use_fireballDash_explode
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}