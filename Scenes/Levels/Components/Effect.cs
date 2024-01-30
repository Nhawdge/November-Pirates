using System.Numerics;
using System.Runtime.CompilerServices;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class Effect
    {
        internal float Duration;
        internal float Elapsed;

        public Vector2 Motion = Vector2.Zero;
        public Vector2 TruePosition = Vector2.Zero;

        public bool Fadeout = false;

        public bool CreateTrail = false;

        public bool Wiggle = false;
        public float WiggleTimerOffset = 0f;

        public float FadeStart = 1f;
    }
}
