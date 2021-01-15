using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using targil_mesakem.Models;

namespace targil_mesakem.Models.DAL
{
    public class DBServices
    {
        //static List<Businesses> businessesList;

        //public void Insert(Businesses business)
        //{
        //    if (businessesList == null)
        //        businessesList = new List<Businesses>();
        //    businessesList.Add(business);
        //}

        //public List<Businesses> Read()
        //{
        //    return businessesList;
        //}
        public SqlDataAdapter da;
        public DataTable dt;
        private string a;

        public DBServices()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        //--------------------------------------------------------------------------------------------------
        // 
        //--------------------------------------------------------------------------------------------------
        public int Insert(Businesses business)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to connect to DB", ex);
            }

            String cStr = BuildInsertCommand(business);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to add to favorites, Try again!", ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        public int InsertResHigh(Businesses business, List<Highlight> hlist)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to connect to DB", ex);
            }

            String cStr = BuildInsertCommandResHigh(business, hlist);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to add to favorites, Try again!", ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //--------------------------------------------------------------------
        // Build the Insert command String
        //--------------------------------------------------------------------
        private String BuildInsertCommand(Businesses business)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string
            a = business.Name.Replace("'", "''");
            sb.AppendFormat("Values( '{0}','{1}','" + a + "','{2}','{3}','{4}','{5}','{6}','{7}')", business.Id,
                business.Photo_url, business.Rating, business.Category, business.Price_range,
                business.Address, business.Phones, business.Url);

            String prefix = "INSERT INTO Restaurants_2021 " + "([id], [photo_url],[name],[rating],[category],[price_range],[address],[phones],[url])";

            command = prefix + sb.ToString();

            return command;
        }
        private String BuildInsertCommandResHigh(Businesses business, List<Highlight> hlist)
        {
            String command = "";


            // use a string builder to create the dynamic string
            for (int i = 0; i < hlist.Count; i++)
            {
                for (int j = 0; j < business.Highlights.Length; j++)
                {
                    if (business.Highlights[j] == hlist[i].Name)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Values( '{0}','{1}','{2}')", business.Id, hlist[i].Id, hlist[i].Name);
                        String prefix = "INSERT INTO ResHigh_2021 " + "([id_res], [id_highlight],[name_highlight])";

                        command = prefix + sb.ToString();
                    }
                }
            }




            return command;
        }
        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }
        public int Insert(Customers customer)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to connect to DB", ex);
            }

            String cStr = BuildInsertCommand(customer);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to add a new customer, Try again!", ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //--------------------------------------------------------------------
        // Build the Insert command String
        //--------------------------------------------------------------------
        private String BuildInsertCommand(Customers customer)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string

            sb.AppendFormat("Values( '{0}','{1}','{2}','{3}','{4}','{5}')", customer.Fname, customer.Lname, customer.Email,
                customer.Phone, customer.Password, customer.Image);

            String prefix = "INSERT INTO Customers_2021 " + "([fname],[lname],[email],[phone],[password],[image])";

            command = prefix + sb.ToString();

            return command;
        }

        //---------------------------------------------------------------------------------
        // Read from the DB into a list - dataReader
        //---------------------------------------------------------------------------------
        public List<Businesses> getfavorites()
        {

            SqlConnection con = null;
            List<Businesses> bList = new List<Businesses>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Restaurants_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row

                    Businesses b = new Businesses();

                    b.Id = Convert.ToInt32(dr["id"]);
                    b.Photo_url = (string)dr["photo_url"];
                    b.Name = Convert.ToString(dr["name"]);
                    b.Rating = Convert.ToDouble(dr["rating"]);
                    b.Category = (string)dr["category"];
                    b.Price_range = Convert.ToInt32(dr["price_range"]);
                    b.Address = (string)dr["address"];
                    b.Phones = (string)dr["phones"];
                    b.Url = (string)dr["url"];


                    bList.Add(b);

                }

                return bList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public List<Customers> getcustomers()
        {

            SqlConnection con = null;
            List<Customers> cList = new List<Customers>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Customers_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row

                    Customers c = new Customers();

                    c.Fname = Convert.ToString(dr["fname"]);
                    c.Email = Convert.ToString(dr["email"]);
                    c.Password = Convert.ToString(dr["password"]);


                    cList.Add(c);

                }

                return cList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public List<Highlight> Gethighlights()
        {

            SqlConnection con = null;
            List<Highlight> hList = new List<Highlight>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Highlights_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row

                    Highlight h = new Highlight();

                    h.Id = Convert.ToInt32(dr["id"]);
                    h.Name = Convert.ToString(dr["name"]);

                    hList.Add(h);

                }

                return hList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }


        public List<Businesses> getall()
        {

            SqlConnection con = null;
            List<Businesses> bList = new List<Businesses>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Restaurants_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row

                    Businesses b = new Businesses();
                    b.Id = Convert.ToInt32(dr["id"]);
                    b.Name = Convert.ToString(dr["name"]);
                    bList.Add(b);
                }

                return bList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Campaign> getallcamp()
        {

            SqlConnection con = null;
            List<Campaign> cList = new List<Campaign>();

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM Campaing_2021";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row

                    Campaign c = new Campaign();
                    c.Id = Convert.ToInt32(dr["RestaurantID"]);
                    cList.Add(c);
                }

                return cList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public int Insert(Campaign campaing)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to connect to DB", ex);
            }

            String cStr = BuildInsertCommand(campaing);      // helper method to build the insert string

            cmd = CreateCommand(cStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                throw new Exception("You didnt succeed to add a new campaign, Try again!", ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        private String BuildInsertCommand(Campaign campaign)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            // use a string builder to create the dynamic string

            sb.AppendFormat("Values( '{0}','{1}','{2}','{3}','{4}','{5}')", campaign.Id, campaign.Investment,
                campaign.Income, campaign.View, campaign.Knock, campaign.Status);

            String prefix = "INSERT INTO Campaing_2021 " + "([RestaurantID],[Investment],[Income],[Show],[Knock],[Active])";

            command = prefix + sb.ToString();

            return command;
        }

        public DBServices getCampaignDT()
        {

            SqlConnection con = null;

            try
            {
                // connect
                con = connect("DBConnectionString");

                // create a dataadaptor
                da = new SqlDataAdapter("select * from Campaing_2021", con);

                // automatic build the commands
                SqlCommandBuilder builder = new SqlCommandBuilder(da);

                // create a DataSet
                DataSet ds = new DataSet();


                // Fill the Dataset
                da.Fill(ds);

                // keep the table in a field
                dt = ds.Tables[0];
            }

            catch (Exception)
            {
                // write to log
            }

            finally
            {
                if (con != null)
                    con.Close();
            }

            return this;


        }
        public void Update()
        {
            da.Update(dt);
        }
    }
}