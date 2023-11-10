using NovemberPirates.Components;
using Raylib_CsLo;
using System.Numerics;

namespace NovemberPirates.Extensions
{
    internal static class MiscExtensions
    {
        public static Rectangle GetRectangle(this Texture texture)
            => new Rectangle(0, 0, texture.width, texture.height);

        public static void Draw(this Sprite sprite)
            => Raylib.DrawTexturePro(sprite.Texture, sprite.Source, sprite.Destination, sprite.Origin, sprite.RenderRotation, sprite.Color);

        internal static Vector2 ToVector2(this long[] px)
            => new Vector2(px[0], px[1]);

        internal static float DistanceTo(this Vector2 a, Vector2 b)
            => Vector2.Dot(Vector2.Abs(a - b), Vector2.One);

        internal static Vector2 ToPixels(this Vector2 coords)
            => new Vector2(coords.X * 64, coords.Y * 64);

    }
}
