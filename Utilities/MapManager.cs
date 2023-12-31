using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using NovemberPirates.Components;
using NovemberPirates.Entities.Archetypes;
using NovemberPirates.Extensions;
using QuickType.Map;
using System.Numerics;

namespace NovemberPirates.Utilities
{
    internal class MapManager
    {
        internal static MapManager Instance = new MapManager();

        private MapManager() { }

        public MapDetails LoadMap(string key, World world)
        {
            var mapName = "Pirates";
            var mapData = new LdtkData();
            var mapFile = File.ReadAllText($"Assets/Maps/{mapName}.ldtk");
            var data = LdtkData.FromJson(mapFile);

            var level = data.Levels.FirstOrDefault(x => x.Identifier == key);

            if (level == null)
            {
                throw new ArgumentException($"key: '{key}' not found in {mapName}.ldtk");
            }
            var details = new MapDetails();

            details.MapEdge = new Vector2(level.PxWid, level.PxHei);
            var mapTileArchetype = new ComponentType[] { typeof(MapTile), typeof(Render) };

            foreach (var layer in level.LayerInstances)
            {
                foreach (var tile in layer.AutoLayerTiles)
                {
                    details.TileSize = (int)layer.GridSize;
                    var mapTileEntity = world.Create(mapTileArchetype);

                    var tileSprite = new Render(TextureKey.MapTileset)
                    {
                        Position = tile.Px.ToVector2(),
                        SpriteWidth = (int)layer.GridSize,
                        SpriteHeight = (int)layer.GridSize,
                        Row = (int)(tile.Src.Last() / layer.GridSize),
                        Column = (int)(tile.Src.First() / layer.GridSize),
                        OriginPos = Render.OriginAlignment.LeftTop,
                    };
                    if (layer.Identifier == "Shallow_water")
                    {
                        tileSprite.Collision = CollisionType.Slow;
                    }
                    else if (layer.Identifier == "Islands")
                    {
                        tileSprite.Collision = CollisionType.Solid;
                    }

                    mapTileEntity.Set(tileSprite);

                    var mapTile = new MapTile();

                    mapTile.Coordinates = tile.Px.ToVector2() / layer.GridSize;
                    mapTile.MovementCost = tileSprite.Collision switch
                    {
                        CollisionType.Solid => 9999,
                        CollisionType.Slow => 2,
                        _ => 1,
                    };

                    mapTileEntity.Set(mapTile);
                }
                
                foreach (var entity in layer.EntityInstances)
                {
                    //Console.WriteLine($"entity: {entity.Identifier}");
                    if (entity.Identifier == "Spawn_Point")
                    {
                        EnemyBuilder.CreateSpawnPoint(world, entity.Px.ToVector2(), Team.Red);
                    }
                    if (entity.Identifier == "Patrol_Point")
                    {
                        var order = (int)(entity.FieldInstances.FirstOrDefault(x => x.Identifier == Identifier.Order).Value.Integer);
                        EnemyBuilder.CreatePatrolPoint(world, entity.Px.ToVector2(), Team.Red, order);
                    }
                    if (entity.Identifier == "Player_Spawn")
                    {
                        // TODO - Create player spawn
                    }
                    if (entity.Identifier == "Port")
                    {
                        var team = entity.FieldInstances.FirstOrDefault(x => x.Identifier == Identifier.Faction).Value;
                        world.Create(new Port()
                        {
                            Position = entity.Px.ToVector2(),
                            Team = System.Enum.Parse<Team>(team.String),
                            Currency = 1
                        });
                    }
                }
            }
            return details;
        }
    }

    public class MapDetails
    {
        public Vector2 MapEdge;
        internal int TileSize;
    }
}
