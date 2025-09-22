using System.Collections.Generic;
using System.Linq.Expressions;
using static System.Console;

namespace LINQ_Challenges;

public class LinqChallengeArchitectSet
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

    public LinqChallengeArchitectSet()
    {
        //HighestSpenders_T1();
        //HighValueOrders_T2();
        //CategoryPerformance_T3();
        //WarehouseRotation_T4();
        //FraudDetection_T5();
        //PaginatedOrderHistory_T6();
        //DeferredExecutionTrap_T7();
        //DynamicCustomerSearch_T8();

        //ProductRecommendationGraph_T9();
        //LogStreamAnalysis_T10();
        EfficientPagingBenchmark_T11();
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
    // 9:32 - 9:46
    private void HighestSpenders_T1()
    {
        var highSpenders = (from order in orders
                            join cust in customers on order.CustomerId equals cust.CustomerId
                            group new { order } by cust into custGroup


                            let customer = custGroup.Key
                            let orders = custGroup.Select(x => x.order)
                            let orderCount = custGroup.Count()
                            let totalSpent = orders.Sum(x => x.Amount)
                            let minOrder = orders.Min(x => x.Amount)
                            let maxOrder = orders.Max(x => x.Amount)
                            let avgSpentPerOrder = orders.Average(x => x.Amount)

                            orderby totalSpent descending, customer.Name

                            select new
                            {
                                CustomerId = customer.CustomerId,
                                CustomerName = customer.Name,
                                IsPremium = customer.IsPremium,
                                IsPremiumStr = customer.IsPremium ? "Yes" : "No",
                                OrderCount = orderCount,
                                TotalSpent = Math.Round(totalSpent, 2),
                                AvgSpentPerOrder = Math.Round(avgSpentPerOrder, 2),
                                MinOrder = Math.Round(minOrder, 2),
                                MaxOrder = Math.Round(maxOrder, 2)
                            }).ToList();

        foreach (var item in highSpenders)
        {
            WriteLine($"\nID: {item.CustomerId}\t\tCustomer: {item.CustomerName}\t\tPremium customer: {item.IsPremiumStr}");
            WriteLine($"\tOrder count: {item.OrderCount}");
            WriteLine($"\tTotal Spent: ${item.TotalSpent}");
            WriteLine($"\tAvg Per Order: ${item.AvgSpentPerOrder}");
            WriteLine($"\tMin Order: ${item.MinOrder}");
            WriteLine($"\tMax Order: ${item.MaxOrder}");
        }
    }

    // 🔹 Task 2: Orders Above $5000
    // Brief: Join orders + items + products → filter orders with total > $5000
    // Output: OrderId, CustomerName, TotalOrderValue, ItemCount, ProductCount
    // Pagination: ❌ Not needed
    // Expected Time: 15–18 min
    // 9:28 - 9:46
    private void HighValueOrders_T2()
    {
        var orderDetailsList = (from order in orders
                                join cust in customers on order.CustomerId equals cust.CustomerId
                                join orderItem in orderItems on order.OrderId equals orderItem.OrderId
                                join prod in products on orderItem.ProductId equals prod.ProductId

                                group new { orderItem, prod } by new { order, cust } into orderGroup

                                let customer = orderGroup.Key.cust
                                let order = orderGroup.Key.order
                                let totalValue = orderGroup.Sum(x => x.prod.Price * x.orderItem.Quantity)
                                let itemCount = orderGroup.Select(x => x.orderItem).Count()
                                let productCount = orderGroup.Select(x => x.orderItem.ProductId).Distinct().Count()

                                // totalValue > 5000

                                orderby totalValue descending, itemCount descending, productCount descending, customer.Name

                                select new
                                {
                                    Order = order,
                                    Customer = customer,
                                    ItemCount = itemCount,
                                    ProductCount = productCount,
                                    TotalValue = Math.Round(totalValue, 2),
                                }).ToList();

        foreach (var order in orderDetailsList)
        {
            WriteLine($"\nOrder ID: {order.Order.OrderId}\t\tOrder Date: {order.Order.OrderDate.ToShortTimeString()}\t\tTotal Value: ${order.TotalValue}");
            WriteLine($"\tCustomer ID: {order.Order.CustomerId}\t\tName: {order.Customer.Name}");
            WriteLine($"\tItem count: {order.ItemCount}\t\tProduct count: {order.ProductCount}");
        }
    }

    // 🔹 Task 3: Category Performance
    // Brief: Group by category → avg price, total quantity sold
    // Output: Category, AvgPrice, TotalQuantity, TotalEarning, OrderCount
    // Pagination: ❌ Not needed
    // Expected Time: 12–15 min
    // 4:08 - 4:22
    private void CategoryPerformance_T3()
    {
        var query = (from orderItem in orderItems
                     join prod in products on orderItem.ProductId equals prod.ProductId

                     group new { orderItem, prod } by prod.Category into categoryGroup

                     let category = categoryGroup.Key
                     let avgPrice = categoryGroup.Average(x => x.prod.Price)
                     let totalQtySold = categoryGroup.Sum(x => x.orderItem.Quantity)
                     let totalEarning = categoryGroup.Sum(x => x.orderItem.Quantity * x.prod.Price)
                     let orderCount = categoryGroup.Select(x => x.orderItem.OrderId).Distinct().Count()
                     let avgPerOrder = totalEarning / orderCount
                     let avgPricePerItem = totalEarning / totalQtySold
                     let productsCount = categoryGroup.Select(x => x.prod.ProductId).Distinct().Count()

                     let tier = totalEarning switch
                     {
                         > 8000 => "High",
                         >= 4000 => "Medium",
                         _ => "Low"
                     }

                     orderby totalEarning descending, orderCount descending, totalQtySold descending, category

                     select new
                     {
                         Category = category,
                         Tier = tier,
                         OrderCount = orderCount,
                         ProductCount = productsCount,
                         TotalQtySold = totalQtySold,
                         AvgPrice = Math.Round(avgPrice, 2),
                         TotalEarning = Math.Round(totalEarning, 2),
                         AvgPerOrder = Math.Round(avgPerOrder, 2),
                         AvgPricePerItem = Math.Round(avgPricePerItem, 2)
                     }).ToList();

        foreach (var item in query)
        {
            WriteLine($"\nCategory: {item.Category}\t\tTier: {item.Tier}");
            WriteLine($"\tOrders: {item.OrderCount}\t\tTotal Quantity Sold: {item.TotalQtySold}\t\tProduct cound: {item.ProductCount}");
            WriteLine($"\tTotal Earning: ${item.TotalEarning}\t\tAvg Price: ${item.AvgPrice}\t\tAvg Per Order: ${item.AvgPerOrder}\t\tAvg Per Item: ${item.AvgPricePerItem}");
        }
    }

    // 🔹 Task 4: Warehouse Rotation Rate
    // Brief: Join shipments + products → group by warehouse → show rotation rate
    // Output: WarehouseId, Location, TotalQuantity, DistinctProducts, RotationRate
    // Pagination: ❌ Not needed
    // Expected Time: 15–18 min
    // 8:44 - 8:58
    private void WarehouseRotation_T4()
    {
        var list = (from shipment in shipments
                    join warehouse in warehouses on shipment.WarehouseId equals warehouse.WarehouseId

                    group shipment by warehouse into warehouseGroup

                    let warehouseId = warehouseGroup.Key.WarehouseId
                    let shipmentCount = warehouseGroup.Count()
                    let location = warehouseGroup.Key.Location
                    let totalQty = warehouseGroup.Sum(x => x.Quantity)
                    let distinctProductsCount = warehouseGroup.Select(x => x.ProductId).Distinct().Count()
                    let rotationRate = distinctProductsCount > 0 ? (float)totalQty / distinctProductsCount : 0.0

                    let rotationRateTier = rotationRate switch
                    {
                        > 10 => "High Rotation",
                        >= 5 => "Moderate",
                        _ => "Low movement"
                    }

                    orderby rotationRate descending, distinctProductsCount descending, totalQty descending, location

                    select new
                    {
                        WarehouseId = warehouseId,
                        Location = location,
                        ShipmentCount = shipmentCount,
                        TotalQty = totalQty,
                        DistinctProductsCount = distinctProductsCount,
                        RotationRate = Math.Round(rotationRate, 2),
                        RotationRateTier = rotationRateTier
                    }).ToList();

        foreach (var item in list)
        {
            WriteLine($"\nWarehouse {item.WarehouseId}\t\tat: {item.Location}\t\tRotation Tier: {item.RotationRateTier}");
            WriteLine($"\tShipments:{item.ShipmentCount}\t\tTotal Qty: {item.TotalQty}\t\tDistinct Products Count: {item.DistinctProductsCount}\t\tRotation Rate: {item.RotationRate}");
        }
    }

    // 🔹 Task 5: Fraud Detection
    // Brief: Inner join orders with flaggedCustomerIds → show suspicious activity
    // Output: CustomerId, Name, OrderCount, Min/Max Dates, Min/Max/Avg Amount
    // Pagination: ❌ Not needed
    // Expected Time: 10–12 min
    // 1:00 - 1:14
    private void FraudDetection_T5()
    {
        var flaggedCustList = (from order in orders
                               join fc in flaggedCustomerIds on order.CustomerId equals fc
                               join customer in customers on fc equals customer.CustomerId

                               group new { order } by customer into fcGroup


                               let customer = fcGroup.Key
                               let orderCount = fcGroup.Count()
                               let orders = fcGroup.Select(x => x.order)
                               let minDate = orders.Min(x => x.OrderDate)
                               let maxDate = orders.Max(x => x.OrderDate)

                               let minAmount = orders.Min(x => x.Amount)
                               let maxAmount = orders.Max(x => x.Amount)
                               let avgAmount = orders.Average(x => x.Amount)
                               let totalAmount = orders.Sum(x => x.Amount)

                               orderby orderCount descending, maxDate descending, maxAmount descending, avgAmount descending, customer.Name

                               select new
                               {
                                   Customer = customer,
                                   OrderCount = orderCount,
                                   IsPremiumStr = customer.IsPremium ? "Yes" : "No",
                                   MinDate = minDate,
                                   MaxDate = maxDate,
                                   MinAmount = Math.Round(minAmount, 2),
                                   MaxAmount = Math.Round(maxAmount, 2),
                                   AvgAmount = Math.Round(avgAmount, 2),
                                   TotalAmount = Math.Round(totalAmount, 2),
                               }).ToList();

        foreach (var item in flaggedCustList)
        {
            WriteLine($"\nCustomer ID: {item.Customer.CustomerId}\t\tName: {item.Customer.Name}\t\tRegion: {item.Customer.Region}\t\tIs Premium: {item.IsPremiumStr}");
            WriteLine($"Order count: {item.OrderCount}");
            WriteLine($"Min Date: {item.MinDate.ToShortDateString()}\t\tMax Date: {item.MaxDate.ToShortDateString()}");
            WriteLine($"Min Amount: ${item.MinAmount}\t\tMax Amount: ${item.MaxAmount}\t\tAvg Amount: ${item.AvgAmount}\t\tTotal Amount: ${item.TotalAmount}");
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
        var customerOrders = (from ord in orders
                              join cust in customers on ord.CustomerId equals cust.CustomerId
                              group ord by cust into orderGroup

                              let customer = orderGroup.Key
                              let orderCount = orderGroup.Count()
                              let minOrderDate = orderGroup.Min(x => x.OrderDate)
                              let maxOrderDate = orderGroup.Max(x => x.OrderDate)
                              let minAmount = orderGroup.Min(x => x.Amount)
                              let maxAmount = orderGroup.Max(x => x.Amount)
                              let avgAmount = orderGroup.Average(x => x.Amount)

                              let isPremiumStr = customer.IsPremium ? "Premimum Customer" : "Standard Customer"
                              let custTier = avgAmount switch
                              {
                                  >= 1500 => "Very High Profit",
                                  >= 1000 => "Profitable",
                                  _ => "Standard"
                              }

                              orderby orderCount descending, maxAmount descending, avgAmount descending, maxOrderDate descending, customer.Name

                              select new
                              {
                                  Customer = customer,
                                  CustTier = custTier,
                                  IsPremiumStr = isPremiumStr,
                                  OrderCount = orderCount,
                                  MinOrderDate = minOrderDate,
                                  MaxOrderDate = maxOrderDate,
                                  MinAmount = Math.Round(minAmount, 2),
                                  MaxAmount = Math.Round(maxAmount, 2),
                                  AvgAmount = Math.Round(avgAmount, 2),
                              });

        var perPage = 2;
        var totalPages = Math.Ceiling(((float)customerOrders.Count()) / perPage);
        for (var pageNum = 0; ;)
        {
            var pagedResult = customerOrders.Skip(pageNum * perPage).Take(perPage);
            if (!pagedResult.Any())
                break;
            WriteLine($"\n\nPage {++pageNum} of {totalPages}");
            foreach (var item in pagedResult)
            {
                WriteLine($"\nCustomer ID: {item.Customer.CustomerId}\t\t{item.Customer.Name}\t\t{item.Customer.Region}\t\t{item.IsPremiumStr}");
                WriteLine($"\tOrder Count: {item.OrderCount}\t\tCustomer Tier: {item.CustTier}");
                WriteLine($"\tMin Order Date: {item.MinOrderDate.ToShortDateString()}\t\tMax Order Date: {item.MaxOrderDate.ToShortDateString()}");
                WriteLine($"\tMin Amount: ${item.MinAmount}\t\tMax Amount: ${item.MaxAmount}\t\tAvg Amount: ${item.AvgAmount}");
            }
        }
    }

    // 🔹 Task 7: Deferred Execution Trap
    // Brief: Show how modifying source after query affects results
    // Output: Demonstration of mutation visibility in deferred vs immediate execution
    // Pagination: ❌ Not applicable
    // Expected Time: 8–10 min
    // 3:29 - 3:39
    private void DeferredExecutionTrap_T7()
    {
        //var firstCustomerQuery = (from c in customers where c.CustomerId == 1 select c).AsQueryable();
        ////  (1, "Alice", "NSW", true),

        //var firstCustomer = customers.SingleOrDefault(x => x.CustomerId == 1);
        //firstCustomer.IsPremium = false;
        //firstCustomer.Name = "Alice ++";
        //firstCustomer.Region = "nsw++";

        //var firstCustomerFromQuery = firstCustomerQuery.ToList().First();
        //WriteLine($"First Customer : {firstCustomerFromQuery.Name}, {firstCustomerFromQuery.Region}, {firstCustomerFromQuery.IsPremium}");

        var nums = new List<int> { 1, 2, 3, 4 };
        IEnumerable<int> evenNums = from num in nums
                                    where num % 2 == 0
                                    select num;
        WriteLine("Even numbers exepcted are 2 and 4, but linq query is still not executed\n");

        nums.Remove(2);
        nums.Add(6);
        WriteLine("source is modified to remove 2 and have 6, now deferred executing linq query\nand results are\n");

        evenNums = evenNums.ToList();               // execute the linq query

        foreach (var item in evenNums)
        {
            WriteLine(item);
        }
    }

    // 🔹 Task 8: Dynamic Customer Search
    // Brief: Build query dynamically using Expression<Func<T, bool>>
    // Output: CustomerId, Name, Region (filtered dynamically)
    // Pagination: ❌ Optional
    // Expected Time: 15–18 min
    // 9:50 - 10:00
    private void DynamicCustomerSearch_T8()
    {
        Expression<Func<CustomerFilter35, bool>> nswPremiumCustomer = x => x.IsPremium && x.Region.Equals("nsw", StringComparison.OrdinalIgnoreCase);
        foreach (var item in Filter(nswPremiumCustomer))
        {
            WriteLine($"{item.CustomerId}\t{item.Name}\t{item.IsPremium}\t{item.Region}");
        }
    }

    class CustomerFilter35
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public bool IsPremium { get; set; }
    }

    private IEnumerable<CustomerFilter35> Filter(Expression<Func<CustomerFilter35, bool>> expression)
    {
        var list = (from cust in customers
                    select new CustomerFilter35
                    {
                        CustomerId = cust.CustomerId,
                        IsPremium = cust.IsPremium,
                        Region = cust.Region,
                        Name = cust.Name,
                    });
        return list.Where(expression.Compile());
    }

    // 🔹 Task 9: Product Recommendation Graph
    // Brief: Traverse productAdjacency graph → show related products
    // Output: ProductId, Name, RecommendedProductIds, RecommendedNames
    // Pagination: ❌ Not needed
    // Expected Time: 18–22 min
    // 9:35 AM - 9:52 AM
    private void ProductRecommendationGraph_T9()
    {
        var productsDictionary = new Dictionary<int, (int ProductId, string Name, string Category, decimal Price)>();
        products.ForEach(x =>
        {
            productsDictionary.Add(x.ProductId, x);
        });

        var productAdjDetails = from item in productAdjacency
                                let product = productsDictionary.GetValueOrDefault(item.Key)
                                let alsoBaughtList = (from id in item.Value orderby id select productsDictionary.GetValueOrDefault(id)).Where(x => !string.IsNullOrWhiteSpace(x.Name)).OrderByDescending(x => x.Price).ToList()
                                orderby product.ProductId, product.Name
                                select new
                                {
                                    Product = product,
                                    AlsoBaughtList = alsoBaughtList
                                };
        foreach (var item in productAdjDetails)
        {
            WriteLine($"\nProduct ID {item.Product.ProductId} \tName {item.Product.Name} \tcategory: " +
                $"{item.Product.Category}\t price ${Math.Round(item.Product.Price, 2)} Also baught: ");

            foreach (var alsoBaughtItem in item.AlsoBaughtList)
            {
                WriteLine($"\t\tProduct ID {alsoBaughtItem.ProductId} \t Name {alsoBaughtItem.Name} \t category: " +
                            $"{alsoBaughtItem.Category}\t price ${Math.Round(alsoBaughtItem.Price, 2)}");
            }
        }
    }

    // 🔹 Task 10: Log Stream Analysis
    // Brief: Use Queue to analyze recent system events → group by type
    // Output: EventType, Count, Timestamp Range, Messages
    // Pagination: ❌ Optional (limit per type)
    // Expected Time: 10–12 min
    // 8:55 - 9:10
    private void LogStreamAnalysis_T10()
    {
        var eventsByType = (from log in systemLogs
                            group log by log.EventType into logGroup


                            let count = logGroup.Count()
                            let logs = logGroup.OrderBy(x => x.Timestamp).ThenBy(x => x.Message)
                            let timeRange = $"{logs.Min(x => x.Timestamp)} -> {logs.Max(x => x.Timestamp)}"
                            let severityScore = logGroup.Key switch
                            {
                                "INFO" => 0,
                                "WARN" => 1,
                                "ERROR" => 2,
                                _ => 0
                            }

                            let label = severityScore switch
                            {
                                1 => "Check Soon",
                                2 => "Check Immediately",
                                _ => "Check at Leisure"
                            }

                            orderby severityScore descending

                            select new
                            {
                                EventType = logGroup.Key,
                                Count = count,
                                SeverityScore = severityScore,
                                Label = label,
                                TimeRange = timeRange,
                                Logs = logs
                            }).ToList();

        foreach (var item in eventsByType)
        {
            WriteLine($"\n{item.EventType}\tCount: {item.Count}\tSeverity Score:{item.SeverityScore}\tLabel: {item.Label}");
            WriteLine($"Time Range: {item.TimeRange}");
            foreach (var log in item.Logs)
            {
                WriteLine($"\t\t{log.Timestamp}\t\t{log.Message}");
            }
        }
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
    // Expected Time: 10–12 min
    private void SetTheoryChallenge_T12() { }

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
        var list = (from order in orders
                    join
                    cust in customers on order.CustomerId equals cust.CustomerId
                    group order by cust into custGroup

                    let customer = custGroup.Key
                    let ordersCount = custGroup.Count()
                    let orders = custGroup.OrderByDescending(x => x.Amount)
                    let maxOrderAmount = custGroup.Max(x => x.Amount)
                    let minOrderAmount = custGroup.Min(x => x.Amount)
                    let avgOrderAmount = custGroup.Average(x => x.Amount)
                    let minOrderDate = custGroup.Min(x => x.OrderDate)
                    let maxOrderDate = custGroup.Max(x => x.OrderDate)

                    orderby avgOrderAmount descending, ordersCount descending, customer.Name

                    select new
                    {
                        Customer = customer,
                        OrdersCount = ordersCount,
                        Orders = orders,
                        MaxOrderAmount = Math.Round(maxOrderAmount, 2),
                        MinOrderAmount = Math.Round(minOrderAmount, 2),
                        AvgOrderAmount = Math.Round(avgOrderAmount, 2),
                        MinOrderDate = minOrderDate.ToShortDateString(),
                        MaxOrderDate = maxOrderDate.ToShortDateString()
                    }).ToList();

        var pageSize = 2;
        var pages = (int)(Math.Ceiling((decimal)list.Count / pageSize));

        for (var pageNum = 0; ;)
        {
            var pageItems = list.Skip(pageNum * pageSize).Take(pageSize);
            if (!pageItems.Any()) break;
            WriteLine($"\n\nPage[{++pageNum}] of {pages}");

            foreach (var item in pageItems)
            {
                WriteLine($"\n{item.Customer.Name} with {item.OrdersCount} orders.");

                foreach (var order in item.Orders)
                {
                    WriteLine($"\t\t\tOrder ID: {order.OrderId}\tDate: {order.OrderDate.ToShortDateString()}\tOrder Amount: ${order.Amount}");
                }

                WriteLine($"\tAmount Max: ${item.MaxOrderAmount}\tMin $:{item.MinOrderAmount}\tAvg: $:{item.AvgOrderAmount}");
                WriteLine($"\tOrder Date Most Recent: {item.MaxOrderDate}\tOldest: {item.MinOrderDate}");
            }
        }
    }
}