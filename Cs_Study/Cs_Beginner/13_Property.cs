using System;

namespace _Propt
{
    class Rectangle
    {
        private double width;
        private double height;

        public double GetWidth() // Getter
        { return width; }

        public double GetHeight()
        { return height; }

        public void SetWidth(double width) // Setter
        {
            if (width > 0)
                this.width = width;
        }

        public void SetHeight(double height)
        {
            if (height > 0)
                this.height = height;
        }


    }
}