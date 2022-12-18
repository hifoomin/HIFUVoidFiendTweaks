using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HAT.Skills
{
    public class IonSurge : TweakBase
    {
        public static float AoE;
        public static float Cooldown;
        public static float Damage;
        public static float dashSpeed;

        public override string Name => ": Special :: Ion Surge";

        public override string SkillToken => "special_lightning";

        public override string DescText => "<style=cIsDamage>Stunning</style>. Soar and dash, dealing <style=cIsDamage>" + d(Damage) + " damage</style> in a large area.";

        public override void Init()
        {
            AoE = ConfigOption(14f, "Area of Effect", "Vanilla is 14");
            Cooldown = ConfigOption(5f, "Cooldown", "Vanilla is 8");
            Damage = ConfigOption(8f, "Damage", "Decimal. Vanilla is 8");
            dashSpeed = ConfigOption(3.5f, "Dash Speed Multiplier", "Default is 3.5");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.Mage.FlyUpState.OnEnter += FlyUpState_OnEnter;
            // IL.EntityStates.Mage.FlyUpState.HandleMovements += FlyUpState_HandleMovements;
            Changes();
        }

        private void FlyUpState_HandleMovements(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<BaseState>("moveSpeedStat")))
            {
                c.Index++;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<float>>(() =>
                {
                    return 10.15f;
                });
            }
            else
            {
                Main.HATLogger.LogError("Failed to IL hook Ion Surge Handle Movements");
            }
        }

        private void FlyUpState_OnEnter(On.EntityStates.Mage.FlyUpState.orig_OnEnter orig, EntityStates.Mage.FlyUpState self)
        {
            EntityStates.Mage.FlyUpState.blastAttackRadius = AoE;
            EntityStates.Mage.FlyUpState.blastAttackDamageCoefficient = Damage;
            EntityStates.Mage.FlyUpState.duration = 0.3f;
            orig(self);
            if (self.isAuthority)
            {
                Vector3 direction = (self.inputBank.moveVector == Vector3.zero ? Vector3.zero : self.inputBank.moveVector.normalized);
                Vector3 a = direction.normalized * dashSpeed * self.moveSpeedStat;
                self.characterMotor.Motor.ForceUnground();
                self.characterMotor.velocity = a;
            }
        }

        private void Changes()
        {
            var surge = Addressables.LoadAssetAsync<SkillDef>("RoR2/Base/Mage/MageBodyFlyUp.asset").WaitForCompletion();
            surge.baseRechargeInterval = Cooldown;
        }
    }
}