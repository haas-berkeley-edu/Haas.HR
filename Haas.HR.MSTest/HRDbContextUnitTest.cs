using Haas.HR;
using Haas.HR.Data;
using NuGet.Frameworks;

namespace Haas.HR.MSTest
{
    [TestClass]
    public class HRDbContextUnitTest
    {
        [TestMethod]
        public void CreatingDatabase()
        {
            HRDbContext context = HRDataSourceManager.HRDbContext;
            Assert.IsTrue(context != null);
        }
    }
}