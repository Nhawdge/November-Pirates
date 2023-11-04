using NovemberPirates.Components;
using Raylib_CsLo;

namespace NovemberPirates.Extensions
{
    internal static class MiscExtensions
    {
        public static Rectangle GetRectangle(this Texture texture) =>
            new Rectangle(0, 0, texture.width, texture.height);

        public static void Draw(this Sprite sprite) =>
            Raylib.DrawTexturePro(sprite.Texture, sprite.Source, sprite.Destination, sprite.Origin, sprite.RenderRotation, sprite.Color);
    }
}
