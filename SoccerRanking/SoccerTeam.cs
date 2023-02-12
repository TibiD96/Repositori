namespace SoccerRanking
{
    public class SoccerTeam
    {
        private readonly string name;
        private int points;

        public SoccerTeam(string name, int points)
        {
            this.name = name;
            this.points = points;
        }
    }
}