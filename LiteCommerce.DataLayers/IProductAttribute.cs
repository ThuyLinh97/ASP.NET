using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
   public interface IProductAttribute
    {
        /// <summary>
        /// Hiển thị danh sách Attribute
        /// </summary>
        /// <returns></returns>
        List<ProductAttribute> List();
    }
}
