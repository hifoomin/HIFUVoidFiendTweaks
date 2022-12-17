using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HAT.Skills
{
    public class Flamethrower : TweakBase
    {
        public static float Damage;
        public static float BurnChance;
        public static float Range;

        public override string Name => ": Special : Flamethrower";

        public override string SkillToken => "special_fire";

        public override string DescText => "<style=cIsDamage>Ignite</style>. Burn all enemies in front of you for <style=cIsDamage>" + d(Damage + (Damage / (1 / (BurnChance / 100)))) + " damage</style>.";

        public override void Init()
        {
            Damage = ConfigOption(20f, "Damage", "Decimal. Vanilla is 20");
            BurnChance = ConfigOption(100f, "Ignite Chance", "Decimal. Vanilla is 50");
            Range = ConfigOption(20f, "Range", "Vanilla is 20");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.Mage.Weapon.Flamethrower.OnEnter += Flamethrower_OnEnter;
        }

        private void Flamethrower_OnEnter(On.EntityStates.Mage.Weapon.Flamethrower.orig_OnEnter orig, EntityStates.Mage.Weapon.Flamethrower self)
        {
            EntityStates.Mage.Weapon.Flamethrower.ignitePercentChance = BurnChance;
            EntityStates.Mage.Weapon.Flamethrower.totalDamageCoefficient = Damage;
            self.maxDistance = Range;
            orig(self);
        }
    }
}