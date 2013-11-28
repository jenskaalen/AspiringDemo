namespace AspiringDemo
{
    public struct Vector2
    {
        public Vector2(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static int operator -(Vector2 toSubtract, Vector2 subtracer)
        {
            return 0;
        }
    }
}