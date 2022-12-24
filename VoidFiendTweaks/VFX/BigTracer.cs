using HVFT.Skills;
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
            tracer = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamTracer.prefab").WaitForCompletion(), "DevastatorBeamTracer", false);
            var ec = tracer.GetComponent<EffectComponent>();
            ec.applyScale = true;

            tracer.transform.localScale = new Vector3(Drown.FourthRadius, Drown.FourthRadius, Drown.FourthRadius);

            ContentAddition.AddEffect(tracer);
        }
    }
}