using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TestDataTable
{
    class Program
    {
        static void Main(string[] args)
        {

            DataSet ds = new DataSet();
            DataTable tbl;
            //Create the Customers DataTable.
            tbl = ds.Tables.Add("Customers");
            tbl.Columns.Add("CustomerID", typeof(string));



            tbl.PrimaryKey = new DataColumn[] { tbl.Columns["CustomerID"] };
            try
            {
                tbl.Rows.Add("123");
                tbl.Rows.Add("123");
            }
            catch (ConstraintException e)
            {
                Console.WriteLine("found dupl");
            }

            //Create the Order Details DataTable.
            tbl = ds.Tables.Add("Order Details");
            tbl.Columns.Add("OrderID", typeof(int));
            tbl.Columns.Add("ProductID", typeof(int));
            tbl.PrimaryKey = new DataColumn[] {tbl.Columns["OrderID"],
            tbl.Columns["ProductID"]};



        }
    }
}
