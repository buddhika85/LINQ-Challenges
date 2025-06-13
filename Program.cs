using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using static System.Console;

public class Program
{

    private static void Main(string[] args)
    {
        WriteLine("Hello, LINQ\n");
        //Challenge_0();             
        //Challenge_1();            // Filter
        //Challenge_2();            // Sort Asc
        //Challenge_3();            // Sort Desc
        //Challenge_4();              // Filtering & Sorting Together
        //Challenge_5();                  // JOINS
        //Challenge_6();                  // GROUP BY and Aggregation
        //Challenge_7();
        //Challenge_8();              // HAVING sql - linq uses where on groups, no having
        //Challenge_9();                // having
        //Challenge_10();               // sub queries

        Challenge_11();             // mix
    }

    private static void Challenge_11()
    {
        var employees = new List<(int EmployeeId, string Name, int DepartmentId)>
        {
            (1, "Alice", 1),
            (2, "Bob", 2),
            (3, "Charlie", 1),
            (4, "David", 3),
            (5, "Eve", 2),
            (6, "Frank", 1),
            (7, "Grace", 3)
        };

        var departments = new List<(int DepartmentId, string DepartmentName)>
        {
            (1, "HR"),
            (2, "IT"),
            (3, "Finance")
        };

        var salaries = new List<(int EmployeeId, decimal Salary)>
        {
            (1, 6000),
            (2, 7500),
            (3, 5800),
            (4, 9000),
            (5, 7200),
            (6, 6200),
            (7, 8800)
        };

        // Write a LINQ query to JOIN employees with departments, returning:
        //✅ Employee Name
        //✅ Department Name
        //✅ Salary
        // Sort the results by salary in descending order.
        var query = from emp in employees
                    join dept in departments on emp.DepartmentId equals dept.DepartmentId
                    join salary in salaries on emp.EmployeeId equals salary.EmployeeId
                    orderby salary.Salary descending
                    select new
                    {
                        Employee = emp.Name,
                        Salary = salary.Salary,
                        Department = dept.DepartmentName
                    };
        //foreach (var item in query)
        //{
        //    WriteLine($"{item.Employee}\t{item.Salary}\t{item.Department}");
        //}


        // Average Salary Per Department
        //Modify the query to GROUP employees by department and calculate:
        //✅ Total number of employees per department
        //✅ Average salary in each department
        //Sort results by average salary in descending order.
        var query2 = from emp in employees
                     join dept in departments on emp.DepartmentId equals dept.DepartmentId
                     join salary in salaries on emp.EmployeeId equals salary.EmployeeId
                     group new { emp, salary } by new { dept } into deptGroup
                     orderby deptGroup.Average(x => x.salary.Salary) descending
                     select new
                     {
                         Department = deptGroup.Key.dept.DepartmentName,
                         EmployeeCount = deptGroup.Select(x => x.emp.EmployeeId).Distinct().Count(),
                         AvgSalary = deptGroup.Average(x => x.salary.Salary)
                     };
        foreach (var item in query2)
        {
            WriteLine($"{item.Department}\t{item.EmployeeCount} Employees\t${item.AvgSalary}");
        }
        WriteLine();

        // Task 3: Filtering High-Salary Departments Using Subqueries
        // Modify the query to ONLY show departments where the average salary is greater than $7,000.
        var query3 = from emp in employees
                     join dept in departments on emp.DepartmentId equals dept.DepartmentId
                     join salary in salaries on emp.EmployeeId equals salary.EmployeeId
                     group new { emp, salary } by new { dept } into deptGroup

                     where deptGroup.Average(x => x.salary.Salary) > 7000
                     orderby deptGroup.Average(x => x.salary.Salary) descending

                     select new
                     {
                         Department = deptGroup.Key.dept.DepartmentName,
                         EmployeeCount = deptGroup.Select(x => x.emp.EmployeeId).Distinct().Count(),
                         AverageSalary = deptGroup.Average(x => x.salary.Salary)
                     };
        foreach (var item in query3)
        {
            WriteLine($"{item.Department}\t{item.EmployeeCount} Employees\t${item.AverageSalary}");
        }
    }

    private static void Challenge_10()
    {
        var customers = new List<(int CustomerId, string Name)>
        {
            (1, "Alice"),
            (2, "Bob"),
            (3, "Charlie"),
            (4, "David"),
            (5, "Eve")
        };

        var orders = new List<(int OrderId, int CustomerId, DateTime OrderDate)>
        {
            (101, 1, DateTime.Now.AddDays(-10)),
            (102, 1, DateTime.Now.AddDays(-20)),
            (103, 2, DateTime.Now.AddDays(-5)),
            (104, 3, DateTime.Now.AddDays(-15)),
            (105, 4, DateTime.Now.AddDays(-7)),
            (106, 4, DateTime.Now.AddDays(-30)),
            (107, 5, DateTime.Now.AddDays(-2))
        };

        var orderLines = new List<(int OrderId, int ProductId, int Quantity)>
        {
            (101, 201, 2),
            (101, 202, 1),
            (102, 203, 5),
            (103, 201, 3),
            (103, 202, 2),
            (104, 203, 4),
            (105, 201, 1),
            (106, 202, 6),
            (107, 203, 7)
        };

        var products = new List<(int ProductId, string ProductName, decimal Price)>
        {
            (201, "Laptop", 1200),
            (202, "Mouse", 25),
            (203, "Keyboard", 60)
        };

        // query to filter customers who have at least TWO high-value orders > $2,500.
        var query = from customer in customers
                    join order in orders on customer.CustomerId equals order.CustomerId
                    join orderLine in orderLines on order.OrderId equals orderLine.OrderId
                    join product in products on orderLine.ProductId equals product.ProductId
                    group new { order, orderLine, product } by customer into orderGroup

                    let highestValuedOrders
                        = orderGroup.GroupBy(x => x.order.OrderId)
                            .Count(order => order.Sum(x => x.orderLine.Quantity * x.product.Price) > 2500)

                    where highestValuedOrders >= 1


                    orderby
                    orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price) descending

                    select new
                    {
                        Customer = orderGroup.Key.Name,
                        OrderCount = orderGroup.Select(x => x.order.OrderId).Distinct().Count()
                    };
        foreach (var item in query)
        {
            WriteLine($"Customer {item.Customer}\t{item.OrderCount}");
        }
    }

    private static void Challenge_9()
    {
        var customers = new List<(int CustomerId, string Name)>
        {
            (1, "Alice"),
            (2, "Bob"),
            (3, "Charlie"),
            (4, "David"),
            (5, "Eve")
        };

        var orders = new List<(int OrderId, int CustomerId, DateTime OrderDate)>
        {
            (101, 1, DateTime.Now.AddDays(-10)),
            (102, 1, DateTime.Now.AddDays(-20)),
            (103, 2, DateTime.Now.AddDays(-5)),
            (104, 3, DateTime.Now.AddDays(-15)),
            (105, 4, DateTime.Now.AddDays(-7)),
            (106, 4, DateTime.Now.AddDays(-30)),
            (107, 5, DateTime.Now.AddDays(-2))
        };

        var orderLines = new List<(int OrderId, int ProductId, int Quantity)>
        {
            (101, 201, 2),
            (101, 202, 1),
            (102, 203, 5),
            (103, 201, 3),
            (103, 202, 2),
            (104, 203, 4),
            (105, 201, 1),
            (106, 202, 6),
            (107, 203, 7)
        };

        var products = new List<(int ProductId, string ProductName, decimal Price)>
        {
            (201, "Laptop", 1200),
            (202, "Mouse", 25),
            (203, "Keyboard", 60)
        };

        //✅ Find customers who placed at least one order with a total of more than $2,500.
        //✅ Use a subquery to determine order totals before filtering customers.
        //✅ Sort by total spent in descending order.


        //var query = from customer in customers
        //            join order in orders on customer.CustomerId equals order.OrderId
        //            join orderLine in orderLines on order.OrderId equals orderLine.OrderId
        //            join product in products on orderLine.ProductId equals product.ProductId
        //            group new { order, orderLine, product } by customer into orderGroup
        //            select new { 
        //                Customer = orderGroup.Key.Name,
        //                OrderCount = orderGroup.Select(x => x.order.OrderId).Distinct().Count(),
        //                TotalSpent = orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price)
        //            };

        var query = from order in orders
                    join orderLine in orderLines on order.OrderId equals orderLine.OrderId
                    join product in products on orderLine.ProductId equals product.ProductId
                    join customer in customers on order.CustomerId equals customer.CustomerId
                    group new { orderLine, product, customer } by order into orderGroup
                    orderby orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price) descending
                    where orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price) > 2500
                    select new
                    {
                        OrderId = orderGroup.Key.OrderId,
                        OrderSum = orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price),
                        Customer = orderGroup.Select(x => x.customer.Name).First(),
                    };
        foreach (var item in query)
        {
            WriteLine($"Order {item.OrderId}\t${item.OrderSum}\tby {item.Customer}");
        }
    }

    private static void Challenge_8()
    {
        var customers = new List<(int CustomerId, string Name)>
        {
            (1, "Alice"),
            (2, "Bob"),
            (3, "Charlie"),
            (4, "David"),
            (5, "Eve")
        };

        var orders = new List<(int OrderId, int CustomerId, DateTime OrderDate)>
        {
            (101, 1, DateTime.Now.AddDays(-10)),
            (102, 1, DateTime.Now.AddDays(-20)),
            (103, 2, DateTime.Now.AddDays(-5)),
            (104, 3, DateTime.Now.AddDays(-15)),
            (105, 4, DateTime.Now.AddDays(-7)),
            (106, 4, DateTime.Now.AddDays(-30)),
            (107, 5, DateTime.Now.AddDays(-2))
        };

        var orderLines = new List<(int OrderId, int ProductId, int Quantity)>
        {
            (101, 201, 2),
            (101, 202, 1),
            (102, 203, 5),
            (103, 201, 3),
            (103, 202, 2),
            (104, 203, 4),
            (105, 201, 1),
            (106, 202, 6),
            (107, 203, 7)
        };

        var products = new List<(int ProductId, string ProductName, decimal Price)>
        {
            (201, "Laptop", 1200),
            (202, "Mouse", 25),
            (203, "Keyboard", 60)
        };

        //    Task: Join, Group, Sort, and Project
        //Write a LINQ query to:
        //1️ Join customers, orders, and order lines
        //2️ Group by customer to calculate:
        //        -Total number of orders per customer
        //        - Total amount spent per customer
        //3️ Sort by total amount spent in descending order
        //4️ Project only Customer Name, Order Count, and Total Spent Amount
        //5 Apply a HAVING filter to exclude customers who spent less than $1,000

        var query = from customer in customers
                    join order in orders on customer.CustomerId equals order.CustomerId
                    join orderLine in orderLines on order.OrderId equals orderLine.OrderId
                    join product in products on orderLine.ProductId equals product.ProductId
                    group new { order, orderLine, product } by customer into orderGroup

                    where orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price) >= 1000            // having

                    orderby orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price) descending

                    select new
                    {
                        Customer = orderGroup.Key.Name,
                        OrderCount = orderGroup.Select(x => x.order.OrderId).Distinct().Count(),
                        TotalAmount = orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price)
                    };

        foreach (var item in query)
        {
            WriteLine($"{item.Customer}\tTotal Orders:{item.OrderCount}\tTotal amount spent: ${item.TotalAmount}");
        }

    }

    private static void Challenge_7()
    {
        var customers = new List<(int CustomerId, string Name)>
        {
            (1, "Alice"),
            (2, "Bob"),
            (3, "Charlie"),
            (4, "David"),
            (5, "Eve")
        };

        var orders = new List<(int OrderId, int CustomerId, DateTime OrderDate)>
        {
            (101, 1, DateTime.Now.AddDays(-10)),
            (102, 1, DateTime.Now.AddDays(-20)),
            (103, 2, DateTime.Now.AddDays(-5)),
            (104, 3, DateTime.Now.AddDays(-15)),
            (105, 4, DateTime.Now.AddDays(-7)),
            (106, 4, DateTime.Now.AddDays(-30)),
            (107, 5, DateTime.Now.AddDays(-2))
        };

        var orderLines = new List<(int OrderId, int ProductId, int Quantity)>
        {
            (101, 201, 2),
            (101, 202, 1),
            (102, 203, 5),
            (103, 201, 3),
            (103, 202, 2),
            (104, 203, 4),
            (105, 201, 1),
            (106, 202, 6),
            (107, 203, 7)
        };

        var products = new List<(int ProductId, string ProductName, decimal Price)>
        {
            (201, "Laptop", 1200),
            (202, "Mouse", 25),
            (203, "Keyboard", 60)
        };

        //    Task: Join, Group, Sort, and Project
        //Write a LINQ query to:
        //1️ Join customers, orders, and order lines
        //2️ Group by customer to calculate:
        //        -Total number of orders per customer
        //        - Total amount spent per customer
        //3️ Sort by total amount spent in descending order
        //4️ Project only Customer Name, Order Count, and Total Spent Amount


        var query = from cust in customers
                    join order in orders on cust.CustomerId equals order.CustomerId
                    join orderLine in orderLines on order.OrderId equals orderLine.OrderId
                    join product in products on orderLine.ProductId equals product.ProductId
                    group new { order, orderLine, product } by cust into orderGroup
                    orderby orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price) descending
                    select new
                    {
                        Customer = orderGroup.Key.Name,
                        OrderCount = orderGroup.Select(x => x.order).Distinct().Count(),
                        TotalAmountSpent = orderGroup.Sum(x => x.orderLine.Quantity * x.product.Price)
                    };

        foreach (var item in query)
        {
            WriteLine($"{item.Customer}\tTotal Orders:{item.OrderCount}\tTotal amount spent: ${item.TotalAmountSpent}");
        }

    }

    private static void Challenge_6()
    {
        var employees = new List<(int Id, string Name, int Age, int DepartmentId)>
        {
            (1, "Alice", 25, 2),  // IT
            (2, "Bob", 30, 3),    // Finance
            (3, "Charlie", 22, 1), // HR
            (4, "David", 40, 5),  // Sales
            (5, "Eve", 29, 4),    // Marketing
            (6, "Frank", 35, 2),  // IT
            (7, "Grace", 28, 3),  // Finance
            (8, "Hank", 45, 5),   // Sales
            (9, "Ivy", 24, 1),    // HR
            (10, "Jack", 32, 4)   // Marketing
        };

        var departments = new List<(int DepartmentId, string Department)>
        {
            (1, "HR"),
            (2, "IT"),
            (3, "Finance"),
            (4, "Marketing"),
            (5, "Sales")
        };

        // write a query to group employees by their department and calculate:
        // 1️ Total number of employees per department
        //2️ Average age of employees in each department

        var query = from emp in employees
                    join dept in departments on emp.DepartmentId equals dept.DepartmentId
                    group new { emp, dept } by emp.DepartmentId into deptGroup
                    orderby deptGroup.Key
                    select new
                    {
                        DepartmentId = deptGroup.Key,
                        Department = deptGroup.Select(x => x.dept.Department).First(),
                        EmpCount = deptGroup.Count(),
                        AverageAge = deptGroup.Average(x => x.emp.Age)
                    };
        foreach (var item in query)
        {
            WriteLine($"{item.Department}\tEmployee Count {item.EmpCount}\tAverage Age {item.AverageAge}");
        }

    }

    private static void Challenge_5()
    {
        var employees = new List<(int Id, string Name, int Age, int DepartmentId)>
        {
            (1, "Alice", 25, 2),  // Alice → IT
            (2, "Bob", 30, 3),    // Bob → Finance
            (3, "Charlie", 22, 1), // Charlie → HR
            (4, "David", 40, 5),  // David → Sales
            (5, "Eve", 29, 4)     // Eve → Marketing
        };

        var departments = new List<(int DepartmentId, string Department)>
        {
            (1, "HR"),
            (2, "IT"),
            (3, "Finance"),
            (4, "Marketing"),
            (5, "Sales")
        };

        // 🚀 Task: Write a LINQ Query to Join Employees and Departments
        // ✅ Write a LINQ query to join employees with departments using their DepartmentId.
        // ✅ Return Employee Name, Age, and Department Name.
        var query = from emp in employees
                    join
                    dept in departments on emp.DepartmentId equals dept.DepartmentId
                    select new
                    {
                        EmployeeName = emp.Name,
                        Age = emp.Age,
                        DepartmentName = dept.Department
                    };
        foreach (var emp in query)
        {
            WriteLine($"{emp.EmployeeName} age {emp.Age} works in {emp.DepartmentName}");
        }
    }

    private static void Challenge_4()
    {
        var employees = new List<(string Name, int Age)>
                {
                    ("Alice", 25),
                    ("Bob", 30),
                    ("Charlie", 22),
                    ("David", 40),
                    ("Eve", 29)
                };
        // filter employees older than 25 and sort them in descending order by age.
        var query = from emp in employees
                    where emp.Age > 25
                    orderby emp.Age descending
                    select emp;
        Display(query);
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
        Display(query);
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
        var query = from emp in employees orderby emp.Age ascending select emp;
        Display(query);
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
        foreach (var empName in query)
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

    private static void Display(IOrderedEnumerable<(string Name, int Age)> query)
    {
        foreach (var emp in query)
        {
            WriteLine($"{emp.Name} - {emp.Age}");
        }
    }
}