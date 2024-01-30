using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class Wind
    {
        public float WindStrength = 200;
        public Vector2 WindDirection = new Vector2(1, 0);
        internal float LastWindChange = 0;
    }
}
