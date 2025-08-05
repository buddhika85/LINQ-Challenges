using static System.Console;

namespace LINQ_Challeges;

public class LinqChallengeBehavioralOps
{
    private List<(int CustomerId, string Name, DateTime FirstOrderDate)> customers = new()
    {
        (1, "Ava", new(2023, 5, 1)), (2, "Ben", new(2023, 6, 15)), (3, "Cara", new(2023, 7, 10)),
        (4, "Dan", new(2023, 8, 5)), (5, "Ella", new(2023, 8, 22)), (6, "Finn", new(2023, 9, 3))
    };

    private List<(int OrderId, int CustomerId, DateTime OrderDate, decimal Amount)> orders = new()
    {
        (1001, 1, new(2023,5,1), 550), (1002, 2, new(2023,6,15), 1200), (1003, 1, new(2023,7,10), 870),
        (1004, 3, new(2023,7,15), 400), (1005, 4, new(2023,8,5), 650), (1006, 2, new(2023,8,20), 750),
        (1007, 1, new(2023,9,5), 1120), (1008, 5, new(2023,9,10), 430), (1009, 6, new(2023,10,1), 980)
    };

    private List<(int SupplierId, string Name)> suppliers = new()
    {
        (201, "TechDepot"), (202, "GearX"), (203, "GlobalLink")
    };

    private List<(int DeliveryId, int SupplierId, DateTime DeliveredOn, string ProductCategory, int Quantity)> deliveries = new()
    {
        (3001, 201, new(2023,5,20), "Electronics", 50), (3002, 202, new(2023,5,21), "Furniture", 30),
        (3003, 201, new(2023,6,10), "Electronics", 25), (3004, 203, new(2023,7,5), "Office", 40),
        (3005, 202, new(2023,8,1), "Furniture", 20), (3006, 201, new(2023,8,12), "Electronics", 60),
        (3007, 203, new(2023,9,5), "Office", 50), (3008, 202, new(2023,10,1), "Furniture", 45)
    };

    public LinqChallengeBehavioralOps()
    {
        //RepeatCustomerSpend_T1();
        //SupplierDeliveryFrequency_T2();
        //CategoryVelocity_T3();
        MonthlyNewCustomerEngagement_T4();
        //TopCategoriesByVolume_T5();
    }

    // 🔹 Task 1: Repeat Customer Spend
    // Identify customers with ≥2 orders and compute total spend
    // ⏱️ Expected: 10–12 min
    // 5:03 - 5:11
    private void RepeatCustomerSpend_T1()
    {
        var repeatCustomerSpend = from cust in customers
                                  join order in orders on cust.CustomerId equals order.CustomerId

                                  group order by cust into custGroup

                                  let orderCount = custGroup.Count()
                                  where orderCount >= 2

                                  let totalSpent = custGroup.Sum(x => x.Amount)
                                  let avgSpent = custGroup.Average(x => x.Amount)

                                  orderby totalSpent descending, custGroup.Key.Name
                                  select new
                                  {
                                      Customer = custGroup.Key.Name,
                                      OrdersCount = orderCount,
                                      TotalSpent = totalSpent,
                                      AvgSpent = Math.Round(avgSpent, 2),
                                  };

        foreach (var item in repeatCustomerSpend)
        {
            WriteLine($"{item.Customer}\t\tOrders count: {item.OrdersCount}\t\tTotal Spent: ${item.TotalSpent}\t\tAvg Spent: ${item.AvgSpent}");
        }
    }

    // 🔹 Task 2: Supplier Delivery Frequency
    // Show delivery count and total quantity per supplier, ordered by delivery count descending
    // ⏱️ Expected: 12–15 min
    // 5:20 - 5:27
    private void SupplierDeliveryFrequency_T2()
    {
        var supplierDeliveryFrequency = from supp in suppliers
                                        join deli in deliveries on supp.SupplierId equals deli.SupplierId
                                        group deli by supp into suppGroup

                                        let deliveryCount = suppGroup.Count()
                                        let totalQuantity = suppGroup.Sum(x => x.Quantity)
                                        let avgQuantity = deliveryCount > 0 ? suppGroup.Average(x => x.Quantity) : 0.0

                                        orderby deliveryCount descending, suppGroup.Key.Name

                                        select new
                                        {
                                            Supplier = suppGroup.Key.Name,
                                            DeliveryCount = deliveryCount,
                                            TotalQuantity = totalQuantity,
                                            AvgQuantity = Math.Round(avgQuantity, 2)
                                        };
        foreach (var item in supplierDeliveryFrequency)
        {
            WriteLine($"{item.Supplier}\t\tTotal Delivery Count: {item.DeliveryCount}\t\tTotal Quantity: {item.TotalQuantity}\t\tAverage Quantity: {item.AvgQuantity}");
        }
    }

    // 🔹 Task 3: Category Velocity
    // For each product category, show average quantity per delivery and number of suppliers involved
    // ⏱️ Expected: 15–18 min
    // 9:52 - 10:02
    private void CategoryVelocity_T3()
    {
        var query = from del in deliveries
                    join supp in suppliers on del.SupplierId equals supp.SupplierId
                    group new { del, supp } by del.ProductCategory into cateGroup

                    let avgQty = cateGroup.Average(x => x.del.Quantity)
                    let totQty = cateGroup.Sum(x => x.del.Quantity)
                    let minQty = cateGroup.Min(x => x.del.Quantity)
                    let maxQty = cateGroup.Max(x => x.del.Quantity)
                    let countSupp = cateGroup.Select(x => x.supp.SupplierId).Distinct().Count()

                    orderby avgQty descending, cateGroup.Key

                    select new
                    {
                        Category = cateGroup.Key,
                        CountSuppliers = countSupp,
                        AvgQty = Math.Round(avgQty, 2),
                        TotQty = totQty,
                        MinQty = minQty,
                        MaxQty = maxQty
                    };
        foreach (var item in query)
        {
            WriteLine($"{item.Category}\t\tSuppliers: {item.CountSuppliers}\t\tAvg Qty: {item.AvgQty}\t\tMin Qty: {item.MinQty}\t\tMax Qty: {item.MaxQty}\t\tTotal Qty: {item.TotQty}");
        }
    }

    // 🔹 Task 4: Monthly New Customer Engagement
    // Count new customers per month based on `FirstOrderDate`
    // ⏱️ Expected: 10–12 min
    // 10;21 - 
    private void MonthlyNewCustomerEngagement_T4()
    {
        var query = from cust in customers
                    let firstOrderMonth = cust.FirstOrderDate.Month
                    let firstOrderYear = cust.FirstOrderDate.Year

                    group cust by new { firstOrderYear, firstOrderMonth } into yearMonthGroup

                    orderby yearMonthGroup.Key.firstOrderYear, yearMonthGroup.Key.firstOrderMonth descending

                    select new
                    {
                        YearMonth = $"{yearMonthGroup.Key.firstOrderYear} - {yearMonthGroup.Key.firstOrderMonth}",
                        NewCustomerCount = yearMonthGroup.Count()
                    };

        foreach (var item in query)
        {
            WriteLine($"{item.YearMonth} \t\t New Customer Count: {item.NewCustomerCount}");
        }
    }

    // 🔹 Task 5: Top Categories by Volume (Paged)
    // Group by product category, sum quantities, order descending, paginate 3 per page
    // ⏱️ Expected: 12–15 min
    private void TopCategoriesByVolume_T5() { }
}