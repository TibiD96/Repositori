using System;

namespace SoccerRanking
{
    public class Ranking
    {
        public Team[] teams;
        
        public Ranking()
        {
            this.teams = new Team[0];
        }

        public void AddTeam(Team footbalTeam)
        {
            Array.Resize(ref teams, teams.Length + 1);
            teams[teams.Length - 1] = footbalTeam;
        }
    }
}
