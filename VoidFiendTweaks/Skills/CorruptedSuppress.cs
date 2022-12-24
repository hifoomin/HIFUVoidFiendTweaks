using RoR2.Projectile;
using RoR2.Skills;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace HVFT.Skills
{
    public class CorruptedSuppress : TweakBase
    {
        public static float SelfDamage;
        public static float CorruptionGain;
        public static int MaxCharges;
        public static float duration;
        public static float endlag;

        public override string Name => "Special :: Corrupted Suppress";

        public override string SkillToken => "special_uprade";

        public override string DescText => "<style=cKeywordName>【Corruption Upgrade】</style><style=cSub>Transform to crush " + d(SelfDamage) + " health, gaining " + CorruptionGain + "% Corruption instead. Can hold up to " + MaxCharges + " orbs.</style>";

        public override bool isVoid => true;

        public override void Init()
        {
            SelfDamage = ConfigOption(0.19f, "Self Damage", "Decimal. Vanilla is 0.25");
            MaxCharges = ConfigOption(2, "Max Charges", "Vanilla is 2");
            CorruptionGain = ConfigOption(25f, "Corruption Gain", "Vanilla is 25");
            duration = ConfigOption(0.8f, "Animation Duration", "Vanilla is 1");
            endlag = ConfigOption(0.3f, "Endlag", "Vanilla is 1");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.Weapon.CrushBase.OnEnter += CrushBase_OnEnter;
            On.EntityStates.VoidSurvivor.Weapon.ChargeCrushBase.OnEnter += ChargeCrushBase_OnEnter;
            Changes();
        }

        private void ChargeCrushBase_OnEnter(On.EntityStates.VoidSurvivor.Weapon.ChargeCrushBase.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.ChargeCrushBase self)
        {
            if (self is EntityStates.VoidSurvivor.Weapon.ChargeCrushBase)
            {
                self.baseDuration = duration;
            }
            orig(self);
        }

        private void CrushBase_OnEnter(On.EntityStates.VoidSurvivor.Weapon.CrushBase.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.CrushBase self)
        {
            if (self is EntityStates.VoidSurvivor.Weapon.CrushHealth)
            {
                self.selfHealFraction = -SelfDamage;
                self.corruptionChange = CorruptionGain;
                self.baseDuration = endlag;
            }
            orig(self);
        }

        private void Changes()
        {
            var ch = Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/CrushHealth.asset").WaitForCompletion();
            ch.baseMaxStock = MaxCharges;
        }
    }
}