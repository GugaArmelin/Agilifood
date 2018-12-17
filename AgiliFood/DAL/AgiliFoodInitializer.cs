using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AgiliFood.Models;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace AgiliFood.DAL
{
    public class AgiliFoodInitializer : DropCreateDatabaseIfModelChanges<AgiliFoodContext>
    {
        protected override void Seed(AgiliFoodContext context)
        {

            var usuario = new Usuario
            {

                Nome = "Admin",
                Email = "admin@admin.com",
                Senha = "admin",
                Cpf = "12345678901",
                Tipo = "Administrador"
            };




            context.Usuarios.Add(usuario);
            context.SaveChanges();

            

            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            base.Seed(context);
        }
    }
}