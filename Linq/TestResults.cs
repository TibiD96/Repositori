using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class TestResults
    {
        public string Id { get; set; }

        public string FamilyId { get; set; }

        public int Score { get; set; }

        public TestResults(string id, string familyId, int score)
        {
            Id = id;
            FamilyId = familyId;
            Score = score;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            TestResults testResult = (TestResults)obj;
            return Id == testResult.Id && FamilyId == testResult.FamilyId && Score == testResult.Score;
        }

        public override int GetHashCode() => HashCode.Combine(Id, FamilyId, Score);
    }
}
