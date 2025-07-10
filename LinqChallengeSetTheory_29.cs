using static System.Console;

namespace LINQ_Challeges;

public class LinqChallengeSetTheory_29
{
    private List<(int VendorId, string Name, string Region)> vendors = new()
    {
        (1, "TechDepot", "USA"), (2, "GlobalGear", "Germany"), (3, "BytesUnlimited", "India"),
        (4, "OmegaSupply", "Japan"), (5, "EdgeParts", "USA"), (6, "CortexNodes", "Canada")
    };

    private List<(int ProductId, string Name, string Category)> products = new()
    {
        (101, "SSD", "Storage"), (102, "RAM", "Memory"), (103, "CPU", "Processor"),
        (104, "GPU", "Graphics"), (105, "Motherboard", "Mainboard"), (106, "HDD", "Storage"),
        (107, "CoolingFan", "Accessories"), (108, "PowerSupply", "Power")
    };

    private List<(int VendorId, int ProductId)> vendorProducts = new()
    {
        (1,101), (1,102), (2,103), (2,104), (3,105), (3,106), (4,107), (5,108),
        (1,103), (3,101), (4,102), (6,104), (5,101)
    };

    private List<(int TxnId, int VendorId, int ProductId, int Quantity, decimal UnitPrice, DateTime TxnDate)> procurements = new()
    {
        (1001, 1, 101, 50, 110, new(2024,1,10)), (1002, 1, 102, 80, 70, new(2024,1,12)),
        (1003, 2, 103, 40, 290, new(2024,2,5)), (1004, 2, 104, 30, 400, new(2024,2,15)),
        (1005, 3, 105, 20, 150, new(2024,3,1)), (1006, 3, 106, 50, 90, new(2024,3,12)),
        (1007, 4, 107, 100, 20, new(2024,4,1)), (1008, 5, 108, 60, 130, new(2024,4,15)),
        (1009, 1, 103, 25, 300, new(2024,5,2)), (1010, 6, 104, 35, 390, new(2024,5,20))
    };

    private List<(int ProductId, DateTime DateRemoved)> discontinuedProducts = new()
    {
        (102, new(2024,6,1)), (104, new(2024,6,5)), (107, new(2024,6,10))
    };

    private List<(int ReviewerId, int ProductId, int Score)> reviews = new()
    {
        (901,101,5), (902,103,4), (903,106,3), (904,108,5), (905,104,2),
        (906,102,4), (907,105,5), (908,101,4), (909,104,3)
    };

    public LinqChallengeSetTheory_29()
    {
        //CommonProductsBetweenVendors_T1();
        //CommonProductsBetweenVendors_T1_New();
        //DiscontinuedProcurements_T2();
        //DiscontinuedProcurements_T2_New();
        //TopRatedStillAvailableProducts_T3();
        //TopRatedStillAvailableProducts_T3_WithScore();
        RegionWiseDistinctCategories_T4();
    }

    // 🔹 Task 1: Common Products Between Vendors Show products that are offered by both Vendor 1 and Vendor 3
    // Use: Intersect, Join ⏱️ Expected: 10–12 min
    // 8:48 - 9:05
    private void CommonProductsBetweenVendors_T1()
    {
        var v1andV3Prods = (from vp in vendorProducts
                            where vp.VendorId == 1 || vp.VendorId == 3
                            join prod in products on vp.ProductId equals prod.ProductId

                            group prod by vp.VendorId into vendorGroup

                            select new
                            {
                                VendorId = vendorGroup.Key,
                                Producuts = vendorGroup.ToList()
                            }).ToList();
        var v1Prods = (from item in v1andV3Prods where item.VendorId == 1 select item.Producuts).FirstOrDefault();
        var v3Prods = (from item in v1andV3Prods where item.VendorId == 2 select item.Producuts).FirstOrDefault();
        var commonProds = v1Prods == null ? v3Prods
                : v3Prods == null ? v1Prods : v1Prods?.Intersect(v3Prods);
        if (commonProds != null && commonProds.Any())
            foreach (var item in commonProds)
            {
                WriteLine($"{item.ProductId}\t{item.Name}\t\tCategory: {item.Category}");
            }
    }

    private void CommonProductsBetweenVendors_T1_New()
    {
        WriteLine();
        var commonProdIds = vendorProducts.Where(x => x.VendorId == 1).Select(x => x.ProductId)
                            .Intersect(
                                        vendorProducts.Where(x => x.VendorId == 3).Select(x => x.ProductId)
                                    );

        var commonProds = from prod in products
                          join commProdId in commonProdIds on prod.ProductId equals commProdId
                          select prod;
        foreach (var item in commonProds)
        {
            WriteLine($"{item.ProductId}\t{item.Name}\t\tCategory: {item.Category}");
        }
    }

    // 🔹 Task 2: Discontinued Procurements Show all transactions involving discontinued products
    // Use: Subquery / Except / Join ⏱️ Expected: 12–15 min
    // 10:18 - 10:30
    private void DiscontinuedProcurements_T2()
    {
        var discontinuedProcurements = from proc in procurements
                                       join disProd in discontinuedProducts on proc.ProductId equals disProd.ProductId
                                       join prod in products on disProd.ProductId equals prod.ProductId

                                       where proc.TxnDate >= disProd.DateRemoved
                                       group new { proc, prod } by disProd into disProdGroup
                                       select new
                                       {
                                           Product = disProdGroup.Select(x => x.prod).First(),
                                           Transactions = disProdGroup.Select(x => x.proc)
                                       };

        foreach (var item in discontinuedProcurements)
        {
            WriteLine($"{item.Product.Name}");
            foreach (var trans in item.Transactions)
            {
                WriteLine($"Transaction: {trans.TxnId}\t\t on:{trans.TxnDate}\t\t${trans.UnitPrice} Per unit\t\t{trans.Quantity} units");
            }
        }
    }

    // 10:31
    private void DiscontinuedProcurements_T2_New()
    {
        WriteLine();
        var discontinuedProcurements = from proc in procurements
                                       join disProd in discontinuedProducts on proc.ProductId equals disProd.ProductId

                                       where proc.TxnDate >= disProd.DateRemoved
                                       group proc by disProd.ProductId into disProdGroup
                                       select new
                                       {
                                           Product = products.SingleOrDefault(x => x.ProductId == disProdGroup.Key),
                                           Transactions = disProdGroup
                                       };

        foreach (var item in discontinuedProcurements)
        {
            WriteLine($"{item.Product.Name}");
            foreach (var trans in item.Transactions)
            {
                WriteLine($"Transaction: {trans.TxnId}\t\t on:{trans.TxnDate}\t\t${trans.UnitPrice} Per unit\t\t{trans.Quantity} units");
            }
        }
    }

    // 🔹 Task 3: Top Rated Still-Available Products Products with score ≥ 4 and not discontinued
    // Use: Except, GroupBy, Aggregate ⏱️ Expected: 15–18 min
    // 12:20 - 12:34
    private void TopRatedStillAvailableProducts_T3()
    {
        var topRatedProds = from prod in products
                            join review in reviews on prod.ProductId equals review.ProductId

                            group review by prod.ProductId into prodGroup
                            let avgScore = prodGroup.Average(x => x.Score)

                            where avgScore >= 4

                            orderby avgScore descending
                            select prodGroup.Key;

        var topRatedAvailableProdIds = topRatedProds.Except(discontinuedProducts.Select(x => x.ProductId));
        var topRatedAvailableProds = from prod in products
                                     where topRatedAvailableProdIds.Contains(prod.ProductId)
                                     select prod;
        foreach (var item in topRatedAvailableProds)
        {
            WriteLine($"{item.ProductId}\t\t{item.Name}");
        }
    }

    private void TopRatedStillAvailableProducts_T3_WithScore()
    {
        WriteLine();
        var topRatedAvailableProds = from prod in products
                                     join review in reviews on prod.ProductId equals review.ProductId

                                     group review by prod into prodGroup
                                     let avgScore = prodGroup.Average(x => x.Score)

                                     where avgScore >= 4 &&
                                     !discontinuedProducts.Select(x => x.ProductId).Contains(prodGroup.Key.ProductId)

                                     orderby avgScore descending
                                     select new
                                     {
                                         Product = prodGroup.Key,
                                         AvgScore = Math.Round(avgScore, 2)
                                     };


        foreach (var item in topRatedAvailableProds)
        {
            WriteLine($"{item.Product.ProductId}\t\t{item.Product.Name}\t\tAvg Score:{item.AvgScore}");
        }
    }

    // 🔹 Task 4: Region-Wise Distinct Categories Supplied Group vendors by region, list distinct
    // product categories they supply
    // Use: GroupJoin, SelectMany, Distinct, Group ⏱️ Expected: 15–18 min
    // 12:44 - 12:55
    private void RegionWiseDistinctCategories_T4()
    {
        var regionWiseCategories = from vendor in vendors
                                   join vp in vendorProducts on vendor.VendorId equals vp.VendorId
                                   join prod in products on vp.ProductId equals prod.ProductId

                                   group new { vendor, prod } by vendor.Region into regionGroup

                                   let vendorCount = regionGroup.Select(x => x.vendor.VendorId).Distinct().Count()
                                   let categories = regionGroup.Select(x => x.prod.Category).Distinct()
                                   let categoryCount = categories.Count()

                                   orderby categoryCount descending

                                   select new
                                   {
                                       Region = regionGroup.Key,
                                       VendorCount = vendorCount,
                                       CategoryCount = categoryCount,
                                       CategoryList = categories,
                                       Categories = string.Join(',', categories.OrderBy(x => x))
                                   };

        foreach (var item in regionWiseCategories)
        {
            WriteLine($"{item.Region}\t\tVendors: {item.VendorCount}\t\tCategories: {item.CategoryCount}\t\t[{item.Categories}]"); ;
        }
    }
}
