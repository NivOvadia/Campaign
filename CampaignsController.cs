using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using targil_mesakem.Models;

namespace targil_mesakem.Controllers
{
    public class CampaignsController : ApiController
    {
        // GET api/<controller>
        public List<Businesses> Get()
        {
            Businesses business = new Businesses();
            List<Businesses> bList = business.ReadAll();
            return bList;
        }

        [HttpGet]
        [Route("api/Campaigns/camp")]
        public List<Campaign> GetCamp()
        {
            Campaign campaign = new Campaign();
            List<Campaign> cList = campaign.ReadAll();
            return cList;
        }

        [HttpGet]
        [Route("api/Campaigns/status")]
        public List<Campaign> GetStatusCamp()
        {
            Campaign campaign = new Campaign();
            return campaign.NonActive();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Campaign campaing)
        {
            try
            {
                campaing.Insert();
                return Request.CreateResponse(HttpStatusCode.Created, campaing);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}