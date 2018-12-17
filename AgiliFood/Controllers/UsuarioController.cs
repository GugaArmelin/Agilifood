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
using AgiliFood.Repositorios;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;


namespace AgiliFood.Controllers
{
    public class UsuarioController : Controller
    {
        private AgiliFoodContext db = new AgiliFoodContext();


        // GET: Usuario
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());

        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);

            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            // ViewBag.TipoUsuario = new SelectList(db.TipoUsuarios.ToList(), "Id", "Nome");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Cpf,Email,Senha,Tipo,Telefone")] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    if (usuario.Tipo.Equals("Fornecedor"))
                    {
                        return RedirectToAction("Create", "Fornecedor", new { id = usuario.Id });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                }
                // ViewBag.TipoUsuario = new SelectList(db.TipoUsuarios.ToList(), "Id", "Nome");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Não foi possível salvar usuário.");
            }

            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Cpf,Email,Senha,Tipo,Telefone")] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    if (usuario.Tipo.Equals("Fornecedor"))
                    {
                        return RedirectToAction("Edit", "Fornecedor", new { id = usuario.Id });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }


                }

            }
            catch (DataException e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError("", "Não foi possível alterar usuário.");
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
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

        public ActionResult Login()
        {
            ViewBag.HideNavBar = true;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            var usr = db.Usuarios.Where(u => u.Email == usuario.Email && u.Senha == usuario.Senha).FirstOrDefault();
            
            if (RepositorioUsuario.AutenticarUsuario(usuario.Email, usuario.Senha))
            {
                Session["UsuarioId"] = usr.Id.ToString();
                Session["UsuarioNome"] = usr.Nome;          
                Session["UsuarioTipo"] = usr.Tipo;        
                Session["LoggedIn"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["LoggedIn"] = false;
                ModelState.AddModelError("", "Email ou Senha inválidos");
            }
            return View();
        }

        public ActionResult Logoff()
        {

            Session.Abandon();
            Session["LoggedIn"] = false;
            return RedirectToAction("Login");

        }

        [HttpGet]
        public JsonResult AutenticacaoDeUsuario(string email, string senha)
        {
            if (RepositorioUsuario.AutenticarUsuario(email, senha))
            {
                return Json(new
                {
                    OK = true,
                    Mensagem = "Usuário autenticado. Redirecionando..."

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    OK = false,
                    Mensagem = "Usuário não encontrado. Tente novamente."
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
