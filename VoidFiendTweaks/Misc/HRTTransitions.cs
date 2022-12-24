using HVFT;

namespace HIFUVoidFiendTweaks.Misc
{
    internal class HRTTransitions : MiscBase
    {
        public static float Duration;
        public override string Name => "Misc :: Transitions";

        public override void Init()
        {
            Duration = ConfigOption(0.6f, "Duration", "Vanilla is 1");
            base.Init();
        }

        public override void Hooks()
        {
            On.EntityStates.VoidSurvivor.CorruptionTransitionBase.OnEnter += CorruptionTransitionBase_OnEnter;
        }

        private void CorruptionTransitionBase_OnEnter(On.EntityStates.VoidSurvivor.CorruptionTransitionBase.orig_OnEnter orig, EntityStates.VoidSurvivor.CorruptionTransitionBase self)
        {
            if (self is EntityStates.VoidSurvivor.EnterCorruptionTransition)
            {
                self.duration = Duration;
            }
            orig(self);
        }
    }
}