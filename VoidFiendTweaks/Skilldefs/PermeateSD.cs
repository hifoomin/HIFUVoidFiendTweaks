using R2API;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using BepInEx;
using Facepunch.Steamworks;
using RoR2;
using System.Threading.Tasks;


namespace HIFUVoidFiendTweaks.Skilldefs
{
    public static class PermeateSD
    {
        public static SteppedSkillDef sd;
        public static string nameToken = "HVFT_VOIDSURVIVOR_PRIMARY_MELEE_NAME";
        public static float DAMAGE_COEFF = 2.2f;
        public static float DAMAGE_COEFF_CORRUPT = 2.9f;
        public static float NULLIFY_PENALTY_ATK = 0.025f;
        public static float NULLIFY_PENALTY_SPE = 0.1f;
        public static float LUNGE_DISTANCE = 24f;
        public static float CORRUPT_SPEED_BONUS = 1.8f;
        public static bool ENABLED = true;

        public static void HandleConfig() {
            string n1 = "Primary :: Permeate";
            string n2 = "Primary :: Corrupted Permeate";

            Bind<bool>(n1, "Enabled", "Should this skill be enabled?", ref ENABLED);
            Bind<float>(n1, "Damage Coefficient", "The damage coefficient of Permeate.", ref DAMAGE_COEFF);
            Bind<float>(n2, "Damage Coefficient", "The damage coefficient of Corrupt Permeate.", ref DAMAGE_COEFF_CORRUPT);
            Bind<float>(n1, "Leap Distance", "The distance that the second claw of Permeate should lunge.", ref LUNGE_DISTANCE);
            Bind<float>(n2, "Nullify Attack Penalty", "The amount that Corrupt Nullification should reduce attack speed by per stack.", ref NULLIFY_PENALTY_ATK);
            Bind<float>(n2, "Nullify Speed Penalty", "The amount that Corrupt Nullification should reduce speed by per stack.", ref NULLIFY_PENALTY_SPE);
            Bind<float>(n2, "Movement Bonus", "The movement speed increase that Corrupt Permeate should give.", ref CORRUPT_SPEED_BONUS);

            static void Bind<T>(string sec, string name, string desc, ref T val) {
                var config = Main.HVFTConfig.Bind<T>(sec, name, val, desc);
                ConfigManager.HandleConfig<T>(config, Main.HVFTBackupConfig, name);
                val = config.Value;
            }
        }

        public static void Create()
        {
            if (!ENABLED) return;

            sd = ScriptableObject.CreateInstance<SteppedSkillDef>();
            sd.activationState = new(typeof(Skills.Permeate));
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
            sd.cancelSprintingOnActivation = false;
            sd.forceSprintDuringState = true;
            sd.canceledFromSprinting = false;
            sd.isCombatSkill = true;
            sd.mustKeyPress = false;

            sd.stepCount = 2;
            sd.stepGraceDuration = 0.5f;

            sd.icon = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Croco/CrocoSpit.asset").WaitForCompletion().icon;
            sd.skillNameToken = nameToken;
            sd.skillDescriptionToken = "HVFT_VOIDSURVIVOR_PRIMARY_MELEE_DESCRIPTION";
            sd.keywordTokens = new string[] { "KEYWORD_AGILE", "KEYWORD_CORRUPTPERMEATE" };

            LanguageAPI.Add("HVFT_VOIDSURVIVOR_PRIMARY_MELEE_NAME", "『P?ermea??te】");
            LanguageAPI.Add("HVFT_VOIDSURVIVOR_PRIMARY_MELEE_DESCRIPTION", $"<style=cIsUtility>Agile</style>. Claw forward for <style=cIsDamage>{d(DAMAGE_COEFF)}% damage</style>. Every other strike <style=cIsUtility>lunges</style>.");
            ContentAddition.AddSkillDef(sd);

            LanguageAPI.Add("KEYWORD_CORRUPTPERMEATE", $"<style=cKeywordName>【Corruption Upgrade】</style><style=cSub>Tear up targets for " + d(DAMAGE_COEFF_CORRUPT) + "% damage rapidly, slowing their movement and attack speeds.</style>");

            GameObject viend = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();
            SkillLocator loc = viend.GetComponent<SkillLocator>();
            SkillFamily family = loc.primary._skillFamily;
            Array.Resize(ref family.variants, family.variants.Length + 1);
            family.variants[family.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = sd,
                viewableNode = new ViewablesCatalog.Node(sd.skillNameToken, false, null),
            };

            viend.GetComponentInChildren<HitBox>(true).transform.localScale *= 2.5f;

            CorruptedPermeateSD.Create();

            On.EntityStates.VoidSurvivor.CorruptMode.CorruptMode.OnEnter += (orig, self) =>
            {
                bool hasPrimaryAlt = false;
                bool exists = self.skillLocator && self.skillLocator.secondary && self.skillLocator.primary && self.skillLocator.utility && self.skillLocator.special;
                if (self.isAuthority && exists)
                {
                    hasPrimaryAlt = self.skillLocator.primary.skillDef == sd;
                }

                orig(self);

                if (self.isAuthority && exists)
                {
                    if (hasPrimaryAlt)
                    {
                        self.skillLocator.primary.UnsetSkillOverride(self.gameObject, self.specialOverrideSkillDef, GenericSkill.SkillOverridePriority.Upgrade);
                        self.skillLocator.primary.SetSkillOverride(self.gameObject, CorruptedPermeateSD.sd, GenericSkill.SkillOverridePriority.Upgrade);
                    }
                }
            };

            On.EntityStates.VoidSurvivor.CorruptMode.CorruptMode.OnExit += (orig, self) =>
            {
                bool hasPrimaryAlt = false;
                
                if (self.isAuthority)
                {
                    hasPrimaryAlt = self.skillLocator.primary.skillDef == CorruptedPermeateSD.sd;
                }

                orig(self);

                if (self.isAuthority)
                {
                    if (hasPrimaryAlt)
                    {
                        self.skillLocator.primary.UnsetSkillOverride(self.gameObject, CorruptedPermeateSD.sd, GenericSkill.SkillOverridePriority.Upgrade);
                    }
                }
            };
        }

        public static float d(float f) {
            return Mathf.FloorToInt(f * 100f);
        }
    }
}