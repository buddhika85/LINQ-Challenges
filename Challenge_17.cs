using static System.Console;
namespace LINQ_Challeges;
public class Challenge_17
{

    private List<(int UserId, string Name)> users = new()
    {
        (1, "Alice"), (2, "Bob"), (3, "Charlie"), (4, "David"), (5, "Eve"),
        (6, "Frank"), (7, "Grace"), (8, "Hank"), (9, "Ivy"), (10, "Jack")
    };

    private List<(int CourseId, string Title)> courses = new()
    {
        (101, "C# Basics"), (102, "ASP.NET Core"), (103, "LINQ Mastery"),
        (104, "Angular Fundamentals"), (105, "SQL Optimization")
    };

    private List<(int UserId, int CourseId, DateTime EnrolledOn)> enrollments = new()
    {
        (1, 101, DateTime.Now.AddDays(-60)), (1, 103, DateTime.Now.AddDays(-30)),
        (2, 101, DateTime.Now.AddDays(-50)), (2, 102, DateTime.Now.AddDays(-20)),
        (3, 103, DateTime.Now.AddDays(-10)), (3, 105, DateTime.Now.AddDays(-35)),
        (4, 104, DateTime.Now.AddDays(-25)), (5, 102, DateTime.Now.AddDays(-5)),
        (6, 105, DateTime.Now.AddDays(-90)), (7, 101, DateTime.Now.AddDays(-15)),
        (8, 104, DateTime.Now.AddDays(-10)), (9, 103, DateTime.Now.AddDays(-45)),
        (10, 105, DateTime.Now.AddDays(-80))
    };

    private List<(int UserId, int CourseId, int LessonsCompleted, int TotalLessons)> progress = new()
    {
        (1, 101, 10, 10), (1, 103, 8, 10), (2, 101, 5, 10), (2, 102, 6, 8),
        (3, 103, 10, 10), (3, 105, 5, 10), (4, 104, 8, 8), (5, 102, 3, 8),
        (6, 105, 6, 10), (7, 101, 10, 10), (8, 104, 2, 8), (9, 103, 9, 10),
        (10, 105, 7, 10)
    };

    private List<(int UserId, int CourseId, int Rating)> feedback = new()
    {
        (1, 101, 5), (1, 103, 4), (2, 101, 4), (2, 102, 3),
        (3, 103, 5), (4, 104, 5), (6, 105, 3), (7, 101, 4),
        (9, 103, 5), (10, 105, 4)
    };

    public Challenge_17()
    {
        //TopCourseCompleters_T1();
        //sMostActiveEnrollers_T2();
        ConsistantHighRatings_T3();
    }


    // 📘 Task 3: Consistently High Ratings
    //- JOIN feedback, users
    //- GROUP by user
    //- CALCULATE average rating
    //- FILTER only users with all ratings ≥ 4
    //- ORDER by average rating descending
    private void ConsistantHighRatings_T3()
    {
        var highRatedUsers = from user in users
                             join feedbk in feedback on user.UserId equals feedbk.UserId
                             group feedbk by user into userGroup

                             let total = userGroup.Sum(x => x.Rating)
                             let avg = userGroup.Average(x => x.Rating)
                             let min = userGroup.Min(x => x.Rating)
                             let max = userGroup.Max(x => x.Rating)

                             where avg >= 4
                             orderby avg descending
                             select new
                             {
                                 User = userGroup.Key.Name,
                                 TotalFeedback = total,
                                 MinFeedback = min,
                                 MaxFeedback = max,
                                 AvgFeedback = Math.Round(avg, 1),
                             };

        foreach (var item in highRatedUsers)
        {
            WriteLine($"{item.User}\t\tTotal:{item.TotalFeedback}\t\tMin:{item.MinFeedback}\t\tMax:{item.MaxFeedback}\t\tAvg:{item.AvgFeedback}");
        }
    }


    // 📘 Task 2: Most Active Enrollers
    //- GROUP enrollments by user
    //- Use let to count courses enrolled
    //- FILTER users enrolled in 3 or more courses
    //- ORDER by number of courses enrolled
    //- Paginate results: 2 per page
    public void MostActiveEnrollers_T2()
    {
        var mostActiveEnrollers = from user in users
                                  join enrollment in enrollments on user.UserId equals enrollment.UserId
                                  group enrollment by user into userGroup

                                  let enrolmentsCount = userGroup.Count()

                                  where enrolmentsCount >= 3
                                  orderby enrolmentsCount descending

                                  select new
                                  {
                                      User = userGroup.Key.Name,
                                      EnrolmentCount = enrolmentsCount
                                  };

        var goMore = true;
        var resultsPerPage = 2;
        var pageNumber = 0;
        while (goMore)
        {
            var results = mostActiveEnrollers.Skip(pageNumber * resultsPerPage).Take(resultsPerPage);
            if (results.Any())
            {
                WriteLine($"\nPage [ {++pageNumber} ]");
                foreach (var item in results)
                {
                    WriteLine($"{item.User}\t{item.EnrolmentCount} Enrollments");
                }
            }
            else
            {
                goMore = false;
            }
        }
    }





    // 📘 Task 1: Top Course Completers
    //- JOIN users, progress, courses
    //- GROUP by user
    //- Use let to calculate total and completed lessons
    //- Sort by highest completion percentage
    //- Filter: only users who completed more than 70% across all enrolled courses
    //- Show top 5 users
    private void TopCourseCompleters_T1()
    {
        var topCompleters = (from user in users
                             join enrolment in enrollments on user.UserId equals enrolment.UserId
                             join prog in progress on enrolment.CourseId equals prog.CourseId
                             group new { enrolment, prog } by user into userGroup

                             let totalLessons = userGroup.Sum(x => x.prog.TotalLessons)
                             let completedLessons = userGroup.Sum(x => x.prog.LessonsCompleted)
                             let completionPerc = Math.Round((float)completedLessons / totalLessons * 100, 2)

                             orderby completionPerc descending

                             select new
                             {
                                 User = userGroup.Key.Name,
                                 Total = totalLessons,
                                 Completed = completedLessons,
                                 CompletedPer = completionPerc,
                             }).Take(5);

        foreach (var item in topCompleters)
        {
            WriteLine($"{item.User}\t\tTotal:{item.Total}\t\tCompleted:{item.Completed}\t\tPerc:{item.CompletedPer}%");
        }

    }
}