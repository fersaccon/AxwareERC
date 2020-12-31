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

                //Skip one line then reads overall results based on number of overall competitors
                if (championshipResult.OverallCompetitors > 0)
                {
                    championshipResult.Overall = new List<CompetitorChampionship>();

                    var data = from l in lines.Skip(9).Take(championshipResult.OverallCompetitors)
                               let split = l.Split(',')
                               select new CompetitorChampionship
                               {
                                   Number = int.Parse(split[0]),
                                   Name = split[1],
                                   Car = split[2],
                                   //Add values to Points array depending on how many events the file has results for
                                   Points = new int[3] { int.Parse(split[3]), int.Parse(split[4]), int.Parse(split[5])},
                                   Total = int.Parse(split[championshipResult.Events * 3 + 3])
                               };

                    championshipResult.Overall.AddRange(data);
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
                        PropertyInfo[] properties = typeof(CompetitorChampionship).GetProperties();

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
                    if (x.GetType().IsArray)
                    {
                        //If object is array, split elements first
                        string[] strArray = Array.ConvertAll((int[])x, ele => ele.ToString());
                        linie.Append(string.Join(separator, strArray));
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