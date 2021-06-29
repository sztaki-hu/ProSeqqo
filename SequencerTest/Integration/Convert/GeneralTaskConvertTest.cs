using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencerTest.Integration.Convert
{
    [TestClass]
    public class GeneralTaskConvertTest
    {
        [TestInitialize()]
        public void Initialize()
        {

        }

        [TestMethod]
        public void FromSEQ()
        {
            ConvertHelper convertTest = new ConvertHelper();
            convertTest.AssertSerialization(@"./Resources/Example/HelloWorld/HelloWorld.seq", "seq", @"./Resources/Out/HelloWorldSEQ.seq", "seq");
            convertTest.AssertSerialization(@"Resources/Example/HelloWorld/HelloWorld.seq", "seq", @"Resources/Out/HelloWorldSEQ.json", "json");
            convertTest.AssertSerialization(@"Resources/Example/HelloWorld/HelloWorld.seq", "seq", @"Resources/Out/HelloWorldSEQ.xml", "xml");

            convertTest.AssertSerialization(@"Resources/Example/CameraPickAndPlace.seq", "seq", @"Resources/Out/CameraPickAndPlace.seq", "seq");
            convertTest.AssertSerialization(@"Resources/Example/CameraPickAndPlace.seq", "seq", @"Resources/Out/CameraPickAndPlace.json", "json");
            convertTest.AssertSerialization(@"Resources/Example/CameraPickAndPlace.seq", "seq", @"Resources/Out/CameraPickAndPlace.xml", "xml");

            convertTest.AssertSerialization("Resources/Example/CelticLaser.seq", "seq", "Resources/Out/CelticLaser.seq", "seq");
            convertTest.AssertSerialization("Resources/Example/CelticLaser.seq", "seq", "Resources/Out/CelticLaser.json", "json");
            convertTest.AssertSerialization("Resources/Example/CelticLaser.seq", "seq", "Resources/Out/CelticLaser.xml", "xml");

            convertTest.AssertSerialization("Resources/Example/CubeCastleBuilding.seq", "seq", "Resources/Out/CubeCastleBuilding.seq", "seq");
            convertTest.AssertSerialization("Resources/Example/CubeCastleBuilding.seq", "seq", "Resources/Out/CubeCastleBuilding.json", "json");
            convertTest.AssertSerialization("Resources/Example/CubeCastleBuilding.seq", "seq", "Resources/Out/CubeCastleBuilding.xml", "xml");

            convertTest.AssertSerialization("Resources/Example/CubeCastleTwoBuilding.seq", "seq", "Resources/Out/CubeCastleTwoBuilding.seq", "seq");
            convertTest.AssertSerialization("Resources/Example/CubeCastleTwoBuilding.seq", "seq", "Resources/Out/CubeCastleTwoBuilding.json", "json");
            convertTest.AssertSerialization("Resources/Example/CubeCastleTwoBuilding.seq", "seq", "Resources/Out/CubeCastleTwoBuilding.xml", "xml");

            convertTest.AssertSerialization("Resources/Example/FurnitureParts.seq", "seq", "Resources/Out/FurnitureParts.seq", "seq");
            convertTest.AssertSerialization("Resources/Example/FurnitureParts.seq", "seq", "Resources/Out/FurnitureParts.json", "json");
            convertTest.AssertSerialization("Resources/Example/FurnitureParts.seq", "seq", "Resources/Out/FurnitureParts.xml", "xml");

            convertTest.AssertSerialization("Resources/Example/RoboticDrawing.seq", "seq", "Resources/Out/RoboticDrawing.seq", "seq");
            convertTest.AssertSerialization("Resources/Example/RoboticDrawing.seq", "seq", "Resources/Out/RoboticDrawing.json", "json");
            convertTest.AssertSerialization("Resources/Example/RoboticDrawing.seq", "seq", "Resources/Out/RoboticDrawing.xml", "xml");
        }

        [TestMethod]
        public void FromJSON()
        {
            ConvertHelper convertTest = new ConvertHelper();
            convertTest.AssertSerialization("Resources/Example/HelloWorld/HelloWorld.json", "json", "Resources/HelloWorldJSON.seq", "seq");
            convertTest.AssertSerialization("Resources/Example/HelloWorld/HelloWorld.json", "json", "Resources/HelloWorldJSON.json", "json");
            convertTest.AssertSerialization("Resources/Example/HelloWorld/HelloWorld.json", "json", "Resources/HelloWorldJSON.xml", "seq");
        }

        [TestMethod]
        public void FromXML()
        {
            ConvertHelper convertTest = new ConvertHelper();
            convertTest.AssertSerialization("Resources/Example/HelloWorld/HelloWorld.xml", "xml", "Resources/HelloWorldXML.seq", "seq");
            convertTest.AssertSerialization("Resources/Example/HelloWorld/HelloWorld.xml", "xml", "Resources/HelloWorldXML.json", "json");
            convertTest.AssertSerialization("Resources/Example/HelloWorld/HelloWorld.xml", "xml", "Resources/HelloWorldXML.xml", "seq");
        }

    }
}
