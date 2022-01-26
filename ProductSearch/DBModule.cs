using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSearch
{
    public class DBModule
    {
        private SqlConnection con;
        private SqlCommand cmd;
        private SqlDataAdapter da;

        public DBModule()
        {
            con = new SqlConnection();
            cmd = new SqlCommand();
            da = new SqlDataAdapter();
        }

        public void StartConnection()
        {
            String connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(connection);
            con.Open();
        }

        public DataTable FillDataTable()
        {
            StartConnection();
            DataTable dtCurrency = new DataTable();
            cmd = new SqlCommand("select ProductId, SKU, Title, Artist from Product", con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dtCurrency);
            con.Close();

            return dtCurrency;

        }

        public void UpdateDB(string id, string sku, string artist, string title)
        {
            StartConnection();
            cmd = new SqlCommand("UPDATE Product SET SKU = @sku, Title = @title, Artist = @artist, ModifiedDate = @md WHERE ProductId = @id", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@artist", artist);
            cmd.Parameters.AddWithValue("@md", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataTable SearchDB(string SKU, string artist, string title)
        {
            StartConnection();
            DataTable dtCurrency = new DataTable();
            cmd = new SqlCommand("SearchTable", con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (!title.Equals(string.Empty) && title != null)
            {
                cmd.Parameters.AddWithValue("@sTitle", SqlDbType.VarChar).Value = title;
            }
            if (!SKU.Equals(string.Empty) && SKU != null)
            {
                cmd.Parameters.AddWithValue("@sSKU", SqlDbType.VarChar).Value = SKU;
            }
            if (!artist.Equals(string.Empty) && artist != null)
            {
                cmd.Parameters.AddWithValue("@sArtist", SqlDbType.VarChar).Value = artist;
            }

            da = new SqlDataAdapter(cmd);
            da.Fill(dtCurrency);
            con.Close();

            return dtCurrency;
        }
    }
}
