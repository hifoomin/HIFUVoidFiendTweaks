using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HAT.Skills
{
    public class PlasmaBolt : TweakBase
    {
        public static float AoE;

        public override string Name => ": Primary :: Plasma Bolt";

        public override string SkillToken => "primary_lightning";

        public override string DescText => "Fire a bolt for <style=cIsDamage>280% damage</style> that <style=cIsDamage>explodes</style> in a medium area. Hold up to 4.</style>";

        public override void Init()
        {
            AoE = ConfigOption(7f, "Area of Effect", "Vanilla is 6");
            base.Init();
        }

        public override void Hooks()
        {
            Changes();
        }

        public static void Changes()
        {
            var pbolt = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningboltBasic.prefab").WaitForCompletion();
            pbolt.GetComponent<ProjectileImpactExplosion>().blastRadius = AoE;
        }
    }
}