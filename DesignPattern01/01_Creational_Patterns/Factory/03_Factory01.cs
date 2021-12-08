using System;

namespace Factory01
{
    public abstract class Shape
    {
        public abstract double GetArea();
    }

    public abstract class ShapeCreator
    {
        public abstract Shape Create();
    }
}