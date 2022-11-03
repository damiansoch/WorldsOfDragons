using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragons.Integrations.WorldsAndDragonsApiV2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dragons.Integrations.Tests.WorldsAndDragonsApiV2
{
    [TestClass]
    public class WorldsAndDragonsApiClientTests
    {
        [TestMethod]
        public async Task GetWorlds_NoParameters()
        {
            var ApiClient = new WorldsAndDragonsApiClient();
            var results = await ApiClient.GetWorlds();
        }
    }
}
