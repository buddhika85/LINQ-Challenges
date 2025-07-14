using static System.Console;
namespace LINQ_Challeges;

public class LinqChallenge_30
{
    private List<(int CustomerId, string Name, string Region)> customers = new()
    {
        (1, "Ava", "Sydney"), (2, "Ben", "Melbourne"), (3, "Cara", "Brisbane"),
        (4, "Dan", "Sydney"), (5, "Ella", "Perth"), (6, "Finn", "Adelaide"),
        (7, "Gina", "Melbourne"), (8, "Hugo", "Sydney"), (9, "Ivy", "Brisbane"), (10, "Jack", "Melbourne")
    };

    private List<(int OrderId, int CustomerId, DateTime Date, string Store)> orders = new()
    {
        (1001, 1, new(2024,1,10), "Store A"), (1002, 2, new(2024,1,15), "Store B"),
        (1003, 3, new(2024,2,5), "Store A"), (1004, 4, new(2024,2,20), "Store C"),
        (1005, 1, new(2024,3,3), "Store B"), (1006, 5, new(2024,3,18), "Store C"),
        (1007, 6, new(2024,4,10), "Store A"), (1008, 7, new(2024,4,12), "Store B"),
        (1009, 8, new(2024,5,5), "Store A"), (1010, 9, new(2024,5,7), "Store B")
    };

    private List<(int OrderId, int ProductId, int Quantity)> orderItems = new()
    {
        (1001, 201, 2), (1001, 202, 1), (1002, 203, 3),
        (1003, 204, 1), (1004, 205, 2), (1005, 201, 1),
        (1006, 206, 4), (1007, 203, 2), (1008, 204, 2),
        (1009, 202, 1), (1010, 205, 1)
    };

    private List<(int ProductId, string Name, string Category, decimal UnitPrice)> products = new()
    {
        (201, "Laptop", "Electronics", 1800), (202, "Headphones", "Electronics", 250),
        (203, "Desk Chair", "Furniture", 600), (204, "Keyboard", "Electronics", 120),
        (205, "Desk Lamp", "Furniture", 90), (206, "Monitor", "Electronics", 400)
    };

    private List<(int OrderId, decimal AmountPaid, bool Refunded)> payments = new()
    {
        (1001, 3850, false), (1002, 1800, false), (1003, 120, true),
        (1004, 180, false), (1005, 1800, false), (1006, 1600, false),
        (1007, 1200, false), (1008, 240, false), (1009, 250, true),
        (1010, 90, false)
    };

    public LinqChallenge_30()
    {
        //HighestSpenders_T1();
        //HighestSpendersAbove5000_T2();
        //ElectronicsSpendPerStore_T3();
        MonthlyOrderCounts_T4();
        //RefundImpactByRegion_T5();
    }

    // 🔹 Task 1: Highest Spenders
    // Join customers → orders → payments. Group by customer, sum paid amount.
    // Order descending.
    // ⏱️ Expected: 10–12 min
    // 11:38 - 11:46
    private void HighestSpenders_T1()
    {
        var highestSpenders = from cust in customers
                              join order in orders on cust.CustomerId equals order.CustomerId
                              join pay in payments on order.OrderId equals pay.OrderId

                              where !pay.Refunded

                              group new { pay, order } by cust into custGroup

                              let paidAmountSum = custGroup.Sum(x => x.pay.AmountPaid)

                              orderby paidAmountSum descending
                              select new
                              {
                                  Customer = custGroup.Key.Name,
                                  PaidAmountSum = paidAmountSum
                              };
        foreach (var item in highestSpenders)
        {
            WriteLine($"{item.Customer}\t\t${item.PaidAmountSum}");
        }
    }

    // 🔹 Task 2: Highest Spenders Above $5,000 (Paged)
    // Filter Task 1 result to ≥ $5000, paginate 3 per page
    // ⏱️ Expected: 12–15 min
    // 11:50 - 12:01
    private void HighestSpendersAbove5000_T2()
    {
        var highestSpenders = (from cust in customers
                               join order in orders on cust.CustomerId equals order.CustomerId
                               join pay in payments on order.OrderId equals pay.OrderId

                               group new { pay, order } by cust into custGroup

                               let totalPaid = custGroup.Sum(x => x.pay.AmountPaid)
                               let totalRefunded = custGroup.Where(x => x.pay.Refunded).Sum(x => x.pay.AmountPaid)
                               let actualPay = totalPaid - totalRefunded

                               where actualPay >= 5000
                               orderby actualPay descending

                               select new
                               {
                                   Customer = custGroup.Key.Name,
                                   TotalPaid = Math.Round(totalPaid, 2),
                                   TotalRefunded = Math.Round(totalRefunded, 2),
                                   ActualPay = Math.Round(actualPay, 2)
                               }).ToArray();
        var perPage = 3;
        for (var pageNum = 0; ;)
        {
            var pagedResult = highestSpenders.Skip(pageNum * perPage).Take(perPage);
            if (!pagedResult.Any())
                break;
            WriteLine($"\nPage[ {++pageNum} ]");
            foreach (var item in pagedResult)
            {
                WriteLine($"Customer {item.Customer}\tTotal Paid$: {item.TotalPaid}\tRefunded$: {item.TotalRefunded}\tActual Paid$: {item.ActualPay}");
            }
        }
    }

    // 🔹 Task 3: Electronics Spend Per Store
    // Total value of electronics products sold per store (use product category + joins)
    // ⏱️ Expected: 12–15 min
    // 8:36 - 8:44
    private void ElectronicsSpendPerStore_T3()
    {
        var electronicsPerStore = from order in orders
                                  join orderItem in orderItems on order.OrderId equals orderItem.OrderId
                                  join prod in products on orderItem.ProductId equals prod.ProductId
                                  where prod.Category.Equals("Electronics", StringComparison.OrdinalIgnoreCase)

                                  group new { orderItem, prod } by order.Store into storeGroup

                                  let totalValue = storeGroup.Sum(x => x.orderItem.Quantity * x.prod.UnitPrice)
                                  orderby totalValue descending

                                  select new
                                  {
                                      Store = storeGroup.Key,
                                      TotalValue = Math.Round(totalValue, 2)
                                  };

        foreach (var item in electronicsPerStore)
        {
            WriteLine($"{item.Store}\t\tTotal Electronic Income ${item.TotalValue}");
        }
    }

    // 🔹 Task 4: Monthly Order Counts
    // Group orders by Month-Year, show total count per month
    // ⏱️ Expected: 10–12 min
    // 8:47 - 8:58
    private void MonthlyOrderCounts_T4()
    {
        var monthlyOrderCounts = from order in orders
                                 group order.Date by new { order.Date.Year, order.Date.Month } into yearMonthGroup
                                 orderby yearMonthGroup.Key.Year descending,
                                    yearMonthGroup.Key.Month descending
                                 select new
                                 {
                                     YearMonth = $"{yearMonthGroup.Key.Year}/{yearMonthGroup.Key.Month}",
                                     OrdersCount = yearMonthGroup.Count()
                                 };
        foreach (var item in monthlyOrderCounts)
        {
            WriteLine($"{item.YearMonth}\t\t {item.OrdersCount} orders");
        }
    }

    // 🔹 Task 5: Refund Impact by Region
    // For each region, show % of total paid that was refunded (aggregate & filter)
    // ⏱️ Expected: 15–18 min
    private void RefundImpactByRegion_T5() { }
}
