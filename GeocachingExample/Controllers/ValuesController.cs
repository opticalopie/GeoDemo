using GeocachingExample.Filters;
using GeocachingExample.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeocachingExample.Controllers
{
    [HandleUncheckedException]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<GeocacheItem> Get()
        {
            //Get and return a list of geocache items.
            return new Repository().GetGeocacheItemNamesList();
        }

        // GET api/values/5
        public GeocacheItem Get(int id)
        {
            //Get a geocache item with the given id.
            return new Repository().GetGeocacheItem(id);
        }

        // POST api/values
        [ModelValidation]
        public HttpResponseMessage Post([FromBody]GeocacheItem geocacheItem)
        {
            try
            {
                //Add the item to storage.
                if (new Repository().StoreGeocacheItem(ref geocacheItem))
                {
                    //If successful, return a status ok message and the ID of Geocache item.
                    return Request.CreateResponse(HttpStatusCode.OK, geocacheItem.GeocacheID);
                }
                //The item was not stored in the database, return an error.
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Geocache Item Not Created.");
            }
            catch (GeocacheException ex)
            {
                //There was a specific error in the repository that would prevent adding to storage.
                return Request.CreateErrorResponse(ex.ResopnseCode, ex.Message);
            }
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            //Remove the item with the given id from storage.
            if (new Repository().DeleteGeocacheItem(id))
            {
                //If successful, return a status ok message.
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            //The item was not removed from storage, return an error message.
            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Geocache Item Not Deleted.");
        }
    }
}