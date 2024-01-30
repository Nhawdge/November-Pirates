using NovemberPirates.Utilities;
using QuickType.Aseprite;
using Raylib_CsLo;
using System.Numerics;
using System.Text.Json;

namespace NovemberPirates.Scenes.Levels.Components
{
    internal class Sprite : Render
    {
        public Dictionary<string, AnimationSets> Animations = new();
        public string AnimationDataPath = string.Empty;
        public float FrameCounter = 0;
        public int CurrentFrameIndex = 0;
        public AnimationSets CurrentAnimation;

        public Sprite(TextureKey key, string animationDataPath = null, float scale = 1, bool isCentered = true) : base(key, scale, isCentered)
        {
            if (animationDataPath != null)
            {
                AnimationDataPath = animationDataPath;
                var data = File.ReadAllText(animationDataPath + ".json");
                var json = JsonSerializer.Deserialize<AsepriteData>(data);

                Animations = json.Meta.FrameTags.ToDictionary(x => x.Name,
                    x => new AnimationSets(x.Name, (int)x.From, (int)x.To, Enum.Parse<Direction>(x.Direction),
                    json.Frames.Where(f => f.Filename.StartsWith(x.Name)).Select(z => new Frame(z.Frame.X, z.Frame.Y, z.Frame.W, z.Frame.H, z.Duration)).ToList()));

                CurrentAnimation = Animations.First().Value;
            }
        }

        public Sprite() : base()
        {

        }

        public override Rectangle Source
        {
            get
            {
                var frame = CurrentFrame;
                if (frame == null)
                    return new Rectangle();
                FrameCounter += Raylib.GetFrameTime();
                if (FrameCounter > frame.Duration / 1000)
                {
                    FrameCounter -= frame.Duration / 1000;
                    var totalFrames = CurrentAnimation.To - CurrentAnimation.From + 1;
                    CurrentFrameIndex = (CurrentFrameIndex + 1) % totalFrames;
                }

                return new Rectangle(
                       frame.X,
                       frame.Y,
                       frame.W * (IsFlipped ? -1 : 1),
                       frame.H
                       );
            }
        }

        public override Rectangle Destination
        {
            get
            {
                var frame = CurrentFrame;
                if (frame == null)
                    return new Rectangle();
                return new Rectangle(
                    Position.X,
                    Position.Y,
                    frame.W * Scale,
                    frame.H * Scale);
            }
        }

        public Frame? CurrentFrame
        {
            get
            {
                var frame = CurrentAnimation?.Frames[CurrentFrameIndex];
                if (frame == null)
                    return null;
                return frame;
            }
        }

        public override Vector2 Origin
        {
            get
            {
                var frame = CurrentFrame;
                if (frame == null)
                    return Vector2.Zero;

                switch (OriginPos)
                {
                    case OriginAlignment.Center:
                        return new Vector2(frame.W / 2 * Scale, frame.H / 2 * Scale);
                    case OriginAlignment.LeftCenter:
                        return new Vector2(0, frame.Y + frame.H / 2 * Scale);
                    case OriginAlignment.LeftBottom:
                        return new Vector2(0, frame.Y + frame.H * Scale);
                    case OriginAlignment.LeftTop:
                    default:
                        return Vector2.Zero;
                }
            }
        }

        public void Play(string animationName, bool forceStart = false)
        {
            if (CurrentAnimation?.Name == animationName && forceStart == false)
                return;
            if (Animations.TryGetValue(animationName, out var animations))
            {
                CurrentAnimation = animations;
                CurrentFrameIndex = 0;
            }
            //else
            //    throw new ArgumentException($"Invalid Animation name '{animationName}' for '{AnimationDataPath}'");
        }

        public record AnimationSets(string Name, int From, int To, Direction direction, List<Frame> Frames);
        //{ "name": "WalkD", "from": 0, "to": 3, "direction": "forward" },
        public class Frame
        {
            public int X;
            public int Y;
            public int W;
            public int H;
            public float Duration;

            public Frame(long x, long y, long w, long h, float duration)
            {
                X = (int)x;
                Y = (int)y;
                W = (int)w;
                H = (int)h;
                Duration = duration;
            }

            public Frame(Rectangle rect, float duration)
            {
                X = (int)rect.x;
                Y = (int)rect.y;
                W = (int)rect.width;
                H = (int)rect.height;
                Duration = duration;
            }
        }

        public enum Direction
        {
            forward,
            backward,
            pingpong
        }
    }
}