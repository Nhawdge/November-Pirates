using NovemberPirates.Scenes.Levels.Components;

namespace NovemberPirates.Scenes.Menus.Components
{
    internal class SpriteButton : UiTitle
    {
        internal Sprite ButtonSprite;
        internal Sprite TextSprite;
        internal Action Action { get; set; } = () => { };
    }
}
