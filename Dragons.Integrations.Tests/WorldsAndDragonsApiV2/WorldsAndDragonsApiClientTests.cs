using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            apiClient = new WorldsAndDragonsApiClient("damiansoch@hotmail.com", "damian1");
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
            var results = await apiClient.GetWorlds(1, 1);
            results.Should().NotBeNullOrEmpty();
            results.Length.Should().Be(1);
        }

        [TestMethod]
        public async Task GetWorlds_WithSearch()
        {
            var results = await apiClient.GetWorlds(null, null, "house");
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
                return await apiClient.GetWorld(1000);
            }).Should().ThrowAsync<WorldOrDragonNotFoundException>();
        }
        [TestMethod]
        public async Task GetCourse_BadCredentials()
        {
            apiClient = new WorldsAndDragonsApiClient("badUsername", "badPassword");
            await new Func<Task<World>>(async () =>
            {
                return await apiClient.GetWorld(1);
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

        #region updateCourse
        [TestMethod]
        public async Task UpdateWorld_ValidInput()
        {
            var request = new UpdateWorldRequest
            {
                Name = "World updated"
            };
            await apiClient.UpdateWorld(0, request);
            var world = await apiClient.GetWorld(0);
            world.Should().NotBeNull();
            world.Name.Should().NotBeNull();
            world.Name.Should().Be("World updated");
        }
        [TestMethod]
        public async Task UpdareWorld_WorldDoesNotExist()
        {
            await new Func<Task>(async () =>
            {
                await apiClient.UpdateWorld(1000, new UpdateWorldRequest());
            }).Should().ThrowAsync<WorldOrDragonNotFoundException>();
        }
        [TestMethod]
        public async Task UpdateWorld_BadCredentials()
        {
            await new Func<Task>(async () =>
            {
                apiClient = new WorldsAndDragonsApiClient("badUsername", "badPassword");
                await apiClient.UpdateWorld(0, new UpdateWorldRequest());
            }).Should().ThrowAsync<AuthorizationException>();

        }
        #endregion

        #region DeleteCourse
        [TestMethod]
        public async Task DeleteWorld_valid()
        {
            var newWorld = new AddWorldRequest
            {
                Name = "TestForDelete"
            };
            var responce = await apiClient.AddWorld(newWorld);
            await apiClient.DeleteWorld(responce.World.Id);

            await new Func<Task>(async () =>
            {
                await apiClient.DeleteWorld(responce.World.Id);
            }).Should().ThrowAsync<WorldOrDragonNotFoundException>();
        }
        [TestMethod]
        public async Task DeleteWorld_WorldDoesNotExist()
        {
            await new Func<Task>(async () =>
            {
                await apiClient.DeleteWorld(1000);
            }).Should().ThrowAsync<WorldOrDragonNotFoundException>();
        }
        [TestMethod]
        public async Task DeleteWorld_BadCredentials()
        {
            apiClient = new WorldsAndDragonsApiClient("badUsername", "badPassword");
            await new Func<Task>(async () =>
            {
                await apiClient.DeleteWorld(1);
            }).Should().ThrowAsync<AuthorizationException>();
        }
        #endregion

        #region SetImage
        [TestMethod]
        public async Task SetDragonImage_GoodImage()
        {
            byte[] image = null;
            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("Dragons.Integrations.Tests.WorldsAndDragonsApiV2.TestImage.jpg"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    image = memoryStream.ToArray();
                }
            }

            await apiClient.SetDragonImage(0, 1, image, "TestImage.jpg");
        }
        [TestMethod]
        public async Task SetDragonImage_WorldAndDragonDoesntExist()
        {
            //
            byte[] image = null;
            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("Dragons.Integrations.Tests.WorldsAndDragonsApiV2.TestImage.jpg"))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    image = memoryStream.ToArray();
                }
            }
            //
            await new Func<Task>(async () =>
            {
                await apiClient.SetDragonImage(1000, 1000, image, "TestImage.jpg");
            }).Should().ThrowAsync<WorldOrDragonNotFoundException>();
        }
        [TestMethod]
        public async Task SetDragonImage_BadCredentials()
        {
           
            apiClient = new WorldsAndDragonsApiClient("badUsername", "badPassword");
            await new Func<Task>(async () =>
            {
                await apiClient.SetDragonImage(0, 1, null, null);
            }).Should().ThrowAsync<AuthorizationException>();
        }
        #endregion
    }
}
