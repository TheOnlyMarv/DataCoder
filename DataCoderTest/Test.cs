using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataCoder;
using System.Collections.Generic;
using System;
using System.IO;

namespace DataCoderTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestCoderDecoder()
        {
            int upperInt = int.MaxValue;
            int lowerInt = int.MinValue;
            byte upperByte = byte.MaxValue;
            byte lowerByte = byte.MinValue;
            string testString = "ABCabc123.,-/ÜÜ\n";
            long upperLong = long.MaxValue;
            long lowerLong = long.MinValue;

            Coder c = new Coder();

            c.addInteger(upperInt);
            c.addInteger(lowerInt);
            c.addByte(upperByte);
            c.addByte(lowerByte);
            c.addString(testString);
            c.addLong(upperLong);
            c.addLong(lowerLong);

            List<object> result = Decoder.getData(c.getStream());
            Assert.AreEqual(7, result.Count);
            object[] aResult = result.ToArray();
            Assert.IsInstanceOfType(aResult[0], typeof(int));
            Assert.IsInstanceOfType(aResult[1], typeof(int));
            Assert.IsInstanceOfType(aResult[2], typeof(byte));
            Assert.IsInstanceOfType(aResult[3], typeof(byte));
            Assert.IsInstanceOfType(aResult[4], typeof(string));
            Assert.IsInstanceOfType(aResult[5], typeof(long));
            Assert.IsInstanceOfType(aResult[6], typeof(long));

            Assert.AreEqual(upperInt, aResult[0]);
            Assert.AreEqual(lowerInt, aResult[1]);
            Assert.AreEqual(upperByte, aResult[2]);
            Assert.AreEqual(lowerByte, aResult[3]);
            Assert.AreEqual(testString, aResult[4]);
            Assert.AreEqual(upperLong, aResult[5]);
            Assert.AreEqual(lowerLong, aResult[6]);

            FileStream fs = new FileStream("test.stream", FileMode.Create);
            Stream s = c.getStream();
            long pos = s.Position;
            s.Position = 0;
            s.CopyTo(fs);
            s.Position = pos;
            fs.Flush();
            fs.Close();
        }
    }
}
