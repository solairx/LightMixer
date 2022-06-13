using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LightMixer.Model.Tests
{
    [TestClass()]
    public class SharedEffectModelTests
    {
        [TestMethod()]
        public void GetSpecterColorByteGoingDownTest()
        {

            var target = new SharedEffectModel(null);
            Assert.AreEqual(50, target.GetSpecterColorByte(60, 40, 10));

            Assert.AreEqual(40, target.GetSpecterColorByte(50, 40, 10));

            Assert.AreEqual(40, target.GetSpecterColorByte(45, 40, 10));

            Assert.AreEqual(0, target.GetSpecterColorByte(10, 0, 10));
            Assert.AreEqual(0, target.GetSpecterColorByte(0, 0, 10));
        }

        [TestMethod()]
        public void GetSpecterColorByteGoingUpTest()
        {

            var target = new SharedEffectModel(null);
            Assert.AreEqual(70, target.GetSpecterColorByte(60, 100, 10));

            Assert.AreEqual(70, target.GetSpecterColorByte(60, 70, 10));

            Assert.AreEqual(65, target.GetSpecterColorByte(60, 65, 10));

            Assert.AreEqual(255, target.GetSpecterColorByte(255, 255, 10));
            Assert.AreEqual(255, target.GetSpecterColorByte(250, 255, 10));
        }
    }
}