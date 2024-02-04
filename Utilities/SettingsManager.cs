using System.Text.Json;

namespace NovemberPirates.Utilities
{
    internal class SettingsManager
    {
        private SettingsManager() { LoadSettings(); }

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
                Console.WriteLine("Settings file not found, creating new settings file");
                Settings.Add(SettingKeys.MainVolume, 50f);
                Settings.Add(SettingKeys.MusicVolume, 50f);
                Settings.Add(SettingKeys.SfxVolume, 50f);
                Settings.Add(SettingKeys.Fullscreen, 0);
            }
        }

        internal void SaveSettings()
        {
            try
            {
                File.WriteAllText("settings.json", JsonSerializer.Serialize(Settings));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        internal enum SettingKeys
        {
            MainVolume,
            MusicVolume,
            SfxVolume,
            Language,
            Fullscreen
        }
    }
}
