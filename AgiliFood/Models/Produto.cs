using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgiliFood.Models
{
    public class Produto
    {
        public int Id { get; set; }

        public int FornecedorId { get; set; }

        public string Tipo { get; set; }

        public string Nome { get; set; }

        public decimal Valor { get; set; }
    }
}