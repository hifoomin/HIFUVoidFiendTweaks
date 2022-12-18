using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HAT.Skills
{
    public class PlasmaBolt : TweakBase
    {
        public static float AoE;
        public static float Damage;

        public override string Name => ": Primary :: Plasma Bolt";

        public override string SkillToken => "primary_lightning";

        public override string DescText => "Fire a bolt for <style=cIsDamage>" + d(Damage) + " damage</style> that <style=cIsDamage>explodes</style> in a medium area. Hold up to 4.</style>";

        public override void Init()
        {
            AoE = ConfigOption(6f, "Area of Effect", "Vanilla is 6");
            Damage = ConfigOption(3f, "Damage", "Decimal. Vanilla is 2.8");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.Mage.Weapon.FireFireBolt.OnEnter += FireFireBolt_OnEnter;
            Changes();
        }

        private void FireFireBolt_OnEnter(On.EntityStates.Mage.Weapon.FireFireBolt.orig_OnEnter orig, EntityStates.Mage.Weapon.FireFireBolt self)
        {
            if (self is EntityStates.Mage.Weapon.FireLightningBolt)
            {
                self.damageCoefficient = Damage;
            }
            orig(self);
        }

        public static void Changes()
        {
            var pbolt = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningboltBasic.prefab").WaitForCompletion();
            pbolt.GetComponent<ProjectileImpactExplosion>().blastRadius = AoE;
        }
    }
}