using Jeopardy_Backend.Models;
using Jeopardy_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Jeopardy_Backend.Controllers
{
    [Route("api/Result")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly ResultsService service;

        public ResultsController(ResultsService service)
        {
            this.service = service;
        }

        [HttpGet("{matchId}")]
        public async Task<ActionResult<IEnumerable<Match>>> Get(int matchId)
        {
            return Ok(await this.service.GetResults(matchId));
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<Match>> Post([FromBody] Result resultRecord)
        {
            var result = await this.service.AddResult(resultRecord);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Player>> Put(int id, [FromBody] Result resultRecord)
        {
            if (id != resultRecord.Id)
                return BadRequest();

            var result = await this.service.UpdateResult(resultRecord);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.service.DeleteResult(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
