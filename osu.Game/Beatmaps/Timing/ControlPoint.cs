// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

namespace osu.Game.Beatmaps.Timing
{
    public class ControlPoint
    {
        public static ControlPoint Default = new ControlPoint
        {
            BeatLength = 500,
            TimingChange = true,
        };

        public double Time;
        public double BeatLength;
        public double VelocityAdjustment;
        public bool TimingChange;
        public Metre Metre;
    }
    public struct Metre
    {
        /// <summary>
        /// Number of beats per measure. Ex.: <see cref="Beats"/> = 3, <see cref="Measure"/> = 4, then the metre is 3/4.
        /// </summary>
        public int Beats { get; private set; }
        /// <summary>
        /// Notes of the measure. Ex.: <see cref="Beats"/> = 3, <see cref="Measure"/> = 4, then the metre is 3/4.
        /// </summary>
        public int Measure { get; private set; }

        public Metre(int beats)
            : this(beats, 4)
        {
        }

        public Metre(int beats, int measure)
        {
            Beats = beats;
            Measure = measure;
        }

        public override string ToString()
        {
            return $"{Beats}/{Measure} Metre";
        }
    }

    internal enum TimeSignatures
    {
        SimpleQuadruple = 4,
        SimpleTriple = 3
    }
}
