using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AgiliFood.DAL;
using AgiliFood.Models;
using AgiliFood.ViewModel;

namespace AgiliFood.Controllers
{
    public class PedidoController : Controller
    {
        private AgiliFoodContext db = new AgiliFoodContext();

        // GET: Pedido
        public ActionResult Index()
        {
            var idUsuario = Convert.ToInt32(Session["UsuarioId"]);
            var pedidos = from p in db.Pedidos
                          where p.UsuarioId == idUsuario
                          select p;

            return View(pedidos);
        }

        // GET: Pedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedido/Create
        public ActionResult Create(int? fornecedorId, string Itens)
        {
            var idUsuario = Convert.ToInt32(Session["UsuarioId"]);

            var usuario = db.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();
            

            var model = from c in db.Cardapios
                        where DbFunctions.TruncateTime(c.Data) == DbFunctions.TruncateTime(DateTime.Now)
                        join f in db.Fornecedores.Where(f => f.Status == true) on c.FornecedorId equals f.Id 
                        select new CardapioViewModel
                        {
                            FornecedorId = f.Id,
                            NomeFornecedor = f.NomeFantasia,
                            ItensCardapio = c.Item
                        };

            if (fornecedorId != null)
            {
                var fornecedor = db.Fornecedores.Where(f => f.Id == fornecedorId).FirstOrDefault();
                var produtos = db.Produtos.Where(p => p.FornecedorId == fornecedorId).OrderBy(p => p.Tipo);
                Pedido pedido = new Pedido
                {
                    UsuarioId = idUsuario,
                    UsuarioNome = usuario.Nome,
                    FornecedorId = fornecedorId ?? 0,
                    FornecedorNome = fornecedor.NomeFantasia,
                    Data = DateTime.Now.Date,
                    Cardapios = model,
                    Produtos = produtos,
                    ItensCardapioSelecionado = Itens,
                    Observacao = string.Empty
                };
                return View(pedido);
            }
            else { 
            
                Pedido pedido = new Pedido
                {
                    UsuarioId = idUsuario,
                    FornecedorId = fornecedorId ?? 0,
                    Data = DateTime.Now.Date,
                    Cardapios = model,
                    Observacao = string.Empty
                };
                return View(pedido);
            }
        }

        // GET: Pedido/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UsuarioId,Data,Valor,Observacao")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult PreencheItens(int fornecedorId, string Itens)
        {

            var itens = Itens;

            return RedirectToAction("Create", new {FornecedorId = fornecedorId, Itens = itens });
        }

        [HttpGet]
        public ActionResult AumentaValor(int fornecedorId, DateTime data, IEnumerable<CardapioViewModel> cardapios, string itensSelecionados, double valorPedido, double? valor, string observacao, string itensP)
        {
            var idUsuario = Convert.ToInt32(Session["UsuarioId"]);
            var usuario = db.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();
            var produtos = db.Produtos.Where(p => p.FornecedorId == fornecedorId).OrderBy(p => p.Tipo);
            var fornecedor = db.Fornecedores.Where(f => f.Id == fornecedorId).FirstOrDefault();            
            valorPedido += valor ?? 0;
            Pedido pedido = new Pedido
            {
                UsuarioId = idUsuario,
                UsuarioNome = usuario.Nome,
                FornecedorId = fornecedorId,
                FornecedorNome = fornecedor.NomeFantasia,
                Data = DateTime.Now.Date,
                Cardapios = cardapios,
                Produtos = produtos,
                Valor = valorPedido,
                ItensCardapioSelecionado = itensSelecionados,
                Observacao = observacao
            };

            if (Session["ItensPedidos"] == null || Session["ItensPedidos"] == string.Empty)
            {
                Session["ItensPedidos"] = itensP;
                pedido.ItensPedidos = itensP;
            }
            else
            {
                Session["ItensPedidos"] = (string)Session["ItensPedidos"] + ", " + itensP;
                pedido.ItensPedidos = (string)Session["ItensPedidos"];
            }

            return View("Create",pedido);
        }

        public ActionResult GravaPedido(int fornecedorId, DateTime data, IEnumerable<CardapioViewModel> cardapios, string itensSelecionados, double valorPedido, string observacao)
        {
            var idUsuario = Convert.ToInt32(Session["UsuarioId"]);
            var usuario = db.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();
            var produtos = db.Produtos.Where(p => p.FornecedorId == fornecedorId).OrderBy(p => p.Tipo);
            var fornecedor = db.Fornecedores.Where(f => f.Id == fornecedorId).FirstOrDefault();
            Pedido pedido = new Pedido
            {
                UsuarioId = idUsuario,
                UsuarioNome = usuario.Nome,
                FornecedorId = fornecedorId,
                FornecedorNome = fornecedor.NomeFantasia,
                Data = DateTime.Now.Date,
                Cardapios = cardapios,
                Produtos = produtos,
                Valor = valorPedido,
                ItensCardapioSelecionado = itensSelecionados,
                ItensPedidos = (string)Session["ItensPedidos"],
                Observacao = observacao
            };
            if (pedido != null)
            {
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                Session["ItensPedidos"] = string.Empty;
                return RedirectToAction("Index");
            }

            return View("Create", pedido);
        }
    }
}
