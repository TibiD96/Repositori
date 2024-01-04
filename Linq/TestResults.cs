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
    }
}
