using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using targil_mesakem.Models.DAL;

namespace targil_mesakem.Models
{
    public class Campaign
    {
        int id;
        double investment;
        double income;
        int view;
        int knock;
        bool status;

        public Campaign(int id, double investment, double income, int view, int knock, bool status)
        {
            Id = id;
            Investment = investment;
            Income = income;
            View = view;
            Knock = knock;
            Status = status;
        }

        public int Id { get => id; set => id = value; }
        public double Investment { get => investment; set => investment = value; }
        public double Income { get => income; set => income = value; }
        public int View { get => view; set => view = value; }
        public int Knock { get => knock; set => knock = value; }
        public bool Status { get => status; set => status = value; }

        public Campaign() { }

        public void Insert()
        {
            DBServices dbs = new DBServices();
            dbs.Insert(this);
        }

        public List<Campaign> ReadAll()
        {
            DBServices dbs = new DBServices();
            List<Campaign> c = dbs.getallcamp();
            return c;
        }

        public List<Campaign> NonActive()
        {

            List<Campaign> cList = new List<Campaign>();
            DBServices dbs = new DBServices();
            dbs = dbs.getCampaignDT();
            dbs.dt = NonActiveCamp(dbs.dt);
            dbs.Update();

            foreach (DataRow dr in dbs.dt.Rows)
            {
                Campaign c = new Campaign();
                c.Investment = Convert.ToDouble(dr["Investment"]);
                c.Income = Convert.ToDouble(dr["Income"]);
                c.View = Convert.ToInt32(dr["Show"]);
                c.Knock = Convert.ToInt32(dr["Knock"]);
                c.Status = Convert.ToBoolean(dr["Active"]);
                cList.Add(c);
            }
            return cList;
        }

        private DataTable NonActiveCamp(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                double investment = Convert.ToDouble(dr["Investment"]);
                double income = Convert.ToDouble(dr["Income"]);
                bool active = Convert.ToBoolean(dr["Active"]);
                double profit = investment - income;
                if (profit <= 0)
                {
                    active = false;
                }
                dr["Active"] = active;
            }
            return dt;
        }

    }
}