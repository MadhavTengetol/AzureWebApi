﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyVaultWebApi.Data
{
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<FileAttributes> FileAttributes { get; set; }
        public DbSet<Acl> Acl { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<BusinessUnit> BusinessUnit { get; set; }
        public DbSet<ReadGroups> ReadGroups { get; set; }
        public DbSet<ReadUsers> ReadUsers { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        //optionsBuilder.UseSqlServer("Server =.; Database = UKHO_DB; Integrated Security = True");
        //        optionsBuilder.UseSqlServer("Server=tcp:ukhodbserver.database.windows.net,1433;Initial Catalog=UKHO_DB;Persist Security Info=False;User ID=dba;Password=Pass@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //    }
        //}
    }
}
