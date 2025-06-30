using static System.Console;

namespace LINQ_Challeges
{
    public class LinqChallenge_24
    {
        private List<(int CustomerId, string Name, string Region)> customers = new()
        {
            (1, "Alice", "USA"), (2, "Ben", "UK"), (3, "Cara", "Germany"),
            (4, "Dan", "USA"), (5, "Ella", "Australia"), (6, "Finn", "Germany"),
            (7, "Gina", "UK"), (8, "Hugo", "Canada"), (9, "Ivy", "USA"), (10, "Jack", "Australia")
        };

        private List<(int ProductId, string Name, string Category)> products = new()
        {
            (101, "Laptop", "Electronics"), (102, "Headphones", "Electronics"),
            (103, "Shoes", "Fashion"), (104, "T-shirt", "Fashion"),
            (105, "Blender", "Home"), (106, "Smartphone", "Electronics")
        };

        private List<(int OrderId, int CustomerId, int ProductId, int Quantity, decimal Price, DateTime OrderDate)> orders = new()
        {
            (1001, 1, 101, 1, 1300, new DateTime(2024, 1, 12)),
            (1002, 2, 103, 2, 120, new DateTime(2024, 1, 15)),
            (1003, 3, 104, 3, 75, new DateTime(2024, 2, 10)),
            (1004, 4, 102, 2, 200, new DateTime(2024, 2, 20)),
            (1005, 5, 106, 1, 999, new DateTime(2024, 3, 3)),
            (1006, 6, 103, 1, 65, new DateTime(2024, 3, 15)),
            (1007, 7, 101, 1, 1400, new DateTime(2024, 4, 1)),
            (1008, 8, 105, 1, 250, new DateTime(2024, 4, 10)),
            (1009, 9, 101, 1, 1300, new DateTime(2024, 4, 12)),
            (1010, 10, 106, 2, 1950, new DateTime(2024, 4, 16))
        };

        private List<(int CustomerId, int ProductId, int Score)> reviews = new()
        {
            (1, 101, 5), (2, 103, 4), (3, 104, 3),
            (4, 102, 4), (5, 106, 5), (6, 103, 2),
            (7, 101, 5), (9, 101, 4), (10, 106, 3)
        };

        public LinqChallenge_24()
        {
            //TopCustomersBySpend_T1();
            //ProductPopularity_T2();
            //CategoryScoreInsights_T3();
            RegionalRevenueBreakdown_T4();
        }

        // 🔹 Task 1: Top Customers by Spend
        // For each customer, compute total spend and sort descending.
        // ⏱️ Expected time: 10–12 minutes
        // 4:33 - 4:38
        private void TopCustomersBySpend_T1()
        {
            var topCustomerSpend = from cust in customers
                                   join order in orders on cust.CustomerId equals order.CustomerId
                                   group order by cust into custGroup
                                   let totalSpent = custGroup.Sum(x => x.Quantity * x.Price)
                                   orderby totalSpent descending
                                   select new
                                   {
                                       Customer = custGroup.Key.Name,
                                       TotalSpent = totalSpent
                                   };

            foreach (var item in topCustomerSpend)
            {
                WriteLine($"{item.Customer}\t\tSpent ${item.TotalSpent}");
            }
        }

        // 🔹 Task 2: Product Popularity
        // For each product, calculate total units sold. Paginate 3 per page.
        // ⏱️ Expected time: 10 minutes
        // 4:38 - 4:45
        private void ProductPopularity_T2()
        {
            var productPopularity = (from prod in products
                                     join order in orders on prod.ProductId equals order.ProductId

                                     group order by prod into prodGroup

                                     let totalQtySold = prodGroup.Sum(x => x.Quantity)

                                     orderby totalQtySold descending

                                     select new
                                     {
                                         Product = prodGroup.Key.Name,
                                         TotalQtySold = totalQtySold
                                     }).ToList();
            var perPage = 3;
            for (var pageNum = 0; ;)
            {
                var result = productPopularity.Skip(pageNum * perPage).Take(perPage);
                if (!result.Any())
                    break;

                WriteLine($"\nPage[ {++pageNum} ]");
                foreach (var item in result)
                {
                    WriteLine($"{item.Product}\t\tSold {item.TotalQtySold} items");
                }
            }
        }

        // 🔹 Task 3: Category Score Insights
        // Calculate average review score per product category. Include only categories with 2+ reviews.
        // ⏱️ Expected time: 12–15 minutes
        // 4:45 - 4:50
        private void CategoryScoreInsights_T3()
        {
            var categoryScoreInsights = from prod in products
                                        join review in reviews on prod.ProductId equals review.ProductId

                                        group review by prod.Category into prodGroup
                                        let reviewCount = prodGroup.Count()

                                        where reviewCount >= 2

                                        let avgReviewScore = prodGroup.Average(x => x.Score)

                                        orderby avgReviewScore descending

                                        select new
                                        {
                                            Product = prodGroup.Key,
                                            AvgReviewScore = Math.Round(avgReviewScore, 2)
                                        };

            foreach (var item in categoryScoreInsights)
            {
                WriteLine($"{item.Product}\t\t Avg Score {item.AvgReviewScore}");
            }
        }

        // 🔹 Task 4: Regional Revenue Breakdown
        // Group orders by region and return total revenue and avg order size per region.
        // ⏱️ Expected time: 15–18 minutes
        // 4:50 - 4:59
        private void RegionalRevenueBreakdown_T4()
        {
            var regionalRevenueBreakdown = from cust in customers
                                           join order in orders on cust.CustomerId equals order.CustomerId

                                           group order by cust.Region into regionGroup

                                           let totalRevenue = regionGroup.Sum(x => x.Quantity * x.Price)
                                           let avgOrderRevenue = regionGroup.Average(x => x.Quantity * x.Price)


                                           orderby totalRevenue descending

                                           select new
                                           {
                                               Region = regionGroup.Key,
                                               AvgOrderRevenue = Math.Round(avgOrderRevenue, 2),
                                               TotalRevenue = totalRevenue
                                           };

            foreach (var item in regionalRevenueBreakdown)
            {
                WriteLine($"{item.Region}\t\tAverage Revenue ${item.AvgOrderRevenue}\t\tTotal Revenue ${item.TotalRevenue}");
            }
        }
    }
}
