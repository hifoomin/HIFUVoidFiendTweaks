using HIFUVoidFiendTweaks.Skills;
using HVFT.Skills;
using RoR2.Skills;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HIFUVoidFiendTweaks.Misc
{
    public static class CorruptedDescriptions
    {
        public static void Hooks()
        {
            On.EntityStates.VoidSurvivor.CorruptMode.CorruptModeBase.OnEnter += CorruptModeBase_OnEnter1;
        }

        private static void CorruptModeBase_OnEnter1(On.EntityStates.VoidSurvivor.CorruptMode.CorruptModeBase.orig_OnEnter orig, EntityStates.VoidSurvivor.CorruptMode.CorruptModeBase self)
        {
            var hg = self.characterBody.GetComponent<HopooGames>();
            if (self is EntityStates.VoidSurvivor.CorruptMode.CorruptMode)
            {
                hg.PerformSwap();
            }
            if (self is EntityStates.VoidSurvivor.CorruptMode.UncorruptedMode)
            {
                hg.PerformSwap();
            }
            orig(self);
        }
    }

    public class HopooGames : MonoBehaviour
    {
        public SkillLocator sl => GetComponent<SkillLocator>();

        private string d(float f)
        {
            return (f * 100f).ToString() + "%";
        }

        public string uncorruptedM1Description => "Fire a <style=cIsUtility>slowing</style> long-range beam for <style=cIsDamage>" + d(Drown.Damage) + " damage</style>. Every fouth shot fires a large devastating beam for an additional <style=cIsDamage>" + d(Drown.FourthDamage) + " damage</style>.";

        public string uncorruptedM2Description = "Fire a plasma bolt for <style=cIsDamage>600% damage</style>. Fully charge it for an explosive plasma ball instead, dealing <style=cIsDamage>1100% damage</style>.";

        public string uncorruptedSpecialDescription => "Crush <style=cIsVoid>" + Suppress.CorruptionLoss + "% Corruption</style> to heal yourself for <style=cIsHealing>" + d(Suppress.Healing) + " maximum health</style>." +
                                           (Suppress.MaxCharges > 1 ? " <style=cIsUtility>Can hold up to " + Suppress.MaxCharges + " orbs</style>." : "");

        public string corruptedM1Description => "Fire a short-range beam for <style=cIsDamage>" + d(CorruptedDrown.Damage) + " damage</style>.";

        public string corruptedM2Description => "Fire an arcing black hole for <style=cIsDamage>" + d(CorruptedFlood.Damage) + " damage</style>.";

        public string corruptedSpecialDescription => "Crush <style=cIsHealing>" + d(CorruptedSuppress.SelfDamage) + " health</style> to gain <style=cIsVoid>" + CorruptedSuppress.CorruptionGain + "% Corruption<style>.";

        public SkillDef uncorruptedM1SkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/FireHandBeam.asset").WaitForCompletion();
        public SkillDef uncorruptedM2SkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/ChargeMegaBlaster.asset").WaitForCompletion();
        public VoidSurvivorSkillDef uncorruptedSpecialSkillDef => Addressables.LoadAssetAsync<VoidSurvivorSkillDef>("RoR2/DLC1/VoidSurvivor/CrushCorruption.asset").WaitForCompletion();

        public SkillDef corruptedM1SkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/FireCorruptBeam.asset").WaitForCompletion();
        public SkillDef corruptedM2SkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/FireCorruptDisk.asset").WaitForCompletion();
        public SkillDef corruptedSpecialSkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/CrushHealth.asset").WaitForCompletion();

        public void PerformSwap()
        {
            var p = sl.primary;
            if (p)
            {
                if (p.skillDef = uncorruptedM1SkillDef) LanguageAPI.Add(p.skillDescriptionToken, uncorruptedM1Description);
                if (p.skillDef = corruptedM1SkillDef) LanguageAPI.Add(p.skillDescriptionToken, corruptedM1Description);
            }

            var se = sl.secondary;
            if (se)
            {
                if (se.skillDef = uncorruptedM2SkillDef) LanguageAPI.Add(se.skillDescriptionToken, uncorruptedM2Description);
                if (se.skillDef = corruptedM2SkillDef) LanguageAPI.Add(se.skillDescriptionToken, corruptedM2Description);
            }

            var sp = sl.special;
            if (sp)
            {
                if (sp.skillDef = uncorruptedSpecialSkillDef) LanguageAPI.Add(sp.skillDescriptionToken, uncorruptedSpecialDescription);
                if (sp.skillDef = corruptedSpecialSkillDef) LanguageAPI.Add(sp.skillDescriptionToken, corruptedSpecialDescription);
            }
        }
    }
}