using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Slovenia_simulator
{
    class DataParser
    {
        public static void ParseData(string[] file, object obj)
        {
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] != "" && file[i][0] != '#' && file[i].Contains('='))
                {
                    try
                    {
                        string[] line = file[i].Replace(" ", "").Split('=');
                        Type t = obj.GetType().GetField(line[0]).FieldType;
                        object value = null;
                        if (t == typeof(string)) value = line[1];
                        else if (t == typeof(int)) value = Misc.toInt(line[1]);
                        else if (t == typeof(float)) value = Misc.toFloat(line[1]);
                        else if (t == typeof(bool)) value = Convert.ToBoolean(line[1]);
                        else if (t == typeof(Vector2[]))
                        {
                            string[] points = line[1].Split(';');
                            Vector2[] r = new Vector2[0];
                            for (int j = 0; j < points.Length; j++)
                            {
                                if (points[j].Contains(':'))
                                {
                                    string[] tocka = points[j].Split(':');
                                    Misc.Push<Vector2>(new Vector2(Misc.toFloat(tocka[0]), Misc.toFloat(tocka[1])), ref r);
                                }
                            }
                            value = r;
                        }
                        obj.GetType().GetField(line[0]).SetValue(obj, value);
                    }
                    catch { }
                }
            }
        }
    }
}
