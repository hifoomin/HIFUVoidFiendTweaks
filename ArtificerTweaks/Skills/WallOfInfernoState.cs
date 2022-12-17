using EntityStates;
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
        public float interval = 1f;

        public GameObject wallPrefab = WallOfInfernoProjectile.prefab;
        private Vector3 dashVector;
        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(duration + 1f);
            PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", duration);
            dashVector = GetVector();
            AkSoundEngine.PostEvent(2855368448, gameObject); // Play_item_use_fireballDash_start
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;
            if (isAuthority)
            {
                if (characterMotor && characterDirection)
                {
                    characterMotor.velocity = Vector3.zero;
                    characterMotor.rootMotion += dashVector * (moveSpeedStat * 2f * Time.fixedDeltaTime);
                }
                if (fixedAge >= duration)
                {
                    outer.SetNextStateToMain();
                }
                else if (timer >= interval)
                {
                    //Vector3 vector = Vector3.Cross(Vector3.up, characterBody.transform.forward);
                    var vector = characterBody.transform.forward;
                    bool flag = Util.CheckRoll(critStat, characterBody.master);
                    //ProjectileManager.instance.FireProjectile(wallPrefab, characterBody.corePosition, Util.QuaternionSafeLookRotation(vector), gameObject, damageStat * 1f, 0f, flag, DamageColorIndex.Default, null, -1f);
                    
                    FireProjectileInfo info = new();
                    info.projectilePrefab = wallPrefab;
                    info.position = characterBody.corePosition;
                    info.rotation = Util.QuaternionSafeLookRotation(-vector);
                    info.owner = gameObject;
                    info.damage = damageStat;
                    info.force = 0f;
                    info.damageColorIndex = DamageColorIndex.Default;

                    ProjectileManager.instance.FireProjectile(info);
                    
                    Util.PlaySound(EntityStates.Mage.Weapon.Flamethrower.endAttackSoundString, gameObject);

                    timer = 0f;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayAnimation("Gesture, Additive", "FireWall");
            AkSoundEngine.PostEvent(561188827, gameObject); // Play_item_use_fireballDash_explode
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        protected Vector3 GetVector()
        {
            Vector3 direction = inputBank.aimDirection;
            direction.y = 0;
            return direction.normalized;
        }
    }
}