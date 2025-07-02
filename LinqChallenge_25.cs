using static System.Console;
namespace LINQ_Challeges;

public class LinqChallenge_25
{
    private List<(int StudentId, string Name, string Country)> students = new()
    {
        (1, "Ava", "India"), (2, "Ben", "USA"), (3, "Cara", "Germany"),
        (4, "Dan", "India"), (5, "Ella", "USA"), (6, "Finn", "Canada"),
        (7, "Gina", "Germany"), (8, "Hugo", "India"), (9, "Ivy", "USA"), (10, "Jack", "Canada")
    };

    private List<(int CourseId, string Title, string Level)> courses = new()
    {
        (101, "Intro to C#", "Beginner"), (102, "Advanced LINQ", "Advanced"),
        (103, "Entity Framework Core", "Intermediate"), (104, "React Basics", "Beginner"),
        (105, "Microservices with .NET", "Advanced")
    };

    private List<(int StudentId, int CourseId, DateTime EnrolledOn, bool Completed)> enrollments = new()
    {
        (1,101, new(2024,1,5), true), (1,102, new(2024,2,10), true), (1,105, new(2024,3,2), false),
        (2,101, new(2024,1,8), true), (2,103, new(2024,2,5), false),
        (3,102, new(2024,1,15), true), (4,105, new(2024,3,10), false),
        (5,103, new(2024,2,20), true), (6,101, new(2024,1,10), true), (6,104, new(2024,2,15), true),
        (7,104, new(2024,3,2), false), (8,101, new(2024,1,25), true),
        (9,102, new(2024,1,20), false), (10,105, new(2024,3,6), false)
    };

    private List<(int StudentId, int CourseId, int Rating)> reviews = new()
    {
        (1,101, 5), (1,102, 5), (2,101, 4), (3,102, 4),
        (5,103, 3), (6,101, 5), (6,104, 4), (8,101, 5)
    };

    private List<(int StudentId, int CourseId, DateTime Day, int Minutes)> studyLogs = new()
    {
        (1,101, new(2024,1,6), 60), (1,102, new(2024,2,12), 90),
        (1,105, new(2024,3,5), 30), (2,103, new(2024,2,10), 45),
        (3,102, new(2024,1,18), 75), (4,105, new(2024,3,15), 20),
        (5,103, new(2024,2,22), 65), (6,101, new(2024,1,12), 50),
        (6,104, new(2024,2,16), 40), (7,104, new(2024,3,3), 35),
        (8,101, new(2024,1,26), 60), (9,102, new(2024,1,25), 45),
        (10,105, new(2024,3,10), 40)
    };

    public LinqChallenge_25()
    {
        //LevelCompletionRates_T1();
        //TopRatedAdvancedCourses_T2();
        //CountryWiseAvgStudyTime_T3();
        InconsistentStudents_T4();
    }

    // 🔹 Task 1: Completion Rates by Course Level
    // For each course level, calculate % of completed enrollments.
    // ⏱️ Expected time: 12–15 minutes
    // 10:33 - 10-41
    private void LevelCompletionRates_T1()
    {
        var levelCompletionRates = from course in courses
                                   join enrol in enrollments on course.CourseId equals enrol.CourseId
                                   group enrol by course.Level into levelGroup

                                   let enrolmentsCount = levelGroup.Count()
                                   let completionCount = levelGroup.Where(x => x.Completed).Count()
                                   let completionRate = Math.Round((float)completionCount / enrolmentsCount, 2) * 100

                                   orderby completionRate descending

                                   select new
                                   {
                                       Level = levelGroup.Key,
                                       EnrolmentsCount = enrolmentsCount,
                                       CompletionCount = completionCount,
                                       CompletionRate = completionRate
                                   };

        foreach (var item in levelCompletionRates)
        {
            WriteLine($"{item.Level}\t\t{item.EnrolmentsCount} Enrolments\t\t{item.CompletionCount} Completions\t\t{item.CompletionRate}% Completion Rate");
        }
    }

    // 🔹 Task 2: Top Rated Advanced Courses
    // Show courses with Level == "Advanced", with avg rating ≥ 4.5 and at least 2 reviews.
    // ⏱️ Expected time: 10–12 minutes
    // 10:42 - 10:47
    private void TopRatedAdvancedCourses_T2()
    {
        var topRatedAdvCourses = from course in courses
                                 join review in reviews on course.CourseId equals review.CourseId
                                 where course.Level == "Advanced"
                                 group review by course into courseGroup

                                 where courseGroup.Count() >= 2

                                 let avgRating = courseGroup.Average(x => x.Rating)

                                 where avgRating >= 4.5

                                 orderby avgRating descending

                                 select new
                                 {
                                     Course = courseGroup.Key.Title,
                                     AvgRating = Math.Round(avgRating, 2)
                                 };
        foreach (var item in topRatedAdvCourses)
        {
            WriteLine($"{item.Course}\t\t{item.AvgRating} Avg Rating");
        }
    }

    // 🔹 Task 3: Country-wise Average Daily Study Time
    // For each country, average total minutes logged per student.
    // Include only countries with 2+ students and 100+ total minutes.
    // ⏱️ Expected time: 14–17 minutes
    // 10:48 - 10:55
    private void CountryWiseAvgStudyTime_T3()
    {
        var countryAvgStudyTime = from stud in students
                                  join log in studyLogs on stud.StudentId equals log.StudentId

                                  group log by stud.Country into countryGroup

                                  let totalStudyTime = countryGroup.Sum(x => x.Minutes)
                                  let minStudyTime = countryGroup.Min(x => x.Minutes)
                                  let maxStudyTime = countryGroup.Max(x => x.Minutes)
                                  let avgStudyTime = countryGroup.Average(x => x.Minutes)

                                  orderby avgStudyTime descending

                                  select new
                                  {
                                      Country = countryGroup.Key,
                                      TotalStudyTime = totalStudyTime,
                                      MinStudyTime = minStudyTime,
                                      MaxStudyTime = maxStudyTime,
                                      AvgStudyTime = Math.Round(avgStudyTime, 2)
                                  };

        foreach (var item in countryAvgStudyTime)
        {
            WriteLine($"{item.Country}\t\t{item.AvgStudyTime} Avg\t\t{item.TotalStudyTime} Tot\t\t{item.MaxStudyTime} Max\t\t{item.MinStudyTime} min");
        }
    }

    // 🔹 Task 4: Inconsistent Students
    // Identify students who rated a course they didn’t complete.
    // ⏱️ Expected time: 15–18 minutes
    // 10:55 - 11:04
    private void InconsistentStudents_T4()
    {
        var inconsitentStudents = from stud in students
                                  join enrol in enrollments on stud.StudentId equals enrol.StudentId
                                  join review in reviews on new { enrol.StudentId, enrol.CourseId } equals new { review.StudentId, review.CourseId }

                                  where !enrol.Completed

                                  group new { review, enrol } by stud into studGroup

                                  let avgRating = studGroup.Average(x => x.review.Rating)
                                  let incompleteCount = studGroup.Count()

                                  orderby avgRating descending

                                  select new
                                  {
                                      Student = studGroup.Key.Name,
                                      AvgRating = Math.Round(avgRating, 2),
                                      IncompleteRateCount = incompleteCount
                                  };

        foreach (var item in inconsitentStudents)
        {
            WriteLine($"{item.Student}\t\t{item.AvgRating} Avg\t\t{item.IncompleteRateCount} Incompleted Course Ratings");
        }
    }
}
