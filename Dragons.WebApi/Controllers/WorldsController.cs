using AutoMapper;
using Dragons.Models.Dragons;
using Dragons.Services.Dragons;
using Dragons.WebApi.Models.Worlds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salesforce.Common.Models.Json;

namespace Dragons.WebApi.Controllers
{
   

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class WorldsController : ControllerBase
    {
        //constructor + dependency injections
        private IDragonService _dragonService;
        private IMapper _mapper;
        public WorldsController(IDragonService dragonService,IMapper mapper)
        {
            _dragonService = dragonService;
            _mapper = mapper;
        }

        //all worlds list
        /// <summary>
        /// Gets a pageable list of Words
        /// that matches the query from the DataStore
        /// </summary>
        /// <param name="request">Query parameters</param>
        /// <returns>A list of Worlds</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(statusCode:StatusCodes.Status200OK,Type = typeof(WorldDto[]))]
        public WorldDto[] GetWorldList([FromQuery] GetWorldListRequest request)
        {
            var worlds = _dragonService.GetWorldList(request.Skip ?? 0, request.Take ?? 25,request.Search);
            var worldDtos = _mapper.Map<WorldDto[]>(worlds);
            
            return worldDtos;
        }

        //world by id
        /// <summary>
        /// Gets a World from the DataStore
        /// </summary>
        /// <param name="worldId">The Id of the world</param>
        /// <returns></returns>
        /// <response code="404">When the World doesn't exist</response>
        [HttpGet]
        [Route("{worldId:int}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(WorldDto[]))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult GetWorldById([FromRoute]int worldId)
        {
            var world = _dragonService.GetWorld(worldId);
            if(world == null)
            {
                return NotFound();
            }
            var worldDto = _mapper.Map<WorldDto>(world);
            return Ok(worldDto);
        }

        //add world
        /// <summary>
        /// Adds the World to a DataStore
        /// </summary>
        /// <param name="request">The details of the course to be created</param>
        /// <returns></returns>
        /// <response code="422">When there is a validation errors</response>
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created, Type = typeof(WorldDto[]))]
        [ProducesResponseType(statusCode: StatusCodes.Status422UnprocessableEntity)]
        public IActionResult AddWorld([FromBody]AddWorldRequest request)
        {
            var worldEntity = _mapper.Map<World>(request);

            var id = _dragonService.AddWorld(worldEntity);
            var world = _dragonService.GetWorld(id);
            var worldDto = _mapper.Map<WorldDto>(world);

            return CreatedAtAction(nameof(GetWorldById), routeValues: new {worldId = id}, value:worldDto);
        }

        //update world
        /// <summary>
        /// Updates World in the DataStore
        /// </summary>
        /// <param name="worldId">The Id of the World to update</param>
        /// <param name="request">The details of the course to be updated</param>
        /// <returns></returns>
        /// <response code="422">When there is a validation errors</response>
        /// <response code="404">When the World was not found by its id</response>
        [HttpPut]
        [Route("{worldId:int}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, Type = typeof(WorldDto[]))]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesResponseType(statusCode: StatusCodes.Status422UnprocessableEntity)]
        public IActionResult UpdateWorld([FromRoute] int worldId, [FromBody] UpdateWorldRequest request)
        {
            if (!_dragonService.DoesWorldExist(worldId))
            {
                return NotFound();
            }
            var worldToUpdate = _mapper.Map<World>(request);
            worldToUpdate.Id = worldId;
            _dragonService.UpdateWorld(worldToUpdate);
            return NoContent();
        }

        /// <summary>
        /// Deletes World from the DataStore
        /// </summary>
        /// <param name="worldId">The Id of the World to be deleted</param>
        /// <returns></returns>
        /// <response code="404">When the World was not found by its id</response>
        [HttpDelete]
        [Route("{worldId:int}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult DeleteWorld([FromRoute] int worldId)
        {
            if (!_dragonService.DoesWorldExist(worldId))
            {
                return NotFound();
            }
            _dragonService.DeleteWorld(worldId);
            return NoContent();
        }
    }

}
