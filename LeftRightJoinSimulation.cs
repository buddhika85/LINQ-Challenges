using static System.Console;

namespace LINQ_Challeges;

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

    // Departments right join Employees
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
