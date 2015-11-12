using GeocachingExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeocachingExample.Data;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace GeocachingExample
{
    /// <summary>
    /// The Business layer.
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// Name of connection string.
        /// </summary>
        protected const string CONNECTION_STRING = "GeocachingConnectionString";

        /// <summary>
        /// Get an ordered list of objects representing the name and ID of stored Geocaches.
        /// </summary>
        /// <returns>A list of objects that contain the name and id of geocaches</returns>
        internal IEnumerable<GeocacheItem> GetGeocacheItemNamesList()
        {
            //Create an empty list to return. 
            IEnumerable<GeocacheItem> items = new List<GeocacheItem>();
            try
            {
                //connect to the data source
                using (DataContext context = new DataContext(CONNECTION_STRING))
                {
                    //use link to select the name and id of the geocaches.
                    items = context.Geocache.Select(x => new GeocacheItem()
                    {
                        GeocacheID = x.GeocacheID,
                        Name = x.Name,
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
                }
            }
            catch (Exception ex)
            {
                //Any error logging handled here.
                //If we want to fail silently, do not re-throw the exception.
                //If we want to bubble to failure, throw the exception
                //throw ex;
                //For now, lets fail silently
            }
            return items;
        }

        /// <summary>
        /// Get the Geocache item with the matching ID from the database.
        /// </summary>
        /// <param name="id">The ID of the geocache object to get</param>
        /// <returns></returns>
        internal GeocacheItem GetGeocacheItem(int id)
        {
            //Get a holder for the return value.
            GeocacheItem item = null;
            try
            {
                //connect to the data source
                using (DataContext context = new DataContext(CONNECTION_STRING))
                {
                    //find the items that have a matching ID.  this data set will only have 1.
                    //select the properties of the item and create a new GeocacheItem.
                    //return the item or null if nothing was found.
                    item = context.Geocache
                        .Where(x => x.GeocacheID == id) 
                        .Select(x => new GeocacheItem()
                        {
                            GeocacheID = x.GeocacheID,
                            Name = x.Name,
                            Latitude = x.Latitude,
                            Longitude = x.Longitude
                        })
                        .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //Any error logging handled here.
                //If we want to fail silently, do not re-throw the exception.
                //If we want to bubble to failure, throw the exception
                //throw ex;
                //For now, lets fail silently
            }
            return item;
        }

        /// <summary>
        /// Add the Geocache item to the data base.  
        /// If successful the geocacheItem.GeocacheID will be set to something other than 0.
        /// </summary>
        /// <param name="geocacheItem">A reference to a GeocacheItem.  Will set the GeocacheID property.</param>
        /// <returns>True if successful, otherwise false.</returns>
        internal bool StoreGeocacheItem(ref GeocacheItem geocacheItem)
        {
            //return value to signify if the method was successful or not.
            bool rcode = false;
            try
            {
                //reset the GeocacheID to 0
                if (geocacheItem.GeocacheID != 0)
                {
                    geocacheItem.GeocacheID = 0;
                }

                //connect to the data source
                using (DataContext context = new DataContext(CONNECTION_STRING))
                {
                    //Create a new data.Geocache item to be added to the record set.
                    //Use the values to be stored.
                    Geocache item = new Geocache()
                    {
                        Name = geocacheItem.Name,
                        Latitude = geocacheItem.Latitude,
                        Longitude = geocacheItem.Longitude
                    };

                    //check if a Geocache item already exists with the given name.
                    if (context.Geocache.FirstOrDefault(x => x.Name.Equals(item.Name)) != null)
                    {
                        //if so, throw an error.
                        throw new GeocacheException(HttpStatusCode.NotAcceptable, "Name already exists.");
                    }

                    //add the item to the dataset and same the dataset.
                    context.Geocache.Add(item);
                    context.SaveChanges();

                    //if successful, the item will have a new unique id.
                    geocacheItem.GeocacheID = item.GeocacheID;
                    if (geocacheItem.GeocacheID > 0)
                    {
                        rcode = true;
                    }
                }
            }
            catch (GeocacheException ex)
            {
                //Pass the exception on.
                throw ex;
            }
            catch (Exception ex)
            {
                //Any error logging handled here.
                //If we want to fail silently, do not re-throw the exception.
                //If we want to bubble to failure, throw the exception
                //throw ex;
                //For now, lets fail silently
            }

            return rcode;
        }

        /// <summary>
        /// Remove the Geocache item with the given ID from the database.
        /// </summary>
        /// <param name="id">The id of the item to remove from the database.</param>
        /// <returns>If the item was removed true, otherwise false.</returns>
        internal bool DeleteGeocacheItem(int id)
        {
            //The return value
            bool rcode = false;
            try
            {
                //connect to the data source
                using (DataContext context = new DataContext(CONNECTION_STRING))
                {
                    //Get the item from the database with the matching ID.
                    Geocache item = context.Geocache.FirstOrDefault(x => x.GeocacheID == id);
                    
                    //Check that there was an item returned.
                    if (item != null)
                    {
                        //Remove it from the dataset and save the dataset.
                        context.Geocache.Remove(item);
                        context.SaveChanges();
                    }

                    //Check that there is no longer any item with the given ID.
                    if (context.Geocache.FirstOrDefault(x => x.GeocacheID == id) == null)
                    {
                        rcode = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Any error logging handled here.
                //If we want to fail silently, do not re-throw the exception.
                //If we want to bubble to failure, throw the exception
                //throw ex;
                //For now, lets fail silently
            }
            return rcode;
        }
    }
}