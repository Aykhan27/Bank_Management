namespace kursac
{
    class Rect
    {
        public int left;
        public int width;
        public int top;
        public int height;

        public Rect(int _left = 0, int _top = 0, int _width = 0, int _height = 0)
        {
            left = _left;
            width = _width;
            top = _top;
            height = _height;
        }

        public Rect ChangeTop(int _top)
        {
            return new Rect(left, top + _top, width, height);
        }

        public Rect ChangeLeft(int _left)
        {
            return new Rect(left + _left, top, width, height);
        }

        public Rect ChangeHeight(int _height)
        {
            return new Rect(left, top, width, height + _height);
        }

        public Rect ChangeWidth(int _width)
        {
            return new Rect(left, top, width + _width, height);
        }
    }
}