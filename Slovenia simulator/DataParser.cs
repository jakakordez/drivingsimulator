using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using OpenTK;
using Newtonsoft.Json;

namespace Slovenia_simulator
{
    class DataParser
    {
        public static void ParseData(string path, object obj)
        {
            
            switch (path.Split('.')[1])
            {
                case "dat":
                    string[] file = File.ReadAllLines(path);
                    for (int i = 0; i < file.Length; i++)
                    {
                        if (file[i] != "" && file[i][0] != '#' && file[i].Contains('='))
                        {
                            string[] line = file[i].Replace(" ", "").Split('=');
                            if (obj.GetType().GetField(line[0]) != null)
                            {

                                Type t = obj.GetType().GetField(line[0]).FieldType;
                                obj.GetType().GetField(line[0]).SetValue(obj, parseValue(line[1], t));
                            }
                        }
                    }
                    break;
                case "json":
                    string json = File.ReadAllText(path);
                    JsonConvert.PopulateObject(json, obj);
                    break;
            }
            
        }

        private static object parseValue(string v, Type t)
        {
            object value = null;
            if (t == typeof(string)) value = v;
            else if (t == typeof(int)) value = Misc.toInt(v);
            else if (t == typeof(float)) value = Misc.toFloat(v);
            else if (t == typeof(bool)) value = Convert.ToBoolean(v);
            else if (t == typeof(Vector2))
            {
                string[] p = v.Split(',');
                value = new Vector2(Misc.toFloat(p[0]), Misc.toFloat(p[1]));
            }
            else if (t == typeof(Vector3))
            {
                string[] p = v.Split(',');
                value = new Vector3(Misc.toFloat(p[0]), Misc.toFloat(p[1]), Misc.toFloat(p[2]));
            }
            else if (t == typeof(Vector2[]))
            {
                string[] points = v.Split(';');
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
            return value;
        }
    }
}
