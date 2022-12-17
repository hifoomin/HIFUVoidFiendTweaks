using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using static RoR2.TeleporterInteraction;
using UnityEngine;
using RoR2.Skills;
using HIFUArtificerTweaks.Skills;
using UnityEngine.AddressableAssets;
using RoR2;

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
            sd.baseMaxStock = 1;
            sd.baseRechargeInterval = 12f;
            sd.beginSkillCooldownOnSkillEnd = true;
            sd.canceledFromSprinting = false;
            sd.cancelSprintingOnActivation = false;
            sd.fullRestockOnAssign = true;
            sd.interruptPriority = EntityStates.InterruptPriority.PrioritySkill;
            sd.isCombatSkill = true;
            sd.mustKeyPress = false;
            sd.rechargeStock = 1;
            sd.icon = null;
            sd.skillNameToken = nameToken;
            sd.skillDescriptionToken = "HAT_MAGE_UTILITY_FIRE_DESCRIPTION";
            sd.stockToConsume = 1;
            sd.keywordTokens = new string[] { "KEYWORD_IGNITE" };

            LanguageAPI.Add("HAT_MAGE_UTILITY_FIRE_NAME", "Wall of Inferno");
            LanguageAPI.Add("HAT_MAGE_UTILITY_FIRE_DESCRIPTION", "<style=cIsDamage>Ignite</style>. Rush forward, summoning pillars of fire behind you that deal <style=cIsDamage>100% damage per second</style> and explode.");
            /* maybe aside you instead? wanna make this skill really aggressive tho
            collider is fucked up idk
            movement is janky
            visual looks kinda bad rn, not sure how to properly scale it
            vector math is fucked i hate and it doesn't work properly
            */
            ContentAddition.AddSkillDef(sd);
        }
    }
}