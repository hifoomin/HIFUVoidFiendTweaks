using R2API;

namespace HVFT
{
    public abstract class TweakBase
    {
        public abstract string Name { get; }
        public abstract string SkillToken { get; }
        public abstract string DescText { get; }
        public abstract bool isVoid { get; }
        public virtual bool isEnabled { get; } = true;

        public T ConfigOption<T>(T value, string name, string description)
        {
            return Main.HVFTConfig.Bind<T>(Name, name, value, description).Value;
        }

        public abstract void Hooks();

        public string d(float f)
        {
            return (f * 100f).ToString() + "%";
        }

        public virtual void Init()
        {
            Hooks();
            string descriptionToken;
            if (isVoid)
            {
                descriptionToken = "VOIDSURVIVOR_" + SkillToken.ToUpper() + "_TOOLTIP";
            }
            else
            {
                descriptionToken = "VOIDSURVIVOR_" + SkillToken.ToUpper() + "_DESCRIPTION";
            }
            LanguageAPI.Add(descriptionToken, DescText);
            Main.HVFTLogger.LogInfo("Added " + Name);
        }
    }
}