using System.Text.RegularExpressions;

namespace LINQ_Challeges;

public class LinqChallenge_36
{
    // 🔹 Core Data Models
    private List<(int CustomerId, string Name, string Region, bool IsPremium)> customers = new() {
        (1, "Alice", "NSW", true),
        (2, "Bob", "VIC", false),
        (3, "Charlie", "QLD", true),
        (4, "Diana", "NSW", false),
        (5, "Ethan", "WA", true)
    };
    private List<(int OrderId, int CustomerId, DateTime OrderDate, decimal Amount)> orders = new(){
        (101, 1, DateTime.Today.AddDays(-10), 1200),
        (102, 1, DateTime.Today.AddDays(-5), 3800),
        (103, 2, DateTime.Today.AddDays(-8), 600),
        (104, 3, DateTime.Today.AddDays(-3), 5200),
        (105, 4, DateTime.Today.AddDays(-1), 250),
        (106, 5, DateTime.Today.AddDays(-2), 7000)
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
        (101, 202, 2),
        (102, 205, 3),
        (103, 203, 1),
        (104, 201, 2),
        (104, 202, 1),
        (105, 204, 1),
        (106, 201, 1),
        (106, 205, 2)
    };
    private List<(int WarehouseId, string Location)> warehouses = new()
    {
        (1, "Sydney"),
        (2, "Melbourne")
    };
    private List<(int ShipmentId, int WarehouseId, int ProductId, DateTime Date, int Quantity)> shipments = new()
    {
        (301, 1, 201, DateTime.Today.AddDays(-7), 10),
        (302, 1, 202, DateTime.Today.AddDays(-6), 15),
        (303, 2, 203, DateTime.Today.AddDays(-5), 5),
        (304, 2, 204, DateTime.Today.AddDays(-4), 8),
        (305, 1, 205, DateTime.Today.AddDays(-3), 12)
    };
    private Queue<(DateTime Timestamp, string EventType, string Message)> systemLogs = new(new[]
    {
        (DateTime.Now.AddMinutes(-30), "INFO", "System started"),
        (DateTime.Now.AddMinutes(-25), "WARN", "High memory usage"),
        (DateTime.Now.AddMinutes(-20), "ERROR", "Unhandled exception"),
        (DateTime.Now.AddMinutes(-15), "INFO", "Heartbeat OK"),
        (DateTime.Now.AddMinutes(-10), "ERROR", "Database timeout"),
        (DateTime.Now.AddMinutes(-5), "INFO", "User login success")
    });
    private HashSet<int> flaggedCustomerIds = new() { 1, 3, 5 };
    private Dictionary<int, List<int>> productAdjacency = new()
    {
        [201] = new List<int> { 202, 205 }, // Laptop → Phone, Monitor
        [202] = new List<int> { 201 },      // Phone → Laptop
        [203] = new List<int> { 204 },      // Desk → Chair
        [204] = new List<int> { 203 },      // Chair → Desk
        [205] = new List<int> { 201 }       // Monitor → Laptop
    };

    public LinqChallenge_36()
    {
        //HighestSpenders_T1();
        //HighValueOrders_T2();
        //CategoryPerformance_T3();
        //WarehouseRotation_T4();
        FraudDetection_T5();
        //PaginatedOrderHistory_T6();
        //DeferredExecutionTrap_T7();
        //DynamicCustomerSearch_T8();

        //ProductRecommendationGraph_T9();
        //LogStreamAnalysis_T10();
        //EfficientPagingBenchmark_T11();
        //SetTheoryChallenge_T12();
        //LeftJoinWithDefault_T13();
        //CrossJoinMatrix_T14();
        //NestedProjectionChallenge_T15();
    }

    // 🔹 Task 1: Highest Spenders
    // Brief: Group orders by customer → sum total spend → order descending
    // Output: CustomerId, Name, OrderCount, TotalSpent, AvgSpentPerOrder
    // Pagination: ❌ Not needed
    // Expected Time: 10–12 min
    // 10:23 - 10:37
    private void HighestSpenders_T1()
    {
        var result = (from customer in customers
                      join
                      order in orders on customer.CustomerId equals order.CustomerId
                      group new { order, customer } by customer.CustomerId into customerGroup
                      let name = customerGroup.Select(x => x.customer).FirstOrDefault().Name ?? "Unknown"
                      let orderCount = customerGroup.Count()
                      let totalSpent = (double)customerGroup.Sum(x => x.order.Amount)
                      let avgSpentPerOrder = orderCount > 0 ? Math.Round((totalSpent / orderCount), 2) : 0.0

                      orderby totalSpent descending, avgSpentPerOrder descending, orderCount descending, name

                      select new
                      {
                          CustomerId = customerGroup.Key,
                          Name = name,
                          OrderCount = orderCount,
                          TotalSpent = totalSpent,
                          AvgSpentPerOrder = avgSpentPerOrder
                      });

        var perPage = 2;
        var pageCount = Math.Ceiling((float)result.Count() / perPage);
        for (var pageNum = 0; ;)
        {
            var pagedResult = result.Skip(pageNum * perPage).Take(perPage);
            if (!pagedResult.Any())
                return;
            Console.WriteLine($"\nPage: {++pageNum}");

            foreach (var item in pagedResult)
            {
                Console.WriteLine($"ID: {item.CustomerId}\tName: {item.Name}\tOrders: {item.OrderCount}\tTotal Spent: ${item.TotalSpent}\tAvg Per Order: ${item.AvgSpentPerOrder}");
            }
        }
    }

    // 🔹 Task 2: Orders Above $100
    // Brief: Join orders + items + products → filter orders with total > $100
    // Output: OrderId, CustomerName, TotalOrderValue, ItemCount, ProductCount
    // Pagination: ❌ Not needed
    // Expected Time: 15–18 min
    // 7:17 - 7:34
    private void HighValueOrders_T2()
    {
        var query = from order in orders
                    join orderItem in orderItems on order.OrderId equals orderItem.OrderId
                    join product in products on orderItem.ProductId equals product.ProductId
                    group new { order, orderItem, product } by order.OrderId into orderGroup

                    let orderInfo = orderGroup.Select(x => x.order).First()
                    let customerName = customers.FirstOrDefault(x => x.CustomerId == orderInfo.CustomerId).Name
                    let totalValue = orderGroup.Sum(x => x.product.Price * x.orderItem.Quantity)
                    let productsCount = orderGroup.Select(x => x.product.ProductId).Distinct().Count()
                    let itemCount = orderGroup.Sum(x => x.orderItem.Quantity)
                    let orderLinesCount = orderGroup.Select(x => x.orderItem).Distinct().Count()

                    where totalValue > 100

                    orderby totalValue descending, itemCount descending, customerName                  

                    select new 
                    {
                        orderInfo.OrderId,
                        CustomerName = customerName,
                        TotalValue = Math.Round(totalValue, 2),
                        ItemCount = itemCount,
                        ProductsCount = productsCount,
                        OrderLinesCount = orderLinesCount
                    };

        var perPage = 2;
        var pageNum = 0;
        var pageCount = Math.Ceiling((float)query.Count() / perPage);
        while(pageNum < pageCount)
        {
            var pagedResult = query.Skip(pageNum * perPage).Take(perPage);
            if (!pagedResult.Any())
                return;
            Console.WriteLine($"\nPage: {++pageNum}");
            foreach (var item in pagedResult)
            {
                Console.WriteLine($"Order Id: {item.OrderId}\tCustomer: {item.CustomerName}\tTotal Order Value: ${item.TotalValue}\tItem Count: {item.ItemCount}" +
                    $"\tProduct Count: {item.ProductsCount}\tOrder Lines Count: {item.OrderLinesCount}");
            }
        }
    }

    // 🔹 Task 3: Category Performance
    // Brief: Group by category → avg price, total quantity sold
    // Output: Category, AvgPrice, TotalQuantity, TotalEarning, OrderCount
    // Pagination: ❌ Not needed
    // Expected Time: 12–15 min
    // 6:51 - 7:06
    private void CategoryPerformance_T3()
    {
        //var query = from product in products
        //            join orderItems in orderItems on product.ProductId equals orderItems.ProductId
        //            group new { product, orderItems } by product.Category into categoryGroup

        //            let category = categoryGroup.Key
        //            let avgPrice = Math.Round(categoryGroup.Select(x => x.product).Average(x => x.Price), 2)
        //            let totalQuantity = categoryGroup.Select(x => x.orderItems).Sum(x => x.Quantity)
        //            let totalEarned = Math.Round(categoryGroup.Sum(x => x.product.Price * x.orderItems.Quantity), 2)
        //            let orderCount = categoryGroup.Select(x => x.orderItems.OrderId).Distinct().Count()

        //            orderby totalEarned descending, totalQuantity descending, orderCount descending, catetory

        //            select new
        //            {
        //                Category = category,
        //                AvgPrice = avgPrice,
        //                TotalQuantity = totalQuantity, 
        //                TotalEarned = totalEarned, 
        //                OrderCount = orderCount
        //            };

        /*
         Category: Electronics   Avg Price: $985.71      Total Quantity: 12      Total Earned: $10400    Order count: 4
Category: Furniture     Avg Price: $225 Total Quantity: 2       Total Earned: $450      Order count: 2
         */

        var query = products
                           .Join(orderItems,
                               product => product.ProductId,
                               orderItem => orderItem.ProductId,
                               (product, orderItem) => new { product, orderItem })
                           .GroupBy(x => x.product.Category)
                           .Select(g => new
                           {
                               Category = g.Key,
                               AvgPrice = Math.Round(g.Average(x => x.product.Price), 2),
                               TotalQuantity = g.Sum(x => x.orderItem.Quantity),
                               TotalEarned = Math.Round(g.Sum(x => x.product.Price * x.orderItem.Quantity), 2),
                               OrderCount = g.Select(x => x.orderItem.OrderId).Distinct().Count()
                           })
                           .OrderByDescending(x => x.TotalEarned)
                           .ThenByDescending(x => x.TotalQuantity)
                           .ThenByDescending(x => x.OrderCount)
                           .ThenBy(x => x.Category);

        var perPage = 2;
        var pageCount = Math.Ceiling((float)query.Count() / perPage);
        var pagedShowed = 0;

        while(pagedShowed < pageCount)
        {
            var pagedResult = query.Skip(pagedShowed * perPage).Take(perPage);
            if (!pagedResult.Any())
                return;

            Console.WriteLine($"Page [{++pagedShowed}]");
            foreach (var item in pagedResult)
            {
                Console.WriteLine($"Category: {item.Category}\tAvg Price: ${item.AvgPrice}\tTotal Quantity: {item.TotalQuantity}\tTotal Earned: ${item.TotalEarned}\tOrder count: {item.OrderCount}");
            }
        }
    }

    // 🔹 Task 4: Warehouse Rotation Rate
    // Brief: Join shipments + products → group by warehouse → show rotation rate
    // Output: WarehouseId, Location, TotalQuantity, DistinctProducts, RotationRate
    // Pagination: ❌ Not needed
    // Expected Time: 15–18 min
    // 6:44 - 7:01
    /*
        Warehouse ID: 1 Location: Sydney        Total Quantity: 37      Rotational Rate: 12.33  Rotational Label: High
        Warehouse ID: 2 Location: Melbourne     Total Quantity: 13      Rotational Rate: 6.5    Rotational Label: Moderate
     */
    private void WarehouseRotation_T4()
    {
        var query = from shipment in shipments
                    join product in products on shipment.ProductId equals product.ProductId
                    join warehouse in warehouses on shipment.WarehouseId equals warehouse.WarehouseId
                    group new { shipment, product } by warehouse into warehouseGroup

                    let warehouseId = warehouseGroup.Key.WarehouseId
                    let location = warehouseGroup.Key.Location
                    let totalQty = warehouseGroup.Sum(x => x.shipment.Quantity)
                    let productCount = warehouseGroup.Select(x => x.product).Distinct().Count()
                    let rotationalRate = productCount > 0 ? Math.Round((float)totalQty / productCount, 2) : 0.0
                    let rotationalLabel = rotationalRate switch
                    {
                        >= 10 => "High",
                        >= 5 => "Moderate",
                        _ => "Low"
                    }

                    orderby rotationalRate descending, location
                    select new
                    {
                        WarehouseId = warehouseId,
                        Location = location,
                        TotalQuantity = totalQty,
                        RotationalRate = rotationalRate,
                        RotationalLabel = rotationalLabel
                    };

        // same as above using method syntax
        var query2 = shipments.Join(products,
                        shipment => shipment.ProductId,
                        product => product.ProductId,
                        (shipment, product) => new { shipment, product }).Join(warehouses,
                                                                                shipProduct => shipProduct.shipment.WarehouseId,
                                                                                warehouse => warehouse.WarehouseId,
                                                                                (shipProduct, warehouse) => new { shipProduct.product, shipProduct.shipment, warehouse })
                        .GroupBy(x => x.warehouse)
                        .Select(group =>
                        {
                            var warehouseId = group.Key.WarehouseId;
                            var location = group.Key.Location;
                            var totalQty = group.Sum(x => x.shipment.Quantity);
                            var productCount = group.Select(x => x.product).Distinct().Count();
                            var rotationRate = productCount > 0 ? Math.Round((float)totalQty / productCount, 2) : 0.0;
                            var rotationalLabel = rotationRate switch
                            {
                                >= 10 => "High",
                                >= 5 => "Moderate",
                                _ => "Low"
                            };
                            return new
                            {
                                WarehouseId = warehouseId,
                                Location = location,
                                TotalQuantity = totalQty,
                                ProductCount = productCount,
                                RotationalRate = rotationRate,
                                RotationalLabel = rotationalLabel
                            };
                        });



        foreach (var item in query2)
        {
            Console.WriteLine($"Warehouse ID: {item.WarehouseId}\tLocation: {item.Location}\tTotal Quantity: {item.TotalQuantity}\tRotational Rate: {item.RotationalRate}\tRotational Label: {item.RotationalLabel}");
        }
    }

    // 🔹 Task 5: Fraud Detection
    // Brief: Inner join orders with flaggedCustomerIds → show suspicious activity
    // Output: CustomerId, Name, OrderCount, Min/Max Dates, Min/Max/Avg Amount
    // Pagination: ❌ Not needed
    // Expected Time: 10–12 min
    // 6:47 - 7:01
    private void FraudDetection_T5()
    {
        var query = from customer in customers 
                    join order in orders on customer.CustomerId equals order.CustomerId

                    where flaggedCustomerIds.Contains(customer.CustomerId)

                    group order by customer into orderGroup
                    
                    let customerId = orderGroup.Key.CustomerId
                    let name = orderGroup.Key.Name
                    let orderCount = orderGroup.Count()

                    let orderDates = orderGroup.Select(x => x.OrderDate)
                    let minDate = orderDates.Min().ToShortDateString()
                    let maxDate = orderDates.Max().ToShortDateString()

                    let minAmount = Math.Round(orderGroup.Min(x => x.Amount), 2)
                    let maxAmount = Math.Round(orderGroup.Max(x => x.Amount), 2)
                    let avgAmount = Math.Round(orderGroup.Average(x => x.Amount), 2)

                    orderby orderCount descending, avgAmount descending, name

                    select new
                    {
                        CustomerId = customerId, 
                        Name = name,
                        OrderCount = orderCount, 
                        Mindate = minDate,
                        MaxDate = maxDate,
                        MinAmount = minAmount, 
                        MaxAmount = maxAmount, 
                        AvgAmount = avgAmount,
                    };

        foreach (var item in query)
        {
            Console.WriteLine($"ID: {item.CustomerId}\tName: {item.Name}\tOrderCount: {item.OrderCount}\tMindate: {item.Mindate}\tMaxDate: {item.MaxDate}" +
                $"\t\tMinAmount: ${item.MinAmount}\t\tMaxAmount: ${item.MaxAmount}\t\tAvgAmount: ${item.AvgAmount}");
        }
    }

    // 🔹 Task 6: Paginated Order History
    // Brief: Show orders per customer → paginate 5 per page
    // Output: CustomerId, Name, OrderCount, Min/Max Dates, Min/Max/Sum Amount
    // Pagination: ✅ Yes
    // Expected Time: 12–15 min
    // 8:28 - 8:45
    private void PaginatedOrderHistory_T6()
    {

    }

    // 🔹 Task 7: Deferred Execution Trap
    // Brief: Show how modifying source after query affects results
    // Output: Demonstration of mutation visibility in deferred vs immediate execution
    // Pagination: ❌ Not applicable
    // Expected Time: 8–10 min
    // 3:29 - 3:39
    private void DeferredExecutionTrap_T7()
    {

    }

    // 🔹 Task 8: Dynamic Customer Search
    // Brief: Build query dynamically using Expression<Func<T, bool>>
    // Output: CustomerId, Name, Region (filtered dynamically)
    // Pagination: ❌ Optional
    // Expected Time: 15–18 min
    // 9:50 - 10:00
    private void DynamicCustomerSearch_T8()
    {

    }



    // 🔹 Task 9: Product Recommendation Graph
    // Brief: Traverse productAdjacency graph → show related products
    // Output: ProductId, Name, RecommendedProductIds, RecommendedNames
    // Pagination: ❌ Not needed
    // Expected Time: 18–22 min
    // 9:35 AM - 9:52 AM
    private void ProductRecommendationGraph_T9()
    {

    }

    // 🔹 Task 10: Log Stream Analysis
    // Brief: Use Queue to analyze recent system events → group by type
    // Output: EventType, Count, Timestamp Range, Messages
    // Pagination: ❌ Optional (limit per type)
    // Expected Time: 10–12 min
    // 8:55 - 9:10
    private void LogStreamAnalysis_T10()
    {

    }



    // 🔹 Task 11: Efficient Paging Benchmark
    // Brief: Compare Skip/Take vs materialized paging for large datasets
    // Output: PageNum, ItemsPerPage, QueryTime, ResultCount
    // Pagination: ✅ Yes
    // Expected Time: 15–20 min
    // 9:05
    // Small, static data --> Materialized paging (ToList() then Skip/Take)
    // Large, dynamic data --> Deferred execution (IQueryable.Skip/Take)

    private void EfficientPagingBenchmark_T11()
    {
    }

    // 🔹 Task 12: Set Theory Challenge
    // Brief: Use Union, Intersect, Except to compare customer sets
    // Output: CustomerId, Name, MembershipStatus
    // Pagination: ❌ Not needed
    // Expected Time: 10–12 
    private void SetTheoryChallenge_T12()
    {

    }


    // 🔹 Task 13: Left Join with DefaultIfEmpty()
    // Brief: Show customers with or without orders using left join
    // Output: CustomerId, Name, OrderCount, LastOrderDate
    // Pagination: ❌ Not needed
    // Expected Time: 12–15 min
    private void LeftJoinWithDefault_T13() { }

    // 🔹 Task 14: Cross Join Matrix
    // Brief: Generate all combinations of customers and products
    // Output: CustomerId, ProductId, CustomerName, ProductName
    // Pagination: ❌ Optional
    // Expected Time: 10–12 min
    private void CrossJoinMatrix_T14() { }

    // 🔹 Task 15: Nested Projection Challenge
    // Brief: Group orders by customer → project nested order summaries
    // Output: CustomerId, Name, Orders: [OrderId, Date, Amount]
    // Pagination: ❌ Optional
    // Expected Time: 15–18 min
    // 5:05 - 5:25 
    private void NestedProjectionChallenge_T15()
    {

    }
}
