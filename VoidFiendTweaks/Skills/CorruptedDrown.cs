using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HIFUVoidFiendTweaks.Skills
{
    public class CorruptedDrown : TweakBase
    {
        public static float Damage;
        public static float PPS;

        public override string Name => "Primary :: Corrupted Drown";

        public override string SkillToken => "primary_uprade";

        public override string DescText => "<style=cKeywordName>【Corruption Upgrade】</style><style=cSub>Transform into a " + d(Damage) + " damage short-range beam.</style>";

        public override bool isVoid => true;

        public override void Init()
        {
            Damage = ConfigOption(12f, "Damage", "Decimal. Vanilla is 20");
            PPS = ConfigOption(4f, "Proc Coefficient Per Second", "Vanilla is 5");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptHandBeam.OnEnter += FireCorruptHandBeam_OnEnter;
        }

        private void FireCorruptHandBeam_OnEnter(On.EntityStates.VoidSurvivor.Weapon.FireCorruptHandBeam.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireCorruptHandBeam self)
        {
            self.damageCoefficientPerSecond = Damage;
            self.procCoefficientPerSecond = PPS;
            orig(self);
        }
    }
}