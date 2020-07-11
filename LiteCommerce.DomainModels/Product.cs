﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// Quản lý sản phẩm
    /// </summary>
    public class Product
    {
        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public string Descriptions { get; set; }
        public string PhotoPath { get; set; }

        //[ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        //[ForeignKey("SupplierID")]
        public virtual Supplier Supplier { get; set; }


    }
}
