using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;

namespace LiteCommerce.BusinessLayers
{
    public static class CountryBLL
    {
        public static void Initialize(string connectionString)
        {
            CountryDB = new DataLayers.SqlServer.CountryDAL(connectionString);

        }
        private static ICountryDAL CountryDB { get; set; }

        public static List<Country> ListOfCountry()
        {
            return CountryDB.List();
        }
        public static List<Country> ListOfCoutries(int page, int pageSize, string searchValue, out int rowCount)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 0)
                pageSize = 3;
            rowCount = CountryDB.Count(searchValue);
            return CountryDB.List(page, pageSize, searchValue);
        }
        /// <summary>
        /// lấy thông tin 1 Country dựa vào id
        /// </summary>
        /// <param name="CountryName"></param>
        /// <returns></returns>
        public static Country GetCountry(string countryName)
        {
            return CountryDB.Get(countryName);
        }
        /// <summary>
        /// Thêm 1 Country
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AddCountry(Country data)
        {
            return CountryDB.Add(data);
        }
        /// <summary>
        /// update 1 Country
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCountry(Country data,string countryName)
        {
            return CountryDB.Update(data,countryName);
        }
        /// <summary>
        /// xóa nhiều countryNames dựa vào id
        /// </summary>
        /// <param name="CountryIDs"></param>
        /// <returns></returns>
        public static int DeleteCountry(string[] countryNames)
        {
            return CountryDB.Delete(countryNames);
        }


    }
}
