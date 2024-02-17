using System;

public class Position
{
    public int Row { get; private set; }
    public int Col { get; private set; }

    public float CalcDistanceFrom(Position otherPos)
    {
        return MathF.Sqrt(MathF.Pow(otherPos.Row - this.Row, 2) + MathF.Pow(otherPos.Col - this.Col, 2));
    }

    public static float CalcDistanceTo(Position pos1, Position pos2)
    {
        return MathF.Sqrt(MathF.Pow(pos1.Row - pos2.Row, 2) + MathF.Pow(pos1.Col - pos2.Col, 2));
    }
}