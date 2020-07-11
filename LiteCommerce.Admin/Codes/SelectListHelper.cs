using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public static class SelectListHelper
    {
        //public static List<SelectListItem> Countries()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    list.Add(new SelectListItem() { Value = "USA", Text = "United States" });
        //    list.Add(new SelectListItem() { Value = "VietNam", Text = "Việt Nam" });
        //    list.Add(new SelectListItem() { Value = "UK", Text = "English" });
        //    list.Add(new SelectListItem() { Value = "China", Text = "trungquoc" });
        //    return list;
        //}
        public static List<SelectListItem> Countries (bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "", Text = "---All Country---" });
            }
            foreach (var country in CountryBLL.ListOfCountry())
            {
                list.Add(new SelectListItem()
                {
                    Value=country.CountryName,
                    Text = country.CountryName
                });
            }
            return list;
        }
        public static List<SelectListItem> Categories (bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "0", Text = "---All Category---" });
            }
           foreach( var category in CatalogBLL.ListOfCategory())
            {
                list.Add(new SelectListItem()
                {
                    Value = category.CategoryID.ToString(),
                    Text = category.CategoryName
                });
            }
            return list;
        }
        /// <summary>
        /// Get list Supplier
        /// </summary>
        /// <param name="allowSelectAll"></param>
        /// <returns></returns>
        public static List<SelectListItem> Suppliers(bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "0", Text = "---All Supplier---" });
            }
            foreach (var supplier in CatalogBLL.ListOfSuppliers())
            {
                list.Add(new SelectListItem()
                {
                    Value = supplier.SupplierID.ToString(),
                    Text = supplier.CompanyName
                });
            }
            return list;
        }
        public static List<SelectListItem> Employees(bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "0", Text = "---All Employee---" });
            }
            foreach (var order in OrderBLL.ListOfEmployees())
            {
                list.Add(new SelectListItem()
                {
                    Value = order.EmployeeID.ToString(),
                    Text = order.FirstName+ " "+order.LastName
                });
            }
            return list;
        }
        public static List<SelectListItem> Customer(bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "", Text = "---All Customer---" });
            }
            foreach (var customer in OrderBLL.ListOfCustomers())
            {
                list.Add(new SelectListItem()
                {
                    Value = customer.CustomerID.ToString(),
                    Text = customer.CompanyName
                });
            }
            return list;
        }
        public static List<SelectListItem> Shipper(bool allowSelectAll = true)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            if (allowSelectAll)
            {
                list.Add(new SelectListItem() { Value = "", Text = "---All Shipper---" });
            }
            foreach (var shipper in OrderBLL.ListOfShipper())
            {
                list.Add(new SelectListItem()
                {
                    Value = shipper.ShipperID.ToString(),
                    Text = shipper.CompanyName
                });
            }
            return list;
        }
    }
}