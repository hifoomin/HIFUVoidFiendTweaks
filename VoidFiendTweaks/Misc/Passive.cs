using HVFT;
using MonoMod.Cil;
using R2API;
using RoR2;
using UnityEngine.Networking;

namespace HIFUVoidFiendTweaks.Misc
{
    internal class Passive : MiscBase
    {
        public static float CPSinCombat;
        public static float CPSOutOfCombat;
        public static float CPerCrit;
        public static float ArmorBuff;
        public static float CFullHeal;
        public static float CFullDamage;
        public static float HealingMultiplier;
        public override string Name => "Misc : Corruption System";

        public override void Init()
        {
            CPSinCombat = ConfigOption(5f, "Corruption Per Second In Combat", "Vanilla is 3");
            CPSOutOfCombat = ConfigOption(3.5f, "Corruption Per Second Out Of Combat", "Vanilla is 3");
            CPerCrit = ConfigOption(0f, "Corruption Per Crit", "Vanilla is 2");
            ArmorBuff = ConfigOption(-40f, "Corruption Armor", "Vanilla is 100");
            CFullHeal = ConfigOption(-115f, "Corruption For Full Heal", "Vanilla is -100. This is used to reduce corruption while healing, formula: Heal Amount / (Max HP + Max Shields) * Corruption For Full Heal");
            CFullDamage = ConfigOption(40f, "Corruption For Full Damage", "Vanilla is 50. This is used to increase corruption while taking damage, formula: Damage Taken Amount / (Max HP + Max Shields) * Corruption For Full Damage");
            HealingMultiplier = ConfigOption(0.6f, "Healing Multiplier", "Vanilla is 1");
            base.Init();
        }

        public override void Hooks()
        {
            On.RoR2.VoidSurvivorController.OnEnable += VoidSurvivorController_OnEnable;
            IL.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
            Changes();
        }

        private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen)
        {
            if (NetworkServer.active && amount > 0f && nonRegen == true)
            {
                if (self.body.inventory && self.body.inventory.GetItemCount(DLC1Content.Items.VoidmanPassiveItem) > 0)
                {
                    if (!procChainMask.HasProc(ProcType.VoidSurvivorCrush))
                    {
                        amount *= HealingMultiplier;
                    }
                }
            }
            return orig(self, amount, procChainMask, nonRegen);
        }

        private void CharacterBody_RecalculateStats(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdsfld("RoR2.DLC1Content/Buffs", "VoidSurvivorCorruptMode"),
                x => x.MatchCallOrCallvirt<CharacterBody>("HasBuff"),
                x => x.MatchBrtrue(out _),
                x => x.MatchLdcR4(out _),
                x => x.MatchBr(out _),
                x => x.MatchLdcR4(100f)))
            {
                c.Index += 5;
                c.Next.Operand = ArmorBuff;
            }
            else
            {
                Main.HVFTLogger.LogError("Failed to apply Corrupted Armor IL Hook");
            }
        }

        private void VoidSurvivorController_OnEnable(On.RoR2.VoidSurvivorController.orig_OnEnable orig, VoidSurvivorController self)
        {
            self.corruptionPerSecondInCombat = CPSinCombat;
            self.corruptionPerSecondOutOfCombat = CPSOutOfCombat;
            self.corruptionPerCrit = CPerCrit;
            self.corruptionForFullHeal = CFullHeal;
            self.corruptionForFullDamage = CFullDamage;
            orig(self);
        }

        private void Changes()
        {
            LanguageAPI.Add("KEYWORD_VOIDCORRUPTION", "<style=cKeywordName>Corruption</style><style=cSub>Gain Corruption by receiving damage" +
                           (CPerCrit > 0 ? ", dealing Critical Strikes," : "") +
                           " or holding Void Items. Reduce Corruption by healing.</style>");
        }
    }
}