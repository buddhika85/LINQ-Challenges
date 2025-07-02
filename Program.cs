using LINQ_Challeges;
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

        //Challenge_11();             // mix
        //Challenge_12();             // mix
        //Challenge_13();             // mix

        //Challenge_14();                 // mix 
        //Challenge_15();                   // pagination

        //new Challenge_16();
        //new Challenge_17();
        //new Challenge_18();
        //new Challenge_19();

        //new Challenge_20();
        //new Challenge_21();

        //new LinqChallenge_22();
        //new LinqChallenge_23();
        //new LinqChallenge_24();
        //new LinqChallenge_25();
    }

    private static void Challenge_15()
    {
        var employees = new List<(int EmployeeId, string Name)>
        {
            (1, "Alice"), (2, "Bob"), (3, "Charlie"), (4, "David"), (5, "Eve"),
            (6, "Frank"), (7, "Grace"), (8, "Hank"), (9, "Ivy"), (10, "Jack")
        };

        var projects = new List<(int ProjectId, string ProjectName)>
        {
            (101, "E-Commerce App"), (102, "AI Chatbot"), (103, "Finance Dashboard"),
            (104, "Cloud Migration"), (105, "Cybersecurity Audit")
        };

        var employeeProjects = new List<(int EmployeeId, int ProjectId, int HoursWorked)>
        {
            (1, 101, 120), (1, 102, 80), (1, 104, 90),
            (2, 101, 100), (2, 103, 95), (2, 105, 120),
            (3, 102, 110), (3, 105, 75),
            (4, 103, 130), (4, 104, 100),
            (5, 101, 70), (5, 102, 85), (5, 105, 60),
            (6, 103, 140), (7, 102, 95), (8, 105, 110),
            (9, 104, 125), (10, 101, 90), (10, 105, 80)
        };

        var performanceReviews = new List<(int EmployeeId, int Year, int Rating)>
        {
            (1, 2023, 4), (1, 2024, 5), (1, 2025, 5),
            (2, 2023, 3), (2, 2024, 4), (2, 2025, 4),
            (3, 2023, 5), (3, 2024, 5), (3, 2025, 5),
            (4, 2023, 4), (4, 2024, 4), (4, 2025, 3),
            (5, 2023, 3), (5, 2024, 2), (5, 2025, 4),
            (6, 2023, 4), (6, 2024, 4), (7, 2023, 5),
            (8, 2024, 3), (8, 2025, 4), (9, 2025, 5),
            (10, 2023, 4), (10, 2024, 5)
        };

        //🚀 Task 3: Employees Who Are BOTH High Performers & Hard Workers
        //✅ Combine results from Task 1 and Task 2
        //✅ Use a subquery to filter employees appearing in both datasets
        //✅ Retrieve only employees who worked over 200 hours AND have an average rating above 4
        // Fetches only 2 employees per page
        //✅ Uses variables for pageNumber and pageSize
        //✅ Applies.Skip(pageNumber * pageSize).Take(pageSize) to paginate results

        var pagedQuery = from emp in employees
                         join review in performanceReviews on emp.EmployeeId equals review.EmployeeId
                         join project in employeeProjects on emp.EmployeeId equals project.EmployeeId
                         group new { review, project } by emp into empGroup

                         let totalHrsWrkd = empGroup.Sum(x => x.project.HoursWorked)
                         let avgRating = Math.Round(empGroup.Average(x => x.review.Rating), 1)
                         let minRating = empGroup.Min(x => x.review.Rating)
                         let maxRating = empGroup.Max(x => x.review.Rating)

                         orderby totalHrsWrkd descending,
                                    avgRating descending

                         where totalHrsWrkd > 200 && avgRating > 4

                         select new
                         {
                             Employee = empGroup.Key.Name,
                             AvgRating = avgRating,
                             MinRating = minRating,
                             MaxRating = maxRating,
                             TotalHrsWrkd = totalHrsWrkd,
                         };
        foreach (var item in pagedQuery)
        {
            WriteLine($"{item.Employee}\t\t{item.TotalHrsWrkd} hrs\t\t{item.AvgRating} Avg\t\t{item.MinRating} LOW\t\t{item.MaxRating} HIGH");
        }

        WriteLine("\n------------------------------------Displaying Paged Results------------------------------------");
        var goMore = true;
        var resultsPerPage = 2;
        var pageNumber = 0;
        while (goMore)
        {
            var result = pagedQuery.Skip(pageNumber++ * resultsPerPage).Take(resultsPerPage);
            if (!result.Any())
            {
                goMore = false;
            }
            else
            {
                WriteLine($"\n\nPAGE [ {pageNumber} ]");
                foreach (var item in result)
                {
                    WriteLine($"{item.Employee}\t\t{item.TotalHrsWrkd} hrs\t\t{item.AvgRating} Avg\t\t{item.MinRating} LOW\t\t{item.MaxRating} HIGH");
                }
            }
        }
    }

    private static void Display(IEnumerable<(string Employee, double AvgRating, double MinRating, double MaxRating, int TotalHrsWrkd)> enumerable)
    {
        foreach (var item in enumerable)
        {
            WriteLine($"{item.Employee}\t\t{item.TotalHrsWrkd} hrs\t\t{item.AvgRating} Avg\t\t{item.MinRating} LOW\t\t{item.MaxRating} HIGH");
        }
    }

    private static void Challenge_14()
    {
        var employees = new List<(int EmployeeId, string Name)>
        {
            (1, "Alice"), (2, "Bob"), (3, "Charlie"), (4, "David"), (5, "Eve"),
            (6, "Frank"), (7, "Grace"), (8, "Hank"), (9, "Ivy"), (10, "Jack")
        };

        var projects = new List<(int ProjectId, string ProjectName)>
        {
            (101, "E-Commerce App"), (102, "AI Chatbot"), (103, "Finance Dashboard"),
            (104, "Cloud Migration"), (105, "Cybersecurity Audit")
        };

        var employeeProjects = new List<(int EmployeeId, int ProjectId, int HoursWorked)>
        {
            (1, 101, 120), (1, 102, 80), (1, 104, 90),
            (2, 101, 100), (2, 103, 95), (2, 105, 120),
            (3, 102, 110), (3, 105, 75),
            (4, 103, 130), (4, 104, 100),
            (5, 101, 70), (5, 102, 85), (5, 105, 60),
            (6, 103, 140), (7, 102, 95), (8, 105, 110),
            (9, 104, 125), (10, 101, 90), (10, 105, 80)
        };

        var performanceReviews = new List<(int EmployeeId, int Year, int Rating)>
        {
            (1, 2023, 4), (1, 2024, 5), (1, 2025, 5),
            (2, 2023, 3), (2, 2024, 4), (2, 2025, 4),
            (3, 2023, 5), (3, 2024, 5), (3, 2025, 5),
            (4, 2023, 4), (4, 2024, 4), (4, 2025, 3),
            (5, 2023, 3), (5, 2024, 2), (5, 2025, 4),
            (6, 2023, 4), (6, 2024, 4), (7, 2023, 5),
            (8, 2024, 3), (8, 2025, 4), (9, 2025, 5),
            (10, 2023, 4), (10, 2024, 5)
        };

        //🚀 Task 3: Employees Who Are BOTH High Performers & Hard Workers
        //✅ Combine results from Task 1 and Task 2
        //✅ Use a subquery to filter employees appearing in both datasets
        //✅ Retrieve only employees who worked over 200 hours AND have an average rating above 4
        var top3Emps = (from emp in employees
                        join projs in employeeProjects on emp.EmployeeId equals projs.EmployeeId
                        join reviews in performanceReviews on emp.EmployeeId equals reviews.EmployeeId
                        group new { projs, reviews } by emp into empGroup

                        let totalHrsWrkd = empGroup.Select(x => x.projs).Sum(x => x.HoursWorked)
                        let avgRating = empGroup.Select(x => x.reviews).Average(x => x.Rating)
                        let lowestRating = empGroup.Select(x => x.reviews).Min(x => x.Rating)
                        let highestRating = empGroup.Select(x => x.reviews).Max(x => x.Rating)

                        where totalHrsWrkd > 200 && avgRating > 4

                        select new
                        {
                            Employee = empGroup.Key.Name,
                            TotalHrsWrkd = totalHrsWrkd,
                            AvgRating = avgRating,
                            LowestRating = lowestRating,
                            HighestRating = highestRating
                        }).Take(3);


        /*
         employeeProjects and performanceReviews both are joined using same column which is EmployeeId, there are no duplicates with in the group data is it ? YES
        If they were grouped based on a different key they may contain duplicates which will need distinct is it ? YES         
         */
        foreach (var item in top3Emps)
        {
            WriteLine($"{item.Employee}\t{item.TotalHrsWrkd} hrs\t{item.AvgRating} Avg\t{item.LowestRating} LOW\t{item.HighestRating} HIGH");
        }
    }

    private static void Challenge_13()
    {
        var employees = new List<(int EmployeeId, string Name)>
        {
            (1, "Alice"),
            (2, "Bob"),
            (3, "Charlie"),
            (4, "David"),
            (5, "Eve")
        };

        var projects = new List<(int ProjectId, string ProjectName)>
        {
            (101, "E-Commerce App"),
            (102, "AI Chatbot"),
            (103, "Finance Dashboard")
        };

        var employeeProjects = new List<(int EmployeeId, int ProjectId, int HoursWorked)>
        {
            (1, 101, 120),
            (1, 102, 80),
            (2, 101, 100),
            (2, 103, 95),
            (3, 102, 110),
            (4, 103, 130),
            (5, 101, 70),
            (5, 102, 85)
        };

        var performanceReviews = new List<(int EmployeeId, int Year, int Rating)>
        {
            (1, 2023, 4),
            (1, 2024, 5),
            (2, 2023, 3),
            (2, 2024, 4),
            (3, 2023, 5),
            (3, 2024, 5),
            (4, 2023, 4),
            (4, 2024, 4),
            (5, 2023, 3),
            (5, 2024, 2)
        };

        //🚀 Task 1: Top Employees Based on Average Rating
        //✅ JOIN employees with performance reviews
        //✅ GROUP by employee
        //✅ Calculate average rating
        //✅ Use let to store computed rating
        //✅ FILTER employees whose average rating is above 4
        var topEmployees = from emp in employees
                           join review in performanceReviews on emp.EmployeeId equals review.EmployeeId
                           group new { review } by emp into reviewsGroup

                           let avgRating = reviewsGroup.Average(x => x.review.Rating)

                           orderby avgRating descending
                           where avgRating > 4

                           select new
                           {
                               Employee = reviewsGroup.Key.Name,
                               AvgRating = avgRating
                           };
        foreach (var item in topEmployees)
        {
            WriteLine($"{item.Employee}\t{item.AvgRating}");
        }
        WriteLine();

        // 🚀 Task 2: Most Dedicated Employees
        //✅ JOIN employees with projects
        //✅ GROUP by employee
        //✅ Calculate total hours worked across all projects
        //✅ FILTER employees who worked more than 200 hours total
        //✅ Use let to store hours worked
        //✅ ORDER BY hours in descending order
        var dedicatedEmps = from emp in employees
                            join empProjects in employeeProjects on emp.EmployeeId equals empProjects.EmployeeId
                            group new { empProjects } by emp into projGroup

                            let totalHoursWrkd = projGroup.Sum(x => x.empProjects.HoursWorked)

                            where totalHoursWrkd >= 200
                            orderby totalHoursWrkd descending

                            select new
                            {
                                Employee = projGroup.Key.Name,
                                TotalHrsWrkd = totalHoursWrkd
                            };
        foreach (var item in dedicatedEmps)
        {
            WriteLine($"{item.Employee}\t{item.TotalHrsWrkd}");
        }
        WriteLine();

        //🚀 Task 3: Employees Who Are BOTH High Performers & Hard Workers
        //✅ Combine results from Task 1 and Task 2
        //✅ Use a subquery to filter employees appearing in both datasets
        //✅ Retrieve only employees who worked over 200 hours AND have an average rating above 4
        var goodEmps = from emp in employees
                       join review in performanceReviews on emp.EmployeeId equals review.EmployeeId
                       join empProjs in employeeProjects on emp.EmployeeId equals empProjs.EmployeeId
                       group new { review, empProjs } by emp into ratingwHoursGroup

                       let avgRating = ratingwHoursGroup.Select(x => x.review).Distinct().Average(x => x.Rating)
                       let totalHours = ratingwHoursGroup.Select(x => x.empProjs).Distinct().Sum(x => x.HoursWorked)

                       where totalHours >= 200 && avgRating > 4

                       select new
                       {
                           Employee = ratingwHoursGroup.Key.Name,
                           AvgRating = avgRating,
                           TotalHours = totalHours,
                       };
        foreach (var item in goodEmps)
        {
            WriteLine($"{item.Employee}\tTotal Hours Worked: {item.TotalHours}\t\tAvg Rating: {item.AvgRating}");
        }

    }

    private static void Challenge_12()
    {
        var books = new List<(int BookId, string Title, string Genre)>
        {
            (1, "C# in Depth", "Programming"),
            (2, "Clean Code", "Programming"),
            (3, "The Pragmatic Programmer", "Programming"),
            (4, "Atomic Habits", "Self-Help"),
            (5, "Deep Work", "Self-Help"),
            (6, "Harry Potter", "Fiction"),
            (7, "The Lord of the Rings", "Fiction")
        };

        var borrowers = new List<(int BorrowerId, string Name)>
        {
            (101, "Alice"),
            (102, "Bob"),
            (103, "Charlie"),
            (104, "David"),
            (105, "Eve")
        };

        var transactions = new List<(int TransactionId, int BorrowerId, int BookId, DateTime BorrowDate)>
        {
            (201, 101, 1, DateTime.Now.AddDays(-30)), // Alice borrowed "C# in Depth"
            (202, 101, 2, DateTime.Now.AddDays(-15)), // Alice borrowed "Clean Code"
            (202, 101, 2, DateTime.Now.AddDays(-15)), // Alice borrowed "Clean Code"
            (203, 102, 4, DateTime.Now.AddDays(-40)), // Bob borrowed "Atomic Habits"
            (204, 103, 5, DateTime.Now.AddDays(-10)), // Charlie borrowed "Deep Work"
            (205, 104, 6, DateTime.Now.AddDays(-25)), // David borrowed "Harry Potter"
            (206, 104, 7, DateTime.Now.AddDays(-5)),  // David borrowed "The Lord of the Rings"
            (207, 105, 3, DateTime.Now.AddDays(-20))  // Eve borrowed "The Pragmatic Programmer"
        };

        //🚀 Task 1: Books Borrowed Per Genre
        //✅ JOIN books and transactions
        //✅ GROUP books by Genre
        //✅ COUNT how many times each genre was borrowed
        //✅ Use let to store computed borrow count
        //✅ ORDER BY borrow count in descending order
        var query = from book in books
                    join transaction in transactions on book.BookId equals transaction.BookId
                    group new { transaction } by book.Genre into genreGroup

                    let borrowalCount = genreGroup.Count()         //genreGroup.Select(x => x.transaction.TransactionId).Count()
                    orderby borrowalCount descending

                    select new
                    {
                        Genre = genreGroup.Key,
                        Count = borrowalCount
                    };

        foreach (var item in query)
        {
            WriteLine($"{item.Genre}\t{item.Count}");
        }
        WriteLine();

        //🚀 Task 2: Borrowers Who Borrowed Multiple Books
        //✅ JOIN borrowers, transactions, and books
        //✅ GROUP by Borrower Name
        //✅ COUNT total books borrowed
        //✅ **FILTER borrowers who borrowed at least 2 books
        //✅ Sort borrowers by total borrowed in descending order
        //✅ Use let to store computed borrow count
        var query2 = from transaction in transactions
                     join borrower in borrowers on transaction.BorrowerId equals borrower.BorrowerId
                     group new { transaction } by borrower into borrowersGroup

                     let countOfTransactions = borrowersGroup.Count()   // borrowersGroup.Select(x => x.transaction.TransactionId).Count()


                     orderby countOfTransactions descending

                     select new
                     {
                         Borrower = borrowersGroup.Key.Name,
                         CountOfTransactions = countOfTransactions
                     };
        foreach (var item in query2)
        {
            WriteLine($"{item.Borrower}\t{item.CountOfTransactions}");
        }
        WriteLine();

        // Task 3: Most Popular Book
        //✅ JOIN books and transactions
        //✅ GROUP by Book Title
        //✅ COUNT how many times each book was borrowed
        //✅ Use let to store borrow count
        //✅ Retrieve the most borrowed book using Max()

        var query3 = (from book in books
                      join transaction in transactions on book.BookId equals transaction.BookId
                      group new { transaction } by book into bookGroup

                      let borrowalsMaxCount = bookGroup.Count()

                      orderby borrowalsMaxCount descending
                      select new
                      {
                          Title = bookGroup.Key.Title,
                          Count = borrowalsMaxCount
                      }).FirstOrDefault();

        WriteLine($"Most popular book is {query3?.Title} and its borrowals count is {query3?.Count}");
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
        WriteLine();

        // Task 4 - same as task 3 using let
        var query4 = from emp in employees
                     join dept in departments on emp.DepartmentId equals dept.DepartmentId
                     join salary in salaries on emp.EmployeeId equals salary.EmployeeId
                     group new { emp, salary } by new { dept } into deptGroup

                     let avgSalary = deptGroup.Average(x => x.salary.Salary)

                     where avgSalary > 7000
                     orderby avgSalary descending

                     select new
                     {
                         Department = deptGroup.Key.dept.DepartmentName,
                         EmployeeCount = deptGroup.Select(x => x.emp.EmployeeId).Distinct().Count(),
                         AverageSalary = avgSalary
                     };
        foreach (var item in query4)
        {
            WriteLine($"{item.Department}\t{item.EmployeeCount} Employees\t${item.AverageSalary}");
        }
        WriteLine();


        var query5 = from emp in employees
                     join dept in departments on emp.DepartmentId equals dept.DepartmentId
                     join salary in salaries on emp.EmployeeId equals salary.EmployeeId
                     group new { emp, salary } by new { dept } into deptGroup

                     let avgSalary = deptGroup.Average(x => x.salary.Salary)
                     let department = deptGroup.Key.dept.DepartmentName
                     let employeeCount = deptGroup.Select(x => x.emp.EmployeeId).Distinct().Count()

                     where avgSalary > 7000
                     orderby avgSalary descending

                     select new
                     {
                         Department = department,
                         EmployeeCount = employeeCount,
                         AverageSalary = avgSalary
                     };
        foreach (var item in query5)
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