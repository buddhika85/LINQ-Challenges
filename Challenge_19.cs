namespace LINQ_Challeges;
using static System.Console;
public class Challenge_19
{
    private List<(int EmpId, string Name)> employees = new()
    {
        (1, "Alice"), (2, "Bob"), (3, "Charlie"), (4, "Diana"), (5, "Ethan"),
        (6, "Fiona"), (7, "George"), (8, "Holly"), (9, "Ian"), (10, "Julia")
    };

    private List<(int TrainingId, string Title, int DurationDays)> trainings = new()
    {
        (101, "Security Awareness", 5),
        (102, "Cloud Fundamentals", 7),
        (103, "Advanced .NET", 10),
        (104, "Data Privacy", 3),
        (105, "Leadership 101", 6)
    };

    private List<(int EmpId, int TrainingId, DateTime EnrolledOn)> enrollments = new()
    {
        (1,101, new DateTime(2024,1,5)), (1,103, new DateTime(2024,2,10)),
        (2,101, new DateTime(2024,1,10)), (2,104, new DateTime(2024,3,1)),
        (3,102, new DateTime(2024,2,15)), (3,103, new DateTime(2024,2,20)),
        (4,105, new DateTime(2024,3,5)), (5,101, new DateTime(2024,1,12)),
        (6,102, new DateTime(2024,2,8)), (7,103, new DateTime(2024,2,11)),
        (8,104, new DateTime(2024,3,12)), (9,105, new DateTime(2024,3,6)),
        (10,102, new DateTime(2024,2,9)), (10,103, new DateTime(2024,2,22))
    };

    private List<(int EmpId, int TrainingId, int Score)> evaluations = new()
    {
        (1,101,85), (1,103,78), (2,101,90), (2,104,88),
        (3,102,70), (3,103,95), (4,105,82), (5,101,60),
        (6,102,75), (7,103,80), (8,104,91), (9,105,85),
        (10,102,89), (10,103,92)
    };

    public Challenge_19()
    {
        //TopScoringEmployees_t1();
        TrainingPopularity_t2();
    }

    //✅ Task 2: Training Popularity
    //- JOIN trainings → enrollments
    //- GROUP BY training
    //- COUNT enrolled employees
    //- ORDER by count DESC
    //- INCLUDE program duration in result
    private void TrainingPopularity_t2()
    {
        var trainingPopularity = from training in trainings
                                 join enrollment in enrollments on training.TrainingId equals enrollment.TrainingId
                                 group enrollment by training into trainingGroup

                                 let count = trainingGroup.Count()

                                 orderby count descending

                                 select new
                                 {
                                     Training = trainingGroup.Key.Title,
                                     Enrollments = count,
                                     Duration = trainingGroup.Key.DurationDays
                                 };

        foreach (var item in trainingPopularity)
        {
            WriteLine($"{item.Training}\t{item.Enrollments} count\t\t{item.Duration} days");
        }
    }

    //✅ Task 1: Top Scoring Employees
    //- JOIN employees → evaluations
    //- GROUP BY employee
    //- AVG their scores
    //- FILTER only those with avg score ≥ 85
    //- ORDER by score descending
    //- Paginate 3 per page
    private void TopScoringEmployees_t1()
    {
        var topScoringEmps = from emp in employees
                             join eval in evaluations on emp.EmpId equals eval.EmpId
                             group eval by emp into empGroup

                             let avg = Math.Round(empGroup.Average(x => x.Score), 1)

                             where avg >= 85
                             orderby avg descending

                             select new
                             {
                                 Employee = empGroup.Key.Name,
                                 Avg = avg
                             };

        var goMore = true;
        var pageNumber = 0;
        var perPage = 3;
        while (goMore)
        {
            var result = topScoringEmps.Skip(pageNumber * perPage).Take(perPage);
            if (result.Any())
            {
                WriteLine($"\n\n[ Page {++pageNumber} ]");
                foreach (var item in result)
                {
                    WriteLine($"{item.Employee}\t{item.Avg}%");
                }
            }
            else
            {
                goMore = false;
            }
        }
    }
}
