using AutoMapper;
using Dragons.Models.Dragons;
using Dragons.Services.Dragons;
using Dragons.WebApi.Attributes;
using Dragons.WebApi.Models.ApiV2.Dragons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace Dragons.WebApi.Controllers.ApiV2
{
    //v2
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("apiV2/worlds/{worldId:int}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [BasicAuthorization]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class DragonController : ControllerBase

    {
        private IDragonService _dragonService;
        private IMapper _mapper;

        public DragonController(IDragonService dragonService, IMapper mapper)
        {
            _dragonService = dragonService;
            _mapper = mapper;
        }
        /// <summary>
        /// Gets the list of the dragons in a world from the DataStore
        /// </summary>
        /// /// <param name="worldId">The id of the World to search</param>
        /// <param name="skip">The number of the dragons at the biggining of the list to skip</param>
        /// <param name="take">How many dragons to include, after thse that are skipped</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DragonDto[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListOfDragons([FromRoute] int worldId, [FromQuery] int? skip, [FromQuery] int? take, [FromQuery] string? search)
        {
            if (!_dragonService.DoesWorldExist(worldId))
            {
                return NotFound();
            }
            var dragons = _dragonService.GetDragonsinWorldList(worldId, skip ?? 0, take ?? 5, search);
            var dragonDtos = _mapper.Map<DragonDto[]>(dragons);
            return Ok(dragonDtos);

        }

        /// <summary>
        /// Returns single dragon from the world
        /// </summary>
        /// <param name="worldId">The id of the World to search</param>
        /// <param name="dragonId">The id of the dragon to retrieve</param>
        /// <returns></returns>
        [HttpGet("{dragonId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DragonDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDragon([FromRoute] int worldId, [FromRoute] int dragonId)
        {
            if (!_dragonService.DoesWorldExist(worldId)
                || (!_dragonService.DoesDragonExistInWorld(worldId, dragonId)
                ))
            {
                return NotFound();
            }
            var dragon = _dragonService.GetDragonInWorld(worldId, dragonId);
            var dragonDto = _mapper.Map<DragonDto>(dragon);
            return Ok(dragonDto);
        }

        /// <summary>
        /// Adds dragon into the world
        /// </summary>
        /// <param name="wordId">The id of the world to add the dragon to</param>
        /// <param name="dragon">The details of the dragon</param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DragonDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddDragon([FromRoute] int wordId, [FromBody] AddDragonRequest dragon)
        {
            if (!_dragonService.DoesWorldExist(wordId))
            {
                return NotFound();
            }
            var dragonEntity = _mapper.Map<Dragon>(dragon);
            var dragonId = _dragonService.AddDragonToWorld(wordId, dragonEntity);
            var addedDragon = _dragonService.GetDragonInWorld(wordId, dragonId);
            var addedDragonDto = _mapper.Map<DragonDto>(addedDragon);

            return CreatedAtAction(nameof(GetDragon), new { worldId = wordId, dragonId = dragonId }, addedDragonDto);
        }

        /// <summary>
        /// Updates selected Dragon in selected world  
        /// </summary>
        /// <param name="worldId">The id of the world with the dragon to update</param>
        /// <param name="dragonId">The id of the dragon to be updated</param>
        /// <param name="dragon">The new details of the dragon </param>
        /// <returns></returns>
        [HttpPut("{dragonId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateDragon([FromRoute] int worldId, [FromRoute] int dragonId, [FromBody] UpdateDragonInWorldRequest dragon)
        {
            if (!_dragonService.DoesWorldExist(worldId)
                || !_dragonService.DoesDragonExistInWorld(worldId, dragonId))
            {
                return NotFound();
            }
            var dragonToUpdate = _mapper.Map<Dragon>(dragon);
            dragonToUpdate.Id = dragonId;
            _dragonService.UpdateDragonInWorld(worldId, dragonToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Deletes selected dragon in a selected world
        /// </summary>
        /// <param name="worldId">The id of the world with the dragon to be deleted</param>
        /// <param name="dragonId">The id of the dragon to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{dragonId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteDragon([FromRoute] int worldId, [FromRoute] int dragonId)
        {
            if (!_dragonService.DoesWorldExist(worldId)
                || !_dragonService.DoesDragonExistInWorld(worldId, dragonId))
            {
                return NotFound();
            }
            _dragonService.DeleteDragonFromWorld(worldId, dragonId);

            return NoContent();
        }

        //---------------------------------------------setting up the image method


        [HttpPut]
        [Route("{dragonId:int}/indentificationimage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetImage([FromRoute] int worldId, [FromRoute] int dragonId, [FromForm] IFormFile? file)
        {
            if (!_dragonService.DoesWorldExist(worldId)
                || !_dragonService.DoesDragonExistInWorld(worldId, dragonId))
            {
                return NotFound();
            }
            if (file == null
                || file.Length > 16777216
                || file.Length == 0)
            {
                return BadRequest();
            }

            byte[]? dragonImageBytes = null;
            await using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                dragonImageBytes = memoryStream.ToArray();
            }
            _dragonService.SetDragonImage(worldId, dragonId, dragonImageBytes, file.FileName);

            return NoContent();
        }
        [HttpDelete]
        [Route("{dragonId:int}/indentificationimage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteImage([FromRoute] int worldId, [FromRoute] int dragonId)
        {
            if (!_dragonService.DoesWorldExist(worldId)
                || !_dragonService.DoesDragonExistInWorld(worldId, dragonId))
            {
                return NotFound();
            }
            _dragonService.SetDragonImage(worldId, dragonId, null, null);
            return NoContent();
        }
    }
}
