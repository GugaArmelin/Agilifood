using AgiliFood.DAL;
using AgiliFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace AgiliFood.Repositorios
{
    public class RepositorioUsuario
    {
        public static bool AutenticarUsuario(string Email, string Senha)
        {
            var SenhaCriptografada = FormsAuthentication.HashPasswordForStoringInConfigFile(Senha, "sha1");
            try
            {
                using(AgiliFoodContext db = new AgiliFoodContext())
                {
                    var QueryAutenticaUsuarios = db.Usuarios.Where(x => x.Email == Email && x.Senha == Senha).SingleOrDefault();
                    if (QueryAutenticaUsuarios == null)
                    {
                        return false;
                    }
                    else
                    {
                        RepositorioCookies.RegistraCookieAutenticacao(QueryAutenticaUsuarios.Id);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Usuario RecuperaUsuarioPorID(int Id)
        {
            try
            {
                using (AgiliFoodContext db = new AgiliFoodContext())
                {
                    var usuario = db.Usuarios.Where(u => u.Id == Id).SingleOrDefault();
                    return usuario;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Usuario VerificaSeOUsuarioEstaLogado()
        {
            var usuario = HttpContext.Current.Request.Cookies["UserCookieAuthentication"];
            if (usuario == null)
            {
                return null;
            }
            else
            {
                int Id = Convert.ToInt32(RepositorioCriptografia.Descriptografar(usuario.Values["Id"]));

                var usuarioRetornado = RecuperaUsuarioPorID(Id);
                return usuarioRetornado;

            }
        }
    }
}