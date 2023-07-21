using HIFUVoidFiendTweaks.Skills;
using RoR2.Skills;
using R2API;
using UnityEngine.AddressableAssets;

namespace HIFUVoidFiendTweaks.Misc
{
    public static class CorruptedDescriptions
    {
        private static string d(float f)
        {
            return (f * 100f).ToString() + "%";
        }

        public static string corruptedM1Description => "Fire a short-range beam for <style=cIsDamage>" + d(CorruptedDrown.Damage) + " damage</style>.";

        public static string corruptedM2Description => "Fire an arcing black hole for <style=cIsDamage>" + d(CorruptedFlood.Damage) + " damage</style>.";

        public static string corruptedSpecialDescription => "Crush <style=cIsHealing>" + d(CorruptedSuppress.SelfDamage) + " health</style> to gain <style=cIsVoid>" + CorruptedSuppress.CorruptionGain + "% Corruption</style>.";

        public static SkillDef corruptedM1SkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/FireCorruptBeam.asset").WaitForCompletion();
        public static SkillDef corruptedM2SkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/FireCorruptDisk.asset").WaitForCompletion();
        public static SkillDef corruptedSpecialSkillDef => Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/CrushHealth.asset").WaitForCompletion();

        public static void ChangeDescriptions()
        {
            LanguageAPI.Add("VOIDSURVIVOR_PRIMARY_ALT_DESCRIPTION", corruptedM1Description);
            LanguageAPI.Add("VOIDSURVIVOR_SECONDARY_ALT_DESCRIPTION", corruptedM2Description);
            LanguageAPI.Add("VOIDSURVIVOR_SPECIAL_ALT_DESCRIPTION", corruptedSpecialDescription);
            LanguageAPI.Add("VOIDSURVIVOR_SPECIAL_NAME", "【Supp??ress』");
        }
    }
}