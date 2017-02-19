// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Screens;
using osu.Game.Screens.Backgrounds;
using OpenTK.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Framework.Allocation;
using osu.Game.Configuration;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Timing;
using osu.Game.Modes;
using osu.Framework.Graphics;
using System.Linq;
using System.Threading;
using osu.Framework.Graphics.Sprites;
using OpenTK;

namespace osu.Game.Screens.Edit
{
    class Editor : OsuScreen
    {
        public BeatmapInfo BeatmapInfo { get; set; }

        IAdjustableClock sourceClock;

        protected override void OnEntering(Screen last)
        {
            var beatmap = Beatmap.Beatmap;
            if (beatmap.BeatmapInfo?.Mode > PlayMode.Osu)
            {
                //we only support osu! mode for now because the hitobject parsing is crappy and needs a refactor.
                Exit();
                return;
            }
            sourceClock.Reset();
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, BeatmapDatabase beatmaps, OsuConfigManager config)
        {
            Beatmap = beatmaps.GetWorkingBeatmap(BeatmapInfo);
            var track = Beatmap.Track;

            if (track != null)
            {
                audio.Track.SetExclusive(track);
                sourceClock = track;
            }
        }

        protected override bool OnExiting(Screen next)
        {
            return false;
        }

        protected override void LoadComplete()
        {
            Children = new Drawable[]
            {
                new Timeline(Beatmap)
                {
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.X,
                    Width = 1f,
                    Height = 100f
                }
            };
            base.LoadComplete();
        }
    }
}
