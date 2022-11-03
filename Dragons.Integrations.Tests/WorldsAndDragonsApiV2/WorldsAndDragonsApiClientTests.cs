using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragons.Integrations.WorldsAndDragonsApiV2;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dragons.Integrations.Tests.WorldsAndDragonsApiV2
{
    

    [TestClass]
    public class WorldsAndDragonsApiClientTests
    {
        private WorldsAndDragonsApiClient apiClient;
        [TestInitialize]
        public void TestInitialize()
        {
            apiClient = new WorldsAndDragonsApiClient("damiansoch@hotmail.com","damian1");
        }

        #region GetWorlds

        [TestMethod]
        public async Task GetWorlds_NoParameters()
        {
            var results = await apiClient.GetWorlds();
            results.Should().NotBeNullOrEmpty();
            results.Length.Should().Be(2);
        }
        
        [TestMethod]
        public async Task GetWorlds_WithSkipAndTake()
        {
            var results = await apiClient.GetWorlds(1,1);
            results.Should().NotBeNullOrEmpty();
            results.Length.Should().Be(1);
        }

        [TestMethod]
        public async Task GetWorlds_WithSearch()
        {
            var results = await apiClient.GetWorlds(null,null,"house");
            results.Should().NotBeNullOrEmpty();
            results.Length.Should().Be(1);
        }

        [TestMethod]
        public async Task GetCourses_BadCredentials()
        {
            apiClient = new WorldsAndDragonsApiClient("badusername", "badpassword");
            var action = new Func<Task>(async () =>
            {
                await apiClient.GetWorlds();
            }).Should().ThrowAsync<AuthorizationException>();
        }

        #endregion

        #region GetWorld
        [TestMethod]
        public async Task GetCourse_Exist()
        {
            var result = await apiClient.GetWorld(1);
            result.Should().NotBeNull();
            
        }
        [TestMethod]
        public async Task GetCourse_DoesNotExist()
        {
            await new Func<Task<World>>(async () =>
            {
              return  await apiClient.GetWorld(1000);
            }).Should().ThrowAsync<WorldOrDragonNotFoundException>();
        }
        [TestMethod]
        public async Task GetCourse_BadCredentials()
        {
            apiClient = new WorldsAndDragonsApiClient("badUsername", "badPassword");
            await new Func<Task<World>>(async () =>
            {
              return  await apiClient.GetWorld(1);
            }).Should().ThrowAsync<AuthorizationException>();
        }
        #endregion

        #region AddWorld
        [TestMethod]
        public async Task AddWorld_ValidInput()
        {
            var newWorld = new AddWorldRequest
            {
                Name = "TestWorld"
            };
            var result = await apiClient.AddWorld(newWorld);
            result.Should().NotBeNull();
            result.World.Should().NotBeNull();
            result.World.Name.Should().Be(newWorld.Name);
            result.ResourceUri.Should().NotBeNullOrEmpty();
            result.ResourceUri.Should().Be($"https://localhost:44377/apiv2/Worlds/{result.World.Id}");
        }
        [TestMethod]
        public async Task AddWorld_InvalidInput()
        {
            var newWorld = new AddWorldRequest
            {
                Name = null,
            };
            await new Func<Task<AddWorldResponce>>(async () =>
            {
                return await apiClient.AddWorld(newWorld);
            }).Should().ThrowAsync<ValidationException>();
        }
        [TestMethod]
        public async Task AddWorld_BadCredentials()
        {
            apiClient = new WorldsAndDragonsApiClient("badUsername", "BadPassword");
            await new Func<Task<AddWorldResponce>>(async () =>
            {
                var newWorld = new AddWorldRequest
                {
                    Name = "TestName1"
                };
                return await apiClient.AddWorld(newWorld);
            }).Should().ThrowAsync<AuthorizationException>();
        }
        #endregion
    }
}
