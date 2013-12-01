namespace AspiringDemo.Gamecore.Types
{
    public class Rect
    {
        public int X1 { get; private set; }
        public int Y1 { get; private set; }
        public int X2 { get; private set; }
        public int Y2 { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Vector2 Center { get; private set; }

        public Rect(int x1Pos, int y1Pos, int height, int width)
        {
            X1 = x1Pos;
            Y1 = y1Pos;
            Height = height;
            Width = width;
            //X2 = CappedValue(500, X1 + Width);
            //Y2 = CappedValue(500, Y1 + Height);
            X2 = X1 + Width;
            Y2 = Y1 + height;
            var xCenter = (X1 + X2) / 2;
            var yCenter = (Y1 + Y2) / 2;
            Center = new Vector2(xCenter, yCenter);
        }

        public bool Contains(Rect rect)
        {
            return X1 <= rect.X2 && X2 >= rect.X1
                && Y1 <= rect.Y2 && Y2 >= rect.Y1;
        }

        public bool Contains(Vector2 pos)
        {
            return (pos.X > X1 && pos.X < X2) && (pos.Y > Y1 && pos.Y < Y2);
        }

        private int CappedValue(int maxValue, int value)
        {
            return value > maxValue ? maxValue : value;
        }
    }
}
