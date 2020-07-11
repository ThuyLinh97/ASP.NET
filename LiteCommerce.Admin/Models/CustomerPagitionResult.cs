﻿using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Models
{
    public class CustomerPagitionResult : PaginationResult
    {
        public string Country { get; set; }
        public List<Customer> Data { get; set; }
    }
}