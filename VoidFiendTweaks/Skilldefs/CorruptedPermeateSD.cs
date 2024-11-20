using R2API;
using RoR2.Skills;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;
using BepInEx;
using System;

namespace HIFUVoidFiendTweaks.Skilldefs
{
    public static class CorruptedPermeateSD
    {
        public static SteppedSkillDef sd;
        public static string nameToken = "HVFT_VOIDSURVIVOR_PRIMARY_MELEE_ALT_NAME";
        public static GameObject Slash1;
        public static GameObject Slash2;
        public static BuffDef CorruptNullification;
        public static GameObject CorruptNullificationEffect;
        public static DamageAPI.ModdedDamageType CorruptNullifyType = DamageAPI.ReserveDamageType();

        public static void Create()
        {
            sd = ScriptableObject.CreateInstance<SteppedSkillDef>();
            sd.activationState = new(typeof(Skills.PermeateCorrupt));
            sd.activationStateMachineName = "Weapon";
            sd.interruptPriority = EntityStates.InterruptPriority.Skill;

            sd.baseRechargeInterval = 0f;
            sd.baseMaxStock = 1;
            sd.rechargeStock = 1;
            sd.requiredStock = 1;
            sd.stockToConsume = 1;

            sd.resetCooldownTimerOnUse = false;
            sd.fullRestockOnAssign = true;
            sd.dontAllowPastMaxStocks = false;
            sd.beginSkillCooldownOnSkillEnd = true;
            sd.cancelSprintingOnActivation = true;
            sd.forceSprintDuringState = false;
            sd.canceledFromSprinting = false;
            sd.isCombatSkill = true;
            sd.mustKeyPress = false;

            sd.stepCount = 2;
            sd.stepGraceDuration = 0.2f;

            sd.icon = CorruptPermeateIcon.GetSprite();
            sd.skillNameToken = nameToken;
            sd.skillDescriptionToken = "HVFT_VOIDSURVIVOR_PRIMARY_MELEE_ALT_DESCRIPTION";

            ContentAddition.AddSkillDef(sd);

            Material old = Utils.Assets.Material.matVoidSurvivorMeleeSlash;
            Material newMat = Utils.Assets.Material.matVoidSurvivorBlasterCoreCorrupted;
            Slash1 = CloneWithMaterialSwap(Utils.Assets.GameObject.VoidSurvivorMeleeSlash1, "RedMeleeSlash1", old, newMat);
            Slash2 = CloneWithMaterialSwap(Utils.Assets.GameObject.VoidSurvivorMeleeSlash2, "RedMeleeSlash2", old, newMat);

            CorruptNullification = ScriptableObject.CreateInstance<BuffDef>();
            CorruptNullification.iconSprite = Utils.Assets.BuffDef.bdNullified.iconSprite;
            CorruptNullification.buffColor = Color.red;
            CorruptNullification.isDebuff = true;
            CorruptNullification.canStack = true;

            ContentAddition.AddBuffDef(CorruptNullification);

            On.RoR2.CharacterBody.RecalculateStats += HandleBuffPenalty;

            CorruptNullificationEffect = CloneWithMaterialSwap(
                Utils.Assets.GameObject.NullifyStack3Effect,
                "Corrupted Nullify",
                Utils.Assets.Material.matNullifierStarTrail,
                Utils.Assets.Material.matVoidSurvivorCrabCannonTrail
            );

            On.RoR2.HealthComponent.TakeDamageProcess += OnTakeDamage;

            HVFTVisualEffects.Apply();
        }

        private static void OnTakeDamage(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);

            if (damageInfo.HasModdedDamageType(CorruptNullifyType)) {
                self.body.AddTimedBuff(CorruptNullification, 2f);
            }
        }

        private static void HandleBuffPenalty(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            int c = self.GetBuffCount(CorruptNullification);

            if (c > 0) {
                self.attackSpeed *= Mathf.Clamp(1f - (c * PermeateSD.NULLIFY_PENALTY_ATK), 0.4f, 1f);
                self.moveSpeed *= Mathf.Clamp(1f - (c * PermeateSD.NULLIFY_PENALTY_SPE), 0.2f, 1f);
            }
        }

        public static GameObject CloneWithMaterialSwap(GameObject target, string name, Material oldMat, Material newMat) {
            GameObject obj = PrefabAPI.InstantiateClone(target, name);
            
            foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>()) {
                if (renderer.sharedMaterial == oldMat) {
                    renderer.sharedMaterial = newMat;
                }
            }

            if (obj.GetComponent<EffectComponent>()) {
                ContentAddition.AddEffect(obj);
            }

            return obj;
        }

        public class HVFTVisualEffects : MonoBehaviour {
            public TemporaryVisualEffect CorruptNullification;
            public void UpdateTemporaryEffects(CharacterBody body) {
                body.UpdateSingleTemporaryVisualEffect(ref CorruptNullification, CorruptNullificationEffect, body.radius, body.HasBuff(CorruptedPermeateSD.CorruptNullification));
            }
            internal static void Apply() {
                On.RoR2.CharacterBody.UpdateAllTemporaryVisualEffects += OnUpdateEffects;
            }

            private static void OnUpdateEffects(On.RoR2.CharacterBody.orig_UpdateAllTemporaryVisualEffects orig, CharacterBody self)
            {
                orig(self);

                HVFTVisualEffects hvt = self.GetComponent<HVFTVisualEffects>();
                if (!hvt) {
                    hvt = self.gameObject.AddComponent<HVFTVisualEffects>();
                }

                hvt.UpdateTemporaryEffects(self);
            }
        }
    }
}