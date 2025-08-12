using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_34
{
    private List<(int CustomerId, string Name, string Region)> customers = new()
    {
        (1, "Alice", "NSW"),
        (2, "Bob", "VIC"),
        (3, "Charlie", "QLD"),
        (4, "Diana", "NSW"),
        (5, "Ethan", "WA")
    };

    private List<(int OrderId, int CustomerId, DateTime OrderDate, decimal Amount)> orders = new()
    {
        (101, 1, new DateTime(2025, 7, 1), 1200),
        (102, 2, new DateTime(2025, 7, 2), 5400),
        (103, 3, new DateTime(2025, 7, 3), 300),
        (104, 1, new DateTime(2025, 7, 4), 800),
        (105, 4, new DateTime(2025, 7, 5), 6200),
        (106, 5, new DateTime(2025, 7, 6), 150),
        (107, 2, new DateTime(2025, 7, 7), 4700)
    };

    private List<(int ProductId, string Name, string Category, decimal Price)> products = new()
    {
        (201, "Laptop", "Electronics", 1500),
        (202, "Phone", "Electronics", 800),
        (203, "Desk", "Furniture", 300),
        (204, "Chair", "Furniture", 150),
        (205, "Monitor", "Electronics", 400)
    };

    private List<(int OrderId, int ProductId, int Quantity)> orderItems = new()
    {
        (101, 201, 1),
        (102, 202, 2),
        (102, 205, 3),
        (103, 204, 1),
        (104, 203, 2),
        (105, 201, 2),
        (105, 202, 1),
        (106, 204, 1),
        (107, 205, 4)
    };

    private List<(int WarehouseId, string Location)> warehouses = new()
    {
        (301, "Sydney"),
        (302, "Melbourne"),
        (303, "Brisbane")
    };

    private List<(int ShipmentId, int WarehouseId, int ProductId, DateTime Date, int Quantity)> shipments = new()
    {
        (401, 301, 201, new DateTime(2025, 6, 20), 10),
        (402, 302, 202, new DateTime(2025, 6, 21), 15),
        (403, 303, 203, new DateTime(2025, 6, 22), 5),
        (404, 301, 204, new DateTime(2025, 6, 23), 20),
        (405, 302, 205, new DateTime(2025, 6, 24), 8)
    };

    private Queue<(DateTime Timestamp, string EventType, string Message)> systemLogs = new(new[]
    {
        (new DateTime(2025, 8, 10, 9, 0, 0), "INFO", "System started"),
        (new DateTime(2025, 8, 10, 9, 5, 0), "WARN", "High memory usage"),
        (new DateTime(2025, 8, 10, 9, 10, 0), "ERROR", "Database timeout"),
        (new DateTime(2025, 8, 10, 9, 15, 0), "INFO", "Recovered from error"),
        (new DateTime(2025, 8, 10, 9, 20, 0), "INFO", "New order received")
    });

    private HashSet<int> flaggedCustomerIds = new() { 2, 5 };

    private Dictionary<int, List<int>> productAdjacency = new()
    {
        [201] = new List<int> { 202, 205 }, // Laptop → Phone, Monitor
        [202] = new List<int> { 201 },      // Phone → Laptop
        [203] = new List<int> { 204 },      // Desk → Chair
        [204] = new List<int> { 203 },      // Chair → Desk
        [205] = new List<int> { 201 }       // Monitor → Laptop
    };


    public LinqChallenge_34()
    {
        //HighestSpenders_T1();
        HighValueOrders_T2();
        //ProductCategoryPerformance_T3();
        //WarehouseStockRotation_T4();
        //FraudFlaggedCustomerOrders_T5();
        //PaginatedOrderHistory_T6();
        //DeferredExecutionTrap_T7();
        //DynamicCustomerSearch_T8();
        //GraphBasedProductRecommendations_T9();
        //LogStreamAnalysis_T10();
    }

    // 🔹 Task 1: Highest Spenders
    // Group orders by customer → sum total spend → order descending
    // ⏱️ Expected: 10–12 min
    // 10 - 10:08
    private void HighestSpenders_T1()
    {
        var query = from order in orders
                    group order by order.CustomerId into custGroup
                    join cust in customers on custGroup.Key equals cust.CustomerId

                    let sumSpent = custGroup.Sum(x => x.Amount)
                    let orderCount = custGroup.Count()
                    let avgSpentPerOrder = sumSpent / orderCount

                    orderby sumSpent descending, orderCount descending, cust.Name

                    select new
                    {
                        CustomerId = custGroup.Key,
                        Customer = cust.Name,
                        OrderCount = orderCount,
                        SumSpent = Math.Round(sumSpent, 2),
                        AvgSpentPerOrder = avgSpentPerOrder
                    };

        foreach (var item in query)
        {
            WriteLine($"ID {item.CustomerId}\tName: {item.Customer}\t\t{item.OrderCount} orders.");
            WriteLine($"\t\tTotal Spent: ${item.SumSpent}\t\tAvg Spent Per Order: ${item.AvgSpentPerOrder}");
        }
    }

    // 🔹 Task 2: High Value Orders
    // Join orders + items + products → filter orders with total > $5000
    // ⏱️ Expected: 15–18 min
    // 9:14 - 9:28
    private void HighValueOrders_T2()
    {
        var query = from order in orders
                    join cust in customers on order.CustomerId equals cust.CustomerId
                    join orderItem in orderItems on order.OrderId equals orderItem.OrderId
                    join product in products on orderItem.ProductId equals product.ProductId

                    group new { cust, orderItem, product } by order into orderGroup

                    let total = orderGroup.Sum(x => x.orderItem.Quantity * x.product.Price)

                    //where total > 5000
                    let products = orderGroup.Select(x => x.product).Distinct()
                    let orderItemCount = orderGroup.Select(x => x.orderItem).Count()
                    let productCount = products.Count()
                    let customer = orderGroup.Select(x => x.cust).FirstOrDefault()
                    let avgProductPrice = products.Average(x => x.Price)

                    orderby total descending, orderItemCount descending, avgProductPrice descending, customer.Name
                    select new
                    {
                        Order = orderGroup.Key,
                        Customer = customer,
                        OrderTotal = Math.Round(total, 2),
                        OrderItemCount = orderItemCount,
                        ProductCount = productCount,
                        AvgProductPrice = Math.Round(avgProductPrice, 2),
                    };

        foreach (var item in query)
        {
            WriteLine($"Order {item.Order.OrderId}\t\t made on: {item.Order.OrderDate.ToShortDateString()}");
            WriteLine($"\tBy Customer ID: {item.Customer.CustomerId}\t\tCustomer: {item.Customer.Name}");

            WriteLine($"\t\tOrder Total: ${item.OrderTotal}\t\tAvg Product Price: ${item.AvgProductPrice}\t\tOrder Lines Count: {item.OrderItemCount}\t\tProduct Count: {item.ProductCount}");
        }
    }

    // 🔹 Task 3: Product Category Performance
    // Group by category → avg price, total quantity sold
    // ⏱️ Expected: 12–15 min
    private void ProductCategoryPerformance_T3() { }

    // 🔹 Task 4: Warehouse Stock Rotation
    // Join shipments + products → group by warehouse → show rotation rate
    // ⏱️ Expected: 15–18 min
    private void WarehouseStockRotation_T4() { }

    // 🔹 Task 5: Fraud-Flagged Customer Orders
    // Inner join orders with flaggedCustomerIds → show suspicious activity
    // ⏱️ Expected: 10–12 min
    private void FraudFlaggedCustomerOrders_T5() { }

    // 🔹 Task 6: Paginated Order History
    // Show orders per customer → paginate 5 per page
    // ⏱️ Expected: 12–15 min
    private void PaginatedOrderHistory_T6() { }

    // 🔹 Task 7: Deferred Execution Trap
    // Show how modifying source after query affects results
    // ⏱️ Expected: 8–10 min
    private void DeferredExecutionTrap_T7() { }

    // 🔹 Task 8: Dynamic Customer Search
    // Build query dynamically using Expression<Func<T, bool>>
    // ⏱️ Expected: 15–18 min
    private void DynamicCustomerSearch_T8() { }

    // 🔹 Task 9: Graph-Based Product Recommendations
    // Traverse productAdjacency graph → show related products
    // ⏱️ Expected: 18–22 min
    private void GraphBasedProductRecommendations_T9() { }

    // 🔹 Task 10: Log Stream Analysis
    // Use Queue to analyze recent system events → group by type
    // ⏱️ Expected: 10–12 min
    private void LogStreamAnalysis_T10() { }

}
