using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeocachingExample.Data
{
    /// <summary>
    /// Data context that does not get rebuilt when something in the edmx changes.
    /// </summary>
    public class DataContext : GeocachingEntities1
    {
        public DataContext(string connctionString)
            : base (connctionString)
        {

        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Geocache>().Property(a => a.Latitude).HasPrecision(18, 9);
            modelBuilder.Entity<Geocache>().Property(a => a.Longitude).HasPrecision(18, 9);
        }
    }
}