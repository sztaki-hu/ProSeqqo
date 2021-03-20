using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencerTest.Units.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class StrictEdgeWeightSetTest
    {
        private StrictEdgeWeightSet set;
        private List<StrictEdgeWeight> list;
        private Position a;
        private Position b;


        [TestInitialize()]
        public void Initialize()
        {
            a = TestObjects.GetPosA().In;
            b = TestObjects.GetPosB().In;
            list = InitList();
        }

        private List<StrictEdgeWeight> InitList()
        {
            var list = new List<StrictEdgeWeight>();
            list.Add(new StrictEdgeWeight(a, b, 3, false));
            list.Add(new StrictEdgeWeight(b, a, 9, false));
            return list;
        }

        [TestMethod()]
        public void ConstructorTest()
        {
            set = new StrictEdgeWeightSet();
            Assert.IsNotNull(set.GetAll());
            Assert.AreEqual(set.GetAll().Count,0);

            set = new StrictEdgeWeightSet(InitList());
            Assert.IsNotNull(set.GetAll());
            Assert.AreEqual(set.GetAll().Count, 2);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            set = new StrictEdgeWeightSet(InitList());
            set.Delete(a, b, false);
            Assert.AreEqual(set.GetAll().Count, 1);

            set = new StrictEdgeWeightSet(InitList());
            set.Delete(a, b, false);
            Assert.AreEqual(set.GetAll().Count, 1);

            set = new StrictEdgeWeightSet(InitList());
            set.Delete(a, b, true);
            Assert.AreEqual(set.GetAll().Count, 0);

            set = new StrictEdgeWeightSet(InitList());
            set.Delete(a);
            Assert.AreEqual(set.GetAll().Count, 0);

            set = new StrictEdgeWeightSet(InitList());
            set.DeleteAll();
            Assert.AreEqual(set.GetAll().Count, 0);

            set = new StrictEdgeWeightSet(InitList());
            set.DeleteAll();
            set.DeleteAll();
            Assert.AreEqual(set.GetAll().Count, 0);

            set = new StrictEdgeWeightSet(InitList());
            set.DeleteAll();
            set.Delete(a);
            Assert.AreEqual(set.GetAll().Count, 0);
        }

        [TestMethod()]
        public void AddTest()
        {
            set = new StrictEdgeWeightSet(InitList());
            set.Add(a, b, 10);
            Assert.AreEqual(set.GetAll().Count, 3);

            set = new StrictEdgeWeightSet();
            set.Add(a, b, 10);
            Assert.AreEqual(set.GetAll().Count, 1);
        }

        [TestMethod()]
        public void GetTest()
        {
            set = new StrictEdgeWeightSet(InitList());
            Assert.AreEqual(set.Get(a).Count, 2);

            set = new StrictEdgeWeightSet(InitList());
            var edge = set.Get(a, b);
            Assert.AreEqual(edge.Weight, 3);
            edge = set.Get(b, a);
            Assert.AreEqual(edge.Weight, 9);
            edge = set.Get(b, b);
            Assert.IsNull(edge);
        }

        [TestMethod()]
        public void ToLogTest()
        {
            set = new StrictEdgeWeightSet(InitList());
            SeqLogger.InitBacklog();
            set.ToLog(LogLevel.Trace);
            //Assert.IsFalse(SeqLogger.Backlog.Count==0);
        }
    }
}
