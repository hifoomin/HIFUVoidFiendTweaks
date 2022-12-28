using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using RoR2.Skills;
using R2API;
using R2API.ContentManagement;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using HIFUVoidFiendTweaks.VFX;
using HIFUVoidFiendTweaks.Misc;

namespace HVFT
{
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(PrefabAPI.PluginGUID)]
    [BepInDependency(R2APIContentManager.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "HIFU";
        public const string PluginName = "HIFUVoidFiendTweaks";
        public const string PluginVersion = "1.0.0";

        public static ConfigFile HVFTConfig;
        public static ManualLogSource HVFTLogger;

        /* TODO:
        Add decaying armor instead of instant -60 (probably 15 or 20/s)
        Fix VFX size being way too small (impossible)
        Add Melee alt M1
            Lunge forward, dealing x%, every second hit charges? Uncorrupted
            Charge forward, dealing x%, every third performs a downwards slam punch? Corrupted, maybe a healing mechanic cause of -60 armor?
        */

        public void Awake()
        {
            HVFTLogger = Logger;
            HVFTConfig = Config;

            var vf = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();
            var esm = vf.AddComponent<EntityStateMachine>();
            esm.customName = "Flood";
            esm.initialStateType = new(typeof(EntityStates.Idle));
            esm.mainStateType = new(typeof(EntityStates.Idle));

            var uncorrFlood = Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/ChargeMegaBlaster.asset").WaitForCompletion();
            uncorrFlood.activationStateMachineName = "Flood";

            IEnumerable<Type> enumerable = from type in Assembly.GetExecutingAssembly().GetTypes()
                                           where !type.IsAbstract && type.IsSubclassOf(typeof(TweakBase))
                                           select type;

            HVFTLogger.LogInfo("==+----------------==TWEAKS==----------------+==");

            foreach (Type type in enumerable)
            {
                TweakBase based = (TweakBase)Activator.CreateInstance(type);
                if (ValidateTweak(based))
                {
                    based.Init();
                }
            }

            IEnumerable<Type> enumerable2 = from type in Assembly.GetExecutingAssembly().GetTypes()
                                            where !type.IsAbstract && type.IsSubclassOf(typeof(MiscBase))
                                            select type;

            HVFTLogger.LogInfo("==+----------------==MISC==----------------+==");

            foreach (Type type in enumerable2)
            {
                MiscBase based = (MiscBase)Activator.CreateInstance(type);
                if (ValidateMisc(based))
                {
                    based.Init();
                }
            }

            BigTracer.Create();
            CorruptedDescriptions.ChangeDescriptions();
        }

        public bool ValidateTweak(TweakBase tb)
        {
            if (tb.isEnabled)
            {
                bool enabledfr = Config.Bind(tb.Name, "Enable?", true, "Vanilla is false").Value;
                if (enabledfr)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ValidateMisc(MiscBase mb)
        {
            if (mb.isEnabled)
            {
                bool enabledfr = Config.Bind(mb.Name, "Enable?", true, "Vanilla is false").Value;
                if (enabledfr)
                {
                    return true;
                }
            }
            return false;
        }

        private void WITHINDESTRUCTIONMYFUCKINGBELOVED()
        {
        }
    }
}