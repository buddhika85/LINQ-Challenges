using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_34
{
    private List<(int CustomerId, string Name, string Region)> customers = new() { /* ... */ };
    private List<(int OrderId, int CustomerId, DateTime OrderDate, decimal Amount)> orders = new() { /* ... */ };
    private List<(int ProductId, string Name, string Category, decimal Price)> products = new() { /* ... */ };
    private List<(int OrderId, int ProductId, int Quantity)> orderItems = new() { /* ... */ };
    private List<(int WarehouseId, string Location)> warehouses = new() { /* ... */ };
    private List<(int ShipmentId, int WarehouseId, int ProductId, DateTime Date, int Quantity)> shipments = new() { /* ... */ };
    private Queue<(DateTime Timestamp, string EventType, string Message)> systemLogs = new(); // FIFO log stream
    private HashSet<int> flaggedCustomerIds = new(); // For fraud detection
    private Dictionary<int, List<int>> productAdjacency = new(); // Simulated graph: product recommendations

    public LinqChallenge_34()
    {
        HighestSpenders_T1();
        //HighValueOrders_T2();
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
    private void HighestSpenders_T1() { }

    // 🔹 Task 2: High Value Orders
    // Join orders + items + products → filter orders with total > $5000
    // ⏱️ Expected: 15–18 min
    private void HighValueOrders_T2() { }

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
