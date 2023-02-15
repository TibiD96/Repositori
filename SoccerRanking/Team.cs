namespace SoccerRanking
{
    public class Team
    {
        private readonly string name;
        private int points;

        public Team(string name, int points)
        {
            this.name = name;
            this.points = points;
        }

        public void ModifyPoints(Team soccerTeam, int points)
        {
            soccerTeam.points = soccerTeam.points + points;
        }
    }
}