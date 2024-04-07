using System;

namespace DataTypes
{
    public class Position
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void ChangeRow(int row)
        {
            this.Row = row;
        }

        public void ChangeCol(int col)
        {
            this.Col = col;
        }

        public void Change(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        public float CalcDistanceFrom(Position otherPos)
        {
            return MathF.Sqrt(MathF.Pow(otherPos.Row - this.Row, 2) + MathF.Pow(otherPos.Col - this.Col, 2));
        }

        public static float CalcDistanceTo(Position pos1, Position pos2)
        {
            return MathF.Sqrt(MathF.Pow(pos1.Row - pos2.Row, 2) + MathF.Pow(pos1.Col - pos2.Col, 2));
        }

        public override bool Equals(object other)
        {
            if (other is Position)
            {
                Position otherPos = (Position)other;

                return this.Row == otherPos.Row && this.Col == otherPos.Col;
            }

            return false;
        }
        public override int GetHashCode()
        {
            return Row*100+Col;
        }

    }
}