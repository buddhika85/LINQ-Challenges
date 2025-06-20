using LINQ_Challeges.Models;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using static System.Console;

namespace LINQ_Challeges
{
    public class Challenge_18
    {
        #region dateHelpers
        static DateTime Jan(int day) => new DateTime(2024, 1, day);
        static DateTime Feb(int day) => new DateTime(2024, 2, day);
        static DateTime Mar(int day) => new DateTime(2024, 3, day);
        static DateTime Apr(int day) => new DateTime(2024, 4, day);
        static DateTime Jun(int day) => new DateTime(2024, 6, day);
        static DateTime Jul(int day) => new DateTime(2024, 7, day);
        static DateTime Sep(int day) => new DateTime(2024, 9, day);
        static DateTime Nov(int day) => new DateTime(2024, 11, day);
        static DateTime Dec(int day) => new DateTime(2024, 12, day);
        #endregion 
        private List<(int DevId, string Name)> developers = new()
        {
            (1,"Alice"), (2,"Bob"), (3,"Charlie"), (4,"David"), (5,"Eve"),
            (6,"Frank"), (7,"Grace"), (8,"Hank"), (9,"Ivy"), (10,"Jack"),
            (11,"Karen"), (12,"Leo"), (13,"Mia"), (14,"Nora"), (15,"Omar")
        };

        private List<(int ProjectId, string Title, DateTime Start, DateTime End, decimal Budget)> projects = new()
        {
            (101, "Billing System", Jan(1), Jun(30), 50000),
            (102, "E-Commerce Platform", Feb(15), Sep(1), 80000),
            (103, "Chat AI SDK", Mar(5), Jul(20), 35000),
            (104, "HR Portal", Jan(10), Dec(15), 120000),
            (105, "Customer Insights", Apr(1), Nov(30), 60000)
        };

        List<(int DevId, int ProjectId, int HoursPerWeek)> assignments = new()
        {
            (1,101,20), (1,102,15), (2,101,25), (3,103,30), (3,104,10),
            (4,105,35), (5,102,10), (5,105,20), (6,104,25), (6,105,10),
            (7,103,40), (8,101,20), (8,104,10), (9,105,30), (10,102,15),
            (11,101,20), (12,104,25), (13,102,35), (14,103,30), (15,105,20)
        };

        List<(int DevId, int ProjectId, int Score)> reviews = new()
        {
            (1,101,5), (1,102,4), (2,101,4), (3,103,5), (3,104,3),
            (4,105,5), (5,105,2), (6,104,4), (7,103,4), (8,101,4),
            (9,105,5), (10,102,3), (11,101,5), (12,104,4), (13,102,5),
            (14,103,5), (15,105,3)
        };

        public Challenge_18()
        {
            //TopContributors_T1();
            //TopQualityProject_T2();
            TopReviewdDevs_T3();
        }


        // 🔸 Task 3: Developer Efficiency
        //- Combine assignments and reviews per dev-project pair
        //- Calculate hours per review point
        //- SORT lowest-to-highest for most efficient contributors
        //- PAGINATE top 5 efficiency score
        private void TopReviewdDevs_T3()
        {
            var topReviewedDevs = (from dev in developers
                                   join task in assignments on dev.DevId equals task.DevId
                                   join review in reviews on new { dev.DevId, task.ProjectId } equals new { review.DevId, review.ProjectId }
                                   group new { review, task } by dev into devGroup

                                   let totalHours = devGroup.Sum(x => x.task.HoursPerWeek)
                                   let totalScore = devGroup.Sum(x => x.review.Score)
                                   let efficiency = Math.Round((float)totalHours / totalScore, 2)

                                   orderby efficiency ascending

                                   select new
                                   {
                                       Dev = devGroup.Key.Name,
                                       TotalHours = totalHours,
                                       TotalScore = totalScore,
                                       Efficiency = efficiency
                                   }).Take(5);

            foreach (var item in topReviewedDevs)
            {
                WriteLine($"{item.Dev}\t\tHours:{item.TotalHours}\t\tScore:{item.TotalScore}\t\tEfficiecy:{item.Efficiency}");
            }
        }

        // 🚀 Challenge Set
        //🔸 Task 1: Top Contributors by Hours
        //- JOIN developers → assignments
        //- GROUP BY developer
        //- SUM all HoursPerWeek
        //- FILTER: only those contributing 50+ hours/week
        //- ORDER BY total hours DESCENDING
        //- Paginate results(3 per page)
        private void TopContributors_T1()
        {
            var topContributors = from dev in developers
                                  join task in assignments on dev.DevId equals task.DevId
                                  group task by dev into devGroup

                                  let totalHoursPerWeek = devGroup.Sum(x => x.HoursPerWeek)
                                  where totalHoursPerWeek > 50

                                  orderby totalHoursPerWeek descending
                                  select new
                                  {
                                      Developer = devGroup.Key.Name,
                                      TotalHoursPerWeek = totalHoursPerWeek
                                  };

            var resultsPerPage = 3;
            var goMore = true;
            var pageNumber = 0;
            while (goMore)
            {
                var results = topContributors.Skip(pageNumber * resultsPerPage).Take(resultsPerPage);
                if (results.Any())
                {
                    WriteLine($"\n\nPage [ {++pageNumber} ]");
                    foreach (var result in results)
                    {
                        WriteLine($"{result.Developer}\t\t{result.TotalHoursPerWeek} hours");
                    }
                }
                else
                {
                    goMore = false;
                }
            }
        }



        // Task 2: Project Quality Rankings
        //- JOIN developers → reviews → projects
        //- GROUP BY project
        //- CALCULATE:
        //- Average review score
        //- Total reviewers
        //- FILTER: only projects with at least 3 reviews
        //- ORDER BY average score DESCENDING
        public void TopQualityProject_T2()
        {
            var topQualityProjects = from project in projects
                                     join review in reviews on project.ProjectId equals review.ProjectId
                                     group review by project into projectGroup

                                     let avg = Math.Round(projectGroup.Average(x => x.Score), 1)
                                     let count = projectGroup.Count()

                                     where count >= 3
                                     orderby avg descending
                                     select new
                                     {
                                         Project = projectGroup.Key.Title,
                                         AvgScore = avg,
                                         ReviewCount = count
                                     };

            foreach (var item in topQualityProjects)
            {
                WriteLine($"{item.Project}\t\tAvg: {item.AvgScore}\t\tCount: {item.ReviewCount}");
            }
        }

    }
}
