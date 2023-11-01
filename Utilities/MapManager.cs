using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
using NovemberPirates.Components;
using QuickType.Map;
using System.Numerics;

namespace NovemberPirates.Utilities
{
    internal class MapManager
    {
        internal static MapManager Instance = new MapManager();

        private MapManager() { }

        internal void LoadMap(string mapName)
        {

        }

        public void LoadMap(string key, World world)
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
            var mapTileArchetype = new ComponentType[] { typeof(MapTile), typeof(Render) };
            
            foreach (var layer in level.LayerInstances)
            {
                //var tilesprite = Raylib_CsLo.Raylib.LoadTexture($"Assets/Maps/{layer.TilesetRelPath}");
                foreach (var tile in layer.AutoLayerTiles)
                {
                    var mapTile = world.Create(mapTileArchetype);

                    var tileSprite = new Render(TextureKey.MapTileset)
                    {
                        SpriteHeight = (int)layer.GridSize,
                        SpriteWidth = (int)layer.GridSize,
                        Position = new Vector2(tile.Px.First(), tile.Px.Last()),
                        Column = (int)(tile.Src.First() / layer.GridSize),
                        Row = (int)(tile.Src.Last() / layer.GridSize),
                    };
                    if (layer.Identifier == "Shallow_water")
                    {
                        tileSprite.Collision = CollisionType.Slow;
                    }
                    else if (layer.Identifier == "Islands")
                    {
                        tileSprite.Collision = CollisionType.Solid;
                    }

                    mapTile.Set(tileSprite);
                }

            }
        }
    }
}
