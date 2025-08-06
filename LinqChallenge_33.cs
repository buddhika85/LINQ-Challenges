using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_33
{
    private List<(int EmpId, string Name, string Role, int DeptId, decimal Salary)> employees = new()
    {
        (1, "Ava", "Dev", 101, 120000), (2, "Ben", "Dev", 101, 135000), (3, "Cara", "QA", 102, 90000),
        (4, "Dan", "Dev", 101, 110000), (5, "Ella", "QA", 102, 95000), (6, "Finn", "Manager", 103, 160000),
        (7, "Gina", "DevOps", 104, 130000), (8, "Hugo", "QA", 102, 88000)
    };

    private List<(int DeptId, string Name)> departments = new()
    {
        (101, "Engineering"), (102, "Quality"), (103, "Management"), (104, "Operations"), (105, "Research")
    };

    private List<(int ProjectId, string Title, int DeptId)> projects = new()
    {
        (1001, "Alpha", 101), (1002, "Beta", 102), (1003, "Gamma", 104), (1004, "Delta", 101)
    };

    private List<(int EmpId, int ProjectId)> assignments = new()
    {
        (1,1001), (2,1001), (3,1002), (4,1004), (5,1002), (7,1003)
    };

    public LinqChallenge_33()
    {
        //SalaryAggregationByRole_T1();
        DepartmentEmployeeMap_T2();
        //LeftJoinDepartmentProjects_T3();
        //ProjectAssignmentMatrix_T4();
        //EmployeeActivityPaging_T5();
    }

    // 🔹 Task 1: Salary Aggregation by Role and Department
    // Group by DeptId + Role → Sum, Avg, Count, Min, Max
    // ⏱️ Expected: 15–18 min
    // 3:40 - 3:53
    private void SalaryAggregationByRole_T1()
    {
        var query = from emp in employees
                    join dept in departments on emp.DeptId equals dept.DeptId
                    group new { emp, dept } by new { emp.DeptId, emp.Role } into roleGroup

                    let department = roleGroup.Select(x => x.dept).FirstOrDefault()
                    let departmentName = department.Name ?? "Unknown"
                    let sumSalary = roleGroup.Sum(x => x.emp.Salary)
                    let avgSalary = roleGroup.Average(x => x.emp.Salary)
                    let countEmps = roleGroup.Count()
                    let minSalary = roleGroup.Min(x => x.emp.Salary)
                    let maxSalary = roleGroup.Max(x => x.emp.Salary)

                    orderby countEmps descending

                    select new
                    {
                        Role = $"{departmentName} - {roleGroup.Key.Role}",
                        Department = departmentName,
                        EmployeesCount = countEmps,
                        SumSalary = Math.Round(sumSalary, 2),
                        AvgSalary = Math.Round(avgSalary, 2),
                        MinSalary = Math.Round(minSalary, 2),
                        MaxSalary = Math.Round(maxSalary, 2),
                    };
        foreach (var item in query)
        {
            WriteLine($"{item.Role}\t\tDepartment: {item.Department}\t\tEmployees Count: {item.EmployeesCount}\nSalaries:");
            WriteLine($"\t\tSum $:{item.SumSalary}\t\tAvg:{item.AvgSalary}\t\tMin: {item.MinSalary}\t\t Max: {item.MaxSalary}\n");
        }
    }

    // 🔹 Task 2: Department with Employee Lists
    // Project departments → nested employee projections
    // ⏱️ Expected: 12–15 min
    // 9:07 - 9:20
    private void DepartmentEmployeeMap_T2()
    {
        // INNER JOIN
        //var query = from dept in departments
        //            join emp in employees on dept.DeptId equals emp.DeptId
        //            group emp by dept into deptGroup

        //            let empCount = deptGroup.Count()
        //            let department = deptGroup.Key
        //            let avgSalary = deptGroup.Average(x => x.Salary)
        //            let employees = deptGroup.OrderByDescending(x => x.Salary).ThenBy(x => x.Name)

        //            orderby empCount descending, department.Name

        //            select new
        //            {
        //                DepartmentId = department.DeptId,
        //                Department = department.Name,
        //                AvgSalary = Math.Round(avgSalary, 2),
        //                EmployeesCount = empCount,
        //                Employees = employees
        //            };

        // LEFT Join
        var query = from dept in departments
                    join emp in employees on dept.DeptId equals emp.DeptId into deptGroup

                    from emp in deptGroup.DefaultIfEmpty()                                          // left join, emp list is null(default) if empty

                    let empCount = deptGroup?.Count() ?? 0
                    let department = dept
                    let avgSalary = deptGroup.Any() ? Math.Round(deptGroup.Average(x => x.Salary), 2) : 0.00m
                    let employees = deptGroup?.OrderByDescending(x => x.Salary)?.ThenBy(x => x.Name)

                    orderby empCount descending, department.Name

                    select new
                    {
                        DepartmentId = department.DeptId,
                        Department = department.Name,
                        AvgSalary = avgSalary,
                        EmployeesCount = empCount,
                        Employees = employees
                    };

        foreach (var item in query)
        {
            WriteLine($"\nID: {item.DepartmentId} {item.Department} Has {item.EmployeesCount} Employees. AvgSalary: ${item.AvgSalary}");
            if (item.EmployeesCount > 0)
            {
                WriteLine("\tEmployee List:");
                foreach (var emp in item.Employees)
                {
                    WriteLine($"\t\tID: {emp.EmpId}\t{emp.Name}\t\tRole: {emp.Role}\t\tSalary: {Math.Round(emp.Salary, 2)}");
                }
            }
        }
    }

    // 🔹 Task 3: Left Join Departments & Projects
    // Use GroupJoin + DefaultIfEmpty → list projects per dept (handle no-project depts)
    // ⏱️ Expected: 15–18 min
    private void LeftJoinDepartmentProjects_T3() { }

    // 🔹 Task 4: Assignment Cross Matrix
    // Create matrix of Emp × Project (cross join style), mark assigned vs not
    // ⏱️ Expected: 12–14 min
    private void ProjectAssignmentMatrix_T4() { }

    // 🔹 Task 5: Paginate Top Employees by Activity
    // Order employees by project count descending, take 3 per page
    // ⏱️ Expected: 12–15 min
    private void EmployeeActivityPaging_T5() { }
}