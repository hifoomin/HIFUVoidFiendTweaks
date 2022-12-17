using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HAT.Skills
{
    public class CastNanoSpear : TweakBase
    {
        public static float minDamage;
        public static float maxDamage;

        public override string Name => ": Secondary :: Cast Nano-Spear";

        public override string SkillToken => "secondary_ice";

        public override string DescText => "<style=cIsUtility>Freezing</style>. Charge up a <style=cIsDamage>piercing</style> nano-spear that deals <style=cIsDamage>" + d(minDamage) + "-" + d(maxDamage) + "</style> damage.";

        public override void Init()
        {
            minDamage = ConfigOption(4f, "Minimum Damage", "Decimal. Vanilla is 4");
            maxDamage = ConfigOption(16f, "Maximum Damage", "Decimal. Vanilla is 12");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.Mage.Weapon.BaseThrowBombState.OnEnter += BaseThrowBombState_OnEnter;
        }

        private void BaseThrowBombState_OnEnter(On.EntityStates.Mage.Weapon.BaseThrowBombState.orig_OnEnter orig, EntityStates.Mage.Weapon.BaseThrowBombState self)
        {
            if (self is EntityStates.Mage.Weapon.ThrowIcebomb)
            {
                self.minDamageCoefficient = minDamage;
                self.maxDamageCoefficient = maxDamage;
            }
            orig(self);
        }
    }
}