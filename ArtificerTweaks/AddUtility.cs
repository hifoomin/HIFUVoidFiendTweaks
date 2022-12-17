using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using RoR2;
using HIFUArtificerTweaks.Skilldefs;
using RoR2.Skills;

namespace HIFUArtificerTweaks
{
    public static class AddUtility
    {
        public static void Create()
        {
            var arti = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageBody.prefab").WaitForCompletion();
            var sl = arti.GetComponent<SkillLocator>();

            var skillFamily = sl.utility.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = WallOfInfernoSD.sd,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(WallOfInfernoSD.nameToken, false, null)
            };
        }
    }
}