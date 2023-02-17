using Xunit;

namespace SoccerRanking
{
    public class TeamFacts
    {
        [Fact]
        public void CorrectlyUpdatePointsOfATeam()
        {
            Team firstTeam = new Team("Dinamo", 5);
            Team secondTeam = new Team("Steaua", 3);
            secondTeam.ModifyPoints(3);
            Assert.True(firstTeam.CompareTeamsBasedOnPoints(secondTeam));
        }
    }
}
