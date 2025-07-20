using static System.Console;

namespace LINQ_Challeges;

public class LinqChallenge_31
{
    private List<(int WarehouseId, string Location)> warehouses = new()
    {
        (1, "Sydney"), (2, "Melbourne"), (3, "Brisbane")
    };

    private List<(int ProductId, string Name, string Category, decimal CostPrice)> products = new()
    {
        (101, "Laptop", "Electronics", 1400),
        (102, "Desk Chair", "Furniture", 220),
        (103, "Monitor", "Electronics", 400),
        (104, "Keyboard", "Electronics", 110),
        (105, "Desk Lamp", "Furniture", 70)
    };

    private List<(int StockId, int WarehouseId, int ProductId, int Quantity)> stock = new()
    {
        (201, 1, 101, 15), (202, 1, 102, 30), (203, 2, 103, 10),
        (204, 2, 104, 40), (205, 3, 105, 50), (206, 3, 101, 5)
    };

    private List<(int ShipmentId, int WarehouseId, int ProductId, DateTime DateDelivered, int Quantity, bool Backordered)> shipments = new()
    {
        (301, 1, 101, new(2024,1,12), 10, false),
        (302, 2, 103, new(2024,1,15), 20, true),
        (303, 1, 104, new(2024,2,1), 30, false),
        (304, 3, 105, new(2024,2,10), 25, false),
        (305, 1, 102, new(2024,2,18), 15, true),
        (306, 2, 104, new(2024,3,2), 10, false),
        (307, 3, 101, new(2024,3,10), 5, true)
    };

    public LinqChallenge_31()
    {
        //WarehouseStockSummary_T1();
        //BackorderFrequencyByProduct_T2();
        MonthlyRestockCounts_T3();
        //ElectronicsRotationRate_T4();
        //HighVolumeStockPages_T5();
    }

    // 🔹 Task 1: Warehouse Stock Summary
    // Group by warehouse → show total stock value by multiplying quantity × cost price
    // ⏱️ Expected: 12–14 min
    // 10:18 - 10:28
    private void WarehouseStockSummary_T1()
    {
        var warehouseStockSummary = from wh in warehouses
                                    join stockEntry in stock on wh.WarehouseId equals stockEntry.WarehouseId
                                    join prod in products on stockEntry.ProductId equals prod.ProductId

                                    group new { stockEntry, prod } by wh into whGroup


                                    let totalValue = whGroup.Sum(x => x.prod.CostPrice * x.stockEntry.Quantity)
                                    let totalProductCount = whGroup.Select(x => x.prod.ProductId).Distinct().Count()
                                    orderby totalValue descending,
                                        whGroup.Key.Location ascending

                                    select new
                                    {
                                        Warehouse = whGroup.Key,
                                        ProductCount = totalProductCount,
                                        TotalValue = Math.Round(totalValue, 2),
                                    };
        foreach (var item in warehouseStockSummary)
        {
            WriteLine($"{item.Warehouse.WarehouseId}\t\t{item.Warehouse.Location}\t\tProducts count: {item.ProductCount}\t\tTotal Value $:{item.TotalValue}");
        }
    }

    // 🔹 Task 2: Backorder Frequency by Product
    // Count how many shipments for each product were backordered
    // ⏱️ Expected: 10–12 min
    // 10:37 - 10:41
    private void BackorderFrequencyByProduct_T2()
    {
        var backorderFrequencyByProduct = from prod in products
                                          join ship in shipments on prod.ProductId equals ship.ProductId
                                          where ship.Backordered
                                          group ship by prod into prodGroup

                                          let backOrderCount = prodGroup.Count()
                                          orderby backOrderCount descending

                                          select new
                                          {
                                              Product = prodGroup.Key.Name,
                                              BackOrderCount = backOrderCount
                                          };
        foreach (var item in backorderFrequencyByProduct)
        {
            WriteLine($"{item.Product}\t\tBack Order Count : {item.BackOrderCount}");
        }
    }

    // 🔹 Task 3: Monthly Restock Counts
    // Group shipments by month/year → show total quantity delivered per month
    // ⏱️ Expected: 10–13 min
    // 10:59 - 11:06
    private void MonthlyRestockCounts_T3()
    {
        var monthlyRestock = from shipment in shipments
                             group shipment by new { shipment.DateDelivered.Year, shipment.DateDelivered.Month } into shipmentGroup

                             let quantitySum = shipmentGroup.Sum(x => x.Quantity)
                             let shipmentCounts = shipmentGroup.Count()
                             let quantityPerShipment = shipmentCounts == 0 ? 0.0 : quantitySum / shipmentCounts

                             orderby quantitySum descending, shipmentGroup.Key.Year descending, shipmentGroup.Key.Month descending
                             select new
                             {
                                 Year = shipmentGroup.Key.Year,
                                 Month = shipmentGroup.Key.Month,
                                 QuantitySum = quantitySum,
                                 ShipmentCounts = shipmentCounts,
                                 QuantityPerShipment = Math.Round(quantityPerShipment, 2)
                             };
        foreach (var item in monthlyRestock)
        {
            WriteLine($"{item.Year} - {item.Month}\t\tRestock Count: {item.QuantitySum}\t\tShipments count: {item.ShipmentCounts}\t\tQuantity Per Shipment: {item.QuantityPerShipment}");
        }
    }

    // 🔹 Task 4: Electronics Rotation Rate
    // For each warehouse → show electronics stock currently held vs received in shipments
    // ⏱️ Expected: 15–18 min
    private void ElectronicsRotationRate_T4() { }

    // 🔹 Task 5: High-Volume Stock Pages
    // Paginate list of products with total stock quantity ≥ 40 → sorted by total quantity
    // ⏱️ Expected: 12–15 min
    private void HighVolumeStockPages_T5() { }
}
