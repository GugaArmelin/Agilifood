using AgiliFood.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AgiliFood.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string UsuarioNome { get; set; }

        public int FornecedorId { get; set; }

        public string FornecedorNome { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        public double Valor { get; set; }

        public string ItensCardapioSelecionado { get; set; }

        public string Observacao { get; set; }

        public string ItensPedidos { get; set; }

        public virtual IEnumerable<CardapioViewModel> Cardapios { get; set; }

        public virtual IEnumerable<Produto> Produtos { get; set; }
    }
}