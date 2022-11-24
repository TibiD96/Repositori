using System;

namespace RunningContest
{
    struct Contestant
    {
        public string Name;
        public string Country;
        public double Time;

        public Contestant(string name, string country, double time)
        {
            this.Name = name;
            this.Country = country;
            this.Time = time;
        }
    }

    struct ContestRanking
    {
        public Contestant[] Contestants;
    }

    struct Contest
    {
        public ContestRanking[] Series;
        public ContestRanking GeneralRanking;
    }

    class Program
    {
        static void Main()
        {
            Contest contest = ReadContestSeries();
            GenerateGeneralRanking(ref contest);
            Print(contest.GeneralRanking);
            Console.Read();
        }

        private static void Print(ContestRanking contestRanking)
        {
            for (int i = 0; i < contestRanking.Contestants.Length; i++)
            {
                Contestant contestant = contestRanking.Contestants[i];
                const string line = "{0} - {1} - {2:F3}";
                Console.WriteLine(string.Format(line, contestant.Name, contestant.Country, contestant.Time));
            }
        }

        static void GenerateGeneralRanking(ref Contest contest)
        {
            int generalRankingContestantsNumber = contest.Series.Length * contest.Series[0].Contestants.Length;
            int i = 0;
            Contest finalRank = new Contest();
            finalRank.Series = new ContestRanking[1];
            finalRank.Series[0].Contestants = new Contestant[generalRankingContestantsNumber];
            int j = 0;
            for (int seriesCounter = 0; seriesCounter < contest.Series.Length; seriesCounter++)
            {
                while (i < contest.Series[0].Contestants.Length)
                {
                    finalRank.Series[0].Contestants[j] = contest.Series[seriesCounter].Contestants[i];
                    i++;
                    j++;
                }

                i = 0;
            }

            finalRank = Sorting(finalRank);
            contest.GeneralRanking = finalRank.Series[0];
        }

        static Contest Sorting(Contest finalRank)
        {
            const int two = 2;
            Contest tempStart = new Contest();
            tempStart.Series = new ContestRanking[1];
            Contest tempEnd = new Contest();
            tempEnd.Series = new ContestRanking[1];
            Contest result = new Contest();
            result.Series = new ContestRanking[1];
            if (finalRank.Series[0].Contestants.Length <= 1)
            {
                return finalRank;
            }

            int mid = finalRank.Series[0].Contestants.Length / 2;
            tempStart.Series[0].Contestants = new Contestant[mid];
            tempEnd.Series[0].Contestants = finalRank.Series[0].Contestants.Length % two == 0 ? (new Contestant[mid]) : (new Contestant[mid + 1]);

            for (int i = 0; i < mid; i++)
            {
                tempStart.Series[0].Contestants[i] = finalRank.Series[0].Contestants[i];
            }

            int x = 0;

            for (int i = mid; i < finalRank.Series[0].Contestants.Length; i++)
            {
                tempEnd.Series[0].Contestants[x] = finalRank.Series[0].Contestants[i];
                x++;
            }

            tempStart = Sorting(tempStart);
            tempEnd = Sorting(tempEnd);
            result = Merging(tempStart, tempEnd);
            return result;
        }

        static Contest Merging(Contest tempStart, Contest tempEnd)
        {
            int totalContestants = tempStart.Series[0].Contestants.Length + tempEnd.Series[0].Contestants.Length;
            Contest result = new Contest();
            result.Series = new ContestRanking[1];
            result.Series[0].Contestants = new Contestant[totalContestants];
            int indexStart = 0;
            int indexEnd = 0;
            int indexResult = 0;

            while (indexStart < tempStart.Series[0].Contestants.Length || indexEnd < tempEnd.Series[0].Contestants.Length)
            {
                if (indexStart < tempStart.Series[0].Contestants.Length && indexEnd < tempEnd.Series[0].Contestants.Length)
                {
                    if (tempStart.Series[0].Contestants[indexStart].Time <= tempEnd.Series[0].Contestants[indexEnd].Time)
                    {
                        result.Series[0].Contestants[indexResult] = tempStart.Series[0].Contestants[indexStart];
                        indexStart++;
                        indexResult++;
                    }
                    else
                    {
                        result.Series[0].Contestants[indexResult] = tempEnd.Series[0].Contestants[indexEnd];
                        indexEnd++;
                        indexResult++;
                    }
                }
                else if (indexStart < tempStart.Series[0].Contestants.Length)
                {
                    result.Series[0].Contestants[indexResult] = tempStart.Series[0].Contestants[indexStart];
                    indexStart++;
                    indexResult++;
                }
                else if (indexEnd < tempEnd.Series[0].Contestants.Length)
                {
                    result.Series[0].Contestants[indexResult] = tempEnd.Series[0].Contestants[indexEnd];
                    indexEnd++;
                    indexResult++;
                }
            }

            return result;
        }

        static Contest ReadContestSeries()
        {
            Contest contest = new Contest();

            int seriesNumber = Convert.ToInt32(Console.ReadLine());
            int contestantsPerSeries = Convert.ToInt32(Console.ReadLine());

            contest.Series = new ContestRanking[seriesNumber];

            for (int i = 0; i < seriesNumber; i++)
            {
                contest.Series[i].Contestants = new Contestant[contestantsPerSeries];
                for (int j = 0; j < contestantsPerSeries; j++)
                {
                    string contestantLine = "";

                    while (contestantLine == "")
                    {
                        contestantLine = Console.ReadLine();
                    }

                    contest.Series[i].Contestants[j] = CreateContestant(contestantLine.Split('-'));
                }
            }

            return contest;
        }

        private static Contestant CreateContestant(string[] contestantData)
        {
            const int nameIndex = 0;
            const int countryIndex = 1;
            const int timeIndex = 2;

            return new Contestant(
                contestantData[nameIndex].Trim(),
                contestantData[countryIndex].Trim(),
                Convert.ToDouble(contestantData[timeIndex]));
        }
    }
}