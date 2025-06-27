using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_22
{
    private List<(int CustomerId, string Name, string Region)> customers = new()
    {
        (1, "Alice", "East"), (2, "Bob", "West"), (3, "Cara", "East"),
        (4, "Dan", "North"), (5, "Ella", "West"), (6, "Finn", "South"),
        (7, "Gina", "East"), (8, "Hugo", "North"), (9, "Ivy", "South"), (10, "Jack", "West")
    };

    private List<(int OrderId, int CustomerId, DateTime Date, decimal Amount)> orders = new()
    {
        (1001, 1, new DateTime(2024, 1, 5), 1200), (1002, 2, new DateTime(2024, 1, 7), 3100),
        (1003, 1, new DateTime(2024, 2, 1), 2700), (1004, 3, new DateTime(2024, 2, 9), 4100),
        (1005, 4, new DateTime(2024, 2, 11), 1900), (1006, 5, new DateTime(2024, 3, 2), 4800),
        (1007, 6, new DateTime(2024, 3, 4), 3300), (1008, 2, new DateTime(2024, 3, 6), 950),
        (1009, 3, new DateTime(2024, 3, 7), 620), (1010, 7, new DateTime(2024, 4, 1), 7100),
        (1011, 8, new DateTime(2024, 4, 4), 2000), (1012, 9, new DateTime(2024, 4, 5), 2900),
        (1013, 10, new DateTime(2024, 4, 6), 1200), (1014, 4, new DateTime(2024, 4, 6), 2500),
        (1015, 6, new DateTime(2024, 4, 6), 750)
    };

    public LinqChallenge_22()
    {
        HighestSpenders_T1();
        //HighestSpendersAbove5000_T2();
        //RegionalSpenderStats_T3();
    }

    // 🔹 Task 1: List customers sorted by total spend (high to low)
    // ⏱️ Expected Time: 10–12 minutes
    // 9:32 AM - 9:40 AM
    private void HighestSpenders_T1()
    {
        var highestSpenders = from cust in customers
                              join order in orders on cust.CustomerId equals order.CustomerId
                              group order by cust into custGroup
                              let totalSpend = custGroup.Sum(x => x.Amount)
                              orderby totalSpend descending
                              select new
                              {
                                  Customer = custGroup.Key.Name,
                                  TotalSpend = totalSpend
                              };

        var result = highestSpenders.ToList();      // immediate execuction
        var perPage = 2;
        for (var pageNum = 0; result.Any();)
        {
            var pagedResult = result.Skip(pageNum * perPage).Take(perPage);
            if (pagedResult.Any())
            {
                WriteLine($"\nPage [ {++pageNum} ]");
                foreach (var item in pagedResult)
                {
                    WriteLine($"{item.Customer}\t\t Spent ${item.TotalSpend}");
                }
            }
            else
                break;
        }
    }

    // 🔹 Task 2: Show only customers whose total spend is above $5000
    // ⏱️ Expected Time: 8–10 minutes
    private void HighestSpendersAbove5000_T2()
    {
        // implement LINQ query here...
    }

    // 🔹 Task 3: Group by region and compute:
    //   - Avg spend per customer
    //   - Total number of customers in each region
    //   - Only include regions with 2+ customers
    // ⏱️ Expected Time: 12–15 minutes
    private void RegionalSpenderStats_T3()
    {
        // implement LINQ query here...
    }
}
