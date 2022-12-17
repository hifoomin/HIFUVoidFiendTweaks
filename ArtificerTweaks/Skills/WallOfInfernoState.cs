﻿using EntityStates;
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

        public GameObject wallPrefab = WallOfInfernoProjectile.prefab;

        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(duration + 1f);
            PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", duration);
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
                    characterMotor.rootMotion += inputBank.aimDirection * (moveSpeedStat * 2f * Time.fixedDeltaTime);
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
                    ProjectileManager.instance.FireProjectile(wallPrefab, characterBody.corePosition, Util.QuaternionSafeLookRotation(-vector), gameObject, damageStat * 1f, 0f, flag, DamageColorIndex.Default, null, -1f);
                    Util.PlaySound(EntityStates.Mage.Weapon.Flamethrower.endAttackSoundString, gameObject);
                    timer = 0f;
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayAnimation("Gesture, Additive", "FireWall");
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}