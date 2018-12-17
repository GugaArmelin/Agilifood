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

namespace AgiliFood.Controllers
{
    public class CardapioController : BaseController
    {
        private AgiliFoodContext db = new AgiliFoodContext();

        // GET: Cardapio
        public ActionResult Index()
        {
            var idUsuario = Convert.ToInt32(Session["UsuarioId"]);
            Fornecedor fornecedor = db.Fornecedores.Where(f => f.UsuarioId == idUsuario).FirstOrDefault();
            var cardapios = from p in db.Cardapios
                          where p.FornecedorId == fornecedor.Id
                          select p;
            return View(cardapios);
        }

        // GET: Cardapio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cardapio cardapio = db.Cardapios.Find(id);
            if (cardapio == null)
            {
                return HttpNotFound();
            }
            return View(cardapio);
        }

        // GET: Cardapio/Create
        public ActionResult Create()
        {
            var idUsuario = Convert.ToInt32(Session["UsuarioId"]);
            Fornecedor fornecedor = db.Fornecedores.Where(f => f.UsuarioId == idUsuario).FirstOrDefault();
            Cardapio cardapio = new Cardapio { FornecedorId = fornecedor.Id };
            return View(cardapio);
        }

        // POST: Cardapio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FornecedorId,Nome,Data,Item")] Cardapio cardapio)
        {
            var s1 = String.Format("{0}", Request.Form["Itens"]);
            cardapio.Item = s1;
            if (ModelState.IsValid)
            {
                db.Cardapios.Add(cardapio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cardapio);
        }

        // GET: Cardapio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cardapio cardapio = db.Cardapios.Find(id);
            if (cardapio == null)
            {
                return HttpNotFound();
            }
            return View(cardapio);
        }

        // POST: Cardapio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FornecedorId,Nome,Data")] Cardapio cardapio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cardapio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cardapio);
        }

        // GET: Cardapio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cardapio cardapio = db.Cardapios.Find(id);
            if (cardapio == null)
            {
                return HttpNotFound();
            }
            return View(cardapio);
        }

        // POST: Cardapio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cardapio cardapio = db.Cardapios.Find(id);
            db.Cardapios.Remove(cardapio);
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
    }
}
