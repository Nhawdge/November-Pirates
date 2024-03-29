﻿using NovemberPirates.Utilities;
using System.Numerics;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class AudioEvent
    {
        internal AudioKey Key;
        internal Vector2 Position;
        internal bool Replay = false;
        internal bool AllowMultiple = true;
    }
}
