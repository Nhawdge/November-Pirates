using Arch.Core;
using NovemberPirates.Components;
using NovemberPirates.Scenes.Menus.Components;
using NovemberPirates.Scenes.Menus.Systems;
using NovemberPirates.Utilities;
using Raylib_CsLo;

namespace NovemberPirates.Scenes.Menus.MainMenu
{
    internal class SettingsScene : BaseScene
    {
        public SettingsScene(Singleton singleton, BaseScene lastscene)
        {
            Systems.Add(new MenuSystem());
            Systems.Add(new MenuMusicSystem());
            World.Create(singleton);

            World.Create(new UiContainer { Rectangle = new Rectangle() });

            var index = 1;
            World.Create(new UiTitle { Text = "Settings", Order = index++ });

            World.Create(new UiSlider
            {
                Text = "Main Volume",
                Order = index++,
                Value = SettingsManager.Instance.Settings[SettingsManager.SettingKeys.MainVolume],
                MinValue = 0,
                MaxValue = 100,
                SettingKey = SettingsManager.SettingKeys.MainVolume,
            });

            World.Create(new UiSlider
            {
                Text = "Music Volume",
                Order = index++,
                Value = SettingsManager.Instance.Settings[SettingsManager.SettingKeys.MusicVolume],
                MinValue = 0,
                MaxValue = 100,
                SettingKey = SettingsManager.SettingKeys.MusicVolume,
            });

            World.Create(new UiSlider
            {
                Text = "SFX Volume",
                Order = index++,
                Value = SettingsManager.Instance.Settings[SettingsManager.SettingKeys.SfxVolume],
                MinValue = 0,
                MaxValue = 100,
                SettingKey = SettingsManager.SettingKeys.SfxVolume,
            });

            World.Create(new UiButton
            {
                Text = "Save",
                Action = () =>
                {
                    SettingsManager.Instance.SaveSettings();
                    NovemberPiratesEngine.Instance.ActiveScene = lastscene;
                },
                Order = index += 2
            });

            World.Create(new UiButton
            {
                Text = "Back",
                Action = () =>
                {
                    NovemberPiratesEngine.Instance.ActiveScene = lastscene;
                },
                Order = index += 2,
            });
        }

    }
}
