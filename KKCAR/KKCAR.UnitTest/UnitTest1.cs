using Microsoft.VisualStudio.TestTools.UnitTesting;
using KKCAR.Common.Utilities;

///<summary>
/// Class Name : UnitTest1
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void test_get_fileName()
        {
            string expected = "CAR_201610270640_01_";
            string actual = "CAR_201610270640_01_REQ.txt".ExtractNamePrefix();
            Assert.AreEqual(expected, actual);
        }
    }
}
