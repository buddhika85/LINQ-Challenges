using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_26
{
    private List<(int EmpId, string Name, string Dept)> employees = new()
    {
        (1, "Ava", "Engineering"), (2, "Ben", "Engineering"), (3, "Cara", "Design"),
        (4, "Dan", "QA"), (5, "Ella", "QA"), (6, "Finn", "Engineering"),
        (7, "Gina", "Design"), (8, "Hugo", "Product"), (9, "Ivy", "Product"), (10, "Jack", "Design")
    };

    private List<(int ProjId, string Name, string Dept, string Status)> projects = new()
    {
        (101, "Apollo", "Engineering", "Active"), (102, "Zeus", "QA", "Completed"),
        (103, "Hermes", "Design", "Active"), (104, "Athena", "Product", "Completed"),
        (105, "Poseidon", "Engineering", "Active"), (106, "Hera", "Design", "On Hold")
    };

    private List<(int EmpId, int ProjId, int Hours, bool IsBillable)> contributions = new()
    {
        (1,101, 30,true), (2,101, 25,true), (3,103, 20,false), (4,102, 28,true),
        (5,102, 15,true), (6,105, 40,true), (7,103, 22,false), (8,104, 10,true),
        (9,104, 18,false), (10,106, 12,true), (3,106, 8,true), (2,105, 12,true),
        (1,105, 10,false), (5,102, 5,false), (7,103, 5,true)
    };

    private List<(int ProjId, int ReviewerId, int Score)> reviews = new()
    {
        (101, 8, 4), (101, 9, 5), (102, 3, 4), (102, 1, 3),
        (103, 6, 5), (105, 2, 4), (105, 7, 5), (106, 5, 3), (106, 9, 4)
    };

    public LinqChallenge_26()
    {
        //DepartmentalImpact_T1();
        //HighEffortBillables_T2();
        //ProjectRatingsWithThreshold_T3();
        TopDepartmentsByHours_T4();
    }

    // 🔹 Task 1: Departmental Impact
    // Group contributions by project department and total up billable hours.
    // ⏱️ Expected time: 10–12 min
    // 12:06 - 12:11
    private void DepartmentalImpact_T1()
    {
        var deptImpact = from contri in contributions
                         join proj in projects on contri.ProjId equals proj.ProjId

                         where contri.IsBillable
                         group contri by proj.Dept into deptGroup

                         let totalBillableHrs = deptGroup.Sum(x => x.Hours)
                         orderby totalBillableHrs descending
                         select new
                         {
                             Department = deptGroup.Key,
                             TotalBillableHrs = totalBillableHrs
                         };

        foreach (var item in deptImpact)
        {
            WriteLine($"{item.Department}\t\t{item.TotalBillableHrs} total billable hours");
        }
    }

    // 🔹 Task 2: High-Effort Billables
    // Find employees who contributed ≥ 30 billable hours across projects.
    // Paginate in groups of 2.
    // ⏱️ Expected time: 12–15 min
    // 12:11 - 12:18
    private void HighEffortBillables_T2()
    {
        var highEffortEmps = (from emp in employees
                              join contri in contributions on emp.EmpId equals contri.EmpId

                              where contri.IsBillable
                              group contri by emp into empGroup

                              let totalBillableHours = empGroup.Sum(x => x.Hours)

                              where totalBillableHours >= 30
                              orderby totalBillableHours descending
                              select new
                              {
                                  Employee = empGroup.Key.Name,
                                  TotalBillableHours = totalBillableHours
                              }).ToList();           // immediate execution

        var perPage = 2;
        for (var pageNum = 0; ;)
        {
            var results = highEffortEmps.Skip(pageNum * perPage).Take(perPage);
            if (!results.Any())
                break;
            WriteLine($"\nPage [ {++pageNum} ]");
            foreach (var item in results)
            {
                WriteLine($"{item.Employee}\t\t{item.TotalBillableHours}");
            }
        }
    }

    // 🔹 Task 3: Project Ratings With Threshold
    // Show average scores only for projects with ≥ 2 reviews and score ≥ 4.2
    // ⏱️ Expected time: 10–12 min
    // 12:18 - 12:26
    private void ProjectRatingsWithThreshold_T3()
    {
        var projRatesWithThreshold = from proj in projects
                                     join review in reviews on proj.ProjId equals review.ProjId
                                     group review by proj into projGroup

                                     let reviewCount = projGroup.Count()
                                     let avgScore = projGroup.Average(x => x.Score)

                                     where reviewCount >= 2 && avgScore >= 4.2

                                     orderby avgScore descending

                                     select new
                                     {
                                         Project = projGroup.Key.Name,
                                         ReviewCount = reviewCount,
                                         AvgScore = Math.Round(avgScore, 2)
                                     };

        foreach (var item in projRatesWithThreshold)
        {
            WriteLine($"{item.Project}\t\t{item.ReviewCount} reviews\t\t{item.AvgScore} Average Score");
        }
    }

    // 🔹 Task 4: Top Departments by Total Hours
    // Aggregate total hours by department, ordered descending
    // ⏱️ Expected time: 10 min
    // 12:26 - 12:31
    private void TopDepartmentsByHours_T4()
    {
        var topDepartmentByHours = from emp in employees
                                   join contri in contributions on emp.EmpId equals contri.EmpId
                                   group contri by emp.Dept into deptGroup

                                   let totalHours = deptGroup.Sum(x => x.Hours)

                                   orderby totalHours descending

                                   select new
                                   {
                                       Department = deptGroup.Key,
                                       TotalHours = totalHours,
                                   };
        foreach (var item in topDepartmentByHours)
        {
            WriteLine($"{item.Department}\t\t{item.TotalHours} hours");
        }
    }
}
