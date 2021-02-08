using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxwareERC.Objects
{
    public class CompetitorResult
    {
        //public CompetitionClass Class { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Car { get; set; }
        public string Time1 { get; set; }
        public string Time2 { get; set; }
        public string Time3 { get; set; }
        public string Time4 { get; set; }
        public string Time5 { get; set; }
        public string Time6 { get; set; }
        public string Time7 { get; set; }
        public string Time8 { get; set; }
        public string Time9 { get; set; }
        public string Time10 { get; set; }
        public string Time11 { get; set; }
        public string Time12 { get; set; }
        public string Time13 { get; set; }
        public string Time14 { get; set; }
        public string Time15 { get; set; }
        public string Time16 { get; set; }
        public string Time17 { get; set; }
        public string Time18 { get; set; }
        public string Time19 { get; set; }
        public string Time20 { get; set; }
        public double RawTime { get; set; }
        public double MinusSlowest { get; set; }
        public double FastestLap { get; set; }
        public int Penalties { get; set; }
        public int Position { get; set; }
        public int PositionPoints { get; set; }
        public int CompetitorsInClassPoints { get; set; }
        public int FastestLapPoints { get; set; }
        public int Points { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
