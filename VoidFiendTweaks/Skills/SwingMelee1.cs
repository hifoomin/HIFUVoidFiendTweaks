using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HIFUVoidFiendTweaks.Skills
{
    internal class SwingMelee1 : SwingMeleeBase
    {
        public override void OnEnter()
        {
            base.OnEnter();

            // todo: figure out what thing keb was talking about https://discord.com/channels/562704639141740588/562704639569428506/1057617081543184445

            beginStateSoundString = "Play_bandit2_m2_slash";
            baseDuration = 0.5f;
            animationStateName = "SwingMelee1";
            animationPlaybackRateParameter = "Melee.playbackRate";
            animationLayerName = "LeftArm, Override";
            pushAwayForce = 0f;
            procCoefficient = 1f;
            mecanimHitboxActiveParameter = "Melee.hitBoxActive";
            ignoreAttackSpeed = false;
            hitPauseDuration = 0.1f;
            hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamImpact.prefab").WaitForCompletion();
            hitBoxGroupName = "Melee";
            forwardVelocityCurve = new AnimationCurve
            {
                preWrapMode = WrapMode.Clamp,
                postWrapMode = WrapMode.Clamp,
                keys = new Keyframe[]
                {
                   new Keyframe
                    {
                        time = 0.0f,
                        value = 0.3f,
                        inTangent = -1.0f,
                        outTangent = -1.0f,
                        inWeight = 0.0f,
                        outWeight = 0.06f,
                        weightedMode = WeightedMode.None,
                    },
                    new Keyframe
                    {
                        time = 1.0f,
                        value = 0.0f,
                        inTangent = -0.23f,
                        outTangent = -0.23f,
                        inWeight = 0.1f,
                        outWeight = 0.0f,
                        weightedMode = WeightedMode.None,
                    }
                }
            };
            forceVector = Vector3.zero;
            forceForwardVelocity = false;
            damageCoefficient = 3f;
            bloom = 0f;
            swingEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMeleeSlash1.prefab").WaitForCompletion();
            swingEffectMuzzleString = "MuzzleMelee";
            shorthopVelocityFromHit = 3f;
            scaleHitPauseDurationAndVelocityWithAttackSpeed = true;
            recoilAmplitude = 1f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}