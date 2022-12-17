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

            var pie = wall2.GetComponent<ProjectileImpactExplosion>();
            pie.lifetimeExpiredSound = null;
            pie.destroyOnEnemy = false;
            pie.lifetime = 7f;
            pie.impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion();
            pie.enabled = false;

            var arrowRain = wall2.AddComponent<ProjectileDotZone>();
            arrowRain.damageCoefficient = 1f / 3f;
            arrowRain.attackerFiltering = AttackerFiltering.NeverHitSelf;
            arrowRain.overlapProcCoefficient = 0.25f;
            arrowRain.fireFrequency = 20;
            arrowRain.resetFrequency = 3;
            arrowRain.lifetime = 7f;
            arrowRain.impactEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/OmniExplosionVFXQuick.prefab").WaitForCompletion();

            var hbg = wall2.AddComponent<HitBoxGroup>();

            var hb = new GameObject("hitbox");
            hb.transform.localScale = new Vector3(10f, 2f, 5f);
            hb.AddComponent<HitBox>();
            hb.transform.parent = wall2.transform;

            hbg.hitBoxes = new HitBox[] { hb.GetComponent<HitBox>() };

            var pd = wall2.GetComponent<ProjectileDamage>();
            pd.damageType = DamageType.IgniteOnHit;

            var ghost = wall2.GetComponent<ProjectileController>();
            var newGhost = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageFlamethrowerEffect.prefab").WaitForCompletion(), "WallOfInfernoPillarGhost");
            newGhost.transform.localScale = new Vector3(1f, 1f, 0.5f);
            newGhost.transform.eulerAngles = new Vector3(90, 0, 0);
            newGhost.GetComponent<DestroyOnTimer>().duration = 7f;
            newGhost.AddComponent<ProjectileGhostController>();

            var bone1 = newGhost.transform.GetChild(0);
            var matrix = newGhost.transform.GetChild(1);
            var ico = newGhost.transform.GetChild(2);
            var bb = newGhost.transform.GetChild(3);
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
            ghost.ghostPrefab = newGhost;
        }
    }
}