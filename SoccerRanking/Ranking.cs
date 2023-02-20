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

            Sorting();
        }

        public int GetPosition(Team soccerTeam)
        {
           int positionInRank = 0;
           for (int i = 0; i < teams.Length; i++)
           {
             if (teams[i] == soccerTeam)
             {
               positionInRank = i;

               break;
             }
           }
       

            return positionInRank;

        }

        public Team TeamAtPosition(int position)
        {
            return teams[position];
        }

        public void UpdateTheRankingAfterAMatch (Team homeTeam, int homeTeamGoals, Team awayTeam, int awayTeamGoals)
        {
            int pointsForTheWiner = 3;
            int pointsForEqual = 1;
            if (homeTeamGoals > awayTeamGoals)
            {
                homeTeam.ModifyPoints(pointsForTheWiner);
            }

            if (awayTeamGoals > homeTeamGoals)
            {
                awayTeam.ModifyPoints(pointsForTheWiner);
            }

            if (homeTeamGoals == awayTeamGoals)
            {
                awayTeam.ModifyPoints(pointsForEqual);
                homeTeam.ModifyPoints(pointsForEqual);
            }

            Sorting();

        }

        public void Sorting()
        {
            Team pivotTeam;
            bool rankHasToBeSorted = true;
            while (rankHasToBeSorted)
            {
                rankHasToBeSorted = false;
                for (int i = 0; i < teams.Length - 1; i++)
                {
                    if (teams[i].CompareTeamsBasedOnPoints(teams[i + 1]))
                    {
                        pivotTeam = teams[i + 1];
                        teams[i + 1] = teams[i];
                        teams[i] = pivotTeam;
                        rankHasToBeSorted = true;
                    }
                }
            }
        }


    }
}
