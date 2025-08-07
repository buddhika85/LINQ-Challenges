using static System.Console;

namespace LINQ_Challeges;


/*
 
 ❌ LINQ Does Not Support Right Joins Directly
LINQ (especially the query syntax) only supports left joins natively. There’s no built-in keyword or syntax for a right join.

But here’s the trick:
You can simulate a right join by reversing the direction of a left join.

So instead of:

from A in Left
join B in Right on A.Id equals B.RefId into Group
from B in Group.DefaultIfEmpty()                                    // --> gives dafault NULL for Left records which does have atleast 1 Right record

from B in Right
join A in Left on B.RefId equals A.Id into Group
from A in Group.DefaultIfEmpty()


✅ DefaultIfEmpty() Is the Key to Outer Joins
Whether you're doing a left join or simulating a right join, DefaultIfEmpty() is what allows you to include records even when there’s no match on the other side.

 */

public class LeftRightJoinSimulation
{
    private readonly IEnumerable<Department> departments =
        [
            new Department { Id = 1, Name = "HR" },
            new Department { Id = 2, Name = "IT" },
            new Department { Id = 3, Name = "Finance" }             // Does not have any employees attached
        ];
    private readonly IEnumerable<Employee> employees =
        [
            new Employee { Id = 1, Name = "Alice", DeptId = 1 },        // HR
            new Employee { Id = 2, Name = "Bob", DeptId = 1 },          // HR
            new Employee { Id = 3, Name = "John", DeptId = 2 },         // IT
            new Employee { Id = 4, Name = "James", DeptId = 2 },        // IT
            new Employee { Id = 5, Name = "Charlie", DeptId = null },   // NO Department
            new Employee { Id = 6, Name = "Ben", DeptId = null },       // NO Department
        ];


    // Departments left join Employees
    // Expected
    /*
        1 HR has 2 Employees.
                ID: 1   Name: Alice
                ID: 2   Name: Bob

        2 IT has 2 Employees.
                ID: 3   Name: John
                ID: 4   Name: James
        
        3 Finance has 0 Employees.
     */
    public void DisplayAllDepartmentsWithAnyAssociatedEmployees()
    {
        var query = from dept in departments
                    join emp in employees on dept.Id equals emp.DeptId into deptGroup

                    let empCount = deptGroup.Count()

                    select new
                    {
                        Department = dept,
                        EmployeeCount = empCount,
                        Employees = deptGroup
                    };

        foreach (var dep in query)
        {
            WriteLine($"{dep.Department.Id} {dep.Department.Name} has {dep.EmployeeCount} Employees.");
            foreach (var emp in dep.Employees)
            {
                WriteLine($"\t\tID: {emp.Id}   Name: {emp.Name}");
            }
            WriteLine();
        }
    }

    // LINQ DOES NOT support right join
    // Employees left join Departments
    // Expected
    /*
                ID: 1   Name: Alice     Dept Id: 1      Department: HR
                ID: 2   Name: Bob       Dept Id: 1      Department: HR

        
                ID: 3   Name: John      Dept Id: 2      Department: IT
                ID: 4   Name: James     Dept Id: 2      Department: IT
        
        
                ID: 5   Name: Charlie       Dept Id: -      Department: -
                ID: 6   Name: Ben           Dept Id: -      Department: -
     */
    public void DisplayAllEmployeesWithAnyAssociatedDepartments()
    {
        var query = from emp in employees
                    join dept in departments on emp.DeptId equals dept.Id into deptGroup

                    from dept in deptGroup.DefaultIfEmpty()                     // get left records which does not have any right record

                    select new
                    {
                        Employee = emp,
                        DeptId = dept?.Id.ToString() ?? "-",
                        DeptName = dept?.Name ?? "-",
                    };
        foreach (var item in query)
        {
            WriteLine($"{item.Employee.Id}\tName: {item.Employee.Name}\tDept Id: {item.DeptId}\tDepartment: {item.DeptName}");
        }
    }

    private class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    private class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int? DeptId { get; set; }
    }
}
