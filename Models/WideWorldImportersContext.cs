﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LINQ_Challeges.Models;

public partial class WideWorldImportersContext : DbContext
{
    public WideWorldImportersContext()
    {
    }

    public WideWorldImportersContext(DbContextOptions<WideWorldImportersContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BuyingGroup> BuyingGroups { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ColdRoomTemperature> ColdRoomTemperatures { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customer1> Customers1 { get; set; }

    public virtual DbSet<CustomerCategory> CustomerCategories { get; set; }

    public virtual DbSet<CustomerTransaction> CustomerTransactions { get; set; }

    public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderLine> OrderLines { get; set; }

    public virtual DbSet<PackageType> PackageTypes { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }

    public virtual DbSet<SpecialDeal> SpecialDeals { get; set; }

    public virtual DbSet<StateProvince> StateProvinces { get; set; }

    public virtual DbSet<StockGroup> StockGroups { get; set; }

    public virtual DbSet<StockItem> StockItems { get; set; }

    public virtual DbSet<StockItemHolding> StockItemHoldings { get; set; }

    public virtual DbSet<StockItemStockGroup> StockItemStockGroups { get; set; }

    public virtual DbSet<StockItemTransaction> StockItemTransactions { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Supplier1> Suppliers1 { get; set; }

    public virtual DbSet<SupplierCategory> SupplierCategories { get; set; }

    public virtual DbSet<SupplierTransaction> SupplierTransactions { get; set; }

    public virtual DbSet<SystemParameter> SystemParameters { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<VehicleTemperature> VehicleTemperatures { get; set; }

    public virtual DbSet<VehicleTemperature1> VehicleTemperatures1 { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Buddhika;Database=WideWorldImporters;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_100_CI_AS");

        modelBuilder.Entity<BuyingGroup>(entity =>
        {
            entity.HasKey(e => e.BuyingGroupId).HasName("PK_Sales_BuyingGroups");

            entity
                .ToTable("BuyingGroups", "Sales")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("BuyingGroups_Archive", "Sales");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.BuyingGroupName, "UQ_Sales_BuyingGroups_BuyingGroupName").IsUnique();

            entity.Property(e => e.BuyingGroupId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[BuyingGroupID])")
                .HasColumnName("BuyingGroupID");
            entity.Property(e => e.BuyingGroupName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.BuyingGroups)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_BuyingGroups_Application_People");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK_Application_Cities");

            entity
                .ToTable("Cities", "Application")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Cities_Archive", "Application");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.StateProvinceId, "FK_Application_Cities_StateProvinceID");

            entity.Property(e => e.CityId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[CityID])")
                .HasColumnName("CityID");
            entity.Property(e => e.CityName).HasMaxLength(50);
            entity.Property(e => e.StateProvinceId).HasColumnName("StateProvinceID");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_Cities_Application_People");

            entity.HasOne(d => d.StateProvince).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_Cities_StateProvinceID_Application_StateProvinces");
        });

        modelBuilder.Entity<ColdRoomTemperature>(entity =>
        {
            entity.HasKey(e => e.ColdRoomTemperatureId)
                .HasName("PK_Warehouse_ColdRoomTemperatures")
                .IsClustered(false);

            entity
                .ToTable("ColdRoomTemperatures", "Warehouse")
                .IsMemoryOptimized()
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("ColdRoomTemperatures_Archive", "Warehouse");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.ColdRoomSensorNumber, "IX_Warehouse_ColdRoomTemperatures_ColdRoomSensorNumber");

            entity.Property(e => e.ColdRoomTemperatureId).HasColumnName("ColdRoomTemperatureID");
            entity.Property(e => e.Temperature).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK_Warehouse_Colors");

            entity
                .ToTable("Colors", "Warehouse")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Colors_Archive", "Warehouse");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.ColorName, "UQ_Warehouse_Colors_ColorName").IsUnique();

            entity.Property(e => e.ColorId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[ColorID])")
                .HasColumnName("ColorID");
            entity.Property(e => e.ColorName).HasMaxLength(20);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.Colors)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_Colors_Application_People");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK_Application_Countries");

            entity
                .ToTable("Countries", "Application")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Countries_Archive", "Application");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.CountryName, "UQ_Application_Countries_CountryName").IsUnique();

            entity.HasIndex(e => e.FormalName, "UQ_Application_Countries_FormalName").IsUnique();

            entity.Property(e => e.CountryId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[CountryID])")
                .HasColumnName("CountryID");
            entity.Property(e => e.Continent).HasMaxLength(30);
            entity.Property(e => e.CountryName).HasMaxLength(60);
            entity.Property(e => e.CountryType).HasMaxLength(20);
            entity.Property(e => e.FormalName).HasMaxLength(60);
            entity.Property(e => e.IsoAlpha3Code).HasMaxLength(3);
            entity.Property(e => e.Region).HasMaxLength(30);
            entity.Property(e => e.Subregion).HasMaxLength(30);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.Countries)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_Countries_Application_People");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK_Sales_Customers");

            entity
                .ToTable("Customers", "Sales")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Customers_Archive", "Sales");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.AlternateContactPersonId, "FK_Sales_Customers_AlternateContactPersonID");

            entity.HasIndex(e => e.BuyingGroupId, "FK_Sales_Customers_BuyingGroupID");

            entity.HasIndex(e => e.CustomerCategoryId, "FK_Sales_Customers_CustomerCategoryID");

            entity.HasIndex(e => e.DeliveryCityId, "FK_Sales_Customers_DeliveryCityID");

            entity.HasIndex(e => e.DeliveryMethodId, "FK_Sales_Customers_DeliveryMethodID");

            entity.HasIndex(e => e.PostalCityId, "FK_Sales_Customers_PostalCityID");

            entity.HasIndex(e => e.PrimaryContactPersonId, "FK_Sales_Customers_PrimaryContactPersonID");

            entity.HasIndex(e => new { e.IsOnCreditHold, e.CustomerId, e.BillToCustomerId }, "IX_Sales_Customers_Perf_20160301_06");

            entity.HasIndex(e => e.CustomerName, "UQ_Sales_Customers_CustomerName").IsUnique();

            entity.Property(e => e.CustomerId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[CustomerID])")
                .HasColumnName("CustomerID");
            entity.Property(e => e.AlternateContactPersonId).HasColumnName("AlternateContactPersonID");
            entity.Property(e => e.BillToCustomerId).HasColumnName("BillToCustomerID");
            entity.Property(e => e.BuyingGroupId).HasColumnName("BuyingGroupID");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerCategoryId).HasColumnName("CustomerCategoryID");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.DeliveryAddressLine1).HasMaxLength(60);
            entity.Property(e => e.DeliveryAddressLine2).HasMaxLength(60);
            entity.Property(e => e.DeliveryCityId).HasColumnName("DeliveryCityID");
            entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");
            entity.Property(e => e.DeliveryPostalCode).HasMaxLength(10);
            entity.Property(e => e.DeliveryRun).HasMaxLength(5);
            entity.Property(e => e.FaxNumber).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PostalAddressLine1).HasMaxLength(60);
            entity.Property(e => e.PostalAddressLine2).HasMaxLength(60);
            entity.Property(e => e.PostalCityId).HasColumnName("PostalCityID");
            entity.Property(e => e.PostalPostalCode).HasMaxLength(10);
            entity.Property(e => e.PrimaryContactPersonId).HasColumnName("PrimaryContactPersonID");
            entity.Property(e => e.RunPosition).HasMaxLength(5);
            entity.Property(e => e.StandardDiscountPercentage).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(256)
                .HasColumnName("WebsiteURL");

            entity.HasOne(d => d.AlternateContactPerson).WithMany(p => p.CustomerAlternateContactPeople)
                .HasForeignKey(d => d.AlternateContactPersonId)
                .HasConstraintName("FK_Sales_Customers_AlternateContactPersonID_Application_People");

            entity.HasOne(d => d.BillToCustomer).WithMany(p => p.InverseBillToCustomer)
                .HasForeignKey(d => d.BillToCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers_BillToCustomerID_Sales_Customers");

            entity.HasOne(d => d.BuyingGroup).WithMany(p => p.Customers)
                .HasForeignKey(d => d.BuyingGroupId)
                .HasConstraintName("FK_Sales_Customers_BuyingGroupID_Sales_BuyingGroups");

            entity.HasOne(d => d.CustomerCategory).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers_CustomerCategoryID_Sales_CustomerCategories");

            entity.HasOne(d => d.DeliveryCity).WithMany(p => p.CustomerDeliveryCities)
                .HasForeignKey(d => d.DeliveryCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers_DeliveryCityID_Application_Cities");

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.Customers)
                .HasForeignKey(d => d.DeliveryMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers_DeliveryMethodID_Application_DeliveryMethods");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.CustomerLastEditedByNavigations)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers_Application_People");

            entity.HasOne(d => d.PostalCity).WithMany(p => p.CustomerPostalCities)
                .HasForeignKey(d => d.PostalCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers_PostalCityID_Application_Cities");

            entity.HasOne(d => d.PrimaryContactPerson).WithMany(p => p.CustomerPrimaryContactPeople)
                .HasForeignKey(d => d.PrimaryContactPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Customers_PrimaryContactPersonID_Application_People");
        });

        modelBuilder.Entity<Customer1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Customers", "Website");

            entity.Property(e => e.AlternateContact).HasMaxLength(50);
            entity.Property(e => e.BuyingGroupName).HasMaxLength(50);
            entity.Property(e => e.CityName).HasMaxLength(50);
            entity.Property(e => e.CustomerCategoryName).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.DeliveryMethod).HasMaxLength(50);
            entity.Property(e => e.DeliveryRun).HasMaxLength(5);
            entity.Property(e => e.FaxNumber).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PrimaryContact).HasMaxLength(50);
            entity.Property(e => e.RunPosition).HasMaxLength(5);
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(256)
                .HasColumnName("WebsiteURL");
        });

        modelBuilder.Entity<CustomerCategory>(entity =>
        {
            entity.HasKey(e => e.CustomerCategoryId).HasName("PK_Sales_CustomerCategories");

            entity
                .ToTable("CustomerCategories", "Sales")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("CustomerCategories_Archive", "Sales");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.CustomerCategoryName, "UQ_Sales_CustomerCategories_CustomerCategoryName").IsUnique();

            entity.Property(e => e.CustomerCategoryId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[CustomerCategoryID])")
                .HasColumnName("CustomerCategoryID");
            entity.Property(e => e.CustomerCategoryName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.CustomerCategories)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_CustomerCategories_Application_People");
        });

        modelBuilder.Entity<CustomerTransaction>(entity =>
        {
            entity.HasKey(e => e.CustomerTransactionId)
                .HasName("PK_Sales_CustomerTransactions")
                .IsClustered(false);

            entity.ToTable("CustomerTransactions", "Sales");

            entity.HasIndex(e => e.TransactionDate, "CX_Sales_CustomerTransactions").IsClustered();

            entity.HasIndex(e => new { e.TransactionDate, e.CustomerId }, "FK_Sales_CustomerTransactions_CustomerID");

            entity.HasIndex(e => new { e.TransactionDate, e.InvoiceId }, "FK_Sales_CustomerTransactions_InvoiceID");

            entity.HasIndex(e => new { e.TransactionDate, e.PaymentMethodId }, "FK_Sales_CustomerTransactions_PaymentMethodID");

            entity.HasIndex(e => new { e.TransactionDate, e.TransactionTypeId }, "FK_Sales_CustomerTransactions_TransactionTypeID");

            entity.HasIndex(e => new { e.TransactionDate, e.IsFinalized }, "IX_Sales_CustomerTransactions_IsFinalized");

            entity.Property(e => e.CustomerTransactionId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[TransactionID])")
                .HasColumnName("CustomerTransactionID");
            entity.Property(e => e.AmountExcludingTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.IsFinalized).HasComputedColumnSql("(case when [FinalizationDate] IS NULL then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", true);
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.OutstandingBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerTransactions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_CustomerTransactions_CustomerID_Sales_Customers");

            entity.HasOne(d => d.Invoice).WithMany(p => p.CustomerTransactions)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_Sales_CustomerTransactions_InvoiceID_Sales_Invoices");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.CustomerTransactions)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_CustomerTransactions_Application_People");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.CustomerTransactions)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_Sales_CustomerTransactions_PaymentMethodID_Application_PaymentMethods");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.CustomerTransactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_CustomerTransactions_TransactionTypeID_Application_TransactionTypes");
        });

        modelBuilder.Entity<DeliveryMethod>(entity =>
        {
            entity.HasKey(e => e.DeliveryMethodId).HasName("PK_Application_DeliveryMethods");

            entity
                .ToTable("DeliveryMethods", "Application")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("DeliveryMethods_Archive", "Application");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.DeliveryMethodName, "UQ_Application_DeliveryMethods_DeliveryMethodName").IsUnique();

            entity.Property(e => e.DeliveryMethodId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[DeliveryMethodID])")
                .HasColumnName("DeliveryMethodID");
            entity.Property(e => e.DeliveryMethodName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.DeliveryMethods)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_DeliveryMethods_Application_People");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK_Sales_Invoices");

            entity.ToTable("Invoices", "Sales");

            entity.HasIndex(e => e.AccountsPersonId, "FK_Sales_Invoices_AccountsPersonID");

            entity.HasIndex(e => e.BillToCustomerId, "FK_Sales_Invoices_BillToCustomerID");

            entity.HasIndex(e => e.ContactPersonId, "FK_Sales_Invoices_ContactPersonID");

            entity.HasIndex(e => e.CustomerId, "FK_Sales_Invoices_CustomerID");

            entity.HasIndex(e => e.DeliveryMethodId, "FK_Sales_Invoices_DeliveryMethodID");

            entity.HasIndex(e => e.OrderId, "FK_Sales_Invoices_OrderID");

            entity.HasIndex(e => e.PackedByPersonId, "FK_Sales_Invoices_PackedByPersonID");

            entity.HasIndex(e => e.SalespersonPersonId, "FK_Sales_Invoices_SalespersonPersonID");

            entity.HasIndex(e => e.ConfirmedDeliveryTime, "IX_Sales_Invoices_ConfirmedDeliveryTime");

            entity.Property(e => e.InvoiceId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[InvoiceID])")
                .HasColumnName("InvoiceID");
            entity.Property(e => e.AccountsPersonId).HasColumnName("AccountsPersonID");
            entity.Property(e => e.BillToCustomerId).HasColumnName("BillToCustomerID");
            entity.Property(e => e.ConfirmedDeliveryTime).HasComputedColumnSql("(TRY_CONVERT([datetime2](7),json_value([ReturnedDeliveryData],N'$.DeliveredWhen'),(126)))", false);
            entity.Property(e => e.ConfirmedReceivedBy)
                .HasMaxLength(4000)
                .HasComputedColumnSql("(json_value([ReturnedDeliveryData],N'$.ReceivedBy'))", false);
            entity.Property(e => e.ContactPersonId).HasColumnName("ContactPersonID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerPurchaseOrderNumber).HasMaxLength(20);
            entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");
            entity.Property(e => e.DeliveryRun).HasMaxLength(5);
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PackedByPersonId).HasColumnName("PackedByPersonID");
            entity.Property(e => e.RunPosition).HasMaxLength(5);
            entity.Property(e => e.SalespersonPersonId).HasColumnName("SalespersonPersonID");

            entity.HasOne(d => d.AccountsPerson).WithMany(p => p.InvoiceAccountsPeople)
                .HasForeignKey(d => d.AccountsPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_AccountsPersonID_Application_People");

            entity.HasOne(d => d.BillToCustomer).WithMany(p => p.InvoiceBillToCustomers)
                .HasForeignKey(d => d.BillToCustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_BillToCustomerID_Sales_Customers");

            entity.HasOne(d => d.ContactPerson).WithMany(p => p.InvoiceContactPeople)
                .HasForeignKey(d => d.ContactPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_ContactPersonID_Application_People");

            entity.HasOne(d => d.Customer).WithMany(p => p.InvoiceCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_CustomerID_Sales_Customers");

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.DeliveryMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_DeliveryMethodID_Application_DeliveryMethods");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.InvoiceLastEditedByNavigations)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_Application_People");

            entity.HasOne(d => d.Order).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Sales_Invoices_OrderID_Sales_Orders");

            entity.HasOne(d => d.PackedByPerson).WithMany(p => p.InvoicePackedByPeople)
                .HasForeignKey(d => d.PackedByPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_PackedByPersonID_Application_People");

            entity.HasOne(d => d.SalespersonPerson).WithMany(p => p.InvoiceSalespersonPeople)
                .HasForeignKey(d => d.SalespersonPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Invoices_SalespersonPersonID_Application_People");
        });

        modelBuilder.Entity<InvoiceLine>(entity =>
        {
            entity.HasKey(e => e.InvoiceLineId).HasName("PK_Sales_InvoiceLines");

            entity.ToTable("InvoiceLines", "Sales");

            entity.HasIndex(e => e.InvoiceId, "FK_Sales_InvoiceLines_InvoiceID");

            entity.HasIndex(e => e.PackageTypeId, "FK_Sales_InvoiceLines_PackageTypeID");

            entity.HasIndex(e => e.StockItemId, "FK_Sales_InvoiceLines_StockItemID");

            entity.Property(e => e.InvoiceLineId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[InvoiceLineID])")
                .HasColumnName("InvoiceLineID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ExtendedPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.LineProfit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PackageTypeId).HasColumnName("PackageTypeID");
            entity.Property(e => e.StockItemId).HasColumnName("StockItemID");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_InvoiceLines_InvoiceID_Sales_Invoices");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_InvoiceLines_Application_People");

            entity.HasOne(d => d.PackageType).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.PackageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_InvoiceLines_PackageTypeID_Warehouse_PackageTypes");

            entity.HasOne(d => d.StockItem).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.StockItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_InvoiceLines_StockItemID_Warehouse_StockItems");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Sales_Orders");

            entity.ToTable("Orders", "Sales");

            entity.HasIndex(e => e.ContactPersonId, "FK_Sales_Orders_ContactPersonID");

            entity.HasIndex(e => e.CustomerId, "FK_Sales_Orders_CustomerID");

            entity.HasIndex(e => e.PickedByPersonId, "FK_Sales_Orders_PickedByPersonID");

            entity.HasIndex(e => e.SalespersonPersonId, "FK_Sales_Orders_SalespersonPersonID");

            entity.Property(e => e.OrderId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[OrderID])")
                .HasColumnName("OrderID");
            entity.Property(e => e.BackorderOrderId).HasColumnName("BackorderOrderID");
            entity.Property(e => e.ContactPersonId).HasColumnName("ContactPersonID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerPurchaseOrderNumber).HasMaxLength(20);
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PickedByPersonId).HasColumnName("PickedByPersonID");
            entity.Property(e => e.SalespersonPersonId).HasColumnName("SalespersonPersonID");

            entity.HasOne(d => d.BackorderOrder).WithMany(p => p.InverseBackorderOrder)
                .HasForeignKey(d => d.BackorderOrderId)
                .HasConstraintName("FK_Sales_Orders_BackorderOrderID_Sales_Orders");

            entity.HasOne(d => d.ContactPerson).WithMany(p => p.OrderContactPeople)
                .HasForeignKey(d => d.ContactPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Orders_ContactPersonID_Application_People");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Orders_CustomerID_Sales_Customers");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.OrderLastEditedByNavigations)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Orders_Application_People");

            entity.HasOne(d => d.PickedByPerson).WithMany(p => p.OrderPickedByPeople)
                .HasForeignKey(d => d.PickedByPersonId)
                .HasConstraintName("FK_Sales_Orders_PickedByPersonID_Application_People");

            entity.HasOne(d => d.SalespersonPerson).WithMany(p => p.OrderSalespersonPeople)
                .HasForeignKey(d => d.SalespersonPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_Orders_SalespersonPersonID_Application_People");
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.OrderLineId).HasName("PK_Sales_OrderLines");

            entity.ToTable("OrderLines", "Sales");

            entity.HasIndex(e => e.OrderId, "FK_Sales_OrderLines_OrderID");

            entity.HasIndex(e => e.PackageTypeId, "FK_Sales_OrderLines_PackageTypeID");

            entity.HasIndex(e => e.StockItemId, "IX_Sales_OrderLines_AllocatedStockItems");

            entity.HasIndex(e => new { e.PickingCompletedWhen, e.OrderId, e.OrderLineId }, "IX_Sales_OrderLines_Perf_20160301_01");

            entity.HasIndex(e => new { e.StockItemId, e.PickingCompletedWhen }, "IX_Sales_OrderLines_Perf_20160301_02");

            entity.HasIndex(e => e.OrderId, "IX_Suggested");

            entity.Property(e => e.OrderLineId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[OrderLineID])")
                .HasColumnName("OrderLineID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PackageTypeId).HasColumnName("PackageTypeID");
            entity.Property(e => e.StockItemId).HasColumnName("StockItemID");
            entity.Property(e => e.TaxRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_OrderLines_Application_People");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_OrderLines_OrderID_Sales_Orders");

            entity.HasOne(d => d.PackageType).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.PackageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_OrderLines_PackageTypeID_Warehouse_PackageTypes");

            entity.HasOne(d => d.StockItem).WithMany(p => p.OrderLines)
                .HasForeignKey(d => d.StockItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_OrderLines_StockItemID_Warehouse_StockItems");
        });

        modelBuilder.Entity<PackageType>(entity =>
        {
            entity.HasKey(e => e.PackageTypeId).HasName("PK_Warehouse_PackageTypes");

            entity
                .ToTable("PackageTypes", "Warehouse")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("PackageTypes_Archive", "Warehouse");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.PackageTypeName, "UQ_Warehouse_PackageTypes_PackageTypeName").IsUnique();

            entity.Property(e => e.PackageTypeId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[PackageTypeID])")
                .HasColumnName("PackageTypeID");
            entity.Property(e => e.PackageTypeName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.PackageTypes)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_PackageTypes_Application_People");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK_Application_PaymentMethods");

            entity
                .ToTable("PaymentMethods", "Application")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("PaymentMethods_Archive", "Application");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.PaymentMethodName, "UQ_Application_PaymentMethods_PaymentMethodName").IsUnique();

            entity.Property(e => e.PaymentMethodId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[PaymentMethodID])")
                .HasColumnName("PaymentMethodID");
            entity.Property(e => e.PaymentMethodName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_PaymentMethods_Application_People");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK_Application_People");

            entity
                .ToTable("People", "Application")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("People_Archive", "Application");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.FullName, "IX_Application_People_FullName");

            entity.HasIndex(e => e.IsEmployee, "IX_Application_People_IsEmployee");

            entity.HasIndex(e => e.IsSalesperson, "IX_Application_People_IsSalesperson");

            entity.HasIndex(e => new { e.IsPermittedToLogon, e.PersonId }, "IX_Application_People_Perf_20160301_05");

            entity.Property(e => e.PersonId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[PersonID])")
                .HasColumnName("PersonID");
            entity.Property(e => e.EmailAddress).HasMaxLength(256);
            entity.Property(e => e.FaxNumber).HasMaxLength(20);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.LogonName).HasMaxLength(50);
            entity.Property(e => e.OtherLanguages).HasComputedColumnSql("(json_query([CustomFields],N'$.OtherLanguages'))", false);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PreferredName).HasMaxLength(50);
            entity.Property(e => e.SearchName)
                .HasMaxLength(101)
                .HasComputedColumnSql("(concat([PreferredName],N' ',[FullName]))", true);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.InverseLastEditedByNavigation)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_People_Application_People");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId).HasName("PK_Purchasing_PurchaseOrders");

            entity.ToTable("PurchaseOrders", "Purchasing");

            entity.HasIndex(e => e.ContactPersonId, "FK_Purchasing_PurchaseOrders_ContactPersonID");

            entity.HasIndex(e => e.DeliveryMethodId, "FK_Purchasing_PurchaseOrders_DeliveryMethodID");

            entity.HasIndex(e => e.SupplierId, "FK_Purchasing_PurchaseOrders_SupplierID");

            entity.Property(e => e.PurchaseOrderId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[PurchaseOrderID])")
                .HasColumnName("PurchaseOrderID");
            entity.Property(e => e.ContactPersonId).HasColumnName("ContactPersonID");
            entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.SupplierReference).HasMaxLength(20);

            entity.HasOne(d => d.ContactPerson).WithMany(p => p.PurchaseOrderContactPeople)
                .HasForeignKey(d => d.ContactPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrders_ContactPersonID_Application_People");

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.DeliveryMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrders_DeliveryMethodID_Application_DeliveryMethods");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.PurchaseOrderLastEditedByNavigations)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrders_Application_People");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrders_SupplierID_Purchasing_Suppliers");
        });

        modelBuilder.Entity<PurchaseOrderLine>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderLineId).HasName("PK_Purchasing_PurchaseOrderLines");

            entity.ToTable("PurchaseOrderLines", "Purchasing");

            entity.HasIndex(e => e.PackageTypeId, "FK_Purchasing_PurchaseOrderLines_PackageTypeID");

            entity.HasIndex(e => e.PurchaseOrderId, "FK_Purchasing_PurchaseOrderLines_PurchaseOrderID");

            entity.HasIndex(e => e.StockItemId, "FK_Purchasing_PurchaseOrderLines_StockItemID");

            entity.HasIndex(e => new { e.IsOrderLineFinalized, e.StockItemId }, "IX_Purchasing_PurchaseOrderLines_Perf_20160301_4");

            entity.Property(e => e.PurchaseOrderLineId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[PurchaseOrderLineID])")
                .HasColumnName("PurchaseOrderLineID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ExpectedUnitPricePerOuter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PackageTypeId).HasColumnName("PackageTypeID");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
            entity.Property(e => e.StockItemId).HasColumnName("StockItemID");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.PurchaseOrderLines)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrderLines_Application_People");

            entity.HasOne(d => d.PackageType).WithMany(p => p.PurchaseOrderLines)
                .HasForeignKey(d => d.PackageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrderLines_PackageTypeID_Warehouse_PackageTypes");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderLines)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrderLines_PurchaseOrderID_Purchasing_PurchaseOrders");

            entity.HasOne(d => d.StockItem).WithMany(p => p.PurchaseOrderLines)
                .HasForeignKey(d => d.StockItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_PurchaseOrderLines_StockItemID_Warehouse_StockItems");
        });

        modelBuilder.Entity<SpecialDeal>(entity =>
        {
            entity.HasKey(e => e.SpecialDealId).HasName("PK_Sales_SpecialDeals");

            entity.ToTable("SpecialDeals", "Sales");

            entity.HasIndex(e => e.BuyingGroupId, "FK_Sales_SpecialDeals_BuyingGroupID");

            entity.HasIndex(e => e.CustomerCategoryId, "FK_Sales_SpecialDeals_CustomerCategoryID");

            entity.HasIndex(e => e.CustomerId, "FK_Sales_SpecialDeals_CustomerID");

            entity.HasIndex(e => e.StockGroupId, "FK_Sales_SpecialDeals_StockGroupID");

            entity.HasIndex(e => e.StockItemId, "FK_Sales_SpecialDeals_StockItemID");

            entity.Property(e => e.SpecialDealId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[SpecialDealID])")
                .HasColumnName("SpecialDealID");
            entity.Property(e => e.BuyingGroupId).HasColumnName("BuyingGroupID");
            entity.Property(e => e.CustomerCategoryId).HasColumnName("CustomerCategoryID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DealDescription).HasMaxLength(30);
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.StockGroupId).HasColumnName("StockGroupID");
            entity.Property(e => e.StockItemId).HasColumnName("StockItemID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.BuyingGroup).WithMany(p => p.SpecialDeals)
                .HasForeignKey(d => d.BuyingGroupId)
                .HasConstraintName("FK_Sales_SpecialDeals_BuyingGroupID_Sales_BuyingGroups");

            entity.HasOne(d => d.CustomerCategory).WithMany(p => p.SpecialDeals)
                .HasForeignKey(d => d.CustomerCategoryId)
                .HasConstraintName("FK_Sales_SpecialDeals_CustomerCategoryID_Sales_CustomerCategories");

            entity.HasOne(d => d.Customer).WithMany(p => p.SpecialDeals)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Sales_SpecialDeals_CustomerID_Sales_Customers");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.SpecialDeals)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sales_SpecialDeals_Application_People");

            entity.HasOne(d => d.StockGroup).WithMany(p => p.SpecialDeals)
                .HasForeignKey(d => d.StockGroupId)
                .HasConstraintName("FK_Sales_SpecialDeals_StockGroupID_Warehouse_StockGroups");

            entity.HasOne(d => d.StockItem).WithMany(p => p.SpecialDeals)
                .HasForeignKey(d => d.StockItemId)
                .HasConstraintName("FK_Sales_SpecialDeals_StockItemID_Warehouse_StockItems");
        });

        modelBuilder.Entity<StateProvince>(entity =>
        {
            entity.HasKey(e => e.StateProvinceId).HasName("PK_Application_StateProvinces");

            entity
                .ToTable("StateProvinces", "Application")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StateProvinces_Archive", "Application");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.CountryId, "FK_Application_StateProvinces_CountryID");

            entity.HasIndex(e => e.SalesTerritory, "IX_Application_StateProvinces_SalesTerritory");

            entity.HasIndex(e => e.StateProvinceName, "UQ_Application_StateProvinces_StateProvinceName").IsUnique();

            entity.Property(e => e.StateProvinceId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[StateProvinceID])")
                .HasColumnName("StateProvinceID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.SalesTerritory).HasMaxLength(50);
            entity.Property(e => e.StateProvinceCode).HasMaxLength(5);
            entity.Property(e => e.StateProvinceName).HasMaxLength(50);

            entity.HasOne(d => d.Country).WithMany(p => p.StateProvinces)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_StateProvinces_CountryID_Application_Countries");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.StateProvinces)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_StateProvinces_Application_People");
        });

        modelBuilder.Entity<StockGroup>(entity =>
        {
            entity.HasKey(e => e.StockGroupId).HasName("PK_Warehouse_StockGroups");

            entity
                .ToTable("StockGroups", "Warehouse")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StockGroups_Archive", "Warehouse");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.StockGroupName, "UQ_Warehouse_StockGroups_StockGroupName").IsUnique();

            entity.Property(e => e.StockGroupId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[StockGroupID])")
                .HasColumnName("StockGroupID");
            entity.Property(e => e.StockGroupName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.StockGroups)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockGroups_Application_People");
        });

        modelBuilder.Entity<StockItem>(entity =>
        {
            entity.HasKey(e => e.StockItemId).HasName("PK_Warehouse_StockItems");

            entity
                .ToTable("StockItems", "Warehouse")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("StockItems_Archive", "Warehouse");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.ColorId, "FK_Warehouse_StockItems_ColorID");

            entity.HasIndex(e => e.OuterPackageId, "FK_Warehouse_StockItems_OuterPackageID");

            entity.HasIndex(e => e.SupplierId, "FK_Warehouse_StockItems_SupplierID");

            entity.HasIndex(e => e.UnitPackageId, "FK_Warehouse_StockItems_UnitPackageID");

            entity.HasIndex(e => e.StockItemName, "UQ_Warehouse_StockItems_StockItemName").IsUnique();

            entity.Property(e => e.StockItemId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[StockItemID])")
                .HasColumnName("StockItemID");
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.OuterPackageId).HasColumnName("OuterPackageID");
            entity.Property(e => e.RecommendedRetailPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SearchDetails).HasComputedColumnSql("(concat([StockItemName],N' ',[MarketingComments]))", false);
            entity.Property(e => e.Size).HasMaxLength(20);
            entity.Property(e => e.StockItemName).HasMaxLength(100);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.Tags).HasComputedColumnSql("(json_query([CustomFields],N'$.Tags'))", false);
            entity.Property(e => e.TaxRate).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.TypicalWeightPerUnit).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.UnitPackageId).HasColumnName("UnitPackageID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Color).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.ColorId)
                .HasConstraintName("FK_Warehouse_StockItems_ColorID_Warehouse_Colors");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItems_Application_People");

            entity.HasOne(d => d.OuterPackage).WithMany(p => p.StockItemOuterPackages)
                .HasForeignKey(d => d.OuterPackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItems_OuterPackageID_Warehouse_PackageTypes");

            entity.HasOne(d => d.Supplier).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItems_SupplierID_Purchasing_Suppliers");

            entity.HasOne(d => d.UnitPackage).WithMany(p => p.StockItemUnitPackages)
                .HasForeignKey(d => d.UnitPackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItems_UnitPackageID_Warehouse_PackageTypes");
        });

        modelBuilder.Entity<StockItemHolding>(entity =>
        {
            entity.HasKey(e => e.StockItemId).HasName("PK_Warehouse_StockItemHoldings");

            entity.ToTable("StockItemHoldings", "Warehouse");

            entity.Property(e => e.StockItemId)
                .ValueGeneratedNever()
                .HasColumnName("StockItemID");
            entity.Property(e => e.BinLocation).HasMaxLength(20);
            entity.Property(e => e.LastCostPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.StockItemHoldings)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItemHoldings_Application_People");

            entity.HasOne(d => d.StockItem).WithOne(p => p.StockItemHolding)
                .HasForeignKey<StockItemHolding>(d => d.StockItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PKFK_Warehouse_StockItemHoldings_StockItemID_Warehouse_StockItems");
        });

        modelBuilder.Entity<StockItemStockGroup>(entity =>
        {
            entity.HasKey(e => e.StockItemStockGroupId).HasName("PK_Warehouse_StockItemStockGroups");

            entity.ToTable("StockItemStockGroups", "Warehouse");

            entity.HasIndex(e => new { e.StockGroupId, e.StockItemId }, "UQ_StockItemStockGroups_StockGroupID_Lookup").IsUnique();

            entity.HasIndex(e => new { e.StockItemId, e.StockGroupId }, "UQ_StockItemStockGroups_StockItemID_Lookup").IsUnique();

            entity.Property(e => e.StockItemStockGroupId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[StockItemStockGroupID])")
                .HasColumnName("StockItemStockGroupID");
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.StockGroupId).HasColumnName("StockGroupID");
            entity.Property(e => e.StockItemId).HasColumnName("StockItemID");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.StockItemStockGroups)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItemStockGroups_Application_People");

            entity.HasOne(d => d.StockGroup).WithMany(p => p.StockItemStockGroups)
                .HasForeignKey(d => d.StockGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItemStockGroups_StockGroupID_Warehouse_StockGroups");

            entity.HasOne(d => d.StockItem).WithMany(p => p.StockItemStockGroups)
                .HasForeignKey(d => d.StockItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItemStockGroups_StockItemID_Warehouse_StockItems");
        });

        modelBuilder.Entity<StockItemTransaction>(entity =>
        {
            entity.HasKey(e => e.StockItemTransactionId)
                .HasName("PK_Warehouse_StockItemTransactions")
                .IsClustered(false);

            entity.ToTable("StockItemTransactions", "Warehouse");

            entity.HasIndex(e => e.CustomerId, "FK_Warehouse_StockItemTransactions_CustomerID");

            entity.HasIndex(e => e.InvoiceId, "FK_Warehouse_StockItemTransactions_InvoiceID");

            entity.HasIndex(e => e.PurchaseOrderId, "FK_Warehouse_StockItemTransactions_PurchaseOrderID");

            entity.HasIndex(e => e.StockItemId, "FK_Warehouse_StockItemTransactions_StockItemID");

            entity.HasIndex(e => e.SupplierId, "FK_Warehouse_StockItemTransactions_SupplierID");

            entity.HasIndex(e => e.TransactionTypeId, "FK_Warehouse_StockItemTransactions_TransactionTypeID");

            entity.Property(e => e.StockItemTransactionId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[TransactionID])")
                .HasColumnName("StockItemTransactionID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.StockItemId).HasColumnName("StockItemID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

            entity.HasOne(d => d.Customer).WithMany(p => p.StockItemTransactions)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Warehouse_StockItemTransactions_CustomerID_Sales_Customers");

            entity.HasOne(d => d.Invoice).WithMany(p => p.StockItemTransactions)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK_Warehouse_StockItemTransactions_InvoiceID_Sales_Invoices");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.StockItemTransactions)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItemTransactions_Application_People");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.StockItemTransactions)
                .HasForeignKey(d => d.PurchaseOrderId)
                .HasConstraintName("FK_Warehouse_StockItemTransactions_PurchaseOrderID_Purchasing_PurchaseOrders");

            entity.HasOne(d => d.StockItem).WithMany(p => p.StockItemTransactions)
                .HasForeignKey(d => d.StockItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItemTransactions_StockItemID_Warehouse_StockItems");

            entity.HasOne(d => d.Supplier).WithMany(p => p.StockItemTransactions)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Warehouse_StockItemTransactions_SupplierID_Purchasing_Suppliers");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.StockItemTransactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Warehouse_StockItemTransactions_TransactionTypeID_Application_TransactionTypes");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK_Purchasing_Suppliers");

            entity
                .ToTable("Suppliers", "Purchasing")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("Suppliers_Archive", "Purchasing");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.AlternateContactPersonId, "FK_Purchasing_Suppliers_AlternateContactPersonID");

            entity.HasIndex(e => e.DeliveryCityId, "FK_Purchasing_Suppliers_DeliveryCityID");

            entity.HasIndex(e => e.DeliveryMethodId, "FK_Purchasing_Suppliers_DeliveryMethodID");

            entity.HasIndex(e => e.PostalCityId, "FK_Purchasing_Suppliers_PostalCityID");

            entity.HasIndex(e => e.PrimaryContactPersonId, "FK_Purchasing_Suppliers_PrimaryContactPersonID");

            entity.HasIndex(e => e.SupplierCategoryId, "FK_Purchasing_Suppliers_SupplierCategoryID");

            entity.HasIndex(e => e.SupplierName, "UQ_Purchasing_Suppliers_SupplierName").IsUnique();

            entity.Property(e => e.SupplierId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[SupplierID])")
                .HasColumnName("SupplierID");
            entity.Property(e => e.AlternateContactPersonId).HasColumnName("AlternateContactPersonID");
            entity.Property(e => e.BankAccountBranch).HasMaxLength(50);
            entity.Property(e => e.BankAccountCode).HasMaxLength(20);
            entity.Property(e => e.BankAccountName).HasMaxLength(50);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(20);
            entity.Property(e => e.BankInternationalCode).HasMaxLength(20);
            entity.Property(e => e.DeliveryAddressLine1).HasMaxLength(60);
            entity.Property(e => e.DeliveryAddressLine2).HasMaxLength(60);
            entity.Property(e => e.DeliveryCityId).HasColumnName("DeliveryCityID");
            entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");
            entity.Property(e => e.DeliveryPostalCode).HasMaxLength(10);
            entity.Property(e => e.FaxNumber).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PostalAddressLine1).HasMaxLength(60);
            entity.Property(e => e.PostalAddressLine2).HasMaxLength(60);
            entity.Property(e => e.PostalCityId).HasColumnName("PostalCityID");
            entity.Property(e => e.PostalPostalCode).HasMaxLength(10);
            entity.Property(e => e.PrimaryContactPersonId).HasColumnName("PrimaryContactPersonID");
            entity.Property(e => e.SupplierCategoryId).HasColumnName("SupplierCategoryID");
            entity.Property(e => e.SupplierName).HasMaxLength(100);
            entity.Property(e => e.SupplierReference).HasMaxLength(20);
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(256)
                .HasColumnName("WebsiteURL");

            entity.HasOne(d => d.AlternateContactPerson).WithMany(p => p.SupplierAlternateContactPeople)
                .HasForeignKey(d => d.AlternateContactPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_Suppliers_AlternateContactPersonID_Application_People");

            entity.HasOne(d => d.DeliveryCity).WithMany(p => p.SupplierDeliveryCities)
                .HasForeignKey(d => d.DeliveryCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_Suppliers_DeliveryCityID_Application_Cities");

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.DeliveryMethodId)
                .HasConstraintName("FK_Purchasing_Suppliers_DeliveryMethodID_Application_DeliveryMethods");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.SupplierLastEditedByNavigations)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_Suppliers_Application_People");

            entity.HasOne(d => d.PostalCity).WithMany(p => p.SupplierPostalCities)
                .HasForeignKey(d => d.PostalCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_Suppliers_PostalCityID_Application_Cities");

            entity.HasOne(d => d.PrimaryContactPerson).WithMany(p => p.SupplierPrimaryContactPeople)
                .HasForeignKey(d => d.PrimaryContactPersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_Suppliers_PrimaryContactPersonID_Application_People");

            entity.HasOne(d => d.SupplierCategory).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.SupplierCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_Suppliers_SupplierCategoryID_Purchasing_SupplierCategories");
        });

        modelBuilder.Entity<Supplier1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Suppliers", "Website");

            entity.Property(e => e.AlternateContact).HasMaxLength(50);
            entity.Property(e => e.CityName).HasMaxLength(50);
            entity.Property(e => e.DeliveryMethod).HasMaxLength(50);
            entity.Property(e => e.FaxNumber).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PrimaryContact).HasMaxLength(50);
            entity.Property(e => e.SupplierCategoryName).HasMaxLength(50);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName).HasMaxLength(100);
            entity.Property(e => e.SupplierReference).HasMaxLength(20);
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(256)
                .HasColumnName("WebsiteURL");
        });

        modelBuilder.Entity<SupplierCategory>(entity =>
        {
            entity.HasKey(e => e.SupplierCategoryId).HasName("PK_Purchasing_SupplierCategories");

            entity
                .ToTable("SupplierCategories", "Purchasing")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("SupplierCategories_Archive", "Purchasing");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.SupplierCategoryName, "UQ_Purchasing_SupplierCategories_SupplierCategoryName").IsUnique();

            entity.Property(e => e.SupplierCategoryId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[SupplierCategoryID])")
                .HasColumnName("SupplierCategoryID");
            entity.Property(e => e.SupplierCategoryName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.SupplierCategories)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_SupplierCategories_Application_People");
        });

        modelBuilder.Entity<SupplierTransaction>(entity =>
        {
            entity.HasKey(e => e.SupplierTransactionId)
                .HasName("PK_Purchasing_SupplierTransactions")
                .IsClustered(false);

            entity.ToTable("SupplierTransactions", "Purchasing");

            entity.HasIndex(e => e.TransactionDate, "CX_Purchasing_SupplierTransactions").IsClustered();

            entity.HasIndex(e => new { e.TransactionDate, e.PaymentMethodId }, "FK_Purchasing_SupplierTransactions_PaymentMethodID");

            entity.HasIndex(e => new { e.TransactionDate, e.PurchaseOrderId }, "FK_Purchasing_SupplierTransactions_PurchaseOrderID");

            entity.HasIndex(e => new { e.TransactionDate, e.SupplierId }, "FK_Purchasing_SupplierTransactions_SupplierID");

            entity.HasIndex(e => new { e.TransactionDate, e.TransactionTypeId }, "FK_Purchasing_SupplierTransactions_TransactionTypeID");

            entity.HasIndex(e => new { e.TransactionDate, e.IsFinalized }, "IX_Purchasing_SupplierTransactions_IsFinalized");

            entity.Property(e => e.SupplierTransactionId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[TransactionID])")
                .HasColumnName("SupplierTransactionID");
            entity.Property(e => e.AmountExcludingTax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsFinalized).HasComputedColumnSql("(case when [FinalizationDate] IS NULL then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", true);
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.OutstandingBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.SupplierInvoiceNumber).HasMaxLength(20);
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.SupplierTransactions)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_SupplierTransactions_Application_People");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.SupplierTransactions)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_Purchasing_SupplierTransactions_PaymentMethodID_Application_PaymentMethods");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.SupplierTransactions)
                .HasForeignKey(d => d.PurchaseOrderId)
                .HasConstraintName("FK_Purchasing_SupplierTransactions_PurchaseOrderID_Purchasing_PurchaseOrders");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierTransactions)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_SupplierTransactions_SupplierID_Purchasing_Suppliers");

            entity.HasOne(d => d.TransactionType).WithMany(p => p.SupplierTransactions)
                .HasForeignKey(d => d.TransactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchasing_SupplierTransactions_TransactionTypeID_Application_TransactionTypes");
        });

        modelBuilder.Entity<SystemParameter>(entity =>
        {
            entity.HasKey(e => e.SystemParameterId).HasName("PK_Application_SystemParameters");

            entity.ToTable("SystemParameters", "Application");

            entity.HasIndex(e => e.DeliveryCityId, "FK_Application_SystemParameters_DeliveryCityID");

            entity.HasIndex(e => e.PostalCityId, "FK_Application_SystemParameters_PostalCityID");

            entity.Property(e => e.SystemParameterId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[SystemParameterID])")
                .HasColumnName("SystemParameterID");
            entity.Property(e => e.DeliveryAddressLine1).HasMaxLength(60);
            entity.Property(e => e.DeliveryAddressLine2).HasMaxLength(60);
            entity.Property(e => e.DeliveryCityId).HasColumnName("DeliveryCityID");
            entity.Property(e => e.DeliveryPostalCode).HasMaxLength(10);
            entity.Property(e => e.LastEditedWhen).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PostalAddressLine1).HasMaxLength(60);
            entity.Property(e => e.PostalAddressLine2).HasMaxLength(60);
            entity.Property(e => e.PostalCityId).HasColumnName("PostalCityID");
            entity.Property(e => e.PostalPostalCode).HasMaxLength(10);

            entity.HasOne(d => d.DeliveryCity).WithMany(p => p.SystemParameterDeliveryCities)
                .HasForeignKey(d => d.DeliveryCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_SystemParameters_DeliveryCityID_Application_Cities");

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.SystemParameters)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_SystemParameters_Application_People");

            entity.HasOne(d => d.PostalCity).WithMany(p => p.SystemParameterPostalCities)
                .HasForeignKey(d => d.PostalCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_SystemParameters_PostalCityID_Application_Cities");
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.TransactionTypeId).HasName("PK_Application_TransactionTypes");

            entity
                .ToTable("TransactionTypes", "Application")
                .ToTable(tb => tb.IsTemporal(ttb =>
                    {
                        ttb.UseHistoryTable("TransactionTypes_Archive", "Application");
                        ttb
                            .HasPeriodStart("ValidFrom")
                            .HasColumnName("ValidFrom");
                        ttb
                            .HasPeriodEnd("ValidTo")
                            .HasColumnName("ValidTo");
                    }));

            entity.HasIndex(e => e.TransactionTypeName, "UQ_Application_TransactionTypes_TransactionTypeName").IsUnique();

            entity.Property(e => e.TransactionTypeId)
                .HasDefaultValueSql("(NEXT VALUE FOR [Sequences].[TransactionTypeID])")
                .HasColumnName("TransactionTypeID");
            entity.Property(e => e.TransactionTypeName).HasMaxLength(50);

            entity.HasOne(d => d.LastEditedByNavigation).WithMany(p => p.TransactionTypes)
                .HasForeignKey(d => d.LastEditedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_TransactionTypes_Application_People");
        });

        modelBuilder.Entity<VehicleTemperature>(entity =>
        {
            entity.HasKey(e => e.VehicleTemperatureId)
                .HasName("PK_Warehouse_VehicleTemperatures")
                .IsClustered(false);

            entity
                .ToTable("VehicleTemperatures", "Warehouse")
                .IsMemoryOptimized();

            entity.Property(e => e.VehicleTemperatureId).HasColumnName("VehicleTemperatureID");
            entity.Property(e => e.FullSensorData)
                .HasMaxLength(1000)
                .UseCollation("Latin1_General_CI_AS");
            entity.Property(e => e.Temperature).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehicleRegistration)
                .HasMaxLength(20)
                .UseCollation("Latin1_General_CI_AS");
        });

        modelBuilder.Entity<VehicleTemperature1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VehicleTemperatures", "Website");

            entity.Property(e => e.FullSensorData).HasMaxLength(1000);
            entity.Property(e => e.Temperature).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehicleRegistration).HasMaxLength(20);
            entity.Property(e => e.VehicleTemperatureId)
                .ValueGeneratedOnAdd()
                .HasColumnName("VehicleTemperatureID");
        });
        modelBuilder.HasSequence<int>("BuyingGroupID", "Sequences").StartsAt(3L);
        modelBuilder.HasSequence<int>("CityID", "Sequences").StartsAt(38187L);
        modelBuilder.HasSequence<int>("ColorID", "Sequences").StartsAt(37L);
        modelBuilder.HasSequence<int>("CountryID", "Sequences").StartsAt(242L);
        modelBuilder.HasSequence<int>("CustomerCategoryID", "Sequences").StartsAt(9L);
        modelBuilder.HasSequence<int>("CustomerID", "Sequences").StartsAt(1062L);
        modelBuilder.HasSequence<int>("DeliveryMethodID", "Sequences").StartsAt(11L);
        modelBuilder.HasSequence<int>("InvoiceID", "Sequences").StartsAt(70511L);
        modelBuilder.HasSequence<int>("InvoiceLineID", "Sequences").StartsAt(228266L);
        modelBuilder.HasSequence<int>("OrderID", "Sequences").StartsAt(73596L);
        modelBuilder.HasSequence<int>("OrderLineID", "Sequences").StartsAt(231413L);
        modelBuilder.HasSequence<int>("PackageTypeID", "Sequences").StartsAt(15L);
        modelBuilder.HasSequence<int>("PaymentMethodID", "Sequences").StartsAt(5L);
        modelBuilder.HasSequence<int>("PersonID", "Sequences").StartsAt(3262L);
        modelBuilder.HasSequence<int>("PurchaseOrderID", "Sequences").StartsAt(2075L);
        modelBuilder.HasSequence<int>("PurchaseOrderLineID", "Sequences").StartsAt(8368L);
        modelBuilder.HasSequence<int>("SpecialDealID", "Sequences").StartsAt(3L);
        modelBuilder.HasSequence<int>("StateProvinceID", "Sequences").StartsAt(54L);
        modelBuilder.HasSequence<int>("StockGroupID", "Sequences").StartsAt(11L);
        modelBuilder.HasSequence<int>("StockItemID", "Sequences").StartsAt(228L);
        modelBuilder.HasSequence<int>("StockItemStockGroupID", "Sequences").StartsAt(443L);
        modelBuilder.HasSequence<int>("SupplierCategoryID", "Sequences").StartsAt(10L);
        modelBuilder.HasSequence<int>("SupplierID", "Sequences").StartsAt(14L);
        modelBuilder.HasSequence<int>("SystemParameterID", "Sequences").StartsAt(2L);
        modelBuilder.HasSequence<int>("TransactionID", "Sequences").StartsAt(336253L);
        modelBuilder.HasSequence<int>("TransactionTypeID", "Sequences").StartsAt(14L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
