using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bomberman;
using DataTypes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PositionTests
    {
        [Test]
        public void PositionEqualsTest()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(1, 1);

            Assert.IsTrue(pos1.Equals(pos2));
            Assert.AreEqual(pos1.GetHashCode(), pos2.GetHashCode());
        }

        [Test]
        public void PositionEqualsTest2()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(1, 2);

            Assert.IsFalse(pos1.Equals(pos2));
            Assert.AreNotEqual(pos1.GetHashCode(), pos2.GetHashCode());

            pos2.ChangeCol(1);
            Assert.IsTrue(pos1.Equals(pos2));
            Assert.AreEqual(pos1.GetHashCode(), pos2.GetHashCode());

            pos1.ChangeRow(3);
            Assert.IsFalse(pos1.Equals(pos2));
            Assert.AreNotEqual(pos1.GetHashCode(), pos2.GetHashCode());
        }

        [Test]
        public void PositionDistanceTest()
        {
            Position pos1 = new Position(1, 1);
            Position pos2 = new Position(1, 2);
            float firstDistance = pos1.CalcDistanceFrom(pos2);
            float secondDistance = Position.CalcDistanceTo(pos1, pos2);

            Assert.AreEqual(firstDistance, secondDistance);
        }

        [Test]
        public void PositionDirectionTest()
        {
            Position pos1 = new Position(1, 1);

            Position movedPos1 = Position.CreateCopyAndMoveDir(pos1,Direction.Left);

            Assert.AreEqual(pos1.Col-1, movedPos1.Col);
            Assert.IsFalse(pos1==movedPos1);

            Position pos2 = new Position(1, 6);

            Position movedPos2 = Position.CreateCopyAndMoveDir(pos2, Direction.Up);

            Assert.AreEqual(pos1.Row - 1, movedPos2.Row);

            Position movedPos3 = Position.CreateCopyAndMoveDir(pos1, Direction.Right);

            Assert.AreEqual(pos1.Col + 1, movedPos3.Col);

            Position movedPos4 = Position.CreateCopyAndMoveDir(pos1, Direction.Down);
            Assert.AreEqual(pos1.Row + 1, movedPos4.Row);
        }

        [Test]
        public void PositionDirectionTest2()
        {
            Position pos1 = new Position(1, 1);

            Position pos2 = new Position(pos1);

            Assert.AreEqual(pos1,pos2);

            Assert.IsFalse(pos1==pos2);

            pos1.Change(0,0);

            Assert.AreNotEqual(pos1,pos2);

            pos1.AddToDir(Direction.Right);
            pos1.AddToDir(Direction.Down);
            Assert.AreEqual(pos1, pos2);



        }
    }
}