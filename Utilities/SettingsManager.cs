using System.Text.Json;

namespace NovemberPirates.Utilities
{
    internal class SettingsManager
    {
        private SettingsManager() { }

        internal static SettingsManager Instance = new();

        internal Dictionary<SettingKeys, float> Settings = new();

        internal void LoadSettings()
        {
            if (File.Exists("settings.json"))
            {
                try
                {
                    var settings = File.ReadAllText("settings.json");
                    Settings = JsonSerializer.Deserialize<Dictionary<SettingKeys, float>>(settings);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Settings.Add(SettingKeys.MainVolume, 0.5f);
                Settings.Add(SettingKeys.MusicVolume, 0.5f);
                Settings.Add(SettingKeys.SfxVolume, 0.5f);

                File.WriteAllText("settings.json", JsonSerializer.Serialize(Settings));
            }
        }

        internal enum SettingKeys
        {
            MainVolume,
            MusicVolume,
            SfxVolume,
            Language
        }
    }
}
