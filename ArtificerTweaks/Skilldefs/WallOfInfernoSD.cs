using R2API;
using UnityEngine;
using RoR2.Skills;
using HIFUArtificerTweaks.Skills;
using UnityEngine.AddressableAssets;
using RoR2;
using HAT;

namespace HIFUArtificerTweaks.Skilldefs
{
    public static class WallOfInfernoSD
    {
        public static SkillDef sd;
        public static string nameToken = "HAT_MAGE_UTILITY_FIRE_NAME";

        public static void Create()
        {
            var arti = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageBody.prefab").WaitForCompletion();
            var esm = arti.AddComponent<EntityStateMachine>();
            esm.customName = "Wall";
            esm.initialStateType = new(typeof(EntityStates.Idle));
            esm.mainStateType = new(typeof(EntityStates.Idle));

            sd = ScriptableObject.CreateInstance<SkillDef>();
            sd.activationState = new(typeof(WallOfInfernoState));
            sd.activationStateMachineName = "Wall";
            sd.interruptPriority = EntityStates.InterruptPriority.PrioritySkill;

            sd.baseRechargeInterval = 12f;
            sd.baseMaxStock = 1;
            sd.rechargeStock = 1;
            sd.requiredStock = 1;
            sd.stockToConsume = 1;

            sd.resetCooldownTimerOnUse = false;
            sd.fullRestockOnAssign = true;
            sd.dontAllowPastMaxStocks = false;
            sd.beginSkillCooldownOnSkillEnd = true;
            sd.cancelSprintingOnActivation = false;
            sd.canceledFromSprinting = false;
            sd.isCombatSkill = true;
            sd.mustKeyPress = false;

            sd.icon = Main.hifuartificertweaks.LoadAsset<Sprite>("Assets/Flamewall.png");
            sd.skillNameToken = nameToken;
            sd.skillDescriptionToken = "HAT_MAGE_UTILITY_FIRE_DESCRIPTION";
            sd.keywordTokens = new string[] { "KEYWORD_IGNITE", "KEYWORD_AGILE" };

            LanguageAPI.Add("HAT_MAGE_UTILITY_FIRE_NAME", "Flamewall");
            LanguageAPI.Add("HAT_MAGE_UTILITY_FIRE_DESCRIPTION", "<style=cIsUtility>Agile</style>. <style=cIsDamage>Ignite</style>. Rush forward, summoning pillars of fire in your wake that deal <style=cIsDamage>" + (Main.flamewallDamage.Value * 100) + "% damage per second</style>.");
            ContentAddition.AddSkillDef(sd);
        }
    }
}