﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASDWP.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ASDW_Entities : DbContext
    {
        public ASDW_Entities()
            : base("name=ASDW_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DbColumns> DbColumns { get; set; }
        public virtual DbSet<DbMapping> DbMapping { get; set; }
        public virtual DbSet<DbProviders> DbProviders { get; set; }
        public virtual DbSet<DbQuery> DbQuery { get; set; }
        public virtual DbSet<DbSource> DbSource { get; set; }
        public virtual DbSet<DbTables> DbTables { get; set; }
        public virtual DbSet<DbTableStg> DbTableStg { get; set; }
        public virtual DbSet<DbSchedule> DbSchedule { get; set; }
    }
}