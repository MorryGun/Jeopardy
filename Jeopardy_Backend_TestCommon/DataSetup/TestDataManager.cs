using Jeopardy_Backend;
using Jeopardy_Backend.Models;
using System;

namespace Jeopardy_Backend_TestCommon.DataSetup
{
    public class TestDataManager
    {
        public JeopardyContext context;

        public TestDataManager(JeopardyContext context)
        {
            this.context = context;
        }

        public void AddTestData()
        {
            this.AddPlayers();
            this.AddCompetitions();
            this.AddMatches();
            this.AddResults();
        }

        private void AddPlayers()
        {
            this.context.Players.Add(new Player() { Name = "Anna" });
            this.context.Players.Add(new Player() { Name = "Bertrand" });
            this.context.Players.Add(new Player() { Name = "Cecilia" });
            this.context.Players.Add(new Player() { Name = "Dave" });
            this.context.Players.Add(new Player() { Name = "Elly" });
            this.context.SaveChanges();
        }

        private void AddCompetitions()
        {
            this.context.Competitions.Add(new Competition() { Name = "FirstCompetition", Date = new DateTime(2020, 7, 12) });
            this.context.Competitions.Add(new Competition() { Name = "SecondCompetition", Date = new DateTime(2021, 3, 2) });
            this.context.Competitions.Add(new Competition() { Name = "ThirdCompetition", Date = new DateTime(2021, 4, 26) });
            this.context.SaveChanges();
        }

        private void AddMatches()
        {
            this.context.Matches.Add(new Match() { CompetitionId = 1 });
            this.context.Matches.Add(new Match() { CompetitionId = 1 });
            this.context.Matches.Add(new Match() { CompetitionId = 1 });
            this.context.Matches.Add(new Match() { CompetitionId = 2 });
            this.context.Matches.Add(new Match() { CompetitionId = 2 });
            this.context.SaveChanges();
        }

        private void AddResults()
        {
            this.context.Results.Add(new Result() { MatchId = 1, PlayerId = 1, Score = 0 });
            this.context.Results.Add(new Result() { MatchId = 1, PlayerId = 2, Score = 0 });
            this.context.Results.Add(new Result() { MatchId = 1, PlayerId = 3, Score = 0 });
            this.context.Results.Add(new Result() { MatchId = 1, PlayerId = 4, Score = 0 });

            this.context.Results.Add(new Result() { MatchId = 2, PlayerId = 1, Score = 10 });
            this.context.Results.Add(new Result() { MatchId = 2, PlayerId = 3, Score = -200 });
            this.context.Results.Add(new Result() { MatchId = 2, PlayerId = 4, Score = 30 });
            this.context.Results.Add(new Result() { MatchId = 2, PlayerId = 5, Score = 0 });

            this.context.Results.Add(new Result() { MatchId = 3, PlayerId = 2, Score = 20 });
            this.context.Results.Add(new Result() { MatchId = 3, PlayerId = 3, Score = 0 });
            this.context.Results.Add(new Result() { MatchId = 3, PlayerId = 4, Score = -10 });
            this.context.Results.Add(new Result() { MatchId = 3, PlayerId = 5, Score = 70 });

            this.context.Results.Add(new Result() { MatchId = 4, PlayerId = 1, Score = 10 });
            this.context.Results.Add(new Result() { MatchId = 4, PlayerId = 2, Score = 10 });
            this.context.Results.Add(new Result() { MatchId = 4, PlayerId = 3, Score = 10 });
            this.context.Results.Add(new Result() { MatchId = 4, PlayerId = 5, Score = 0 });

            this.context.Results.Add(new Result() { MatchId = 5, PlayerId = 2, Score = 20 });
            this.context.Results.Add(new Result() { MatchId = 5, PlayerId = 3, Score = 30 });
            this.context.Results.Add(new Result() { MatchId = 5, PlayerId = 4, Score = 10 });
            this.context.SaveChanges();
        }
    }
}
