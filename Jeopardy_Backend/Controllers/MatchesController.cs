using Jeopardy_Backend.Models;
using Jeopardy_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Jeopardy_Backend.Controllers
{
    [Route("api/Match")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly MatchesService service;

        public MatchesController(MatchesService service)
        {
            this.service = service;
        }

        [HttpGet("{competitionId}")]
        public async Task<ActionResult<IEnumerable<Match>>> Get(int competitionId)
        {
            return Ok(await this.service.GetMatches(competitionId));
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<Match>> Post([FromBody] Match match)
        {
            var result = await this.service.AddMatch(match);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteMatch(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
