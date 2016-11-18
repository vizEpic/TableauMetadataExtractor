using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableauDataExtractor.Blob;

namespace TableauDataExtractor.DAL
{
    class TableauContext : DbContext
    {
        private  string STG_SCHEMA = ConfigurationManager.AppSettings["stg_schema"];
        private string BLOB_TABLE = ConfigurationManager.AppSettings["blob_table"];

        public DbSet<TableauBlob> TableauBlobs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TableauBlob>().ToTable(BLOB_TABLE, STG_SCHEMA);
            modelBuilder.Entity<TableauBlob>().HasKey(i => i.Id).Property(i => i.Id).HasColumnName("source_id");
            modelBuilder.Entity<TableauBlob>().Property(i => i.RepositoryId).HasColumnName("repository_id");
            modelBuilder.Entity<TableauBlob>().Property(i => i.SiteId).HasColumnName("site_id");
            modelBuilder.Entity<TableauBlob>().Property(i => i.Data).HasColumnName("data");
            modelBuilder.Entity<TableauBlob>().Property(i => i.SourceType).HasColumnName("source_type");


            modelBuilder.Conventions.Remove<StoreGeneratedIdentityKeyConvention>();
        }
    
    }
}
