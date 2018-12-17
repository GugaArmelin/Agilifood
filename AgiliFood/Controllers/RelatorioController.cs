using AgiliFood.DAL;
using AgiliFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgiliFood.Controllers
{
    public class RelatorioController : Controller
    {
        private AgiliFoodContext db = new AgiliFoodContext();

        // GET: Relatorio
        public ActionResult Index(string nomeUsuario, DateTime? dataInicial, DateTime? dataFinal)
        {
            var pedidos = from p in db.Pedidos select p;


            if (dataInicial != null && dataFinal != null)
            {
                pedidos = pedidos.Where(p => p.Data >= dataInicial && p.Data <= dataFinal);
            }
            else
            {
                dataInicial = dataInicial != null ? dataInicial : DateTime.Now.AddYears(-1);
                dataFinal = dataFinal != null ? dataFinal : DateTime.Now;
                pedidos = pedidos.Where(f => f.Data >= dataInicial && f.Data <= dataFinal);
            }

            if (nomeUsuario != null && nomeUsuario != string.Empty)
            {
                pedidos = pedidos.Where(p => p.UsuarioNome.Contains(nomeUsuario));
            }

            var groupedBills = from p in pedidos
                               group p by p.UsuarioNome into p
                               select new Grupo<string, Pedido> { Key = p.Key, Values = p };

            return View(groupedBills.ToList());
        }
    }
}