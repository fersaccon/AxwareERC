using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxwareERC.Objects
{
    public class Championship
    {
        public int Events { get; set; }
        public int OverallCompetitors { get; set; }
        public int N2Competitors { get; set; }
        public int N4Competitors { get; set; }
        public int E2Competitors { get; set; }
        public int E4Competitors { get; set; }
        public int ProCompetitors { get; set; }
        public int TruckCompetitors { get; set; }
        public List<CompetitorChampionship> Overall { get; set; }
        public List<CompetitorChampionship> N2 { get; set; }
        public List<CompetitorChampionship> N4 { get; set; }
        public List<CompetitorChampionship> E2 { get; set; }
        public List<CompetitorChampionship> E4 { get; set; }
        public List<CompetitorChampionship> Pro { get; set; }
        public List<CompetitorChampionship> Truck { get; set; }
    }
}
