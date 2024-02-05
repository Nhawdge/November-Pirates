using NovemberPirates.Extensions;
using NovemberPirates.Scenes.Levels.Components;
using Raylib_CsLo;
using System.Numerics;
using static NovemberPirates.Scenes.Levels.Components.Sprite;

namespace NovemberPirates.Utilities
{
    internal static class ShipSpriteBuilder
    {
        internal static Sprite GenerateBoat(BoatOptions options)
        {
            var spriteHeight = options.Hull switch
            {
                HullType.Large => 128,
                _ => 108,
            };
            var shipSize = new Vector2(66, spriteHeight);
            var inCache = TextureManager.Instance.GetCachedTexture(options.ToKey());
            if (inCache.HasValue)
            {
                var cachedSprite = new Sprite()
                {
                    Texture = inCache.Value,
                    Animations = new Dictionary<string, AnimationSets>()
                    {
                        {
                            "idle", new AnimationSets("idle", 0,0, Direction.forward, new List<Frame>
                            {
                                new Frame(0, 0, (int)shipSize.X, (int)shipSize.Y, 100f)
                            })
                        }
                    },
                    Column = 0,
                    Row = 0,
                    Color = Raylib.WHITE,
                };
                cachedSprite.Play("idle");
                return cachedSprite;
            }

            var baseHullSprite = options.Hull switch
            {
                HullType.Large => new Sprite(TextureKey.HullLarge, "Assets/Art/hullLarge") { Position = shipSize / 2 },
                HullType.Medium => new Sprite(TextureKey.HullMedium, "Assets/Art/hullMedium") { Position = shipSize / 2 },
                HullType.Small => new Sprite(TextureKey.HullSmall, "Assets/Art/hullSmall") { Position = shipSize / 2 },
                _ => throw new NotImplementedException($"Hull type: '{options.Hull}' does not exist "),
            };
            baseHullSprite.Play($"HullLarge{(int)options.Condition}");

            //Cannons
            var cannons = new List<Sprite>();

            foreach (var cannon in options.Cannons)
            {
                var cannonPosition = new Vector2(
                    ShipCannonCoords.Data[Enum.GetName(cannon.Placement)],
                    ShipCannonCoords.Data[$"{options.Hull}{cannon.Row}"]
                    );

                var cannonSprite = new Sprite(TextureKey.CannonLoose, "Assets/Art/cannonLoose")
                {
                    Position = cannonPosition,
                    Rotation = cannon.Placement == BoatSide.Port ? 180 : 0,
                };
                cannons.Add(cannonSprite);

                //if (cannon.Position == Vector2.Zero)
                // I have no idea why I needed these magic numbers, but they work.
                cannon.Position = cannonPosition with
                {
                    X = cannonPosition.X - cannonSprite.SpriteWidth - 10,
                    Y = cannonPosition.Y - cannonSprite.SpriteHeight - 25
                };
            }

            // Main sail
            var mainSailSprite = new Sprite(TextureKey.SailLarge, "Assets/Art/sailLarge1");

            mainSailSprite.Play($"{options.Color}{(int)options.Condition}");
            mainSailSprite.Position = new Vector2(shipSize.X / 2, 45);

            //Nest
            var nestSprite = new Sprite(TextureKey.Nest, "Assets/Art/nest") { };
            nestSprite.Position = new Vector2(shipSize.X / 2, 25);

            // flag
            var flagSprite = new Sprite(TextureKey.MainFlag, "Assets/Art/mainFlag") { };
            flagSprite.Position = new Vector2(shipSize.X / 2, 15);
            flagSprite.Play($"{options.Color}Flag");

            //Pole
            var poleSprite = new Sprite(TextureKey.Pole, "Assets/Art/pole") { };
            poleSprite.Position = new Vector2(shipSize.X / 2, options.Hull == HullType.Large ? 100 : 80);

            //front sail
            var frontSailSprite = new Sprite(TextureKey.SailSmall, "Assets/Art/sailSmall1");
            frontSailSprite.Play($"{options.Color}{(int)options.Condition}");
            frontSailSprite.Position = new Vector2(shipSize.X / 2, options.Hull == HullType.Large ? 105 : 85);

            // Build Texture
            var renderTexture = Raylib.LoadRenderTexture((int)shipSize.X, (int)shipSize.Y);

            Raylib.BeginTextureMode(renderTexture);
            Raylib.ClearBackground(new Color(0, 0, 0, 0));

            baseHullSprite.Draw();
            cannons.ForEach(x => x.Draw());
            if (options.Sails >= SailStatus.Full)
                mainSailSprite.Draw();
            nestSprite.Draw();
            flagSprite.Draw();
            poleSprite.Draw();
            if (options.Sails >= SailStatus.Half && options.Condition < BoatCondition.Broken)
                frontSailSprite.Draw();

            Raylib.EndTextureMode();

            var sprite = new Sprite()
            {
                Texture = renderTexture.texture,
                Animations = new Dictionary<string, AnimationSets>()
                {
                    {
                        "idle", new AnimationSets("idle", 0,0, Direction.forward, new List<Frame>
                        {
                            new Frame(0, 0, (int)shipSize.X, (int)shipSize.Y, 100f)
                        })
                    }
                },
                Column = 0,
                Row = 0,
                Color = Raylib.WHITE,
            };
            sprite.Play("idle");

            TextureManager.Instance.TextureCache.Add(options.ToKey(), renderTexture.texture);

            return sprite;
        }

    }
    internal record BoatOptions(HullType Hull, BoatColor Color, SailStatus Sails, List<Cannon> Cannons, BoatCondition Condition = BoatCondition.Good)
    {
        internal BoatOptions(Ship ship) : this(ship.HullType, ship.BoatColor, ship.Sail, ship.Cannons, ship.BoatCondition) { }

        internal string ToKey()
        {
            Cannons.Sort(delegate (Cannon x, Cannon y)
            {
                return (int)x.Placement * x.Row - (int)y.Placement * y.Row;
            });
            var cannonKey = string.Join("", Cannons.Select(x => $"{(int)x.Placement}{x.Row}"));

            var key = $"{Hull}-{Color}-{Sails}-{Condition}-{cannonKey}";
            return key;
        }
    }

    internal enum HullType
    {
        Small,
        Medium,
        Large,
    }

    internal enum BoatColor
    {
        White,
        Dead,
        Red,
        Blue,
        Green,
        Yellow,
    }
    internal enum BoatCondition
    { // Values tie to animations, be warned
        Good = 1,
        Torn = 2,
        Broken = 3,
        Empty = 4,
    }

    internal static class ShipCannonCoords
    {
        internal static Dictionary<string, int> Data = new Dictionary<string, int>()
        {
            {$"Port", 13},
            {$"Starboard", 53},

            {$"{HullType.Large}1", 38},
            {$"{HullType.Large}2", 56},
            {$"{HullType.Large}3", 75},

            {$"{HullType.Medium}1", 38},
            {$"{HullType.Medium}2", 56},
            {$"{HullType.Medium}3", 75},

            {$"{HullType.Small}1", 38},
            {$"{HullType.Small}2", 56},
            {$"{HullType.Small}3", 75},
            };
    }



    //    internal static int Row1 = 38;
    //internal static int Row2 = 56;
    //internal static int Row3 = 75;


    //<53, 38>        <13, 38>
    //<53, 56>        <13, 56>
    //<53, 75>        <13, 75>
}
