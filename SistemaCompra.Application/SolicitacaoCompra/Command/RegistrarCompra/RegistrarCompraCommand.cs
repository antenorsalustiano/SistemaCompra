using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommand : IRequest<bool>
    {
        public string Fornecedor { get; set; }
        public string Solicitante { get; set; }
        public string NomeProduto { get; set; }
        public string Descricao { get; set; }
        public Categoria Categoria { get; set; }
        public decimal Valor { get; set; }
        public int Qtd { get; set; }
    }
}
