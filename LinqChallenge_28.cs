using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_28
{
    private List<(int CustomerId, string Name, string Region)> customers = new()
    {
        (1, "Ava", "East"), (2, "Ben", "West"), (3, "Cara", "North"),
        (4, "Dan", "East"), (5, "Ella", "South"), (6, "Finn", "West"),
        (7, "Gina", "South"), (8, "Hugo", "North"), (9, "Ivy", "East"), (10, "Jack", "West")
    };

    private List<(int AccountId, int CustomerId, string Type, decimal Balance)> accounts = new()
    {
        (201, 1, "Savings", 8400), (202, 2, "Checking", 2200), (203, 3, "Savings", 3000),
        (204, 4, "Savings", 9400), (205, 5, "Checking", 1600), (206, 6, "Checking", 7800),
        (207, 7, "Savings", 5200), (208, 8, "Savings", 2400), (209, 9, "Checking", 4500), (210, 10, "Savings", 8300)
    };

    private List<(int TxnId, int AccountId, DateTime Date, string Type, decimal Amount)> transactions = new()
    {
        (1001, 201, new(2024,1,5), "Deposit", 2000),
        (1002, 201, new(2024,1,15), "Withdrawal", 600),
        (1003, 202, new(2024,1,10), "Deposit", 900),
        (1004, 203, new(2024,2,1), "Withdrawal", 1200),
        (1005, 204, new(2024,2,10), "Deposit", 5000),
        (1006, 205, new(2024,2,15), "Withdrawal", 500),
        (1007, 206, new(2024,2,20), "Deposit", 3000),
        (1008, 207, new(2024,3,1), "Deposit", 1200),
        (1009, 208, new(2024,3,5), "Withdrawal", 800),
        (1010, 209, new(2024,3,10), "Deposit", 1800),
        (1011, 210, new(2024,3,15), "Deposit", 2200),
        (1012, 204, new(2024,3,17), "Withdrawal", 900),
        (1013, 207, new(2024,3,20), "Deposit", 3000)
    };

    public LinqChallenge_28()
    {
        //MonthlyDepositTrend_T1();
        //CustomerSpendingStdev_T2();
        //DormantAccounts_T3();
        AccountChurnDetection_T4();
    }

    // 🔹 Task 1: Monthly Deposit Trend
    // For each month, show total deposits (grouped by month). Sorted chronologically.
    // ⏱️ Expected: 10–12 min
    // 4:40-4:52
    private void MonthlyDepositTrend_T1()
    {
        var monthlyDepositTrend = from trans in transactions
                                  where trans.Type == "Deposit"
                                  group trans by new { trans.Date.Year, trans.Date.Month } into monthGroup

                                  let totalDeposits = monthGroup.Sum(x => x.Amount)
                                  orderby monthGroup.Key.Year, monthGroup.Key.Month

                                  select new
                                  {
                                      YearMonth = $"{monthGroup.Key.Year} - {monthGroup.Key.Month}",
                                      TotalDeposits = totalDeposits
                                  };
        foreach (var item in monthlyDepositTrend)
        {
            WriteLine($"{item.YearMonth}\t\t${item.TotalDeposits} Total Deposits");
        }
    }

    // 🔹 Task 2: Customer Spending Standard Deviation
    // For each customer, calculate the std deviation of their withdrawal amounts.
    // ⏱️ Expected: 15–18 min
    // 4:54 - 5:09
    private void CustomerSpendingStdev_T2()
    {
        var custSpendStdDev = from cust in customers
                              join acc in accounts on cust.CustomerId equals acc.CustomerId
                              join trans in transactions on acc.AccountId equals trans.AccountId

                              where trans.Type == "Withdrawal"

                              group trans by cust into custGroup

                              let stdDev = custGroup.Select(x => x.Amount).StandardDeviation()

                              orderby stdDev descending
                              select new
                              {
                                  Customer = custGroup.Key.Name,
                                  WithdrawalStandardDev = stdDev
                              };
        foreach (var item in custSpendStdDev)
        {
            WriteLine($"{item.Customer}\t\tWithdraw amounts std dev : ${item.WithdrawalStandardDev}");
        }
    }


    // 🔹 Task 3: Dormant Accounts
    // List accounts with no transactions in the past 60 days (relative to latest txn).
    // ⏱️ Expected: 12–15 min
    // 8:16 - 8:31
    private void DormantAccounts_T3()
    {
        var dormantAccs = from acc in accounts
                          join trans in transactions on acc.AccountId equals trans.AccountId into transGroup
                          from trans in transGroup.DefaultIfEmpty()
                          join cust in customers on acc.CustomerId equals cust.CustomerId
                          group new { trans, cust } by acc into accGroup

                          let latest = accGroup.Select(x => x.trans).Max(x => x.Date)
                          let customer = accGroup.Select(x => x.cust).FirstOrDefault()

                          where latest < DateTime.Today.AddDays(-60)

                          orderby latest descending
                          select new
                          {
                              AccountId = accGroup.Key.AccountId,
                              Customer = customer.Name,
                              LatestTransDate = latest,
                          };

        foreach (var item in dormantAccs)
        {
            WriteLine($"{item.AccountId}\t\tCustomer:{item.Customer}\t\tlast transaction date: {item.LatestTransDate}");
        }

    }

    // 🔹 Task 4: Account Churn Detection
    // For each customer, show number of accounts closed (simulated via Balance = 0).
    // Include only customers with 2+ accounts and at least 1 closure.
    // ⏱️ Expected: 12–15 min
    // 8:21 - 8:38
    private void AccountChurnDetection_T4()
    {
        var accChurn = from cust in customers
                       join acc in accounts on cust.CustomerId equals acc.CustomerId

                       group acc by cust into custGroup

                       let accCount = custGroup.Count()
                       where accCount >= 2

                       let zeroBalanceAccCount = custGroup.Where(x => x.Balance == 0.0m).Count()
                       where zeroBalanceAccCount >= 1

                       select new
                       {
                           Customer = custGroup.Key.Name,
                           AccountsCount = accCount,
                           ZeroBalanceAccCount = zeroBalanceAccCount
                       };
        foreach (var item in accChurn)
        {
            WriteLine($"{item.Customer}\t\t{item.AccountsCount} accounts\t\t{item.ZeroBalanceAccCount} Zero Accounts");
        }
    }

}
