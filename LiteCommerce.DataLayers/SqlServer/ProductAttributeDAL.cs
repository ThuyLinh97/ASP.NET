using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data.SqlClient;
using System.Data;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class ProductAttributeDAL : IProductAttribute
    {
       
        private string connectionString;
        
        public ProductAttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<ProductAttribute> List()
        {
            List<ProductAttribute> data = new List<ProductAttribute>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //tao lenh thuc thi truy van du lieu
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select * from ProductAttributes";

                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;

                // thuc thi truy van tra ve bien dbReader
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dbReader.Read())
                    {
                        data.Add(new ProductAttribute()
                        {
                            AttributeID = Convert.ToInt32(dbReader["AttributeID"]),
                            AttributeName = Convert.ToString(dbReader["AttributeName"]),
                            AttributeValues = Convert.ToString(dbReader["AttributeValues"]),
                            DisplayOrder = Convert.ToInt32(dbReader["DisplayOrder"]),
                            ProductID = Convert.ToInt32(dbReader["DisplayOrder"])
                        });
                    }
                }
                connection.Close();
            }
            return data;
        }
    }
  
}
