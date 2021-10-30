using Jeopardy_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeopardy_Backend.Services
{
    public class MatchesService
    {
        private readonly JeopardyContext context;

        public MatchesService(JeopardyContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Match>> GetMatches(int competitionId)
        {
            return await this.context.Matches.Where(x => x.Id == competitionId).ToListAsync();
        }

        public async Task<Match> AddMatch(Match match)
        {
            if (!this.context.Competitions.Any(x => x.Id == match.CompetitionId))
                return null;

            await this.context.Matches.AddAsync(match);
            await this.context.SaveChangesAsync();

            return match;
        }

        public async Task<Match> DeleteMatch(int id)
        {
            var match = await this.context.Matches.FindAsync(id);
            if (match == null)
                return null;

            var impactedResults = this.context.Results.Where(x => x.MatchId == id).ToList();

            foreach (var impactedResult in impactedResults)
            {
                this.context.Results.Remove(impactedResult);
                await this.context.SaveChangesAsync();
            }

            this.context.Matches.Remove(match);
            await this.context.SaveChangesAsync();

            return match;
        }
    }
}
