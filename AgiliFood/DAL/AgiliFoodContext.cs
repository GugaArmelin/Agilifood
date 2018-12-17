using AgiliFood.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AgiliFood.DAL
{
    public class AgiliFoodContext : DbContext
    {
        public AgiliFoodContext() : base("AgiliFoodContext")
        {
            Database.SetInitializer<AgiliFoodContext>(new CreateDatabaseIfNotExists<AgiliFoodContext>());
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Cardapio> Cardapios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }
}