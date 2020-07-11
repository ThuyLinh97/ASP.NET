using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class ShipperPagitionResult: PaginationResult
    {
        /// <summary>
        /// Danh sách Shipper
        /// </summary>
        public List<Shipper> Data { get; set; }
    }
}