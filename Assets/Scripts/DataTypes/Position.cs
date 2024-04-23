using System;
using Newtonsoft.Json;

namespace DataTypes
{
    /// <summary>
    /// Personal 2d vector object
    /// </summary>
    public class Position
    {
        /// <summary>
        /// The row position of the position
        /// </summary>
        public int Row { get; private set; }
        /// <summary>
        /// The col position of the position
        /// </summary>
        public int Col { get; private set; }

        /// <summary>
        /// Creates a new position
        /// </summary>
        /// <param name="row">The row of the position</param>
        /// <param name="col">The col of the position</param>
        [JsonConstructor()]
        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        /// <summary>
        /// Copy the position
        /// </summary>
        /// <param name="pos">The position to copy</param>
        public Position(Position pos)
        {
            Row = pos.Row;
            Col = pos.Col;
        }

        /// <summary>
        /// Change the row of the position
        /// </summary>
        /// <param name="row">What to change it into</param>
        public void ChangeRow(int row)
        {
            this.Row = row;
        }

        /// <summary>
        /// Change the col of the position
        /// </summary>
        /// <param name="row">What to change it into</param>
        public void ChangeCol(int col)
        {
            this.Col = col;
        }

        /// <summary>
        /// Changes the position's row and column at the same time
        /// </summary>
        /// <param name="row">What row to change it</param>
        /// <param name="col">What col to change it</param>
        public void Change(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }

        /// <summary>
        /// Calculate the distance to an another position
        /// </summary>
        /// <param name="otherPos">The other position</param>
        /// <returns>The distance</returns>
        public float CalcDistanceFrom(Position otherPos)
        {
            return MathF.Sqrt(MathF.Pow(otherPos.Row - this.Row, 2) + MathF.Pow(otherPos.Col - this.Col, 2));
        }

        /// <summary>
        /// Creates a copy of the position with modified row and col according to the move direction
        /// </summary>
        /// <param name="pos">The position to move</param>
        /// <param name="dir">The direction to move it</param>
        /// <returns>The moved copy</returns>
        public static Position CreateCopyAndMoveDir(Position pos, Direction dir)
        {
            Position newPos = new Position(pos);
            newPos.AddToDir(dir);
            return newPos;
        }

        /// <summary>
        /// Moves the position with modified row and col according to the move direction
        /// </summary>
        /// <param name="dir">The direction to move it</param>
        /// <returns>This position</returns>
        public Position AddToDir(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    this.Col--;
                    break;

                case Direction.Up:
                    this.Row--;
                    break;

                case Direction.Right:
                    this.Col++;
                    break;

                case Direction.Down:
                    this.Row++;
                    break;

                case Direction.None:
                    break;

                default:
                    break;
            }

            return this;
        }

        /// <summary>
        /// Calculate the distance between two positions
        /// </summary>
        /// <param name="pos1">First position</param>
        /// <param name="pos2">Second position</param>
        /// <returns>The distance</returns>
        public static float CalcDistanceTo(Position pos1, Position pos2)
        {
            return MathF.Sqrt(MathF.Pow(pos1.Row - pos2.Row, 2) + MathF.Pow(pos1.Col - pos2.Col, 2));
        }

        /// <summary>
        /// Check if two positions are equals
        /// </summary>
        /// <param name="other">The other position to compare to</param>
        /// <returns>true-if row and col are the same, false-otherwise</returns>
        public override bool Equals(object other)
        {
            if (other is Position)
            {
                Position otherPos = (Position)other;

                return this.Row == otherPos.Row && this.Col == otherPos.Col;
            }

            return false;
        }

        /// <summary>
        /// Gets the HashCode of the position
        /// </summary>
        /// <returns>The position's hashcode</returns>
        public override int GetHashCode()
        {
            return Row * 100 + Col;
        }
    }
}