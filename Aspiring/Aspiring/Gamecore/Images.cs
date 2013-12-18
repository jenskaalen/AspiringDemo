using AspiringDemo.Gamecore.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspiringDemo.Gamecore
{
    public static class Images
    {
        public static void DrawLine(Bitmap bmp, int sizeMultiplier,  Vector2 posStart, Vector2 posEnd)
        {
            Pen blackPen = new Pen(Color.GreenYellow, 1);

            int x1 = posStart.X * sizeMultiplier;
            int y1 = posStart.Y * sizeMultiplier;
            int x2 = posEnd.X * sizeMultiplier;
            int y2 = posEnd.Y * sizeMultiplier;
            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawLine(blackPen, x1, y1, x2, y2);
            }
        }

        public static void DrawRect(Bitmap bmp, int xStart, int yStart, int xEnd, int yEnd)
        {
            Pen blackPen = new Pen(Color.Black, 1);
            Rectangle rect = new Rectangle(xStart, yStart, xEnd, yEnd);

            var brush = new SolidBrush(Color.Black);
            //brush.Color = Color.Black;

            //var rect = new System.Windows.Shapes.Rectangle()
            //{
            //    Stroke = Brushes.LightBlue,
            //    StrokeThickness = 1,
            //    Fill = brush
            //};



            // Draw line to screen.
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.FillRectangle(brush, xStart, yStart, xEnd, yEnd);
                //graphics.DrawRectangle(blackPen, rect);
            }
        }

    }
}
