using Jeopardy_Backend.Models;
using Jeopardy_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Jeopardy_Backend.Controllers
{
    [Route("api/Competition")]
    [ApiController]
    public class CompetitionsController : ControllerBase
    {
        private readonly CompetitionsService service;

        public CompetitionsController(CompetitionsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competition>>> Get()
        {
            return Ok(await this.service.GetCompetitions());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Competition>> Post([FromBody] Competition competition)
        {
            var result = await this.service.AddCompetition(competition);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Competition>> Put(int id, [FromBody] Competition competition)
        {
            if (id != competition.Id)
                return BadRequest();

            var result = await this.service.UpdateCompetition(competition);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Competition>> Delete(int id)
        {
            var result = await this.service.DeleteCompetition(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
