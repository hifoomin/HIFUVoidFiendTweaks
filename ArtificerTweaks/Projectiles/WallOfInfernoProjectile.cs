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
            prefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageIcewallWalkerProjectile.prefab").WaitForCompletion(), "WallOfInferno");
            prefab.name = "WallOfInferno";
            var what = prefab.GetComponent<ProjectileMageFirewallWalkerController>();
            var wall2 = what.firePillarPrefab;
            wall2.name = "WallOfInfernoPillar";
            var det = wall2.transform.GetChild(0);
            Object.Destroy(det.GetComponent<MineProximityDetonator>());
            det.GetComponent<BoxCollider>().size = new Vector3(10f, 2f, 5f);
            var pie = wall2.GetComponent<ProjectileImpactExplosion>();
            pie.lifetimeExpiredSound = null;
            pie.destroyOnEnemy = false;
            pie.lifetime = 7f;
            pie.impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion();
            var pd = wall2.GetComponent<ProjectileDamage>();
            pd.damageType = DamageType.IgniteOnHit;
            var ghost = wall2.GetComponent<ProjectileController>();
            var newGhost = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageFlamethrowerEffect.prefab").WaitForCompletion(), "WallOfInfernoPillarGhost");
            newGhost.transform.localScale = new Vector3(1f, 1f, 0.5f);
            newGhost.transform.eulerAngles = new Vector3(90, 0, 0);
            newGhost.GetComponent<DestroyOnTimer>().duration = 7f;
            newGhost.AddComponent<ProjectileGhostController>();
            var lr = newGhost.transform.GetChild(0).GetComponent<LineRenderer>();
            lr.widthMultiplier = 10f;
            //lr.endColor = new Color32(255, 255, 255, 80);
            ghost.ghostPrefab = newGhost;
        }
    }
}