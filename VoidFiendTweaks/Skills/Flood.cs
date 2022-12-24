/*using HVFT;

namespace HIFUVoidFiendTweaks.Skills
{
    internal class Flood : TweakBase
    {
        public static float Knockback;
        public override string Name => "Secondary : Flood";

        public override string SkillToken => "secondary";

        public override string DescText => "Fire a plasma bolt for <style=cIsDamage>600% damage</style>. Fully charge it for an explosive plasma ball instead, dealing <style=cIsDamage>1100% damage</style>.";

        public override bool isVoid => false;

        public override void Init()
        {
            Knockback = ConfigOption(1500f, "Knockback", "Vanilla is 300");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.OnEnter += FireMegaBlasterBase_OnEnter;
        }

        private void FireMegaBlasterBase_OnEnter(On.EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase.orig_OnEnter orig, EntityStates.VoidSurvivor.Weapon.FireMegaBlasterBase self)
        {
            if (self is EntityStates.VoidSurvivor.Weapon.FireMegaBlasterSmall)
            {
                self.force = Knockback;
            }
            orig(self);
        }
    }
}
*/

// fine as is