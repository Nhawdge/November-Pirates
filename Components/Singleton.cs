namespace NovemberPirates.Components
{
    internal class Singleton
    {
        public DebugLevel Debug = DebugLevel.None;
        public string DebugText = "";
    }

    public enum DebugLevel
    {
        None,
        Low,
        Medium,
        High,
    }
}

