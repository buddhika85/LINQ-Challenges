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
        SalaryAggregationByRole_T1();
        DepartmentEmployeeMap_T2();
        LeftJoinDepartmentProjects_T3();
        ProjectAssignmentMatrix_T4();
        EmployeeActivityPaging_T5();
    }

    // 🔹 Task 1: Salary Aggregation by Role and Department
    // Group by DeptId + Role → Sum, Avg, Count, Min, Max
    // ⏱️ Expected: 15–18 min
    private void SalaryAggregationByRole_T1()
    {
    }

    // 🔹 Task 2: Department with Employee Lists
    // Project departments → nested employee projections
    // ⏱️ Expected: 12–15 min
    private void DepartmentEmployeeMap_T2() { }

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