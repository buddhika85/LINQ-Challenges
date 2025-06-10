using static System.Console;

public class Program
{

    private static void Main(string[] args)
    {
        WriteLine("Hello, LINQ\n");
        //Challenge_0();
        //Challenge_1();
        //Challenge_2();
        Challenge_3();
    }

    private static void Challenge_3()
    {
        var employees = new List<(string Name, int Age)>
                {
                    ("Alice", 25),
                    ("Bob", 30),
                    ("Charlie", 22),
                    ("David", 40),
                    ("Eve", 29)
                };

        //  sort employees in descending order by age (oldest first).
        var query = from emp in employees orderby emp.Age descending select emp;
        foreach (var emp in query)
        {
            WriteLine($"{emp.Name} - {emp.Age}");
        }
    }

    private static void Challenge_2()
    {
        var employees = new List<(string Name, int Age)>
                {
                    ("Alice", 25),
                    ("Bob", 30),
                    ("Charlie", 22),
                    ("David", 40),
                    ("Eve", 29)
                };
        // ❓ Task
        // Write a LINQ query to select all employees, but sort them in ascending order by age.
        var query = from  emp in employees orderby emp.Age ascending select emp;
        foreach (var emp in query)
        {
            WriteLine($"{emp.Name} - {emp.Age}");
        }
    }

    private static void Challenge_1()
    {
        var employees = new List<(string Name, int Age)>
                        {
                            ("Alice", 25),
                            ("Bob", 30),
                            ("Charlie", 22),
                            ("David", 40),
                            ("Eve", 29)
                        };

        // ❓ Task - Write a LINQ query to select only employees who are older than 25.
        // Return their names in a new list.

        var query = from emp in employees where emp.Age > 25 select emp.Name;
        foreach(var empName in query)
        {
            WriteLine($"{empName}");
        }
    }

    static void Challenge_0()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var query = from num in numbers select num;
        foreach (var num in numbers) 
        {
            WriteLine(num);
        }
    }
}