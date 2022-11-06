using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dragons.Integrations.WorldsAndDragonsApiV2
{
    //--------------------------------------------------------------------------------------------------------------------------used by all methods
    public class WorldsAndDragonsApiClient : IWorldsAndDragonsApiClient
    {
        private const string baseUrl = "https://localhost:44377/apiV2/";
        private string username;
        private string password;

        public WorldsAndDragonsApiClient(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            var authorizationText = $"{username}:{password}";
            var autorizationBytes = System.Text.Encoding.UTF8.GetBytes(authorizationText);
            var authorizationHeaderValue = Convert.ToBase64String(autorizationBytes);
            var authenticationHeader = new AuthenticationHeaderValue("Basic", authorizationHeaderValue);
            httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
            return httpClient;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------GetWorlds
        #region GetWorlds
        public async Task<World[]> GetWorlds(int? skip = null, int? take = null, string? search = null, CancellationToken? cancellationToken = null)
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {

                    var url = $"worlds?skip={skip}&take={take}&search={WebUtility.UrlEncode(search)}";

                    using (var responceMessage = await httpClient.GetAsync(url, cancellationToken ?? CancellationToken.None))
                    {
                        if (responceMessage.StatusCode == HttpStatusCode.OK)
                        {
                            var responseBody = await responceMessage.Content.ReadAsStreamAsync(cancellationToken ?? CancellationToken.None);
                            var courses = JsonSerializer.Deserialize<World[]>(responseBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                            return courses;
                        }
                        else if (responceMessage.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            throw new AuthorizationException("Bad credentials");
                        }
                        else
                        {
                            throw new ApiException($"Unexpected http status code: {responceMessage.StatusCode}");
                        }
                    }
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException("Unexpected exception", e);
            }

        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------GetWorld
        #region Get World
        public async Task<World> GetWorld(int worldId, CancellationToken? cancellationToken = null)
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {
                    var url = $"worlds/{worldId}";
                    using (var responseMessage = await httpClient.GetAsync(url, cancellationToken ?? CancellationToken.None))
                    {
                        if (responseMessage.StatusCode == HttpStatusCode.OK)
                        {
                            var responseBody = await responseMessage.Content.ReadAsStreamAsync(cancellationToken ?? CancellationToken.None);
                            var world = JsonSerializer.Deserialize<World>(responseBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                            return world;
                        }
                        else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            throw new AuthorizationException("Bad credentials");
                        }
                        else if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new WorldOrDragonNotFoundException($"World {worldId} not found");
                        }
                        else
                        {
                            throw new ApiException($"Unexpected http status code: {responseMessage.StatusCode}");
                        }
                    }
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException("Unexpected exception", e);
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------AddWorld
        #region AddWorld
        public async Task<AddWorldResponce> AddWorld(AddWorldRequest request, CancellationToken? cancellationToken = null)
        {
            try
            {
                using (var httpClient =  CreateHttpClient())
                {
                    var requestJson = JsonSerializer.Serialize(request);

                    using (var httpContent = new StringContent(requestJson))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        using (var responseMessage = await httpClient.PostAsync("worlds", httpContent, cancellationToken ?? CancellationToken.None))
                        {
                            if (responseMessage.StatusCode == HttpStatusCode.Created)
                            {
                                var responseBody = await responseMessage.Content.ReadAsStringAsync(cancellationToken ?? CancellationToken.None);
                                var world = JsonSerializer.Deserialize<World>(responseBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                                var createdAt = responseMessage.Headers.Location.AbsoluteUri;
                                return new AddWorldResponce
                                {
                                    World = world,
                                    ResourceUri = createdAt,
                                };
                            }
                            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new AuthorizationException("Bad credentials");
                            }
                            if(responseMessage.StatusCode == HttpStatusCode.UnprocessableEntity)
                            {
                                var responseBody = await responseMessage.Content.ReadAsStringAsync(cancellationToken?? CancellationToken.None);
                                var response = JsonSerializer.Deserialize<AddWorld422Responses>(responseBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                                var errors = new List<string>();
                                if ((response?.Errors.Name.Any()).GetValueOrDefault())
                                {
                                    errors.AddRange(response.Errors.Name);
                                }
                                throw new ValidationException("Invalid input") { ValidationErrors = errors.ToArray() };
                            }

                            throw new ApiException($"Unexpected http status code: {responseMessage.StatusCode}");
                        }

                    }
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException("Unexpected http exception", e);
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------UpdateWorld
        #region UpdateWorld
        public async Task UpdateWorld(int worldId, UpdateWorldRequest request, CancellationToken? cancellationToken = null)
        {
            try
            {
                using(var httpClient = CreateHttpClient())
                {
                    var requestJson=JsonSerializer.Serialize(request);
                    using (var httpContent = new StringContent(requestJson))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var url = $"worlds/{worldId}";
                        using (var responseMessage = await httpClient.PutAsync(url, httpContent, cancellationToken ?? CancellationToken.None))
                        {
                            if(responseMessage.StatusCode == HttpStatusCode.NoContent)
                            {
                                return;
                            }
                            if(responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new AuthorizationException("Bad credentials");
                            }
                            if(responseMessage.StatusCode == HttpStatusCode.NotFound)
                            {
                                throw new WorldOrDragonNotFoundException($"World {worldId} not found");
                            }
                            throw new ApiException($"Unexpected status code exception {responseMessage.StatusCode}");
                        } ;
                    }
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new ApiException("Unexpected http eceprion occurer", e);
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------DeleteWorld
        #region DeleteWorld
        public async Task DeleteWorld(int worldId, CancellationToken? cancellationToken = null)
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {
                    var url = $"worlds/{worldId}";
                    using(var responseMessage = await httpClient.DeleteAsync(url, cancellationToken ?? CancellationToken.None))
                    {
                        if(responseMessage.StatusCode == HttpStatusCode.NoContent)
                        {
                            return;
                        }
                        if(responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            throw new AuthorizationException("Bad credentials");
                        }
                        if(responseMessage.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new WorldOrDragonNotFoundException($"World {worldId} not found");
                        }
                        throw new ApiException($"Unexpected http status code, {responseMessage.StatusCode}");
                    }
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApiException($"Unexpected exception,{e}");
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------SetDragonImage
        #region SetDragonImage
        public async Task SetDragonImage(int worldId, int dragonId, byte[] image, string filename, CancellationToken? cancellationToken = null)
        {
            try
            {
                using (var httpClient = CreateHttpClient())
                {
                    using (var httpContent = new MultipartFormDataContent(
                        "Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                    {
                        if(image != null && filename != null)
                        {
                            httpContent.Add(new StreamContent(new MemoryStream(image)), "file", filename);
                        }

                        
                        //do not specify media type
                        var url = $"worlds/{worldId}/dragon/{dragonId}/indentificationimage";
                        using (var responceMessage = await httpClient.PutAsync(url, httpContent, cancellationToken?? CancellationToken.None))
                        {
                            if (responceMessage.StatusCode == HttpStatusCode.NoContent)
                            {
                                return;
                            }
                            if(responceMessage.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new AuthorizationException("Bad credentials");
                            }
                            if(responceMessage.StatusCode == HttpStatusCode.NotFound)
                            {
                                throw new WorldOrDragonNotFoundException($"World {worldId} not found");
                            }
                            throw new ApiException($"Unexpected http status code {responceMessage.StatusCode}");
                        }
                    }
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new ApiException($"Unexpected exception,{e}");
            }
        }
        #endregion
        //----------------------------------------------------------------------------------------------------------------------------------------DeleteDragonImage
        #region DeleteDragonImage
        public async Task DeleteDragonImage(int worldId, int dragonId, CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
