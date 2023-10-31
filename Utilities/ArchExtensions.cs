using Arch.Core;

namespace NovemberPirates.Utilities
{
    internal static class ArchExtensions
    {
        public static Entity QueryFirst<T0>(this World world)
        {
            var desc = new QueryDescription().WithAll<T0>();
            var query = world.Query(desc);

            Entity? result = null;

            foreach (var chunk in query.GetChunkIterator())
            {
                result = chunk.Entity(0);
                break;
            }

            if (result is null) throw new InvalidOperationException();

            return result.Value;
        }

        public static T0 QueryUnique<T0>(this World world) where T0 : struct
        {
            var desc = new QueryDescription().WithAll<T0>();
            var query = world.Query(desc);

            T0? result = null;

            foreach (var chunk in query.GetChunkIterator())
            {
                result = chunk.GetFirst<T0>();
                break;
            }

            if (result is null) throw new InvalidOperationException();

            return result.Value;
        }
    }

}
