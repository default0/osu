using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Game.Overlays
{
    class TextTooltip : Container
    {
        public TextTooltip()
        {
            AutoSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 3f;
            BorderColour = new Color4(31, 31, 31, 255);
            BorderThickness = 2f;
            Children = new Drawable[]
            {
                new Triangle
                {
                    Colour = Color4.Black,
                    Width = 12f,
                    Height = 12f,
                    RelativePositionAxes = Axes.X,
                    EdgeSmoothness = new Vector2(1f, 1f),
                    Origin = Anchor.TopCentre,
                    Position = new Vector2(0.5f, 0f),
                    Edge0 = new Vector2(0, 0f)
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Position = new Vector2(0f, 12f),
                    Colour = Color4.Black,
                },
                new SpriteText()
                {
                    Margin = new Framework.Graphics.Primitives.MarginPadding(5f),
                    Position = new Vector2(0f, 12f),
                    Text = "Test Test Test Lorem Ipsum Dolor Sit Amet Test Test Test asdlfkjasldfkjwoiej",
                    TextSize = 24f,
                    MaximumSize = new Vector2(300f, float.MaxValue)
                }
            };
        }
    }
}
