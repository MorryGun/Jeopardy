using Jeopardy_Backend.Constants;
using Jeopardy_Backend.Models;
using Jeopardy_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Jeopardy_Backend.Controllers
{
    [Route(ApiConstants.PlayersRout)]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayersService service;

        public PlayersController(PlayersService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> Get()
        {
            return Ok(await this.service.GetPlayers());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Player>> Post([FromBody] Player player)
        {
            var result = await this.service.AddPlayer(player);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Player>> Put(int id, [FromBody] Player player)
        {
            if (id != player.Id)
                return BadRequest();

            var result = await this.service.UpdatePlayer(player);

            return Ok(result);
        }

        [Authorize]
        [HttpPut()]
        public async Task<ActionResult<Player>> UpdateRates()
        {
            await this.service.UpdateRates();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> Delete(int id)
        {
            var result = await this.service.DeletePlayer(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
