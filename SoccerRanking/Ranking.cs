using System;
using Xunit.Sdk;

namespace SoccerRanking
{
    public class Ranking
    {
        private Team[] teams;
        
        public Ranking()
        {
            this.teams = new Team[0];
        }

        public void AddTeam(Team soccerTeam)
        {
            Array.Resize(ref teams, teams.Length + 1);
            teams[teams.Length - 1] = soccerTeam;
        }

        public int GetThePositionInRank(Team soccerTeam)
        {
            int positionInRank = 0;
           if (teams.Contains(soccerTeam))
            {
                for (int i = 0; i < teams.Length; i++)
                {
                    if (teams[i] == soccerTeam)
                    {
                        positionInRank = i;

                        break;
                    }
                }
            }

            return positionInRank;

        }

        public Team GetTheTeamFromASpecifiedPosition(int position)
        {
            return teams[position];
        }
    }
}
