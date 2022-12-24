﻿using HIFUVoidFiendTweaks.VFX;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

        public override string DescText => "Fire a <style=cIsUtility>slowing</style> long-range beam for <style=cIsDamage>" + d(Damage) + " damage</style>. Every fouth shot fires a large devastating beam for an additional <style=cIsDamage>" + d(FourthDamage) + " damage</style>.";

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
            FourthRadius = ConfigOption(9f, "Fourth Hit Radius", "Default is 9");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += FireHandBeam_OnEnter;
            IL.EntityStates.VoidSurvivor.Weapon.FireHandBeam.OnEnter += FireHandBeam_OnEnter1;
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
            var vfdbc = self.characterBody.GetComponent<VoidFiendDevastatingBeamComponent>();
            if (vfdbc == null)
            {
                self.characterBody.gameObject.AddComponent<VoidFiendDevastatingBeamComponent>();
            }
            else
            {
                vfdbc.FireCount++;
            }
            orig(self);
        }
    }

    public class VoidFiendDevastatingBeamComponent : MonoBehaviour
    {
        public int FireCount;
        public CharacterBody body;
        private GameObject hitEffectPrefab;

        public void Start()
        {
            body = GetComponent<CharacterBody>();
            hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBeamImpact.prefab").WaitForCompletion();
        }

        public void FixedUpdate()
        {
            if (FireCount >= 4 && Util.HasEffectiveAuthority(body.gameObject))
            {
                EffectManager.SpawnEffect(BigTracer.tracer, new EffectData
                {
                    origin = body.corePosition,
                    scale = Drown.Radius
                }, true);
                Util.PlaySound("Play_voidman_m2_explode", gameObject);
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = body.corePosition,
                    aimVector = body.inputBank ? body.inputBank.aimDirection : transform.forward,
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
                    smartCollision = false,
                    damage = body.damage * Drown.FourthDamage,
                    procCoefficient = 0f,
                    force = 0f,
                    isCrit = Util.CheckRoll(body.crit, body.master),
                    hitEffectPrefab = hitEffectPrefab,
                    stopperMask = LayerIndex.noCollision.mask,
                    bulletCount = 1
                }.Fire();
                FireCount = 0;
            }
        }
    }
}