using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencerTest.Helper
{
    public class AssertSeq
    {
        public static void AssertPosition(Position excepted, Position actual)
        {
            if(excepted != null)
            {
                Assert.AreEqual(excepted.Name, actual.Name);
                Assert.AreEqual(excepted.UserID, actual.UserID);
                Assert.AreEqual(excepted.ResourceID, actual.ResourceID);
                Assert.AreEqual(excepted.SequencingID, actual.SequencingID);
                Assert.AreEqual(excepted.Vector.Length, actual.Vector.Length);
                Assert.AreEqual(excepted.Dimension, actual.Vector.Length);
                for (int i = 0; i < excepted.Dimension; i++)
                {
                    Assert.AreEqual(excepted.Vector[i], actual.Vector[i]);
                }
                Assert.AreEqual(excepted.Virtual, actual.Virtual);
                Assert.AreEqual(excepted.Dimension, actual.Dimension);
            }
            
        }

        public static void AssertLine(Line expected, Line actual)
        {
            
        }

        public static void AssertContour(Contour expected, Contour actual)
        {

        }

    }
}
