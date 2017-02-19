using osu.Game.Graphics;
using osu.Game.Screens.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu.Framework.Screens;
using osu.Framework.Audio.Sample;

namespace osu.Game.Screens.Select
{
    public class PlaySongSelect : SongSelect
    {
        Player player;
        private SampleChannel sampleChangeDifficulty;
        private SampleChannel sampleChangeBeatmap;

        protected override void addFooterButtons(OsuColour colours)
        {
            footer.AddButton(@"mods", colours.Yellow, null);
            footer.AddButton(@"random", colours.Green, carousel.SelectRandom);
            footer.AddButton(@"options", colours.Blue, null);
        }

        protected override void OnResuming(Screen last)
        {
            player = null;

            base.OnResuming(last);
        }

        protected override void start()
        {
            if (player != null || Beatmap == null)
                return;

            //in the future we may want to move this logic to a PlayerLoader gamemode or similar, so we can rely on the SongSelect transition
            //and provide a better loading experience (at the moment song select is still accepting input during preload).
            player = new Player
            {
                BeatmapInfo = carousel.SelectedGroup.SelectedPanel.Beatmap,
                PreferredPlayMode = playMode.Value
            };

            player.Preload(Game, delegate
            {
                if (!Push(player))
                {
                    player = null;
                    //error occured?
                }
            });
        }
    }
}
