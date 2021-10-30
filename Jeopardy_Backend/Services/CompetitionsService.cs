using Jeopardy_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeopardy_Backend.Services
{
    public class CompetitionsService
    {
        private readonly JeopardyContext context;

        public CompetitionsService(JeopardyContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Competition>> GetCompetitions()
        {
            return await this.context.Competitions.ToListAsync();
        }

        public async Task<Competition> AddCompetition(Competition competition)
        {
            await this.context.Competitions.AddAsync(competition);
            await this.context.SaveChangesAsync();

            return competition;
        }

        public async Task<Competition> UpdateCompetition(Competition competition)
        {
            this.context.Entry(competition).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return competition;
        }

        public async Task<Competition> DeleteCompetition(int id)
        {
            var competition = await this.context.Competitions.FindAsync(id);
            if (competition == null)
                return null;

            var impactedMatches = this.context.Matches.Where(x => x.CompetitionId == id).ToList();

            foreach (var impactedMatch in impactedMatches)
            {
                var matchId = impactedMatch.Id;

                var impactedResults = this.context.Results.Where(x => x.MatchId == matchId).ToList();

                foreach (var impactedResult in impactedResults)
                {
                    this.context.Results.Remove(impactedResult);
                    await this.context.SaveChangesAsync();
                }

                this.context.Matches.Remove(impactedMatch);
                await this.context.SaveChangesAsync();
            }

            this.context.Competitions.Remove(competition);
            await this.context.SaveChangesAsync();

            return competition;
        }
    }
}
