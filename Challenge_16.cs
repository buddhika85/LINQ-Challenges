using Azure;
using static System.Console;
namespace LINQ_Challeges;

public class Challenge_16
{
    private List<(int CustomerId, string Name)> customers = new()
        {
            (1, "Alice"), (2, "Bob"), (3, "Charlie"), (4, "David"), (5, "Eve"),
            (6, "Frank"), (7, "Grace"), (8, "Hank"), (9, "Ivy"), (10, "Jack")
        };

    private List<(int ProductId, string ProductName, decimal Price)> products = new()
        {
            (101, "Laptop", 1200), (102, "Phone", 800), (103, "Tablet", 650),
            (104, "Monitor", 300), (105, "Keyboard", 150)
        };

    private List<(int SaleId, int CustomerId, int ProductId, int Quantity, DateTime SaleDate)> sales = new()
        {
            (201, 1, 101, 3, DateTime.Now.AddDays(-30)), // Alice bought 3 Laptops
            (202, 2, 102, 4, DateTime.Now.AddDays(-20)), // Bob bought 4 Phones
            (203, 3, 103, 6, DateTime.Now.AddDays(-10)), // Charlie bought 6 Tablets
            (204, 4, 101, 2, DateTime.Now.AddDays(-25)), // David bought 2 Laptops
            (205, 5, 105, 10, DateTime.Now.AddDays(-5)),  // Eve bought 10 Keyboards
            (206, 6, 103, 3, DateTime.Now.AddDays(-15)),  // Frank bought 3 Tablets
            (207, 7, 101, 1, DateTime.Now.AddDays(-40)), // Grace bought 1 Laptop
            (208, 8, 104, 5, DateTime.Now.AddDays(-12)), // Hank bought 5 Monitors
            (209, 9, 105, 8, DateTime.Now.AddDays(-18)), // Ivy bought 8 Keyboards
            (210, 10, 102, 2, DateTime.Now.AddDays(-22)) // Jack bought 2 Phones
        };
    public Challenge_16()
    {
        //HighestSpenders_T1();
        //HighestSpendersAbove5000_T2();
        //HighestPurchaseOfCust_T3();
        DisplayPaginatedResults_T4();
    }

    // 🚀 Task 1: Total Sales Per Customer
    //✅ JOIN customers with sales and products
    //✅ GROUP BY customer
    //✅ Calculate total spending(Quantity × Price)
    //✅ Use let for computed values (totalSpent)
    //✅ Sort by highest spenders
    private void HighestSpenders_T1()
    {
        var highestSpenders = from cust in customers
                              join sale in sales on cust.CustomerId equals sale.CustomerId

                              group new { sale } by cust into custGroup
                              let totSpend = custGroup.Sum(x => x.sale.Quantity *
                                                                products.First(p => p.ProductId == x.sale.ProductId).Price)
                              orderby totSpend descending
                              select new
                              {
                                  Customer = custGroup.Key.Name,
                                  TotalSpend = totSpend
                              };
        foreach (var item in highestSpenders)
        {
            WriteLine($"{item.Customer}\t${item.TotalSpend}");
        }
        WriteLine();


        // below version is better, because it avoid unnecessary lookups on Products data set
        highestSpenders = from cust in customers
                          join sale in sales on cust.CustomerId equals sale.CustomerId
                          join product in products on sale.ProductId equals product.ProductId

                          group new { sale, product } by cust into custGroup
                          let totSpend = custGroup.Sum(x => x.sale.Quantity * x.product.Price)

                          orderby totSpend descending
                          select new
                          {
                              Customer = custGroup.Key.Name,
                              TotalSpend = totSpend
                          };
        foreach (var item in highestSpenders)
        {
            WriteLine($"{item.Customer}\t${item.TotalSpend}");
        }
        WriteLine();
    }


    // 🚀 Task 2: Filter Customers Who Spent Over $5,000
    //✅ Modify Task 1
    //✅ Apply where to filter customers whose total spent exceeds $5,000
    //✅ Sort results in descending order
    public void HighestSpendersAbove5000_T2()
    {
        var highestAbove5000 = from cust in customers
                               join sale in sales on cust.CustomerId equals sale.CustomerId
                               join prod in products on sale.ProductId equals prod.ProductId
                               group new { sale, prod } by cust into custGroup

                               let totalSpent = custGroup.Sum(x => x.sale.Quantity * x.prod.Price)

                               where totalSpent >= 5000
                               orderby totalSpent descending

                               select new
                               {
                                   Customer = custGroup.Key.Name,
                                   TotalSpent = totalSpent
                               };
        foreach (var item in highestAbove5000)
        {
            WriteLine($"{item.Customer}\t${item.TotalSpent}");
        }
        WriteLine();
    }


    //🚀 Task 3: Find Highest Single Purchase Per Customer
    //✅ Use a subquery to find the highest single purchase per customer
    //✅ Only include purchases worth more than $2,000
    //✅ Sort by purchase amount descending
    public void HighestPurchaseOfCust_T3()
    {
        var highestPurchaseOfCust = from cust in customers
                                    join sale in sales on cust.CustomerId equals sale.CustomerId
                                    join product in products on sale.ProductId equals product.ProductId
                                    group new { sale, product } by cust into custGroup

                                    let highestPurchase = custGroup.Max(x => x.sale.Quantity * x.product.Price)
                                    let lowestPurchase = custGroup.Min(x => x.sale.Quantity * x.product.Price)
                                    let avgPurchase = custGroup.Average(x => x.sale.Quantity * x.product.Price)
                                    let sumPurchases = custGroup.Sum(x => x.sale.Quantity * x.product.Price)

                                    orderby highestPurchase descending

                                    where highestPurchase > 2000

                                    select new
                                    {
                                        Customer = custGroup.Key.Name,
                                        MaxPurchase = highestPurchase,
                                        MinPurchase = lowestPurchase,
                                        AvgPurchase = Math.Round(avgPurchase, 1),
                                        SumPurchase = sumPurchases
                                    };

        foreach (var item in highestPurchaseOfCust)
        {
            WriteLine($"{item.Customer}\t\tMax ${item.MaxPurchase}\tMin ${item.MinPurchase}\tAvg ${item.AvgPurchase}\tTotal ${item.SumPurchase}");
        }
    }

    //🚀 Task 4: Apply Pagination to Sales Results
    //✅ Display sales in pages(Skip() & Take())
    //✅ **Show only 2 records per page
    //✅ Allow dynamic page navigation
    public void DisplayPaginatedResults_T4()
    {
        var highestPurchaseOfCust = from cust in customers
                                    join sale in sales on cust.CustomerId equals sale.CustomerId
                                    join product in products on sale.ProductId equals product.ProductId
                                    group new { sale, product } by cust into custGroup

                                    let highestPurchase = custGroup.Max(x => x.sale.Quantity * x.product.Price)
                                    let lowestPurchase = custGroup.Min(x => x.sale.Quantity * x.product.Price)
                                    let avgPurchase = custGroup.Average(x => x.sale.Quantity * x.product.Price)
                                    let sumPurchases = custGroup.Sum(x => x.sale.Quantity * x.product.Price)

                                    orderby highestPurchase descending

                                    where highestPurchase > 2000

                                    select new
                                    {
                                        Customer = custGroup.Key.Name,
                                        MaxPurchase = highestPurchase,
                                        MinPurchase = lowestPurchase,
                                        AvgPurchase = Math.Round(avgPurchase, 1),
                                        SumPurchase = sumPurchases
                                    };

        var goMore = true;
        var pageNumberDisplayed = 0;
        var resultsPerPage = 2;
        while (goMore)
        {
            var pageResult = highestPurchaseOfCust.Skip(pageNumberDisplayed * resultsPerPage).Take(resultsPerPage);
            if (pageResult.Any())
            {
                WriteLine($"\nPage [ {++pageNumberDisplayed} ]");
                foreach (var item in pageResult)
                {
                    WriteLine($"{item.Customer}\t\tMax ${item.MaxPurchase}\tMin ${item.MinPurchase}\tAvg ${item.AvgPurchase}\tTotal ${item.SumPurchase}");
                }
            }
            else
            {
                goMore = false;
            }
        }
    }

}
