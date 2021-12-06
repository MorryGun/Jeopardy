using Jeopardy_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeopardy_Backend.Services
{
    public class PlayersService
    {
        private readonly JeopardyContext context;

        public PlayersService(JeopardyContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            return await this.context.Players.ToListAsync();
        }

        public async Task<Player> AddPlayer(Player player)
        {
            await this.context.Players.AddAsync(player);
            await this.context.SaveChangesAsync();

            return player;
        }

        public async Task<Player> UpdatePlayer(Player player)
        {
            this.context.Entry(player).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return player;
        }

        public async Task UpdateRates()
        {
            await RestoreRates();

            var orderedMatches = this.context.Matches.OrderBy(x => this.context.Competitions.First(c => c.Id == x.CompetitionId).Date).ToList();

            foreach (var orderedMatch in orderedMatches)
            {
                var results = this.context.Results.Where(x => x.MatchId == orderedMatch.Id);
                var players = new List<Player>();

                results.Select(x => x.PlayerId).ToList()
                                               .ForEach(pId => players.Add(this.context.Players.First(x => x.Id == pId)));

                var prizeFund = GetPrizeFond(players);

                var scores = results.Select(x => x.Score);
                int minScore = scores.Min() < 0 ? 0 : scores.Min();
                var sumOfGamePoints = GetSumOfGamePoints(scores, minScore);

                foreach (var player in players)
                {
                    var playersScore = results.First(x => x.PlayerId == player.Id).Score;
                    var prize = prizeFund * (playersScore - minScore) / sumOfGamePoints;
                    player.Rate = Math.Round(0.75 * player.Rate + prize);
                    await UpdatePlayer(player);
                }
            }
        }

        public async Task<Player> DeletePlayer(int id)
        {
            var player = await this.context.Players.FindAsync(id);

            if (player == null)
                return null;

            var impactedResults = this.context.Results.Where(x => x.PlayerId == id).ToList();

            foreach (var impactedResult in impactedResults)
            {
                this.context.Results.Remove(impactedResult);
                await this.context.SaveChangesAsync();
            }

            this.context.Players.Remove(player);
            await this.context.SaveChangesAsync();

            return player;
        }

        private async Task RestoreRates()
        {
            foreach (var player in this.context.Players.ToList())
            {
                player.Rate = 1000;
                this.context.Entry(player).State = EntityState.Modified;
                await this.context.SaveChangesAsync();
            }
        }

        private double GetPrizeFond(List<Player> players)
        {
            double fond = 0;

            foreach (var player in players)
            {
                fond += player.Rate * 0.25;
            }

            return fond;
        }

        private int GetSumOfGamePoints(IQueryable<int> scores, int minScore)
        {
            int sum = 0;

            foreach (var score in scores)
            {
                sum += score - minScore;
            }

            return sum == 0 ? 1 : sum;
        }
    }
}
