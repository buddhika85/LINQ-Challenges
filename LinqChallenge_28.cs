using System.Diagnostics;
using System.Globalization;
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
        MonthlyDepositTrend_T1();
        //CustomerSpendingStdev_T2();
        //DormantAccounts_T3();
        //AccountChurnDetection_T4();
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
    private void CustomerSpendingStdev_T2()
    {
    }

    // 🔹 Task 3: Dormant Accounts
    // List accounts with no transactions in the past 60 days (relative to latest txn).
    // ⏱️ Expected: 12–15 min
    private void DormantAccounts_T3() { }

    // 🔹 Task 4: Account Churn Detection
    // For each customer, show number of accounts closed (simulated via Balance = 0).
    // Include only customers with 2+ accounts and at least 1 closure.
    // ⏱️ Expected: 12–15 min
    private void AccountChurnDetection_T4() { }

}
