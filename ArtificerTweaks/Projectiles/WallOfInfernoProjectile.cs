using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HIFUArtificerTweaks.Projectiles
{
    public static class WallOfInfernoProjectile
    {
        public static GameObject prefab;

        public static void Create()
        {
            prefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElementalRings/FireTornado.prefab").WaitForCompletion(), "WallOfInfernoPillar");
            prefab.transform.eulerAngles = new Vector3(0, 0, 90);
            /*
            ProjectileImpactExplosion impact = prefab.AddComponent<ProjectileImpactExplosion>();
            impact.lifetimeExpiredSound = null;
            impact.destroyOnEnemy = false;
            impact.lifetime = 7f;
            impact.impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion();
            impact.enabled = false;
            impact.blastRadius = 5f;
            */

            var hitbox = prefab.transform.GetChild(0);
            hitbox.transform.localScale = new Vector3(5.5f, 5.5f, 20f);
            hitbox.transform.localPosition = new Vector3(0, 0f, 8f);

            var rb = prefab.GetComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.useGravity = true;
            rb.freezeRotation = true;

            var cf = prefab.AddComponent<ConstantForce>();
            cf.force = new Vector3(0f, -2500f, 0f);

            var psoi = prefab.AddComponent<ProjectileStickOnImpact>();
            psoi.ignoreCharacters = true;
            psoi.ignoreWorld = false;
            psoi.alignNormals = true;

            var ps = prefab.GetComponent<ProjectileSimple>();
            ps.lifetime = 7f;

            ProjectileDamage pd = prefab.GetComponent<ProjectileDamage>();
            pd.damageType = DamageType.IgniteOnHit;

            ProjectileOverlapAttack overlap = prefab.GetComponent<ProjectileOverlapAttack>();
            overlap.damageCoefficient = 1f;
            overlap.resetInterval = 1f;
            overlap.overlapProcCoefficient = 0.25f;

            ProjectileController projectileController = prefab.GetComponent<ProjectileController>();
            GameObject ghostPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageFlamethrowerEffect.prefab").WaitForCompletion(), "WallOfInfernoPillarGhost");
            ghostPrefab.transform.localScale = new Vector3(1f, 1f, 0.5f);
            ghostPrefab.transform.eulerAngles = new Vector3(0, 0, 90);
            ghostPrefab.GetComponent<DestroyOnTimer>().duration = 7f;
            ghostPrefab.AddComponent<ProjectileGhostController>();
            ghostPrefab.GetComponent<ScaleParticleSystemDuration>().initialDuration = 7f;

            var bone1 = ghostPrefab.transform.GetChild(0);
            var matrix = ghostPrefab.transform.GetChild(1);
            var ico = ghostPrefab.transform.GetChild(2);
            var bb = ghostPrefab.transform.GetChild(3);
            bb.gameObject.SetActive(false);

            var matrixMain = matrix.GetComponent<ParticleSystem>().main;
            matrixMain.duration = 6.4f;
            matrixMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
            matrix.localScale = new Vector3(3f, 3f, 3f);

            var icoMain = ico.GetComponent<ParticleSystem>().main;
            icoMain.duration = 7f;
            icoMain.scalingMode = ParticleSystemScalingMode.Hierarchy;
            ico.localScale = new Vector3(3f, 3f, 3f);

            bone1.transform.localScale = new Vector3(1f, 1f, 2f);
            var lr = bone1.GetComponent<LineRenderer>();
            lr.widthMultiplier = 10f;

            var curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.15f, 1), new Keyframe(0.85f, 1), new Keyframe(1f, 0));

            var asa = bone1.GetComponent<AnimateShaderAlpha>();
            asa.alphaCurve = curve;
            asa.timeMax = 7f;

            projectileController.ghostPrefab = ghostPrefab;
        }
    }
}