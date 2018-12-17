using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgiliFood.Models
{
    public class Cardapio
    {
        public int Id { get; set; }

        public int FornecedorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        public string Item { get; set; }
    }
}