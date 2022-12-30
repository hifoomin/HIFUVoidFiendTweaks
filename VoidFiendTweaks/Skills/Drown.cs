using HIFUVoidFiendTweaks.Skills;
using HIFUVoidFiendTweaks.VFX;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements.Experimental;

namespace HVFT.Skills
{
    public class Drown : TweakBase
    {
        public static float Damage;
        public static float Radius;
        public static int FalloffType;
        public static float FourthDamage;
        public static int FourthFalloff;
        public static float FourthRadius;

        public override string Name => "Primary : Drown";

        public override string SkillToken => "primary";

        public override string DescText => "Fire a <style=cIsUtility>slowing</style> long-range beam for <style=cIsDamage>" + d(Damage) + " damage</style>. Every fourth shot fires a medium beam for an additional <style=cIsDamage>" + d(FourthDamage) + " damage</style>.";

        public override bool isVoid => false;

        public int faaa = FalloffType switch
        {
            1 => (int)BulletAttack.FalloffModel.DefaultBullet,
            2 => (int)BulletAttack.FalloffModel.Buckshot,
            _ => (int)BulletAttack.FalloffModel.None
        };

        public override void Init()
        {
            Damage = ConfigOption(3f, "Damage", "Decimal. Vanilla is 3");
            Radius = ConfigOption(1f, "Radius", "Vanilla is 2");
            FalloffType = ConfigOption(1, "Falloff Type", "0 is None, 1 is Standard, 2 is Buckshot. Vanilla is 0");
            FourthDamage = ConfigOption(1.5f, "Fourth Hit Damage", "Decimal. Default is 1.5");
            FourthFalloff = ConfigOption(0, "Fourth Hit Falloff Type", "0 is None, 1 is Standard, 2 is Buckshot. Default is 0");
            FourthRadius = ConfigOption(5f, "Fourth Hit Radius", "Default is 5");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += FireHandBeam_OnEnter;
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            IL.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += FireHandBeam_OnEnter1;
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody body)
        {
            if (body.name == "VoidSurvivorBody(Clone)")
            {
                var df = body.GetComponent<DrownFour>();
                if (!df)
                {
                    body.gameObject.AddComponent<DrownFour>();
                }
            }
        }

        private void FireHandBeam_OnEnter1(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.Before,
               x => x.MatchLdcI4(0),
               x => x.MatchStfld(typeof(BulletAttack), nameof(BulletAttack.falloffModel))))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, faaa);
            }
            else
            {
                Main.HVFTLogger.LogError("Failed to apply Drown Falloff IL Hook");
            }

            // c.next.operand didnt work
            // c.remove, c.emit doesnt work either??
            // tried a couple different matches, no error but still doesnt change the value
            // ask uhh harb or ideath or randomlyawesome
        }

        private void FireHandBeam_OnEnter(On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireHandBeam self)
        {
            self.damageCoefficient = Damage;
            self.bulletRadius = Radius;
            var vf = self.characterBody.GetComponent<DrownFour>();
            if (vf != null)
            {
                vf.FireCount++;
            }
            orig(self);
        }
    }

    public class DrownFour : MonoBehaviour
    {
        public int FireCount;
        public CharacterBody body => GetComponent<CharacterBody>();
        private GameObject hitEffectPrefab => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamImpact.prefab").WaitForCompletion();

        public void FixedUpdate()
        {
            if (FireCount >= 4 && Util.HasEffectiveAuthority(body.gameObject))
            {
                Util.PlaySound("Play_voidman_m2_explode", gameObject);
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = body.corePosition,
                    aimVector = body.inputBank.GetAimRay().direction,
                    muzzleName = "MuzzleHandBeam",
                    maxDistance = 1000,
                    minSpread = 0f,
                    maxSpread = 0f,
                    radius = Drown.FourthRadius,
                    falloffModel = Drown.FourthFalloff switch
                    {
                        1 => BulletAttack.FalloffModel.DefaultBullet,
                        2 => BulletAttack.FalloffModel.Buckshot,
                        _ => BulletAttack.FalloffModel.None
                    },
                    smartCollision = true,
                    damage = body.damage * Drown.FourthDamage,
                    procCoefficient = 0f,
                    force = 0f,
                    isCrit = Util.CheckRoll(body.crit, body.master),
                    hitEffectPrefab = hitEffectPrefab,
                    stopperMask = LayerIndex.noCollision.mask,
                    bulletCount = 1,
                    tracerEffectPrefab = BigTracer.tracer,
                    damageType = DamageType.SlowOnHit
                }.Fire();
                FireCount = 0;
            }
        }
    }
}