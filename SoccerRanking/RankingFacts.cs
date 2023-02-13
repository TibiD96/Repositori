using Xunit;

namespace SoccerRanking
{
    
    public class RankingFacts
    {
        [Fact]
        public void CorrectAddTeamsInRank()
        {
            Team firstTeam = new Team("CFR CLUJ", 3);
            Team secondTeam = new Team("Jiu Petrosani", 1);
            Team thirdTeam = new Team("Dinamo", 2);
            Ranking ranking = new Ranking();
            ranking.AddTeam(firstTeam);
            ranking.AddTeam(secondTeam);
            ranking.AddTeam(thirdTeam);

            Assert.Equal(secondTeam, ranking.teams[1]);
        }

    }
}
