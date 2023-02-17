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

        public void ModifyPoints(int newPointsValue)
        {
            points += newPointsValue;
        }

        public bool CompareTeamsBasedOnPoints(Team secondTeamInOrder)
        {
            if (secondTeamInOrder == null)
            {
                return false;
            }

            return points < secondTeamInOrder.points;
        }
    }
}