using AxwareERC.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AxwareERC
{
    public static class CompetitorService
    {
        public static List<CompetitorAxware> ReadFile(string filepath)
        {
            var lines = File.ReadAllLines(filepath);
            var data = from l in lines.Skip(1)
                       let split = l.Split('\t')
                       select new CompetitorAxware
                       {
                           Class = (CompetitionClass)Enum.Parse(typeof(CompetitionClass), split[0].ToLower()),
                           Number = int.Parse(split[1]),
                           Name = split[2] + " " + split[3],
                           //FirstName = split[2],
                           //LastName = split[3],
                           Car = split[4],
                           //CarColor = split[5],
                           //Bumped = split[7] == "Yes" ? true : false,
                           //Class2 = string.IsNullOrEmpty(split[8])?null:(CompetitionClass)Enum.Parse(typeof(CompetitionClass), split[8]),
                           //GridNumber = int.Parse(split[9]),
                           //Member = split[10] == "Yes" ? true : false,
                           TotalTime = split[37],
                           //RawPosition = int.Parse(split[38]),
                           Run1 = split.Count() > 45 ? split[45] : "",
                           Pen1 = split.Count() > 46 ? split[46] : "",
                           Run2 = split.Count() > 47 ? split[47] : "",
                           Pen2 = split.Count() > 48 ? split[48] : "",
                           Run3 = split.Count() > 49 ? split[49] : "",
                           Pen3 = split.Count() > 50 ? split[50] : "",
                           Run4 = split.Count() > 51 ? split[51] : "",
                           Pen4 = split.Count() > 52 ? split[52] : "",
                           Run5 = split.Count() > 53 ? split[53] : "",
                           Pen5 = split.Count() > 54 ? split[54] : "",
                           Run6 = split.Count() > 55 ? split[55] : "",
                           Pen6 = split.Count() > 56 ? split[56] : "",
                           Run7 = split.Count() > 57 ? split[57] : "",
                           Pen7 = split.Count() > 58 ? split[58] : "",
                           Run8 = split.Count() > 59 ? split[59] : "",
                           Pen8 = split.Count() > 60 ? split[60] : "",
                           Run9 = split.Count() > 61 ? split[61] : "",
                           Pen9 = split.Count() > 62 ? split[62] : "",
                           Run10 = split.Count() > 63 ? split[63] : "",
                           Pen10 = split.Count() > 64 ? split[64] : "",
                           Run11 = split.Count() > 65 ? split[65] : "",
                           Pen11 = split.Count() > 66 ? split[66] : "",
                           Run12 = split.Count() > 67 ? split[67] : "",
                           Pen12 = split.Count() > 68 ? split[68] : "",
                           Run13 = split.Count() > 69 ? split[69] : "",
                           Pen13 = split.Count() > 70 ? split[70] : "",
                           Run14 = split.Count() > 71 ? split[71] : "",
                           Pen14 = split.Count() > 72 ? split[72] : "",
                           Run15 = split.Count() > 73 ? split[73] : "",
                           Pen15 = split.Count() > 74 ? split[74] : "",
                           Run16 = split.Count() > 75? split[75] : "",
                           Pen16 = split.Count() > 76? split[76] : "",
                           Run17 = split.Count() > 77? split[77] : "",
                           Pen17 = split.Count() > 78? split[78] : "",
                           Run18 = split.Count() > 79? split[79] : "",
                           Pen18 = split.Count() > 80? split[80] : "",
                           Run19 = split.Count() > 81? split[81] : "",
                           Pen19 = split.Count() > 82? split[82] : "",
                           Run20 = split.Count() > 83? split[83] : "",
                           Pen20 = split.Count() > 84? split[84] : "",

                       };

            return data.ToList();
        }

        public static bool WriteResultsFile ( string filepath, List<CompetitorResult> overallResults, 
            List<CompetitorResult> n2Results, List<CompetitorResult> n4Results, List<CompetitorResult> e2Results, List<CompetitorResult> e4Results,
            List<CompetitorResult> proResults, List<CompetitorResult> truckResults)
        {
            if (overallResults.Count > 0)
            {
                try
                {
                    using (Stream stream = File.OpenWrite(filepath))
                    {
                        Type t = typeof(CompetitorResult);
                        FieldInfo[] fields = t.GetFields(BindingFlags.Instance |
                           BindingFlags.Static |
                           BindingFlags.NonPublic |
                           BindingFlags.Public);
                        PropertyInfo[] properties = typeof(CompetitorResult).GetProperties();

                        stream.SetLength(0);
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            // Write field names
                            writer.WriteLine("Overall");
                            string header = String.Join(",", properties.Select(p => p.Name).ToArray());
                            writer.WriteLine(header);
                            // loop through each row of our DataGridView
                            foreach (CompetitorResult row in overallResults)
                                writer.WriteLine(ToCsvValues(",", fields, row));

                            if (n2Results.Count > 0)
                            {
                                writer.WriteLine("Novice 2WD");
                                writer.WriteLine(header);
                                foreach (CompetitorResult row in n2Results)
                                    writer.WriteLine(ToCsvValues(",", fields, row));
                            }

                            if (n4Results.Count > 0)
                            {
                                writer.WriteLine("Novice 4WD");
                                writer.WriteLine(header);
                                foreach (CompetitorResult row in n4Results)
                                    writer.WriteLine(ToCsvValues(",", fields, row));
                            }
                            if (e2Results.Count > 0)
                            {
                                writer.WriteLine("Expert 2WD");
                                writer.WriteLine(header);
                                foreach (CompetitorResult row in e2Results)
                                    writer.WriteLine(ToCsvValues(",", fields, row));
                            }
                            if (e4Results.Count > 0)
                            {
                                writer.WriteLine("Expert 4WD");
                                writer.WriteLine(header);
                                foreach (CompetitorResult row in e4Results)
                                    writer.WriteLine(ToCsvValues(",", fields, row));
                            }
                            if (proResults.Count > 0)
                            {
                                writer.WriteLine("Pro");
                                writer.WriteLine(header);
                                foreach (CompetitorResult row in proResults)
                                    writer.WriteLine(ToCsvValues(",", fields, row));
                            }
                            if (truckResults.Count > 0)
                            {
                                writer.WriteLine("Truck");
                                writer.WriteLine(header);
                                foreach (CompetitorResult row in truckResults)
                                    writer.WriteLine(ToCsvValues(",", fields, row));
                            }
                            writer.Flush();
                        }
                    };

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
                return true;
            }
            return false;
        }

        private static string ToCsvValues(string separator, FieldInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();

            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append(x.ToString());
            }

            return linie.ToString();
        }
    }
}
