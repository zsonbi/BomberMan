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
    }
}