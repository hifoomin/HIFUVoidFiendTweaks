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

            ProjectileImpactExplosion impact = prefab.AddComponent<ProjectileImpactExplosion>();
            impact.lifetimeExpiredSound = null;
            impact.destroyOnEnemy = false;
            impact.lifetime = 7f;
            impact.impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion();
            impact.enabled = true;
            impact.blastRadius = 5f;

            ProjectileDamage pd = prefab.GetComponent<ProjectileDamage>();
            pd.damageType = DamageType.IgniteOnHit;

            ProjectileOverlapAttack overlap = prefab.GetComponent<ProjectileOverlapAttack>();
            overlap.damageCoefficient = 1f;
            overlap.resetInterval = 1f;
            overlap.overlapProcCoefficient = 0.5f;

            ProjectileController projectileController = prefab.GetComponent<ProjectileController>();
            GameObject ghostPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageFlamethrowerEffect.prefab").WaitForCompletion(), "WallOfInfernoPillarGhost");
            ghostPrefab.transform.localScale = new Vector3(1f, 1f, 0.5f);
            ghostPrefab.transform.eulerAngles = new Vector3(0, 90, 0);
            ghostPrefab.GetComponent<DestroyOnTimer>().duration = 7f;
            ghostPrefab.AddComponent<ProjectileGhostController>();

            var bone1 = ghostPrefab.transform.GetChild(0);
            var matrix = ghostPrefab.transform.GetChild(1);
            var ico = ghostPrefab.transform.GetChild(2);
            var bb = ghostPrefab.transform.GetChild(3);
            bone1.GetComponent<AnimateShaderAlpha>().timeMax = 7f;
            var stupidShit1 = matrix.GetComponent<ParticleSystem>().main;
            stupidShit1.duration = 6.4f;
            var stupidShit2 = ico.GetComponent<ParticleSystem>().main;
            stupidShit2.duration = 7f;
            var stupidShit3 = bb.GetComponent<ParticleSystem>().main;
            stupidShit3.duration = 6.7f;

            // these don't work heheheha only the bone1 works, so all the other particles disappear in ~3s and it looks like shit

            // lr.widthMultiplier = 10f;
            // lr.endColor = new Color32(255, 255, 255, 80);
            projectileController.ghostPrefab = ghostPrefab;
        }
    }
}