using HIFUVoidFiendTweaks.Skills;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HIFUVoidFiendTweaks.VFX
{
    public static class BigTracer
    {
        public static GameObject tracer;

        public static void Create()
        {
            tracer = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamTracer.prefab").WaitForCompletion(), "FourthBeamTracer", false);

            var ec = tracer.GetComponent<EffectComponent>();
            ec.applyScale = true;

            var lr = tracer.GetComponent<LineRenderer>();
            lr.widthMultiplier = Drown.FourthRadius * 0.5f;

            var tt = tracer.transform;
            tt.localScale = new Vector3(Drown.FourthRadius, Drown.FourthRadius, Drown.FourthRadius);

            var startTransform = tt.GetChild(0);
            startTransform.localScale = new Vector3(Drown.FourthRadius, Drown.FourthRadius, Drown.FourthRadius);
            var tracerHead = tt.GetChild(1);
            tracerHead.localScale = new Vector3(Drown.FourthRadius, Drown.FourthRadius, Drown.FourthRadius);

            var flash1 = startTransform.GetChild(1).GetComponent<ParticleSystem>();
            flash1.gameObject.SetActive(true);
            var flash1Main = flash1.main;
            flash1Main.startSize = 0.15f * Drown.FourthRadius;

            var pointLight = tracerHead.GetChild(2).GetComponent<Light>();
            pointLight.range = 2f * Drown.FourthRadius;
            pointLight.intensity = 10f;

            ContentAddition.AddEffect(tracer);
        }
    }
}