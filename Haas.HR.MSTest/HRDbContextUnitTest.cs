using Haas.HR;
using Haas.HR.Data;
using Haas.HR.Models;
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

        [TestMethod]
        public async Task DownloadingPingboardUsers()
        {
            PingboardUserDataSource pingboardUserDataSource = new PingboardUserDataSource();
            HRDataSourceConnectionSettings connectionSettings = new HRDataSourceConnectionSettings();
            HRDataSourceDownloadSettings settings = new HRDataSourceDownloadSettings(connectionSettings);
            IHRDataSourceDownloadResult result = await pingboardUserDataSource.DownloadEntityData(settings);
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public async Task DownloadingPingboardGroups()
        {
            PingboardGroupDataSource pingboardGroupDataSource = new PingboardGroupDataSource();
            HRDataSourceConnectionSettings connectionSettings = new HRDataSourceConnectionSettings();
            HRDataSourceDownloadSettings settings = new HRDataSourceDownloadSettings(connectionSettings);
            IHRDataSourceDownloadResult result = await pingboardGroupDataSource.DownloadEntityData(settings);
            Assert.IsTrue(result != null);
        }
    }
}