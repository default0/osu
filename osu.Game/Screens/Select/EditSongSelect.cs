// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Game.Screens.Backgrounds;
using osu.Game.Screens.Edit;
using osu.Game.Graphics;
using osu.Framework.Screens;

namespace osu.Game.Screens.Select
{
    public class EditSongSelect : SongSelect
    {
        Editor editor;

        protected override void addFooterButtons(OsuColour colours)
        {
            footer.AddButton(@"random", colours.Green, carousel.SelectRandom);
            footer.AddButton(@"options", colours.Blue, null);
        }

        protected override void OnResuming(Screen last)
        {
            editor = null;

            base.OnResuming(last);
        }

        protected override void start()
        {
            if (editor != null || Beatmap == null)
                return;

            //in the future we may want to move this logic to a PlayerLoader gamemode or similar, so we can rely on the SongSelect transition
            //and provide a better loading experience (at the moment song select is still accepting input during preload).
            editor = new Editor
            {
                BeatmapInfo = carousel.SelectedGroup.SelectedPanel.Beatmap
            };

            editor.Preload(Game, delegate
            {
                if (!Push(editor))
                {
                    editor = null;
                    //error occured?
                }
            });
        }
    }
}
