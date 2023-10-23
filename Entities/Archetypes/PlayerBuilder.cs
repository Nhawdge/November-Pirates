using Arch.Core;
using NovemberPirates.Components;

namespace NovemberPirates.Entities.Archetypes
{
    internal static class PlayerBuilder
    {
        internal static Entity Create()
        {
            var player = NovemberPiratesEngine.Instance.ActiveScene.World.Create<Player>();

            return player;
        }
    }
}
