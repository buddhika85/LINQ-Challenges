
using static System.Console;

namespace LINQ_Challeges;

public class Challenge_21
{
    private List<(int EmpId, string Name)> employees = new()
    {
        (1, "Ava"), (2, "Ben"), (3, "Cara"), (4, "Dan"), (5, "Ella"),
        (6, "Finn"), (7, "Gina"), (8, "Hugo"), (9, "Ivy"), (10, "Jack")
    };

    private List<(int ProjId, string Name, string TechStack)> projects = new()
    {
        (201, "Apollo", "Angular"), (202, "Nova", ".NET"), (203, "Zenith", "React"),
        (204, "Chronos", "Java"), (205, "Equinox", "Node.js")
    };

    private new List<(int EmpId, int ProjId)> assignments = new()
    {
        (1,201), (1,202), (2,202), (2,203), (3,204), (3,201),
        (4,205), (5,204), (6,203), (7,201), (8,202), (9,204),
        (10,205), (5,205), (4,201), (6,202), (9,202), (10,203)
    };

    private List<(int EmpId, int ProjId, int WeekNo, int Hours)> timeLogs = new()
    {
        (1,201,1,10), (1,201,2,12), (1,202,1,8), (2,203,1,15),
        (3,204,1,12), (4,205,1,10), (5,205,1,5), (5,204,1,6),
        (6,203,1,14), (6,202,1,7), (7,201,1,9), (8,202,1,11),
        (9,204,1,13), (10,205,1,12), (10,203,1,10)
    };

    private List<(int EmpId, int ProjId, int Score)> reviews = new()
    {
        (1,201,5), (1,202,4), (2,203,4), (3,204,3),
        (4,205,5), (5,205,2), (6,203,5), (6,202,4),
        (7,201,4), (8,202,3), (9,204,5), (10,203,5), (10,205,4)
    };

    public Challenge_21()
    {
        //EmployeeContributionReport();
        //ProjectTechImpactAnalysis();
        ProjectLeaderboardEfficiency();
    }

    //For each employee, calculate total hours contributed across all projects.
    //Sort by total hours descending.
    //Filter only those with 15+ hours.
    //Paginate results: 3 per page.
    private void EmployeeContributionReport()
    {
        var empContribution = from emp in employees
                              join time in timeLogs on emp.EmpId equals time.EmpId
                              group time by emp into empGroup

                              let hoursSum = empGroup.Sum(x => x.Hours)
                              where hoursSum >= 15

                              orderby hoursSum descending
                              select new
                              {
                                  Employee = empGroup.Key.Name,
                                  HoursSum = hoursSum,
                              };


        var recordsPerPage = 3;
        for (var pageNum = 0; ;)
        {
            var results = empContribution.Skip(pageNum * recordsPerPage).Take(recordsPerPage);
            if (results.Any())
            {
                WriteLine($"\n[ Page {++pageNum} ]");
                foreach (var emp in results)
                {
                    WriteLine($"{emp.Employee}\t{emp.HoursSum} hours");
                }
            }
            else
                break;
        }
    }

    //    Task 2: Project Tech Impact Analysis
    //For each tech stack, compute:
    //- Number of distinct employees working on it
    //- Average review score across all its projects
    //Only include stacks with at least 2 projects and 3 reviews total
    //Order by average score descending
    private void ProjectTechImpactAnalysis()
    {
        var projectTechAnalysis = from proj in projects
                                  join assignment in assignments on proj.ProjId equals assignment.ProjId
                                  join review in reviews on proj.ProjId equals review.ProjId

                                  group new { assignment, review } by proj.TechStack into projGroup


                                  let projectReviews = projGroup.Select(x => x.review).Distinct()

                                  let reviewCount = projectReviews.Count()
                                  let projectCount = projectReviews.Select(x => x.ProjId).Distinct().Count()

                                  where projectCount >= 2 && reviewCount >= 3
                                  let empCount = projGroup.Select(x => x.assignment.EmpId).Distinct().Count()
                                  let reviewScoreAvg = projectReviews.Average(x => x.Score)

                                  orderby reviewScoreAvg descending

                                  select new
                                  {
                                      TackStack = projGroup.Key,
                                      EmpCount = empCount,
                                      ReviewScoreAvg = Math.Round(reviewScoreAvg, 2),
                                      ProjectCount = projectCount,
                                      ReviewCount = reviewCount
                                  };

        foreach (var item in projectTechAnalysis)
        {
            WriteLine($"{item.TackStack}\t\t{item.ProjectCount} projects\t\t{item.EmpCount} Employees\t\t{item.ReviewCount} reviews\t\t{item.ReviewScoreAvg} Avg Score");
        }
    }

    // 11:40 AM - 11:55 AM
    //    Task 3: Project Leaderboard by Efficiency
    //For each project:
    //- Compute total hours logged and average review score
    //- Define “efficiency” as average score ÷ hours
    //Only include projects with at least 2 participants
    //Show top 5 by efficiency
    private void ProjectLeaderboardEfficiency()
    {
        var LeaderboardByEfficiency = (from proj in projects
                                       join time in timeLogs on proj.ProjId equals time.ProjId
                                       join review in reviews on new { time.ProjId, time.EmpId } equals new { review.ProjId, review.EmpId }

                                       group new { time, review } by proj into projGroup

                                       let particpantsCount = projGroup.Select(x => x.time.EmpId).Distinct().Count()

                                       where particpantsCount >= 2

                                       let totalHoursForProject = projGroup.Sum(x => x.time.Hours)
                                       let avgScoreForProject = projGroup.Average(x => x.review.Score)
                                       let efficiency = avgScoreForProject / totalHoursForProject



                                       orderby efficiency descending

                                       select new
                                       {
                                           Project = projGroup.Key.Name,
                                           TotalHoursForProject = totalHoursForProject,
                                           AvgScoreForProject = Math.Round(avgScoreForProject, 2),
                                           Efficiency = Math.Round(efficiency, 2),
                                           ParticpantsCount = particpantsCount
                                       }).Take(5);

        foreach (var item in LeaderboardByEfficiency)
        {
            WriteLine($"{item.Project}\t\t{item.ParticpantsCount} Particpants\t\t{item.TotalHoursForProject} Hours\t\t{item.AvgScoreForProject} Avg Score\t\t{item.Efficiency} Efficiency\t\t");
        }
    }
}


