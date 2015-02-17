using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Terrain_compiler
{
    class Terrain
    {
        float[,] data;
        public float min, max;
        int w;
        int h;
        public Terrain(string filename)
        {
            w = Properties.Settings.Default.Width;
            h = Properties.Settings.Default.Length;
            data = new float[w, h];
            string del = Properties.Settings.Default.Delimiter;
            if (Properties.Settings.Default.Format == "ASCII")
            {
                string file = File.ReadAllText(filename);
                string[] nodes = file.Replace(del+del, del).Split(del.ToCharArray());
                for (int i = 0; i < nodes.Length; i++)
                {
                    try
                    {
                        data[i % w, i / h] = (float)Convert.ToDouble(nodes[i], System.Globalization.CultureInfo.InvariantCulture);
                        if (data[i % w, i / h] < min) min = data[i % w, i / h];
                        if (data[i % w, i / h] > max) max = data[i % w, i / h];
                    }
                    catch { }
                }
            }
            else
            {

            }
        }

        public Bitmap GenerateBitmap()
        {
            Bitmap b = new Bitmap(w, h);

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    b.SetPixel(i, j, GetRgbValues(min, max, data[i, j]));
                }
            }

            return b;
        }

        public Color GetColor(float value)
        {
            if (value < 0) return Color.CornflowerBlue;
            Int32 v = (Int32)(value / (max) * 16581375);
            return Color.FromArgb((v >> 16)&0xFF, (v>>8)&0xFF, v&0xFF);
           
            if (value < 200) return Color.DarkGreen;
            if (value < 500) return Color.Wheat;
            if (value < 800) return Color.Yellow;
            if (value < 1000) return Color.DarkRed;
            return Color.Gray;
        }

        public Color GetRgbValues(float minimumValue, float maximumValue, float value)
        {
            if (value < 0) return Color.CornflowerBlue;
            minimumValue = 0;
            var halfmax = (minimumValue + maximumValue) / 2.0;
            int b = (int)Math.Max(0.0, 255.0 * (1.0 - value / halfmax));
            int r = (int)Math.Max(0.0, 255.0 * (value / halfmax - 1.0));
            int g = 255 - b - r;

            return Color.FromArgb(r, g, b);
        }

        static double Lerp(double value1, double value2, double amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public void Export(string filename)
        {
            BinaryWriter b = new BinaryWriter(new FileStream(filename, FileMode.Create));
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    //b.Write((float)Lerp(data[i, (int)counter], data[i, (int)counter + 1], counter - Math.Floor(counter)));
                    b.Write((float)data[i, j]);
                }
            }
            b.Flush();
        }
    }
}
