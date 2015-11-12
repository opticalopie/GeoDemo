using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeocachingExample.Models
{
    public class GeocacheItem
    {
        public int? GeocacheID { get; set; }
        [MaxLength(50, 
            ErrorMessage="Name is to long.  Length of 50 max.")]
        [RegularExpression("^[\\w\\d\\s]+$",
            ErrorMessage = "Can only contain letters, numbers and spaces")]
        [Required(AllowEmptyStrings=false,
            ErrorMessage = "A value is required.")]
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }


        public static bool operator ==(GeocacheItem x, GeocacheItem y)
        {
            return (x.Name.Equals(y.Name) &&
                x.GeocacheID == y.GeocacheID &&
                x.Latitude == y.Latitude &&
                x.Longitude == y.Longitude);
        }

        public static bool operator !=(GeocacheItem x, GeocacheItem y)
        {
            return !(x == y);
        }
    }
}