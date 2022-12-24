using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HVFT.Skills
{
    public class Suppress : TweakBase
    {
        public static float Healing;
        public static int MaxCharges;
        public static float CorruptionLoss;

        public override string Name => "Special : Suppress";

        public override string SkillToken => "special";

        public override string DescText => "Crush <style=cIsVoid>" + CorruptionLoss + "% Corruption</style> to heal yourself for <style=cIsHealing>" + d(Healing) + " maximum health</style>." +
                                           (MaxCharges > 1 ? " <style=cIsUtility>Can hold up to " + MaxCharges + " orbs</style>." : "");

        public override bool isVoid => false;

        public override void Init()
        {
            Healing = ConfigOption(0.175f, "Healing", "Decimal. Vanilla is 0.25");
            MaxCharges = ConfigOption(2, "Max Charges", "Vanilla is 1");
            CorruptionLoss = ConfigOption(15f, "Corruption Loss", "Vanilla is 25");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.Weapon.CrushBase.OnEnter += CrushBase_OnEnter;
            Changes();
        }

        private void CrushBase_OnEnter(On.EntityStates.VoidSurvivor.Weapon.CrushBase.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.CrushBase self)
        {
            if (self is EntityStates.VoidSurvivor.Weapon.CrushCorruption)
            {
                self.selfHealFraction = Healing;
                self.corruptionChange = -CorruptionLoss;
            }
            orig(self);
        }

        private void Changes()
        {
            var ch = Addressables.LoadAssetAsync<VoidSurvivorSkillDef>("RoR2/DLC1/VoidSurvivor/CrushCorruption.asset").WaitForCompletion();
            if (MaxCharges > 1)
            {
                ch.baseMaxStock = MaxCharges;
                ch.rechargeStock = 0;
                ch.minimumCorruption = CorruptionLoss;
            }
        }
    }
}