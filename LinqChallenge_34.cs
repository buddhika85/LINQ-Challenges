using System.Linq.Expressions;
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
        //HighValueOrders_T2();
        //ProductCategoryPerformance_T3();
        //WarehouseStockRotation_T4();
        //FraudFlaggedCustomerOrders_T5();
        //PaginatedOrderHistory_T6();
        //DeferredExecutionTrap_T7();
        //DynamicCustomerSearch_T8();
        //GraphBasedProductRecommendations_T9();
        LogStreamAnalysis_T10();
    }


    // 🔹 Task 10: Log Stream Analysis
    // Use Queue to analyze recent system events → group by type
    // ⏱️ Expected: 10–12 min
    // 9:09 - 9:16
    private void LogStreamAnalysis_T10()
    {
        var eventsByType = (from log in systemLogs
                            group log by log.EventType into logGroup
                            let eventCount = logGroup.Count()
                            orderby eventCount descending, logGroup.Key
                            select new
                            {
                                EventType = logGroup.Key,
                                Events = logGroup,
                                EventCount = eventCount,
                            }).ToList();
        foreach (var item in eventsByType)
        {
            WriteLine($"\n{item.EventType} has {item.EventCount}");
            foreach (var evt in item.Events)
            {
                WriteLine($"\t\t{evt.Timestamp} - {evt.Message}");
            }
        }

        //Queue<(DateTime Timestamp, string EventType, string Message)> systemLogsQueue = new();
        //systemLogs.ToList().ForEach(x =>
        //{
        //    systemLogsQueue.Enqueue(x);
        //});s
    }

    // 🔹 Task 9: Graph-Based Product Recommendations
    // Traverse productAdjacency graph → show related products
    // This kind of output is perfect for powering a “Customers also viewed” or “You might also like” feature.
    // ⏱️ Expected: 18–22 min
    // 10:22 - 11:04
    private void GraphBasedProductRecommendations_T9()
    {
        var productsDictionary = products.ToDictionary(x => x.ProductId, x => x.Name);
        foreach (var item in productAdjacency)
        {
            var productFound = productsDictionary.TryGetValue(item.Key, out string? prodName);
            if (productFound && prodName != null)
            {
                WriteLine($"\n{prodName}");
                foreach (var linkedProductId in item.Value)
                {
                    if (productsDictionary.TryGetValue(linkedProductId, out string? linkedProdName) && linkedProdName != null)
                        WriteLine($"\tIs frequently baught with : {linkedProdName}");
                }
            }
        }
    }

    private void DisplayOrderLinesAndProducts()
    {
        // orderToOrderLineAndProduct
        var orderLinesWithProduct = (from orderLine in orderItems
                                     join order in orders on orderLine.OrderId equals order.OrderId
                                     join product in products on orderLine.ProductId equals product.ProductId

                                     group new { orderLine, product } by order.OrderId into orderGroup

                                     let orderLinesWithProducts = orderGroup
                                     let orderId = orderGroup.Key

                                     where orderGroup.Count() > 1

                                     select new
                                     {
                                         OrderId = orderId,
                                         OrderLinesWithProducts = orderLinesWithProducts
                                     }).ToList();

        foreach (var item in orderLinesWithProduct)
        {
            WriteLine($"\nOrder - {item.OrderId}");
            foreach (var orderLine in item.OrderLinesWithProducts)
            {
                WriteLine($"\t{orderLine.product.Name}");
            }
        }
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
    // 10:06 - 10:16
    private void ProductCategoryPerformance_T3()
    {
        var list = (from prod in products
                    join orderItem in orderItems on prod.ProductId equals orderItem.ProductId
                    group new { prod, orderItem } by prod.Category into cateGroup

                    let productsInCategory = cateGroup.Select(x => x.prod).Distinct()

                    let avgPrice = productsInCategory.Average(x => x.Price)
                    let prodCount = productsInCategory.Count()
                    let totalEarning = cateGroup.Sum(x => x.prod.Price * x.orderItem.Quantity)
                    let quantity = cateGroup.Sum(x => x.orderItem.Quantity)
                    let orderCount = cateGroup.Select(x => x.orderItem.OrderId).Distinct().Count()
                    let earningPerCategory = prodCount > 0 ? Math.Round(totalEarning / prodCount, 2) : 0.0m

                    let topSellingProductOfCategory = (from item in cateGroup
                                                       group item by item.prod into prodGroup
                                                       let qtySold = prodGroup.Sum(x => x.orderItem.Quantity)
                                                       orderby qtySold descending
                                                       select new
                                                       {
                                                           Product = prodGroup.Key,
                                                           QtySold = qtySold
                                                       }).ToList().FirstOrDefault()


                    orderby avgPrice descending, quantity descending, orderCount descending, cateGroup.Key

                    select new
                    {
                        Category = cateGroup.Key,
                        TopSellingProductOfCategory = topSellingProductOfCategory,
                        ProductCount = prodCount,
                        TotalEarning = Math.Round(totalEarning, 2),
                        AvgPrice = Math.Round(avgPrice, 2),
                        Quantity = quantity,
                        OrderCount = orderCount,
                        EarningPerCategory = earningPerCategory,
                    }).ToList();
        foreach (var item in list)
        {
            WriteLine($"\nCategory: {item.Category} Details");
            WriteLine($"\tProduct count: {item.ProductCount}");
            WriteLine($"\tTotal Earning: ${item.TotalEarning}");
            WriteLine($"\tAverage Price: ${item.AvgPrice}");
            WriteLine($"\tEarning per category: ${item.EarningPerCategory}");
            WriteLine($"\tQuantity Sold: {item.Quantity}");
            WriteLine($"\tOrder Count: {item.OrderCount}");
            WriteLine($"\tTop selling Product ID: {item.TopSellingProductOfCategory.Product.ProductId}\tName: {item.TopSellingProductOfCategory.Product.Name}");
            WriteLine($"\tTop selling Product Sold Quantity: {item.TopSellingProductOfCategory.QtySold}");
        }
    }

    // 🔹 Task 4: Warehouse Stock Rotation
    // Join shipments + products → group by warehouse → show rotation rate
    // ⏱️ Expected: 15–18 min
    // 9:23 - 9:42
    private void WarehouseStockRotation_T4()
    {
        var list = (from shipment in shipments
                    join prod in products on shipment.ProductId equals prod.ProductId
                    join warehouse in warehouses on shipment.WarehouseId equals warehouse.WarehouseId
                    group new { prod, shipment } by warehouse into warehouseGroup

                    let shipmentCount = warehouseGroup.Count()
                    let prodCount = warehouseGroup.Select(x => x.prod.ProductId).Distinct().Count()
                    let shipmentsPerProduct = (decimal)shipmentCount / prodCount
                    let totalQuantity = warehouseGroup.Sum(x => x.shipment.Quantity)
                    let totalValue = warehouseGroup.Sum(x => x.shipment.Quantity * x.prod.Price)
                    let rotationRate = (decimal)totalQuantity / prodCount
                    let rotationCategory = rotationRate switch
                    {
                        >= 14 => "High",
                        >= 7 => "Medium",
                        _ => "Low"
                    }

                    orderby rotationRate descending, shipmentCount descending, prodCount descending, warehouseGroup.Key.Location

                    select new
                    {
                        Warehouse = warehouseGroup.Key,
                        ShipmentCount = shipmentCount,
                        ProdCount = prodCount,
                        ShipmentsPerProduct = Math.Round(shipmentsPerProduct, 2),
                        TotalQuantity = totalQuantity,
                        TotalValue = Math.Round(totalValue, 2),
                        RotationRate = Math.Round(rotationRate, 2),
                        RotationCategory = rotationCategory

                    }).ToList();

        foreach (var item in list)
        {
            WriteLine($"\nWarehouse: {item.Warehouse.WarehouseId}\t\t {item.Warehouse.Location}");
            WriteLine($"\tRotation Category: {item.RotationCategory}");
            WriteLine($"\tRotation Rate: {item.RotationRate}");
            WriteLine($"\tShipments: {item.ShipmentCount}");
            WriteLine($"\tProdCount: {item.ProdCount}");
            WriteLine($"\tShipments Per Product: {item.ShipmentsPerProduct}");
            WriteLine($"\tTotal Quantity: {item.TotalQuantity}");
            WriteLine($"\tTotal Value: ${item.TotalValue}");
        }
    }

    // 🔹 Task 5: Fraud-Flagged Customer Orders
    // Inner join orders with flaggedCustomerIds → show suspicious activity
    // ⏱️ Expected: 10–12 min
    // 10:10 - 10:27
    private void FraudFlaggedCustomerOrders_T5()
    {
        var list = (from flagged in flaggedCustomerIds
                    join cust in customers on flagged equals cust.CustomerId
                    join order in orders on flagged equals order.CustomerId

                    group new { cust, order } by flagged into flaggedGroup

                    let customer = flaggedGroup.Select(x => x.cust).First()
                    let orderCout = flaggedGroup.Count()
                    let minOrderDate = flaggedGroup.Min(x => x.order.OrderDate)
                    let maxOrderDate = flaggedGroup.Max(x => x.order.OrderDate)
                    let minAmount = flaggedGroup.Min(x => x.order.Amount)
                    let maxAmount = flaggedGroup.Max(x => x.order.Amount)
                    let avgAmount = flaggedGroup.Average(x => x.order.Amount)

                    orderby orderCout descending, maxOrderDate descending, maxAmount descending, customer.Name

                    select new
                    {
                        FlaggedCustomerId = flaggedGroup.Key,
                        Customer = customer,
                        OrderCount = orderCout,
                        MinOrderDate = minOrderDate,
                        MaxOrderDate = maxOrderDate,
                        MinAmount = Math.Round(minAmount, 2),
                        MaxAmount = Math.Round(maxAmount, 2),
                        AvgAmount = Math.Round(avgAmount, 2),

                    }).ToList();

        foreach (var item in list)
        {
            WriteLine($"{item.FlaggedCustomerId}\t\tName: {item.Customer.Name}\t\tRegion: {item.Customer.Region}");
            WriteLine($"\t\tOrder count: {item.OrderCount}");
            WriteLine($"\t\tMin order date: {item.MinOrderDate.ToShortDateString()}");
            WriteLine($"\t\tMax order date: {item.MinOrderDate.ToShortDateString()}");
            WriteLine($"\t\tAvg order amount: ${item.AvgAmount}");
            WriteLine($"\t\tMin order amount: ${item.MinAmount}");
            WriteLine($"\t\tMax order amount: ${item.MaxAmount}");
        }
    }

    // 🔹 Task 6: Paginated Order History
    // Show orders per customer → paginate 2 per page
    // ⏱️ Expected: 12–15 min
    // 9:08 - 9:22
    private void PaginatedOrderHistory_T6()
    {
        var ordersByCustomer = (from cust in customers
                                join order in orders on cust.CustomerId equals order.CustomerId
                                group new { cust, order } by order.CustomerId into custGroup

                                let orders = custGroup.Select(x => x.order)
                                let customer = custGroup.Select(x => x.cust).FirstOrDefault()
                                let orderCount = orders.Count()
                                let minOrderDate = orders.Min(x => x.OrderDate)
                                let maxOrderDate = orders.Max(x => x.OrderDate)
                                let minAmount = orders.Min(x => x.Amount)
                                let maxAmount = orders.Max(x => x.Amount)

                                let sumOrderAmount = orders.Sum(x => x.Amount)
                                let avgOrderAmount = sumOrderAmount / orderCount

                                orderby orderCount descending, sumOrderAmount descending, maxOrderDate descending, customer.Name

                                select new
                                {
                                    Customer = customer,
                                    OrderCount = orderCount,
                                    MinOrderDate = minOrderDate,
                                    MaxOrderDate = maxOrderDate,
                                    MinAmount = Math.Round(minAmount, 2),
                                    MaxAmount = Math.Round(maxAmount, 2),
                                    AvgOrderAmount = Math.Round(avgOrderAmount, 2),
                                    SumOrderAmount = Math.Round(sumOrderAmount, 2)
                                }).ToList();
        var itemsPerPage = 2;
        var totalPages = Math.Ceiling((float)ordersByCustomer.Count() / itemsPerPage);
        for (var pageNum = 0; ;)
        {
            var pagedItems = ordersByCustomer.Skip(pageNum * itemsPerPage).Take(itemsPerPage);
            if (!pagedItems.Any())
                break;
            WriteLine($"\nPage [{++pageNum}] of {totalPages} Pages");
            foreach (var item in pagedItems)
            {
                WriteLine($"Customer {item.Customer.CustomerId}\t {item.Customer.Name}\t{item.Customer.Region}");
                WriteLine($"\tOrder Count :{item.OrderCount}");
                WriteLine($"\tMin Order Date :{item.MinOrderDate.ToShortDateString()}\tMax Order Date :{item.MaxOrderDate.ToShortDateString()}");
                WriteLine($"\tAvergare Order Amount: ${item.AvgOrderAmount}\tMin Amount :${item.MinAmount}\tMax Amount :${item.MaxAmount}\tSum Amount :${item.SumOrderAmount}");
            }
        }
    }

    // 🔹 Task 7: Deferred Execution Trap
    // Show how modifying source after query affects results
    // ⏱️ Expected: 8–10 min
    // 8:43 - 8:49
    private void DeferredExecutionTrap_T7()
    {
        //var firstCustomerQuery = (from cust in customers
        //                          where cust.CustomerId == 1
        //                          select cust);
        //var custOne = customers.First(x => x.CustomerId == 1);
        //custOne.Name = "Updated";

        //var customer = firstCustomerQuery.First();
        //WriteLine(object.ReferenceEquals(custOne, customer));
        //WriteLine($"{customer.CustomerId}\t\t{customer.Name}");

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
    // Build query dynamically using Expression<Func<T, bool>>
    // ⏱️ Expected: 15–18 min
    // 7:39 - 8:08

    private class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Region { get; set; }

        public Customer(int id, string name, string region)
        {
            CustomerId = id;
            Name = name;
            Region = region;
        }

        public override string ToString()
        {
            return $"{CustomerId} - {Name} - {Region}";
        }
    }

    private List<Customer> customerObjs = new()
    {
        new Customer(1, "Alice", "NSW"),
        new Customer(2, "Bob", "VIC"),
        new Customer(3, "Charlie", "QLD"),
        new Customer(4, "Diana", "NSW"),
        new Customer(5, "Ethan", "WA")
    };
    private void DynamicCustomerSearch_T8()
    {
        Expression<Func<Customer, bool>> customerFilter1 =
            x => x.Region != null && x.Region.Equals("WA", StringComparison.OrdinalIgnoreCase);

        Expression<Func<Customer, bool>> customerFilter2 =
            x => x.Name != null && (x.Name.StartsWith('A') || x.Name.StartsWith('E'));

        Expression<Func<Customer, bool>> combinedFilter =
            x => (customerFilter1.Compile())(x) && (customerFilter2.Compile())(x);

        var dynamicQueryList = new List<(string, Expression<Func<Customer, bool>>)>
        {
           ("Customers in WA", customerFilter1),
           ("Customers with names starting with A or E", customerFilter2),
           ("Customers in WA and with names starting with A or E", combinedFilter)
        };

        foreach (var query in dynamicQueryList)
        {
            var customers = SearchByFilter(query.Item2);
            WriteLine($"\n{query.Item1} - Found {customers.Count()} customers");
            foreach (var customer in customers)
            {
                WriteLine($"\t{customer}");
            }
        }
    }
    private IEnumerable<Customer> SearchByFilter(Expression<Func<Customer, bool>> filter)
    {
        var compiledFilter = filter.Compile(); // Converts Expression to Func
        return customerObjs.Where(compiledFilter);
    }


}
