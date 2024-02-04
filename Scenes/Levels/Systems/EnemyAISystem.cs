using Arch.Core;
using NovemberPirates.Systems;

namespace NovemberPirates.Scenes.Levels.Systems
{
    internal class EnemyAISystem : GameSystem
    {
        internal override void Update(World world)
        {

        }
    }

    public enum EnemyGoals
    {
        Sailing,
        Trading,
        Repairing,
        Escaping,
        GetReinforcements,
        Fighting
    }
}
