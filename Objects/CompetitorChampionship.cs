using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxwareERC.Objects
{
    public class CompetitorChampionship
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Car { get; set; }
        // 3 x 15 array to sum 15 events at 3 point categories per event (Position, Competitor per class, Fastest lap)
        public int[] Points { get; set; }
        public int Total { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
