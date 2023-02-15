using System;

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

        public void UpdateTheRankingAfterAMatch (Team homeTeam, int homeTeamGoals, Team awayTeam, int awayTeamGoals)
        {
            int pointsForTheWiner = 3;
            int pointsForEqual = 1;
            if (homeTeamGoals > awayTeamGoals)
            {
                homeTeam.ModifyPoints(homeTeam, pointsForTheWiner);
            }

            if (awayTeamGoals > homeTeamGoals)
            {
                awayTeam.ModifyPoints(awayTeam, pointsForTheWiner);
            }

            if (homeTeamGoals == awayTeamGoals)
            {
                awayTeam.ModifyPoints(awayTeam, pointsForEqual);
                homeTeam.ModifyPoints(homeTeam, pointsForEqual);
            }
        }


    }
}
