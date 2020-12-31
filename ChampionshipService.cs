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
                    //First competitor on Overall class is on line # 9
                    for (int i = 9; i < championshipResult.OverallCompetitors + 9; i++)
                    {
                        string[] split = lines[i].Split(',');
                        var competitorChampionship = new CompetitorChampionship
                        {
                            Number = int.Parse(split[0]),
                            Name = split[1],
                            Car = split[2],
                            Points = new List<EventPoints>(),
                            Total = int.Parse(split[championshipResult.Events * 3 + 3])
                        };
                        for (int j = 0; j < championshipResult.Events; j++)
                        {
                            var eventPoints = new EventPoints
                            {
                                Position = int.Parse(split[3 * j + 3]),
                                CompetitorsInClass = int.Parse(split[3 * j + 4]),
                                FastestLap= int.Parse(split[3 * j + 5])
                            };
                            competitorChampionship.Points.Add(eventPoints);
                        }
                        championshipResult.Overall.Add(competitorChampionship);
                    }
                 }

                //Skip one line then reads N2 results based on number of Overall competitors
                int firstCompetitor = 9 + championshipResult.OverallCompetitors + 1;
                if (championshipResult.N2Competitors > 0)
                {
                    championshipResult.N2 = new List<CompetitorChampionship>();
                    //First competitor on N2 class is on line # 9 + OverallCompetitors + 1
                    for (int i = firstCompetitor; i < firstCompetitor + championshipResult.N2Competitors; i++)
                    {
                        string[] split = lines[i].Split(',');
                        var competitorChampionship = new CompetitorChampionship
                        {
                            Number = int.Parse(split[0]),
                            Name = split[1],
                            Car = split[2],
                            Points = new List<EventPoints>(),
                            Total = int.Parse(split[championshipResult.Events * 3 + 3])
                        };
                        for (int j = 0; j < championshipResult.Events; j++)
                        {
                            var eventPoints = new EventPoints
                            {
                                Position = int.Parse(split[3 * j + 3]),
                                CompetitorsInClass = int.Parse(split[3 * j + 4]),
                                FastestLap = int.Parse(split[3 * j + 5])
                            };
                            competitorChampionship.Points.Add(eventPoints);
                        }
                        championshipResult.N2.Add(competitorChampionship);
                    }
                }

                //Skip one line then reads N4 results based on number of N2 competitors
                firstCompetitor += championshipResult.N2Competitors + 1;
                if (championshipResult.N4Competitors > 0)
                {
                    championshipResult.N4 = new List<CompetitorChampionship>();
                    //First competitor on N2 class is on line # 9 + OverallCompetitors + 1
                    for (int i = firstCompetitor; i < firstCompetitor + championshipResult.N4Competitors; i++)
                    {
                        string[] split = lines[i].Split(',');
                        var competitorChampionship = new CompetitorChampionship
                        {
                            Number = int.Parse(split[0]),
                            Name = split[1],
                            Car = split[2],
                            Points = new List<EventPoints>(),
                            Total = int.Parse(split[championshipResult.Events * 3 + 3])
                        };
                        for (int j = 0; j < championshipResult.Events; j++)
                        {
                            var eventPoints = new EventPoints
                            {
                                Position = int.Parse(split[3 * j + 3]),
                                CompetitorsInClass = int.Parse(split[3 * j + 4]),
                                FastestLap = int.Parse(split[3 * j + 5])
                            };
                            competitorChampionship.Points.Add(eventPoints);
                        }
                        championshipResult.N4.Add(competitorChampionship);
                    }
                }

                //Skip one line then reads E2 results based on number of N4 competitors
                firstCompetitor += championshipResult.N4Competitors + 1;
                if (championshipResult.E2Competitors > 0)
                {
                    championshipResult.E2 = new List<CompetitorChampionship>();
                    //First competitor on E2 class is after firstN4Competitor + championshipResult.N4Competitors + 1
                    for (int i = firstCompetitor; i < firstCompetitor + championshipResult.E2Competitors; i++)
                    {
                        string[] split = lines[i].Split(',');
                        var competitorChampionship = new CompetitorChampionship
                        {
                            Number = int.Parse(split[0]),
                            Name = split[1],
                            Car = split[2],
                            Points = new List<EventPoints>(),
                            Total = int.Parse(split[championshipResult.Events * 3 + 3])
                        };
                        for (int j = 0; j < championshipResult.Events; j++)
                        {
                            var eventPoints = new EventPoints
                            {
                                Position = int.Parse(split[3 * j + 3]),
                                CompetitorsInClass = int.Parse(split[3 * j + 4]),
                                FastestLap = int.Parse(split[3 * j + 5])
                            };
                            competitorChampionship.Points.Add(eventPoints);
                        }
                        championshipResult.E2.Add(competitorChampionship);
                    }
                }

                //Skip one line then reads E4 results based on number of E2 competitors
                firstCompetitor += championshipResult.E2Competitors + 1;
                if (championshipResult.E4Competitors > 0)
                {
                    championshipResult.E4 = new List<CompetitorChampionship>();
                    for (int i = firstCompetitor; i < firstCompetitor + championshipResult.E4Competitors; i++)
                    {
                        string[] split = lines[i].Split(',');
                        var competitorChampionship = new CompetitorChampionship
                        {
                            Number = int.Parse(split[0]),
                            Name = split[1],
                            Car = split[2],
                            Points = new List<EventPoints>(),
                            Total = int.Parse(split[championshipResult.Events * 3 + 3])
                        };
                        for (int j = 0; j < championshipResult.Events; j++)
                        {
                            var eventPoints = new EventPoints
                            {
                                Position = int.Parse(split[3 * j + 3]),
                                CompetitorsInClass = int.Parse(split[3 * j + 4]),
                                FastestLap = int.Parse(split[3 * j + 5])
                            };
                            competitorChampionship.Points.Add(eventPoints);
                        }
                        championshipResult.E4.Add(competitorChampionship);
                    }
                }

                //Skip one line then reads Pro results based on number of E4 competitors
                firstCompetitor += championshipResult.E4Competitors + 1;
                if (championshipResult.ProCompetitors > 0)
                {
                    championshipResult.Pro = new List<CompetitorChampionship>();
                    //First competitor on E2 class is after firstN4Competitor + championshipResult.N4Competitors + 1
                    for (int i = firstCompetitor; i < firstCompetitor + championshipResult.ProCompetitors; i++)
                    {
                        string[] split = lines[i].Split(',');
                        var competitorChampionship = new CompetitorChampionship
                        {
                            Number = int.Parse(split[0]),
                            Name = split[1],
                            Car = split[2],
                            Points = new List<EventPoints>(),
                            Total = int.Parse(split[championshipResult.Events * 3 + 3])
                        };
                        for (int j = 0; j < championshipResult.Events; j++)
                        {
                            var eventPoints = new EventPoints
                            {
                                Position = int.Parse(split[3 * j + 3]),
                                CompetitorsInClass = int.Parse(split[3 * j + 4]),
                                FastestLap = int.Parse(split[3 * j + 5])
                            };
                            competitorChampionship.Points.Add(eventPoints);
                        }
                        championshipResult.Pro.Add(competitorChampionship);
                    }
                }

                //Skip one line then reads Truck results based on number of Pro competitors
                firstCompetitor += championshipResult.ProCompetitors + 1;
                if (championshipResult.TruckCompetitors > 0)
                {
                    championshipResult.Truck = new List<CompetitorChampionship>();
                    for (int i = firstCompetitor; i < firstCompetitor + championshipResult.TruckCompetitors; i++)
                    {
                        string[] split = lines[i].Split(',');
                        var competitorChampionship = new CompetitorChampionship
                        {
                            Number = int.Parse(split[0]),
                            Name = split[1],
                            Car = split[2],
                            Points = new List<EventPoints>(),
                            Total = int.Parse(split[championshipResult.Events * 3 + 3])
                        };
                        for (int j = 0; j < championshipResult.Events; j++)
                        {
                            var eventPoints = new EventPoints
                            {
                                Position = int.Parse(split[3 * j + 3]),
                                CompetitorsInClass = int.Parse(split[3 * j + 4]),
                                FastestLap = int.Parse(split[3 * j + 5])
                            };
                            competitorChampionship.Points.Add(eventPoints);
                        }
                        championshipResult.Truck.Add(competitorChampionship);
                    }
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
                };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return false;
        }

        private static string ToCsvValues(string separator, FieldInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();

            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(",");

                var x = f.GetValue(o);

                if (x != null)
                {
                    if (x.GetType().IsGenericType)
                    {
                        // Object is a list
                        // It is assumed that is an EventPoints list. If other type is parsed, it will break
                        foreach(var points in x as IList<EventPoints>)
                        {
                            Type t = points.GetType();
                            FieldInfo[] epFields = t.GetFields(BindingFlags.Instance |
                               BindingFlags.Static |
                               BindingFlags.NonPublic |
                               BindingFlags.Public);
                            var test = ToCsvValues(",", epFields, points);
                            linie.Append(test);
                        }

                    }
                    else
                    {
                        linie.Append(x.ToString());
                    }
                }

            }

            return linie.ToString();
        }
    }
}