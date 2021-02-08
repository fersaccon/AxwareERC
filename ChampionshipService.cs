using AxwareERC.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AxwareERC
{
    public static class ChampionshipService
    {
        public static Championship ReadFile(string filepath)
        {
            try
            {
                var lines = File.ReadAllLines(filepath);
                var championshipResult = new Championship()
                {
                    Events = Convert.ToInt32(lines[0]),
                    OverallCompetitors = Convert.ToInt32(lines[1]),
                    N2Competitors = Convert.ToInt32(lines[2]),
                    N4Competitors = Convert.ToInt32(lines[3]),
                    E2Competitors = Convert.ToInt32(lines[4]),
                    E4Competitors = Convert.ToInt32(lines[5]),
                    ProCompetitors = Convert.ToInt32(lines[6]),
                    TruckCompetitors = Convert.ToInt32(lines[7])
                };

                //Skip one line then reads Overall results
                if (championshipResult.OverallCompetitors > 0)
                {
                    championshipResult.Overall = new List<CompetitorChampionship>();
                    championshipResult.Overall.AddRange(ClassResults(championshipResult.Events, 9, championshipResult.OverallCompetitors, lines));
                }

                //Skip one line then reads N2 results based on number of Overall competitors
                int firstCompetitor = 9 + championshipResult.OverallCompetitors + 1;
                if (championshipResult.N2Competitors > 0)
                {
                    championshipResult.N2 = new List<CompetitorChampionship>();
                    championshipResult.N2.AddRange(ClassResults(championshipResult.Events, firstCompetitor, championshipResult.N2Competitors, lines));
                }

                //Skip one line then reads N4 results based on number of N2 competitors
                firstCompetitor += championshipResult.N2Competitors + 1;
                if (championshipResult.N4Competitors > 0)
                {
                    championshipResult.N4 = new List<CompetitorChampionship>();
                    championshipResult.N4.AddRange(ClassResults(championshipResult.Events, firstCompetitor, championshipResult.N4Competitors, lines));
                }

                //Skip one line then reads E2 results based on number of N4 competitors
                firstCompetitor += championshipResult.N4Competitors + 1;
                if (championshipResult.E2Competitors > 0)
                {
                    championshipResult.E2 = new List<CompetitorChampionship>();
                    championshipResult.E2.AddRange(ClassResults(championshipResult.Events, firstCompetitor, championshipResult.E2Competitors, lines));
                }

                //Skip one line then reads E4 results based on number of E2 competitors
                firstCompetitor += championshipResult.E2Competitors + 1;
                if (championshipResult.E4Competitors > 0)
                {
                    championshipResult.E4 = new List<CompetitorChampionship>();
                    championshipResult.E4.AddRange(ClassResults(championshipResult.Events, firstCompetitor, championshipResult.E4Competitors, lines));
                }

                //Skip one line then reads Pro results based on number of E4 competitors
                firstCompetitor += championshipResult.E4Competitors + 1;
                if (championshipResult.ProCompetitors > 0)
                {
                    championshipResult.Pro = new List<CompetitorChampionship>();
                    championshipResult.Pro.AddRange(ClassResults(championshipResult.Events, firstCompetitor, championshipResult.ProCompetitors, lines));
                }

                //Skip one line then reads Truck results based on number of Pro competitors
                firstCompetitor += championshipResult.ProCompetitors + 1;
                if (championshipResult.TruckCompetitors > 0)
                {
                    championshipResult.Truck = new List<CompetitorChampionship>();
                    championshipResult.Truck.AddRange(ClassResults(championshipResult.Events, firstCompetitor, championshipResult.TruckCompetitors, lines));
                }

                return championshipResult;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
            return null;
        }

        public static bool WriteFile(string filepath, Championship championship)
        {
            try
            {
                using (Stream stream = File.OpenWrite(filepath))
                {
                    stream.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        Type t = typeof(CompetitorChampionship);
                        FieldInfo[] fields = t.GetFields(BindingFlags.Instance |
                           BindingFlags.Static |
                           BindingFlags.NonPublic |
                           BindingFlags.Public);

                        // Write field names
                        writer.WriteLine(championship.Events);
                        writer.WriteLine(championship.OverallCompetitors);
                        writer.WriteLine(championship.N2Competitors);
                        writer.WriteLine(championship.N4Competitors);
                        writer.WriteLine(championship.E2Competitors);
                        writer.WriteLine(championship.E4Competitors);
                        writer.WriteLine(championship.ProCompetitors);
                        writer.WriteLine(championship.TruckCompetitors);

                        writer.WriteLine("Overall");
                        foreach (var competitor in championship.Overall)
                            writer.WriteLine(ToCsvValues(",", fields, competitor));

                        writer.WriteLine("N2");
                        foreach (var competitor in championship.N2)
                            writer.WriteLine(ToCsvValues(",", fields, competitor));

                        writer.WriteLine("N4");
                        foreach (var competitor in championship.N4)
                            writer.WriteLine(ToCsvValues(",", fields, competitor));

                        writer.WriteLine("E2");
                        foreach (var competitor in championship.E2)
                            writer.WriteLine(ToCsvValues(",", fields, competitor));

                        writer.WriteLine("E4");
                        foreach (var competitor in championship.E4)
                            writer.WriteLine(ToCsvValues(",", fields, competitor));

                        writer.WriteLine("Pro");
                        foreach (var competitor in championship.Pro)
                            writer.WriteLine(ToCsvValues(",", fields, competitor));

                        writer.WriteLine("Truck");
                        foreach (var competitor in championship.Truck)
                            writer.WriteLine(ToCsvValues(",", fields, competitor));

                        writer.Flush();
                    };
                    stream.Close();
                    return true;
                };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return false;
        }

        public static Championship Calculate(List<CompetitorResult> overallCompetitors,
            List<CompetitorResult> n2Competitors,
            List<CompetitorResult> n4Competitors,
            List<CompetitorResult> e2Competitors,
            List<CompetitorResult> e4Competitors,
            List<CompetitorResult> proCompetitors,
            List<CompetitorResult> truckCompetitors)
        {
            // First event of the season
            var championship = new Championship
            {
                Events = 1,
                OverallCompetitors = overallCompetitors.Count(),
                N2Competitors = n2Competitors.Count(),
                N4Competitors = n4Competitors.Count(),
                E2Competitors = e2Competitors.Count(),
                E4Competitors = e4Competitors.Count(),
                ProCompetitors = proCompetitors.Count(),
                TruckCompetitors = truckCompetitors.Count()
            };

            // Overall results
            championship.Overall = new List<CompetitorChampionship>();
            foreach (var competitor in overallCompetitors)
            {
                var competitorChampionship = new CompetitorChampionship()
                {
                    Number = competitor.Number,
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Points = new List<EventPoints>(),
                    Total = competitor.PositionPoints + competitor.CompetitorsInClassPoints + competitor.FastestLapPoints,
                    Penalties = competitor.Penalties
                };
                EventPoints ep = new EventPoints()
                {
                    Position = competitor.PositionPoints,
                    CompetitorsInClass = competitor.CompetitorsInClassPoints,
                    FastestLap = competitor.FastestLapPoints
                };
                competitorChampionship.Points.Add(ep);
                championship.Overall.Add(competitorChampionship);
            };

            // N2 results
            championship.N2 = new List<CompetitorChampionship>();
            foreach (var competitor in n2Competitors)
            {
                var competitorChampionship = new CompetitorChampionship
                {
                    Number = competitor.Number,
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Points = new List<EventPoints>(),
                    Total = competitor.PositionPoints + competitor.CompetitorsInClassPoints + competitor.FastestLapPoints,
                    Penalties = competitor.Penalties
                };
                EventPoints ep = new EventPoints()
                {
                    Position = competitor.PositionPoints,
                    CompetitorsInClass = competitor.CompetitorsInClassPoints,
                    FastestLap = competitor.FastestLapPoints
                };
                competitorChampionship.Points.Add(ep); championship.N2.Add(competitorChampionship);
            };

            // N4 results
            championship.N4 = new List<CompetitorChampionship>();
            foreach (var competitor in n4Competitors)
            {
                var competitorChampionship = new CompetitorChampionship
                {
                    Number = competitor.Number,
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Points = new List<EventPoints>(),
                    Total = competitor.PositionPoints + competitor.CompetitorsInClassPoints + competitor.FastestLapPoints,
                    Penalties = competitor.Penalties
                };
                EventPoints ep = new EventPoints()
                {
                    Position = competitor.PositionPoints,
                    CompetitorsInClass = competitor.CompetitorsInClassPoints,
                    FastestLap = competitor.FastestLapPoints
                };
                competitorChampionship.Points.Add(ep); championship.N4.Add(competitorChampionship);
            };

            // E2 results
            championship.E2 = new List<CompetitorChampionship>();
            foreach (var competitor in e2Competitors)
            {
                var competitorChampionship = new CompetitorChampionship
                {
                    Number = competitor.Number,
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Points = new List<EventPoints>(),
                    Total = competitor.PositionPoints + competitor.CompetitorsInClassPoints + competitor.FastestLapPoints,
                    Penalties = competitor.Penalties
                };
                EventPoints ep = new EventPoints()
                {
                    Position = competitor.PositionPoints,
                    CompetitorsInClass = competitor.CompetitorsInClassPoints,
                    FastestLap = competitor.FastestLapPoints
                };
                competitorChampionship.Points.Add(ep); championship.E2.Add(competitorChampionship);
            };

            // E4 results
            championship.E4 = new List<CompetitorChampionship>();
            foreach (var competitor in e4Competitors)
            {
                var competitorChampionship = new CompetitorChampionship
                {
                    Number = competitor.Number,
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Points = new List<EventPoints>(),
                    Total = competitor.PositionPoints + competitor.CompetitorsInClassPoints + competitor.FastestLapPoints,
                    Penalties = competitor.Penalties
                };
                EventPoints ep = new EventPoints()
                {
                    Position = competitor.PositionPoints,
                    CompetitorsInClass = competitor.CompetitorsInClassPoints,
                    FastestLap = competitor.FastestLapPoints
                };
                competitorChampionship.Points.Add(ep); championship.E4.Add(competitorChampionship);
            };

            // Pro results
            championship.Pro = new List<CompetitorChampionship>();
            foreach (var competitor in proCompetitors)
            {
                var competitorChampionship = new CompetitorChampionship
                {
                    Number = competitor.Number,
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Points = new List<EventPoints>(),
                    Total = competitor.PositionPoints + competitor.CompetitorsInClassPoints + competitor.FastestLapPoints,
                    Penalties = competitor.Penalties
                };
                EventPoints ep = new EventPoints()
                {
                    Position = competitor.PositionPoints,
                    CompetitorsInClass = competitor.CompetitorsInClassPoints,
                    FastestLap = competitor.FastestLapPoints
                };
                competitorChampionship.Points.Add(ep); championship.Pro.Add(competitorChampionship);
            };

            // Truck results
            championship.Truck = new List<CompetitorChampionship>();
            foreach (var competitor in truckCompetitors)
            {
                var competitorChampionship = new CompetitorChampionship
                {
                    Number = competitor.Number,
                    Car = competitor.Car,
                    Name = competitor.Name,
                    Points = new List<EventPoints>(),
                    Total = competitor.PositionPoints + competitor.CompetitorsInClassPoints + competitor.FastestLapPoints,
                    Penalties = competitor.Penalties
                };
                EventPoints ep = new EventPoints()
                {
                    Position = competitor.PositionPoints,
                    CompetitorsInClass = competitor.CompetitorsInClassPoints,
                    FastestLap = competitor.FastestLapPoints
                };
                competitorChampionship.Points.Add(ep); championship.Truck.Add(competitorChampionship);
            };

            return championship;
        }
        public static bool MergeResults(ref Championship championshipPreviousResults, Championship championship)
        {
            // Increment event number
            championshipPreviousResults.Events++;

            //Merge overall results
            championshipPreviousResults.Overall = MergeClassResults(championshipPreviousResults.Overall, championship.Overall, championshipPreviousResults.Events);
            championshipPreviousResults.N2 = MergeClassResults(championshipPreviousResults.N2, championship.N2, championshipPreviousResults.Events);
            championshipPreviousResults.N4 = MergeClassResults(championshipPreviousResults.N4, championship.N4, championshipPreviousResults.Events);
            championshipPreviousResults.E2 = MergeClassResults(championshipPreviousResults.E2, championship.E2, championshipPreviousResults.Events);
            championshipPreviousResults.E4 = MergeClassResults(championshipPreviousResults.E4, championship.E4, championshipPreviousResults.Events);
            championshipPreviousResults.Pro = MergeClassResults(championshipPreviousResults.Pro, championship.Pro, championshipPreviousResults.Events);
            championshipPreviousResults.Truck = MergeClassResults(championshipPreviousResults.Truck, championship.Truck, championshipPreviousResults.Events);

            // Update competitor count
            championshipPreviousResults.OverallCompetitors = championshipPreviousResults.Overall.Count();
            championshipPreviousResults.N2Competitors = championshipPreviousResults.N2.Count();
            championshipPreviousResults.N4Competitors = championshipPreviousResults.N4.Count();
            championshipPreviousResults.E2Competitors = championshipPreviousResults.E2.Count();
            championshipPreviousResults.E4Competitors = championshipPreviousResults.E4.Count();
            championshipPreviousResults.ProCompetitors = championshipPreviousResults.Pro.Count();
            championshipPreviousResults.TruckCompetitors = championshipPreviousResults.Truck.Count();

            // Reorder results based on total points

            return false;
        }

        public static List<CompetitorChampionship> MergeClassResults(List<CompetitorChampionship> previousClassResults, List<CompetitorChampionship> currentClassResults, int events)
        {
            List<int> competitorNumberList = new List<int>();

            bool found = false;
            foreach (var currentResult in currentClassResults)
            {
                competitorNumberList.Add(currentResult.Number);
                int i = 0;
                //var test = from previousResult in championshipPreviousResults where previousResult.Car == currentResult.Car select previousResult.FirstOrDefault();
                while (i < previousClassResults.Count())
                {
                    if (currentResult.Number == previousClassResults[i].Number)
                    {
                        // Names match, so it can proceed
                        if (currentResult.Name == previousClassResults[i].Name)
                        {
                            found = true;
                            break;
                        }
                        else
                        {
                            //Throw message to verify if the competitors are the same
                            var dr = MessageBox.Show("A competitor with same car number was found (#" + currentResult.Number + "). Previous competitor: " + previousClassResults[i].Name + ", current competitor: " + currentResult.Name +
                                ".\nDo you want to merge results?", "Attention required", MessageBoxButtons.YesNo);
                            if(dr == DialogResult.Yes)
                            {
                                // If yes, stop the search and merge the results. Otherwise keep looking until another competitor is found (if found). Duplicated numbers will be created
                                found = true;
                                break;
                            }
                        }
                    }

                    i++;
                }

                if (found)
                {
                    // Found competitor
                    // Add event points to the championship results
                    previousClassResults[i].Points.Add(currentResult.Points.First());
                    // Sum points but remove the worst result
                    previousClassResults[i].Total = CalculateTotalPoints(previousClassResults[i].Points);
                    // Sum the penalties from the current result
                    previousClassResults[i].Penalties += currentResult.Penalties;
                }
                else
                {
                    // Not found. Add another competitor to the results or continue searching (different number?)
                    var eventPoints = new EventPoints();
                    for (int j = 0; j < events - 1;j++)
                        currentResult.Points.Insert(0, eventPoints);

                    previousClassResults.Add(currentResult);
                }
            }

            //Iterate over results list and add null points to competitors that did not attend the current event
            foreach (var currentResult in previousClassResults)
            {
                if (!competitorNumberList.Contains(currentResult.Number))
                {
                    var eventPoints = new EventPoints();
                    for (int j = currentResult.Points.Count(); j < events; j++)
                        currentResult.Points.Add(eventPoints);
                }
            }

            //Reorder list by points
            previousClassResults = previousClassResults.OrderByDescending(o => o.Total).ToList();

            return previousClassResults;
        }

        private static int CalculateTotalPoints(List<EventPoints> eventPoints)
        {
            int totalPoints = 0;
            int count = 0;
            int minimumPoints = int.MaxValue;

            foreach (var points in eventPoints)
            {
                // Skip if is null
                if (!points.Position.HasValue)
                    continue;

                int total = points.Position.Value + points.CompetitorsInClass.Value + points.FastestLap.Value;

                if (total < minimumPoints)
                    minimumPoints = total;

                totalPoints += total;
                count++;
            }
            // Remove the worst result if more than one event and user attended all
            if (count > 1 && count == eventPoints.Count())
                totalPoints -= minimumPoints;

            return totalPoints;
        }

        private static List<CompetitorChampionship> ClassResults(int events, int firstCompetitor, int classCompetitors, string[] lines)
        {
            var classResult = new List<CompetitorChampionship>();
            for (int i = firstCompetitor; i < firstCompetitor + classCompetitors; i++)
            {
                string[] split = lines[i].Split(',');
                var competitorChampionship = new CompetitorChampionship
                {
                    Number = int.Parse(split[0]),
                    Name = split[1],
                    Car = split[2],
                    Points = new List<EventPoints>(),
                    Total = int.Parse(split[events * 3 + 3]),
                    Penalties = int.Parse(split[events * 3 + 4]),
                };
                for (int j = 0; j < events; j++)
                {
                    var eventPoints = new EventPoints
                    {
                        Position = Int32.TryParse(split[3 * j + 3], out int tempVal) ? Int32.Parse(split[3 * j + 3]) : (int?)null,
                        CompetitorsInClass = Int32.TryParse(split[3 * j + 4], out tempVal) ? Int32.Parse(split[3 * j + 4]) : (int?)null,
                        FastestLap = Int32.TryParse(split[3 * j + 5], out tempVal) ? Int32.Parse(split[3 * j + 5]) : (int?)null
                    };
                    competitorChampionship.Points.Add(eventPoints);
                }
                classResult.Add(competitorChampionship);
            }
            return classResult;
        }

        private static string ToCsvValues(string separator, FieldInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();
            bool previousNull = false;

            foreach (var f in fields)
            {
                if (linie.Length > 0 || previousNull)
                    linie.Append(",");

                var x = f.GetValue(o);
                if (x != null)
                {
                    previousNull = false;

                    if (x.GetType().IsGenericType)
                    {
                        // Object is a list
                        // It is assumed that is an EventPoints list. If other type is parsed, it will break
                        bool multiple = false;
                        foreach(var points in x as IList<EventPoints>)
                        {
                            if (multiple) 
                                linie.Append(",");

                            Type t = points.GetType();
                            FieldInfo[] epFields = t.GetFields(BindingFlags.Instance |
                               BindingFlags.Static |
                               BindingFlags.NonPublic |
                               BindingFlags.Public);
                            linie.Append(ToCsvValues(",", epFields, points));
                            multiple = true;
                        }

                    }
                    else
                    {
                        linie.Append(x.ToString());
                    }
                }
                else
                {
                    previousNull = true;
                }

            }

            return linie.ToString();
        }
    }
}