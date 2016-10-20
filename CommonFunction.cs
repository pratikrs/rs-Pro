public enum emInterestType
    {
        [Description("Buyer")]
        Buyer = 1,
        [Description("Seller")]
        Seller = 2,
        [Description("Buyer & Seller")]
        BuyerSeller = 3
    }
//===================================	
	using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RafCompare.Common
{
    public static class CommonFunction
    {
        /// <summary>
        /// Get Enum Custom attribute value
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>Custom attribute value</returns>
        public static string GetCustomAttributeValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            // Get the stringvalue attributes
            CustomAttribute[] attribs = fieldInfo.GetCustomAttributes(
                 typeof(CustomAttribute), false) as CustomAttribute[];
            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].CustomValue : null;
        }

        /// <summary>
        /// Get Enum Description
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// CopyObjectToVieModel(Source, Destination)
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Destination"></param>
        /// <returns></returns>
        public static Object CopyObjectToVieModel(Object Source, Object Destination)
        {
            Type sourceType = Source.GetType();
            Type destinationType = Destination.GetType();

            foreach (PropertyInfo sourceProperty in sourceType.GetProperties())
            {
                PropertyInfo destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty != null)
                {
                    destinationProperty.SetValue(Destination, sourceProperty.GetValue(Source, null), null);
                }
            }
            return Destination;
        }


        //public static List<T> ConvertToList<T>(object dt)
        //{
        //    try
        //    {
        //        var columnNames = dt.Columns.Cast<DataColumn>()
        //            .Select(c => c.ColumnName.ToLower())
        //            .ToList();

        //        var properties = typeof(T).GetProperties();

        //        return dt.AsEnumerable().Select(row =>
        //        {
        //            var objT = Activator.CreateInstance<T>();

        //            foreach (var pro in properties)
        //            {
        //                if (columnNames.Contains(pro.Name.ToLower()))
        //                    pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : row[pro.Name]);
        //            }

        //            return objT;
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        /// <summary>
        /// Adds range of items into collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null)
            {
                System.Diagnostics.Debug.WriteLine("Do extension metody AddRange byly poslany items == null");
                return;
            }
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Clears collection and adds range of items into it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void ClearAndAddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            collection.AddRange(items);
        }

        /// <summary>
        /// Strong-typed object cloning for objects that implement <see cref="ICloneable"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(this T obj) where T : ICloneable
        {
            return (T)(obj as ICloneable).Clone();
        }

        /*Converts List To DataTable*/
        /// <summary>
        /// Converts List To DataTable
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="data"></param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IList<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// Convert string Number into decimal. This will output “1243.50″ if passed “$1,240.50″.
        /// </summary>
        /// <param name="number">formatted Number</param>
        /// <returns>decimal : The unformatted decimal Number</returns>
        public static decimal UnformatNumber(this string number)
        {
            decimal formatedNumber = 0;
            if (!string.IsNullOrEmpty(number))
                Decimal.TryParse(Regex.Replace(number, @"[^0-9.-]", ""), out formatedNumber);

            return formatedNumber;
        }
    }
}
//================
model.lstInterestType = Enum.GetValues(typeof(emInterestType)).Cast<emInterestType>().Select(x => new SelectListItem()
            {
                Text = CommonFunction.GetEnumDescription(x),
                Value = CommonFunction.GetEnumDescription(x)
            }).ToList();