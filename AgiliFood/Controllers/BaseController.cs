using AgiliFood.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgiliFood.Repositorios;

namespace AgiliFood.Controllers
{
    public class BaseController : Controller
    {
        [AutorizacaoDeAcesso]
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}