using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HIFUVoidFiendTweaks.Skills
{
    internal class SwingMeleeBase : BasicMeleeAttack
    {
        public override bool allowExitFire => characterBody && !characterBody.isSprinting;

        public override void OnEnter()
        {
            base.OnEnter();
            characterDirection.forward = GetAimRay().direction;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
        }

        public override void PlayAnimation()
        {
            base.PlayAnimation(animationLayerName, animationStateName, animationPlaybackRateParameter, duration);
        }

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
            base.characterBody.AddSpreadBloom(bloom);
        }

        public override void BeginMeleeAttackEffect()
        {
            AddRecoil(-0.1f * recoilAmplitude, 0.1f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
            base.BeginMeleeAttackEffect();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        [SerializeField]
        public float recoilAmplitude;

        [SerializeField]
        public float bloom;

        [SerializeField]
        public string animationLayerName;

        [SerializeField]
        public string animationStateName;

        [SerializeField]
        public string animationPlaybackRateParameter;
    }
}