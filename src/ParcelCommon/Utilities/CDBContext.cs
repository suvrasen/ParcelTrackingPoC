using ParcelCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ParcelCommon.Utilities
{
    public class CDBContext : DbContext
    {
        public CDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Parcel>().ToContainer("Parcels").HasPartitionKey(pk => pk.ParcelId).HasKey(k => k.Id);
            modelBuilder.Entity<Parcel>().Property(key => key.ParcelId).ToJsonProperty("ParcelId");
            modelBuilder.Entity<Parcel>().OwnsOne(prop => prop.SourceAddress, da =>
            {
                da.Property(p => p.Address1).ToJsonProperty("Address1");
                da.Property(p => p.Address2).ToJsonProperty("Address2");
                da.Property(p => p.City).ToJsonProperty("City");
                da.Property(p => p.Region).ToJsonProperty("Region");
                da.Property(p => p.PostalCode).ToJsonProperty("PostalCode");
                da.Property(p => p.Country).ToJsonProperty("Country");
            });
            modelBuilder.Entity<Parcel>().OwnsOne(prop => prop.DestinationAddess, da =>
            {
                da.Property(p => p.Address1).ToJsonProperty("Address1");
                da.Property(p => p.Address2).ToJsonProperty("Address2");
                da.Property(p => p.City).ToJsonProperty("City");
                da.Property(p => p.Region).ToJsonProperty("Region");               
                da.Property(p => p.PostalCode).ToJsonProperty("PostalCode");
                da.Property(p => p.Country).ToJsonProperty("Country");
            });
            modelBuilder.Entity<ParcelEvent>().ToContainer("Events").HasPartitionKey(pk => pk.TrackingId).HasKey(p => p.Id);
            modelBuilder.Entity<ParcelEvent>().Property(key => key.TrackingId).ToJsonProperty("TrackingId");
        }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<ParcelEvent> Events { get; set; }
    }
}
