using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace kyrsovoy_kiselev
{
    class Captcha
    {
        public Bitmap CreateImage(int Width, int Height, ref String str)
        {
            Bitmap result = new Bitmap(Width, Height);
            int Xpos = 10;
            int Ypos = 10;
            Brush[] colors = { Brushes.Black, Brushes.Red, Brushes.RoyalBlue, Brushes.Green, Brushes.Yellow, Brushes.White, Brushes.Pink };

            Pen[] colorpens = { Pens.Black, Pens.Red, Pens.Green, Pens.RoyalBlue, Pens.White, Pens.Pink, Pens.Yellow };

            FontStyle[] fontstyle = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular, FontStyle.Strikeout, FontStyle.Underline };

            Int16[] rotate = { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5, 6, -6 };

            Graphics g = Graphics.FromImage((Image)result);

            g.Clear(Color.Gray);

            Random rnd = new Random();
            g.RotateTransform(rnd.Next(rotate.Length));

            str = "";
            int indicator = 0;
            for (int i = 0; i < 4; i++)
            {
                indicator = rnd.Next(0, 3);
                switch (indicator)
                {
                    case 0:
                        str += rnd.Next(0, 10).ToString();
                        break;
                    case 1:
                        str += (Char)rnd.Next('a', 'z');
                        break;
                    case 2:
                        str += (Char)rnd.Next('A', 'Z');
                        break;

                }
            }
            g.DrawString(str, new Font("Arial", 25, fontstyle[rnd.Next(fontstyle.Length)]), colors[rnd.Next(colors.Length)], new PointF(Xpos, Ypos));

            g.DrawLine(colorpens[rnd.Next(colorpens.Length)],
                new Point(0, 0),
                new Point(Width - 1, Height - 1));

            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);
            return result;
        }
    }
}
