using Jeopardy_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeopardy_Backend.Services
{
    public class ResultsService
    {
        private readonly JeopardyContext context;

        public ResultsService(JeopardyContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Result>> GetResults(int matchId)
        {
            return await this.context.Results.Where(x => x.MatchId == matchId).ToListAsync();
        }

        public async Task<Result> AddResult(Result result)
        {
            if (!this.context.Players.Any(x => x.Id == result.PlayerId) ||
                this.context.Results.Where(x => x.MatchId == result.MatchId).Any(x => x.PlayerId == result.PlayerId))
                return null;

            await this.context.Results.AddAsync(result);
            await this.context.SaveChangesAsync();

            return result;
        }

        public async Task<Result> UpdateResult(Result result)
        {
            this.context.Entry(result).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return result;
        }

        public async Task<Result> DeleteResult(int id)
        {
            var result = await this.context.Results.FindAsync(id);

            if (result == null)
                return null;

            this.context.Results.Remove(result);
            await this.context.SaveChangesAsync();

            return result;
        }
    }
}
