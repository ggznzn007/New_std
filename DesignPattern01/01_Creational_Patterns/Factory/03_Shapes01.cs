using Factory01;
using System;

public class Circle : Shape
{
    public double Radius;

    public override double GetArea()
    {
        return 2 * Math.PI * Radius;
    }  
}

public class Triangle : Shape
{
    public override double GetArea()
    {
        return 0;
    }
}

public class CircleCreator : ShapeCreator
{
    public override Shape Create()
    {
        return new Circle();
    }
}

public class TriangleCreator : ShapeCreator
{
    public override Shape Create()
    {
        return new Triangle();
    }
}