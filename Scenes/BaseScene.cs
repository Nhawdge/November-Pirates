using Arch.Core;
using NovemberPirates.Systems;

namespace NovemberPirates.Scenes
{
    internal class BaseScene
    {
        internal World World = World.Create();

        internal List<GameSystem> Systems = new();
    }
}
