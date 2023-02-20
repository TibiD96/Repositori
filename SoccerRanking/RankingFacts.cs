using Xunit;

namespace SoccerRanking
{
    
    public class RankingFacts
    {
        [Fact]
        public void CorrectAddTeamsInRank()
        {
            Team firstTeam = new Team("CFR CLUJ", 3);
            Ranking ranking = new Ranking();
            ranking.AddTeam(firstTeam);

            Assert.Equal(firstTeam, ranking.TeamAtPosition(ranking.GetPosition(firstTeam)));
        }

        [Fact]
        public void GiveTheRightPositionForASpecifiedTeam()
        {
            Team firstTeam = new Team("CFR CLUJ", 3);
            Team secondTeam = new Team("Univesitatea CLUJ", 2);
            Team thirdTeam = new Team("Dinamo", 5);
            Team fourthTeam = new Team("Jiu", 1);
            Ranking ranking = new Ranking();
            ranking.AddTeam(firstTeam);
            ranking.AddTeam(secondTeam);
            ranking.AddTeam(thirdTeam);
            ranking.AddTeam(fourthTeam);

            Assert.Equal(0, ranking.GetPosition(thirdTeam));
        }

        [Fact]
        public void GiveTheRightTeamForASpecifiedPosition()
        {
            Team firstTeam = new Team("CFR CLUJ", 3);
            Team secondTeam = new Team("Univesitatea CLUJ", 2);
            Team thirdTeam = new Team("Dinamo", 5);
            Team fourthTeam = new Team("Jiu", 1);
            Ranking ranking = new Ranking();
            ranking.AddTeam(firstTeam);
            ranking.AddTeam(secondTeam);
            ranking.AddTeam(thirdTeam);
            ranking.AddTeam(fourthTeam);

            Assert.Equal(fourthTeam, ranking.TeamAtPosition(3));
        }

        [Fact]
        public void CorrectModifyPointsIfTheResultOfMatchIsNotEqual()
        {
            Team firstTeam = new Team("CFR CLUJ", 3);
            Team secondTeam = new Team("Univesitatea CLUJ", 2);
            Team thirdTeam = new Team("Dinamo", 1);
            Ranking ranking = new Ranking();
            ranking.AddTeam(firstTeam);
            ranking.AddTeam(secondTeam);
            ranking.AddTeam(thirdTeam);
            ranking.UpdateTheRankingAfterAMatch(firstTeam, 1, secondTeam, 2);

            Assert.Equal(0, ranking.GetPosition(secondTeam));
            Assert.Equal(1, ranking.GetPosition(firstTeam));
        }

        [Fact]
        public void CorrectModifyPointsIfTheResultOfMatchIsEqual()
        {
            Team firstTeam = new Team("CFR CLUJ", 3);
            Team secondTeam = new Team("Univesitatea CLUJ", 2);
            Team thirdTeam = new Team("Dinamo", 1);
            Ranking ranking = new Ranking();
            ranking.AddTeam(firstTeam);
            ranking.AddTeam(secondTeam);
            ranking.AddTeam(thirdTeam);
            ranking.UpdateTheRankingAfterAMatch(secondTeam, 1, thirdTeam, 1);

            Assert.Equal(1, ranking.GetPosition(secondTeam));
            Assert.Equal(2, ranking.GetPosition(thirdTeam));
        }

        [Fact]
        public void RankingIsUpdatingCorrecctlyAfterMultipleMatch()
        {
            Team firstTeam = new Team("CFR CLUJ", 3);
            Team secondTeam = new Team("Univesitatea CLUJ", 4);
            Team thirdTeam = new Team("Dinamo", 3);
            Team fourthTeam = new Team("Steaua", 5);
            Team fifthTeam = new Team("Jiu", 2);
            Team sixthTeam = new Team("Craiova", 7);
            Ranking ranking = new Ranking();
            ranking.AddTeam(firstTeam);
            ranking.AddTeam(secondTeam);
            ranking.AddTeam(thirdTeam);
            ranking.AddTeam(fourthTeam);
            ranking.AddTeam(fifthTeam);
            ranking.AddTeam(sixthTeam);
            ranking.UpdateTheRankingAfterAMatch(fourthTeam, 4, sixthTeam, 1);
            ranking.UpdateTheRankingAfterAMatch(thirdTeam, 1, secondTeam, 1);

            Assert.Equal(0, ranking.GetPosition(fourthTeam));
            Assert.Equal(1, ranking.GetPosition(sixthTeam));
            Assert.Equal(2, ranking.GetPosition(secondTeam));
            Assert.Equal(3, ranking.GetPosition(thirdTeam));
            Assert.Equal(4, ranking.GetPosition(firstTeam));
            Assert.Equal(5, ranking.GetPosition(fifthTeam));
        }

    }
}
