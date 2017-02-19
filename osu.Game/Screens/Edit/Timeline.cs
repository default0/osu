using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Timing;
using osu.Game.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Input;
using osu.Game.Overlays.Notifications;
using osu.Framework.Graphics.Transformations;
using osu.Game.Overlays;

namespace osu.Game.Screens.Edit
{
    public class Timeline : Container
    {
        private ScrollContainer scroll;
        private Container scrollContent;

        public float CurrentTime
        {
            get { return scroll.Current; }
            set { scroll.ScrollTo(value, false); }
        }
        public float TimeRange { get; set; }

        private Beatmap beatmap;
        private float trackLengthBacking;
        private float trackLength { get { return trackLengthBacking; }set { trackLengthBacking = value; tickLayout.Invalidate(); } }

        private BeatmapDatabase beatmapDb;

        private TextTooltip notification;

        private Cached tickLayout;

        public Timeline(WorkingBeatmap beatmap)
        {
            this.beatmap = beatmap.Beatmap;
            TimeRange = 10000f;
            Children = new Drawable[]
            {
                notification = new TextTooltip
                {
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.None,
                    Alpha = 0f,
                    //Width = 400f,
                    //Height = 100f
                },
                scroll = new ScrollContainer(Direction.Horizontal)
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 100f,
                    ScrollDraggerVisible = false,
                    Children = new Drawable[]
                    {
                        scrollContent = new Container
                        {
                            AutoSizeAxes = Axes.X,
                            RelativeSizeAxes = Axes.Y
                        }
                    }
                }
            };

            var track = beatmap.Track;
            while (track.Length == 0)
                Thread.Sleep(1);

            trackLength = (float)track.Length;
        }

        [BackgroundDependencyLoader]
        private void load(BeatmapDatabase beatmapDb)
        {
            this.beatmapDb = beatmapDb;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }

        protected override void Update()
        {
            base.Update();

            Height = Parent.DrawSize.X;

            if (!tickLayout.EnsureValid())
                tickLayout.Refresh(buildTimingTicks);
        }

        class TimingChangeTick : Box
        {
            public override bool HandleInput => true;

            public ControlPoint ControlPoint { get; set; }

            private TextTooltip notification;

            public TimingChangeTick(TextTooltip notification)
            {
                this.notification = notification;
            }

            protected override bool OnHover(InputState state)
            {
                Width = 5f;
                notification.FadeIn(250);
                notification.Position = Parent.ToSpaceOfOtherDrawable(Position, notification.Parent);
                notification.Position += new Vector2(0f, DrawSize.Y);
                //notification.Text = $"{(ControlPoint.TimingChange ? "Timing Change" : "Control Point")}";
                return true;
            }
            protected override void OnHoverLost(InputState state)
            {
                Width = 2f;
                notification.FadeOut(750, EasingTypes.InCubic);
                base.OnHoverLost(state);
            }
        }
        private void buildTimingTicks()
        {
            Func<Color4, float, double, bool, Box> addBox = (boxColor, tickSize, time, isControlTick) =>
            {
                Box box = isControlTick ? new TimingChangeTick(notification) : new Box();
                box.Colour = boxColor;
                box.RelativeSizeAxes = Axes.Y;
                box.RelativePositionAxes = Axes.Y;
                box.Size = new Vector2(2f, tickSize);
                box.Origin = Anchor.Centre;
                box.Position = new Vector2((float)time, 0.5f);
                scrollContent.Add(box);
                return box;
            };

            foreach (var cp in beatmap.ControlPoints.Where(cp => !cp.TimingChange))
            {
                var box = addBox(Color4.LightGreen, 1f, cp.Time, true);
                ((TimingChangeTick)box).ControlPoint = cp;
            }

            var timingPoints = beatmap.ControlPoints.Where(cp => cp.TimingChange).OrderBy(cp => cp.Time).ToArray();
            for(var i = 0; i < timingPoints.Length; ++i)
            {
                var point = timingPoints[i];
                var end = (i + 1) == timingPoints.Length ? trackLength : timingPoints[i + 1].Time;
                float msPerMetreNoteValue = (float)point.BeatLength / point.Metre.Beats;
                int tickCount = 1;
                for (var t = point.Time; t <= end; t += msPerMetreNoteValue)
                {
                    Color4 boxColor;

                    var tickSize = 0f;
                    if (tickCount == 1)
                    {
                        boxColor = Color4.White;
                        tickSize = 0.5f;
                    }
                    else if ((point.Metre.Measure & 1) == 0 && tickCount == ((point.Metre.Measure / 2) + 1))
                    {
                        boxColor = Color4.Red;
                        tickSize = 0.35f;
                    }
                    else
                    {
                        boxColor = Color4.CornflowerBlue;
                        tickSize = 0.25f;
                    }
                    if (t == point.Time)
                    {
                        var box = addBox(Color4.Red, 1f, t, true);
                        ((TimingChangeTick)box).ControlPoint = point;
                    }

                    ++tickCount;
                    if (tickCount > point.Metre.Measure)
                        tickCount = 1;

                    addBox(boxColor, tickSize, t, false);
                }
            }
            scrollContent.Scale = new Vector2(DrawHeight / TimeRange, 1f);
            var childScale = TimeRange / DrawHeight;
            foreach (var child in scrollContent.Children)
                child.Scale = new Vector2(childScale, 1f);
        }
    }
}
