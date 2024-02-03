using NovemberPirates.Utilities;

namespace NovemberPirates.Scenes.Menus.Components
{
    internal class UiSlider : UiTitle
    {
        internal SettingsManager.SettingKeys SettingKey;

        public float Value { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
    }
}
