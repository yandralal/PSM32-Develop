using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Psm32.Models
{
    public enum Side
    {
        Left,
        Right,
    }

    public enum Polarity
    {
        U,
        B,
    }

    public class MuscleConfig
    {
        public MuscleConfig()
        {
            //TODO: figure out default values
            Name = new MuscleName();
            AmpPos = 0;
            AmpNeg = 0;
            PwPos = 0;
            PwNeg = 0;
            Freq = 0;
            Enabled = true;

           
            AmpPercent = 100;
            PwPercent = 100;
            FreqPercent = 100;
        }

        public MuscleConfig(
            string shortName,
            decimal ampPos,
            decimal ampNeg,
            decimal pwPos,
            decimal pwNeg,
            int freq,
            string polarity,
            string side,
            bool enabled)
        {
            try
            {
                Polarity = (Polarity)Enum.Parse(typeof(Polarity), polarity);
            }
            catch
            {
                throw new ArgumentException("Invalid Polarity Value");
            }

            try
            {
                Side = (Side)Enum.Parse(typeof(Side), side);
            }
            catch
            {
                throw new ArgumentException("Invalid Channel Side Value");
            }

            AmpPos = ampPos;
            AmpNeg = ampNeg;
            PwPos = pwPos;
            PwNeg = pwNeg;
            Freq = freq;
            Name = new MuscleName(shortName: shortName);
            Enabled = enabled;

            //TODO: set these values from outside?
            AmpPercent = 100;
            PwPercent = 100;
            FreqPercent = 100;
        }

            public MuscleConfig(MuscleConfig muscleConfig)
        {
            Name = muscleConfig.Name;
            AmpPos = muscleConfig.AmpPos;
            AmpNeg = muscleConfig.AmpNeg;
            PwPos = muscleConfig.PwPos;
            PwNeg = muscleConfig.PwNeg;
            Freq = muscleConfig.Freq;
            Polarity = muscleConfig.Polarity;
            Side = muscleConfig.Side;
            Enabled = muscleConfig.Enabled;
            AmpPercent = muscleConfig.AmpPercent;
            PwPercent = muscleConfig.PwPercent;
            FreqPercent = muscleConfig.FreqPercent;
        }


        public override bool Equals(object? obj)
        {
            MuscleConfig? config = obj as MuscleConfig;

            if (config == null)
            {
                return false;
            }

            return Name == config.Name
                & AmpPos == config.AmpPos
                & AmpNeg == config.AmpNeg
                & PwPos == config.PwPos
                & PwNeg == config.PwNeg
                & Freq == config.Freq
                & Polarity == config.Polarity
                & Side == config.Side
                & Enabled == config.Enabled
                & AmpPercent == config.AmpPercent
                & PwPercent == config.PwPercent
                & FreqPercent == config.FreqPercent;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode()
                ^ AmpPos.GetHashCode()
                ^ AmpNeg.GetHashCode()
                ^ PwPos.GetHashCode()
                ^ PwNeg.GetHashCode()
                ^ Freq.GetHashCode()
                ^ Polarity.GetHashCode()
                ^ Side.GetHashCode()
                ^ Enabled.GetHashCode()
                ^ AmpPercent.GetHashCode()
                ^ PwPercent.GetHashCode()
                ^ FreqPercent.GetHashCode();
        }


        // public string ID { get; }
        public MuscleName Name { get; set; }
        public decimal AmpPos { get; set; }
        public decimal AmpNeg { get; set; }
        public int AmpPercent { get; set; }
        public decimal PwPos { get; set; }
        public decimal PwNeg { get; set; }
        public int PwPercent { get; set; }

        public int Freq { get; set; }
        public int FreqPercent { get; set; }
        public Polarity Polarity { get; set; }
        public Side Side { get; set; }
        public bool Enabled { get; set; }
    }
}
