using HVFT;
using R2API;
using RoR2.Skills;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace HIFUVoidFiendTweaks.Skilldefs
{
    public static class CorruptedPermeateSD
    {
        public static SteppedSkillDef sd;
        public static string nameToken = "HVFT_VOIDSURVIVOR_PRIMARY_MELEE_ALT_NAME";

        public static void Create()
        {
            sd = ScriptableObject.CreateInstance<SteppedSkillDef>();
            // sd.activationState = new(typeof(NewrotoxinState));
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

            sd.stepCount = 3;
            sd.stepGraceDuration = 0.5f;

            sd.icon = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Croco/CrocoSpit.asset").WaitForCompletion().icon;
            sd.skillNameToken = nameToken;
            sd.skillDescriptionToken = "HVFT_VOIDSURVIVOR_PRIMARY_MELEE_ALT_DESCRIPTION";
            sd.keywordTokens = new string[] { "KEYWORD_AGILE" };

            LanguageAPI.Add("HVFT_VOIDSURVIVOR_PRIMARY_MELEE_ALT_NAME", "『P?ermea??te】");
            LanguageAPI.Add("HVFT_VOIDSURVIVOR_PRIMARY_MELEE_ALT_DESCRIPTION", "<style=cIsUtility>Agile</style>.");
            ContentAddition.AddSkillDef(sd);
        }
    }
}