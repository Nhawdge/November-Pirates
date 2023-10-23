//namespace NovemberPirates.Entities
//{
//    internal class EntityCache
//    {
//        public static EntityCache Instance = new EntityCache();

//        public Dictionary<string, Entity> Entities = new();

//        private EntityCache()
//        {

//        }

//        public Entity Get(string key)
//        {
//            if (Entities.TryGetValue(key, out var entity))
//            {
//                return entity;
//            }
//            throw new Exception($"Entity with key {key} not found");
//        }

//        public void Add(string key, Entity entity)
//        {
//            //if (Entities.TryAdd(entity.Id.ToString(), entity);
//        }
//    }
//}
