using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgiliFood.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Cnpj { get; set; }

        public string NomeResponsavel { get; set; }

        public string NomeFantasia { get; set; }

        public string RazaoSocial { get; set; }

        public bool Status { get; set; }


    }
}