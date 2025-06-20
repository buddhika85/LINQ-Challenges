﻿using System;
using System.Collections.Generic;

namespace LINQ_Challeges.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string FullName { get; set; } = null!;

    public string PreferredName { get; set; } = null!;

    public string SearchName { get; set; } = null!;

    public bool IsPermittedToLogon { get; set; }

    public string? LogonName { get; set; }

    public bool IsExternalLogonProvider { get; set; }

    public byte[]? HashedPassword { get; set; }

    public bool IsSystemUser { get; set; }

    public bool IsEmployee { get; set; }

    public bool IsSalesperson { get; set; }

    public string? UserPreferences { get; set; }

    public string? PhoneNumber { get; set; }

    public string? FaxNumber { get; set; }

    public string? EmailAddress { get; set; }

    public byte[]? Photo { get; set; }

    public string? CustomFields { get; set; }

    public string? OtherLanguages { get; set; }

    public int LastEditedBy { get; set; }

    public virtual ICollection<BuyingGroup> BuyingGroups { get; set; } = new List<BuyingGroup>();

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual ICollection<Color> Colors { get; set; } = new List<Color>();

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<Customer> CustomerAlternateContactPeople { get; set; } = new List<Customer>();

    public virtual ICollection<CustomerCategory> CustomerCategories { get; set; } = new List<CustomerCategory>();

    public virtual ICollection<Customer> CustomerLastEditedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerPrimaryContactPeople { get; set; } = new List<Customer>();

    public virtual ICollection<CustomerTransaction> CustomerTransactions { get; set; } = new List<CustomerTransaction>();

    public virtual ICollection<DeliveryMethod> DeliveryMethods { get; set; } = new List<DeliveryMethod>();

    public virtual ICollection<Person> InverseLastEditedByNavigation { get; set; } = new List<Person>();

    public virtual ICollection<Invoice> InvoiceAccountsPeople { get; set; } = new List<Invoice>();

    public virtual ICollection<Invoice> InvoiceContactPeople { get; set; } = new List<Invoice>();

    public virtual ICollection<Invoice> InvoiceLastEditedByNavigations { get; set; } = new List<Invoice>();

    public virtual ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();

    public virtual ICollection<Invoice> InvoicePackedByPeople { get; set; } = new List<Invoice>();

    public virtual ICollection<Invoice> InvoiceSalespersonPeople { get; set; } = new List<Invoice>();

    public virtual Person LastEditedByNavigation { get; set; } = null!;

    public virtual ICollection<Order> OrderContactPeople { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderLastEditedByNavigations { get; set; } = new List<Order>();

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    public virtual ICollection<Order> OrderPickedByPeople { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSalespersonPeople { get; set; } = new List<Order>();

    public virtual ICollection<PackageType> PackageTypes { get; set; } = new List<PackageType>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual ICollection<PurchaseOrder> PurchaseOrderContactPeople { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<PurchaseOrder> PurchaseOrderLastEditedByNavigations { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; } = new List<PurchaseOrderLine>();

    public virtual ICollection<SpecialDeal> SpecialDeals { get; set; } = new List<SpecialDeal>();

    public virtual ICollection<StateProvince> StateProvinces { get; set; } = new List<StateProvince>();

    public virtual ICollection<StockGroup> StockGroups { get; set; } = new List<StockGroup>();

    public virtual ICollection<StockItemHolding> StockItemHoldings { get; set; } = new List<StockItemHolding>();

    public virtual ICollection<StockItemStockGroup> StockItemStockGroups { get; set; } = new List<StockItemStockGroup>();

    public virtual ICollection<StockItemTransaction> StockItemTransactions { get; set; } = new List<StockItemTransaction>();

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();

    public virtual ICollection<Supplier> SupplierAlternateContactPeople { get; set; } = new List<Supplier>();

    public virtual ICollection<SupplierCategory> SupplierCategories { get; set; } = new List<SupplierCategory>();

    public virtual ICollection<Supplier> SupplierLastEditedByNavigations { get; set; } = new List<Supplier>();

    public virtual ICollection<Supplier> SupplierPrimaryContactPeople { get; set; } = new List<Supplier>();

    public virtual ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>();

    public virtual ICollection<SystemParameter> SystemParameters { get; set; } = new List<SystemParameter>();

    public virtual ICollection<TransactionType> TransactionTypes { get; set; } = new List<TransactionType>();
}
