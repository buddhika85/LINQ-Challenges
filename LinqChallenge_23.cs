using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_23
{
    private List<(int UserId, string Name, string Tier, string Country)> users = new()
    {
        (1, "Ava", "Pro", "USA"), (2, "Ben", "Basic", "Canada"), (3, "Cara", "Pro", "USA"),
        (4, "Dan", "Free", "UK"), (5, "Ella", "Basic", "Germany"), (6, "Finn", "Pro", "India"),
        (7, "Gina", "Free", "USA"), (8, "Hugo", "Pro", "India"), (9, "Ivy", "Basic", "Canada"),
        (10, "Jack", "Basic", "USA")
    };

    private List<(int PlanId, string Tier, decimal MonthlyPrice)> plans = new()
    {
        (101, "Free", 0), (102, "Basic", 25), (103, "Pro", 70)
    };

    private List<(int UserId, int PlanId, DateTime Start, DateTime? End)> subscriptions = new()
    {
        (1, 103, DateTime.Parse("2024-01-01"), null),
        (2, 102, DateTime.Parse("2024-01-15"), null),
        (3, 103, DateTime.Parse("2024-02-01"), DateTime.Parse("2024-06-01")),
        (4, 101, DateTime.Parse("2024-01-10"), null),
        (5, 102, DateTime.Parse("2024-01-20"), null),
        (6, 103, DateTime.Parse("2024-01-05"), null),
        (7, 101, DateTime.Parse("2024-02-01"), null),
        (8, 103, DateTime.Parse("2024-02-20"), null),
        (9, 102, DateTime.Parse("2024-03-01"), DateTime.Parse("2024-07-01")),
        (10, 102, DateTime.Parse("2024-03-15"), null)
    };

    private List<(int UserId, DateTime Timestamp, string Action)> usageLogs = new()
    {
        (1, DateTime.Parse("2024-03-01"), "Login"), (1, DateTime.Parse("2024-03-03"), "Export"),
        (2, DateTime.Parse("2024-03-05"), "Login"), (2, DateTime.Parse("2024-03-07"), "Share"),
        (3, DateTime.Parse("2024-03-02"), "Export"), (3, DateTime.Parse("2024-03-04"), "Download"),
        (6, DateTime.Parse("2024-03-06"), "Login"), (6, DateTime.Parse("2024-03-07"), "Download"),
        (8, DateTime.Parse("2024-03-03"), "Export"), (8, DateTime.Parse("2024-03-05"), "Export")
    };

    private List<(int UserId, int PlanId, int Score)> feedbacks = new()
    {
        (1, 103, 5), (2, 102, 4), (3, 103, 4),
        (5, 102, 3), (6, 103, 5), (8, 103, 5),
        (9, 102, 3), (10, 102, 4)
    };

    public LinqChallenge_23()
    {
        //TierLeaderboard_T1();
        TopExporters_T2();
        //AvgScorePerTier_T3();
        //ActiveUserRevenueReport_T4();
    }

    // 🔹 Task 1: Tier Leaderboard
    // Group users by Tier, count members, and order by count DESC.
    // ⏱️ Expected time: 8–10 minutes
    // 10:06 AM - 10:11 AM
    private void TierLeaderboard_T1()
    {
        var usersByTier = from user in users
                          group user by user.Tier into userGroup
                          let userCount = userGroup.Count()
                          orderby userCount descending
                          select new
                          {
                              Tier = userGroup.Key,
                              UserCount = userCount
                          };

        foreach (var item in usersByTier)
        {
            WriteLine($"{item.Tier}\t\t{item.UserCount}");
        }
    }

    // 🔹 Task 2: Top Export Users
    // Identify users with the highest number of "Export" actions. Include pagination (3 per page).
    // ⏱️ Expected time: 10–12 minutes
    // 10:12 - 10:21
    private void TopExporters_T2()
    {
        var highestExporters = (from user in users
                                join log in usageLogs on user.UserId equals log.UserId
                                where log.Action.Equals("Export", StringComparison.OrdinalIgnoreCase)

                                group log by user into userGroup

                                let exportCount = userGroup.Count()
                                orderby exportCount descending

                                select new
                                {
                                    User = userGroup.Key.Name,
                                    ExportCount = exportCount
                                }).ToList();

        var perPage = 3;
        for (var pageNum = 0; ;)
        {
            var pagedResult = highestExporters.Skip(pageNum * perPage).Take(perPage);
            if (pagedResult.Any())
            {
                WriteLine($"Page [ {++pageNum} ]");
                foreach (var item in pagedResult)
                {
                    WriteLine($"{item.User}\t\t{item.ExportCount} Exports");
                }
            }
            else
                break;
        }
    }

    // 🔹 Task 3: Feedback Quality by Tier
    // Calculate average feedback score per plan Tier. Only include tiers with 2+ reviews.
    // ⏱️ Expected time: 12–15 minutes
    private void AvgScorePerTier_T3() { }

    // 🔹 Task 4: Active User Revenue Report
    // For active (non-ended) subscriptions: compute revenue per user and total per country.
    // Return top 3 countries by total revenue.
    // ⏱️ Expected time: 15–18 minutes
    private void ActiveUserRevenueReport_T4() { }
}
