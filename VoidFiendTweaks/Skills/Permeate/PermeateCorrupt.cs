using System;
using BepInEx;
using EntityStates;
using HIFUVoidFiendTweaks.Skilldefs;
using HIFUVoidFiendTweaks.Utils;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace HIFUVoidFiendTweaks.Skills {
    public class PermeateCorrupt : CoolerBasicMeleeAttack, SteppedSkillDef.IStepSetter
    {
        public override float BaseDuration => 0.3f;

        public override float DamageCoefficient => PermeateSD.DAMAGE_COEFF_CORRUPT;

        public override string HitboxName => "Melee";

        public override GameObject HitEffectPrefab => Utils.Assets.GameObject.VoidSurvivorBeamImpactCorrupt;

        public override float ProcCoefficient => 1f;

        public override float HitPauseDuration => 0.05f;

        public override GameObject SwingEffectPrefab => null;

        public override string MuzzleString => "MuzzleMelee";
        public override string MechanimHitboxParameter => "Melee.hitBoxActive";
        private int step;

        public override void OnEnter()
        {
            base.OnEnter();

            base.swingEffectPrefab = step switch {
                2 => CorruptedPermeateSD.Slash2,
                _ => CorruptedPermeateSD.Slash1
            };

            base.forceVector = Vector3.zero;
            base.shorthopVelocityFromHit = 3f;

            base.characterMotor.walkSpeedPenaltyCoefficient = PermeateSD.CORRUPT_SPEED_BONUS;
        }

        public override void OnExit()
        {
            base.OnExit();

            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
        }

        public override void PlayAnimation()
        {
            AkSoundEngine.PostEvent(Events.Play_boss_falseson_skill1_swing, base.gameObject);

            switch (step) {
                case 2:
                    PlayAnimation("LeftArm, Override", "SwingMelee2", "Melee.playbackRate", duration);
                    break;
                default:
                    PlayAnimation("LeftArm, Override", "SwingMelee1", "Melee.playbackRate", duration);
                    break;
            }
        }

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            overlapAttack.AddModdedDamageType(CorruptedPermeateSD.CorruptNullifyType);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.StartAimMode(0.2f);
        }

        public void SetStep(int i)
        {
            step = i + 1;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(step);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            step = reader.ReadInt32();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}