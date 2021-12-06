namespace Jeopardy_Backend.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Score { get; set; }
    }
}
