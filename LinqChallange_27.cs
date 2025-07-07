using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_27
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

    public LinqChallenge_27()
    {
        //CustomerBalances_T1();
        //TopSavers_T2();
        //TransactionFrequency_T3();
        RegionalDepositTotals_T4();
    }

    // 🔹 Task 1: Customer Balances
    // Join accounts & customers. Show total balance per customer. Sorted DESC.
    // ⏱️ Expected: 10–12 min
    // 10:06 - 10:11
    private void CustomerBalances_T1()
    {
        var customerBalances = from cust in customers
                               join acc in accounts on cust.CustomerId equals acc.CustomerId
                               group acc by cust into custGroup

                               let totalBalance = custGroup.Sum(x => x.Balance)
                               orderby totalBalance descending
                               select new
                               {
                                   Customer = custGroup.Key.Name,
                                   TotalBalance = totalBalance
                               };
        foreach (var item in customerBalances)
        {
            WriteLine($"{item.Customer}\t\tTotal balance $:{item.TotalBalance}");
        }
    }

    // 🔹 Task 2: Top Savers (Pagination)
    // Filter for "Savings" accounts, group by customer, show top balances paged 3 per page.
    // ⏱️ Expected: 12–15 min
    // 10:12 - 10:18
    private void TopSavers_T2()
    {
        var topSavers = (from cust in customers
                         join acc in accounts on cust.CustomerId equals acc.CustomerId
                         group acc by cust into custGroup

                         let topBalance = custGroup.Max(x => x.Balance)
                         orderby topBalance descending
                         select new
                         {
                             Customer = custGroup.Key.Name,
                             TopBalance = topBalance
                         }).ToList();
        var perPage = 3;
        for (var pageNum = 0; ;)
        {
            var pageResult = topSavers.Skip(pageNum * perPage).Take(perPage).ToList();
            if (!pageResult.Any())
                break;
            WriteLine($"\nPage [ {++pageNum} ]");
            foreach (var item in pageResult)
            {
                WriteLine($"{item.Customer}\t\tTop Balance: ${item.TopBalance}");
            }
        }
    }

    // 🔹 Task 3: Transaction Frequency
    // Group by account and count Deposits vs Withdrawals
    // ⏱️ Expected: 14–17 min
    // 10:19 - 10:35
    private void TransactionFrequency_T3()
    {
        var transactionFrequency = from acc in accounts
                                   join trans in transactions on acc.AccountId equals trans.AccountId
                                   group trans by acc into accGroup

                                   let allDeposits = accGroup.Where(x => x.Type.Equals("Deposit", StringComparison.OrdinalIgnoreCase))
                                   let allWithdrawals = accGroup.Where(x => x.Type.Equals("Withdrawal", StringComparison.OrdinalIgnoreCase))

                                   let depositCount = allDeposits.Count()
                                   let minDeposit = allDeposits.Any() ? allDeposits.Min(x => x.Amount) : 0.0m
                                   let maxDeposit = allDeposits.Any() ? allDeposits.Max(x => x.Amount) : 0.0m
                                   let avgDeposit = allDeposits.Any() ? allDeposits.Average(x => x.Amount) : 0.0m

                                   let withdrawCount = allWithdrawals.Count()
                                   let minWithdrawal = allWithdrawals.Any() ? allWithdrawals.Min(x => x.Amount) : 0.0m
                                   let maxWithdrawal = allWithdrawals.Any() ? allWithdrawals.Max(x => x.Amount) : 0.0m
                                   let avgWithdrawal = allWithdrawals.Any() ? allWithdrawals.Average(x => x.Amount) : 0.0m

                                   let totalTransacions = depositCount + withdrawCount

                                   orderby totalTransacions descending

                                   let accOwner = (from cust in customers where cust.CustomerId == accGroup.Key.CustomerId select cust.Name).FirstOrDefault()

                                   select new
                                   {
                                       AccountNumber = accGroup.Key.AccountId,
                                       AccountOwner = accOwner,
                                       TotalTransactions = totalTransacions,
                                       DepositCount = depositCount,
                                       MinDeposit = minDeposit,
                                       MaxDeposit = maxDeposit,
                                       AvgDeposit = Math.Round(avgDeposit, 2),

                                       WithdrawCount = withdrawCount,
                                       MinWithdrawal = minWithdrawal,
                                       MaxWithdrawal = maxWithdrawal,
                                       AvgWithdrawal = Math.Round(avgWithdrawal, 2)
                                   };
        foreach (var item in transactionFrequency)
        {
            WriteLine($"Acc Num: {item.AccountNumber}\t\t of {item.AccountOwner}\t\t{item.TotalTransactions} Transactions" +
                $"\t\t{item.DepositCount} Deposits\t\t${item.MinDeposit} min\t\t ${item.MaxDeposit} max\t\t${item.AvgDeposit} avg" +
                $"\t\t{item.WithdrawCount} Withdrwals\t\t${item.MinWithdrawal} min\t\t ${item.MaxWithdrawal} max\t\t${item.AvgWithdrawal} avg");
        }
    }

    // 🔹 Task 4: Regional Deposit Totals
    // Total deposit amounts grouped by region. Only include if total ≥ $4,000
    // ⏱️ Expected: 12–15 min
    // 10:53 - 11:04
    private void RegionalDepositTotals_T4()
    {
        var regionalDepositTotals = from trans in transactions
                                    join acc in accounts on trans.AccountId equals acc.AccountId
                                    join cust in customers on acc.CustomerId equals cust.CustomerId

                                    group trans by cust.Region into regionGroup

                                    let allDepositsOfRegion = regionGroup.Where(x => x.Type.Equals("Deposit"))
                                    let totalDeposits = allDepositsOfRegion.Sum(x => x.Amount)

                                    where totalDeposits >= 4000

                                    let depositCount = allDepositsOfRegion.Count()
                                    let minDeposit = depositCount > 0 ? allDepositsOfRegion.Min(x => x.Amount) : 0.0m
                                    let maxDeposit = depositCount > 0 ? allDepositsOfRegion.Max(x => x.Amount) : 0.0m
                                    let avgDeposit = depositCount > 0 ? allDepositsOfRegion.Average(x => x.Amount) : 0.0m

                                    orderby totalDeposits descending

                                    select new
                                    {
                                        Region = regionGroup.Key,
                                        DepositCount = depositCount,
                                        TotalDeposits = totalDeposits,
                                        MinDeposit = minDeposit,
                                        MaxDeposit = maxDeposit,
                                        AvgDeposit = Math.Round(avgDeposit, 2)
                                    };
        foreach (var item in regionalDepositTotals)
        {
            WriteLine($"{item.Region}\t\tCount: {item.DepositCount}\t\tTotal Deposits: ${item.TotalDeposits}\t\tMin ${item.MinDeposit}\t\tMax ${item.MaxDeposit}\t\t Average ${item.AvgDeposit}");
        }
    }
}
