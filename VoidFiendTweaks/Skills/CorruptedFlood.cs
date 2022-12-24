using HVFT;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HIFUVoidFiendTweaks.Skills
{
    internal class CorruptedFlood : TweakBase
    {
        public static float Damage;
        public static float Knockback;
        public static float AoE;
        public static float ProjectileSpeed;
        public static float Lifetime;
        public static float Cooldown;
        public override string Name => "Secondary :: Corrupted Flood";

        public override string SkillToken => "secondary_uprade";

        public override string DescText => "<style=cKeywordName>【Corruption Upgrade】</style><style=cSub>Transform into a " + d(Damage) + " damage black hole that pulls enemies in an enormous radius.</style>";

        public override bool isVoid => true;

        public override void Init()
        {
            Knockback = ConfigOption(-2500f, "Knockback", "Vanilla is 3000");
            AoE = ConfigOption(25f, "Radius", "Vanilla is 10");
            Damage = ConfigOption(4f, "Damage", "Decimal. Vanilla is 11");
            ProjectileSpeed = ConfigOption(40f, "Projectile Speed", "Vanilla is 70");
            Lifetime = ConfigOption(4f, "Projectile Duration", "Vanilla is 99");
            Cooldown = ConfigOption(7f, "Cooldown", "Vanilla is 4");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.OnEnter += FireCorruptDisks_OnEnter;
            Changes();
        }

        private void FireCorruptDisks_OnEnter(On.EntityStates.VoidSurvivor.Weapon.FireCorruptDisks.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireCorruptDisks self)
        {
            self.damageCoefficient = Damage;
            orig(self);
        }

        private void Changes()
        {
            var cfp = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectileCorrupted.prefab").WaitForCompletion();

            var ps = cfp.GetComponent<ProjectileSimple>();
            ps.desiredForwardSpeed = ProjectileSpeed;
            ps.lifetime = Lifetime;
            ps.lifetimeExpiredEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterExplosionCorrupted.prefab").WaitForCompletion();

            var e = cfp.GetComponent<ProjectileImpactExplosion>();
            e.blastRadius = AoE;

            var rf = cfp.AddComponent<RadialForce>();
            rf.radius = AoE;
            rf.damping = 0.5f;
            rf.forceMagnitude = Knockback;
            rf.forceCoefficientAtEdge = 0.5f;

            var ghost = cfp.GetComponent<ProjectileController>().ghostPrefab;
            ghost.GetComponent<ProjectileGhostController>().inheritScaleFromProjectile = true;

            var cd = Addressables.LoadAssetAsync<SkillDef>("RoR2/DLC1/VoidSurvivor/FireCorruptDisk.asset").WaitForCompletion();
            cd.baseRechargeInterval = Cooldown;
        }
    }
}