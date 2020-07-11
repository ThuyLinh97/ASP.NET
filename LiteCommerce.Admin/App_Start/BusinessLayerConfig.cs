using LiteCommerce.BusinessLayers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin
{
    public class BusinessLayerConfig
    {
        public static void Init()
        {
            //lay chuoi khia bao ket noi
            string connectionString = ConfigurationManager.ConnectionStrings["LiteCommerce"].ConnectionString;
            //TODO: khoi tao các BLL khi cần sử dụng đến
            CatalogBLL.Initialize(connectionString);
            CountryBLL.Initialize(connectionString);
            UserAccountBLL.Initialize(connectionString);
            OrderBLL.Initialize(connectionString);
        }
    }
}