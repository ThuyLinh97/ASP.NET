using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
namespace LiteCommerce.DataLayers.SqlServer
{
    public class ProductDAL : IProductDAL
    {
        private string connectionString;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Product data)
        {
            int productId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products
                                          (
	                                          ProductName,
                                              CategoryID,
	                                          QuantityPerUnit,
	                                          UnitPrice,
	                                          Descriptions,
	                                          PhotoPath,
                                              SupplierID
                                          )
                                          VALUES
                                          (
	                                          @ProductName,
                                              @CategoryID,
	                                          @QuantityPerUnit,
	                                          @UnitPrice,
	                                          @Descriptions,
	                                          @PhotoPath,
                                              @SupplierID
                                          );
                                          SELECT @@IDENTITY;";
                //cho biết id vừa sinh ra :Identity-trả về 1 dòng 1 cột
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);            
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                productId = Convert.ToInt32(cmd.ExecuteScalar());
               

                connection.Close();
            }

            return productId;
        }
        /// <summary>
        /// đếm số dòng tìm kiếm đc
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>

        public int Count(string searchValue, int categoryId, int supplierId)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"select COUNT (*) 
                                    from	Products 
	                                where ((@searchValue = N'') or (ProductName like @searchValue))  and (@categoryId=N'' or CategoryID = @categoryId) and (@supplierId=N'' or SupplierID = @supplierId)
                                               ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                cmd.Parameters.AddWithValue("@supplierId", supplierId);

                count = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            //TODO: dem so luong trang
            return count;
        }

        public int Delete(int[] productIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products
                                            WHERE(ProductID = @productId)
                                              AND(ProductID NOT IN(SELECT ProductID FROM OrderDetails))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@productId", SqlDbType.Int);
                foreach (int productId in productIDs)
                {
                    cmd.Parameters["@productId"].Value = productId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }

                connection.Close();
            }
            return countDeleted;
        }

        public Product Get(int productID)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
             
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT Products.*
                                    FROM  Products
                                    WHERE ProductID = @productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@productID", productID);
               
                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        
                        data = new Product()
                        {
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            //UnitPrice = Convert.ToDecimal(Convert.ToString(dbReader["UnitPrice"]).Replace(",", ".").Trim(), new CultureInfo("en-US")),
                            //UnitPrice =  Convert.ToDecimal(Convert.ToString(dbReader["UnitPrice"]), new CultureInfo("en-US")),
                            UnitPrice = Convert.ToDecimal(Convert.ToString(dbReader["UnitPrice"])),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            CategoryID=Convert.ToInt32(dbReader["CategoryID"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"])
                           
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public List<Product> List(int page, int pageSize, string searchValue, int categoryId, int supplierId)
        {
            List<Product> data = new List<Product>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM 
                                        (
	                                        SELECT ROW_NUMBER() OVER(ORDER BY ProductName) AS RowNumber,Products.*
	                                        FROM dbo.Products
	                                        WHERE ((@searchValue = N'') OR (ProductName LIKE @searchValue)) and (@categoryId=N'' or CategoryID = @categoryId) and (@supplierId=N'' or SupplierID = @supplierId)
                                        )AS t  WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND (@page * @pageSize)
                                        ORDER BY t.RowNumber";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.Parameters.AddWithValue("@supplierId", supplierId);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                                CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                                QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                                UnitPrice = Convert.ToDecimal(dbReader["UnitPrice"]),
                                Descriptions = Convert.ToString(dbReader["Descriptions"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                           SET 
                                              ProductName = @ProductName
                                              ,SupplierID = @SupplierID
                                              ,CategoryID = @CategoryID
                                              ,Descriptions = @Descriptions
                                              ,QuantityPerUnit = @QuantityPerUnit
                                              ,UnitPrice = @UnitPrice
                                              ,PhotoPath = @PhotoPath
                                              
                                          WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
               
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
