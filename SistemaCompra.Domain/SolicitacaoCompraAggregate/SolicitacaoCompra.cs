﻿using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaCompra.Domain.SolicitacaoCompraAggregate
{
    public class SolicitacaoCompra : Entity
    {
        public UsuarioSolicitante UsuarioSolicitante { get; private set; }
        public NomeFornecedor NomeFornecedor { get; private set; }
        public List<Item> Itens { get; private set; }
        public DateTime Data { get; private set; }
        public Money TotalGeral { get; private set; }
        public Situacao Situacao { get; private set; }
		public CondicaoPagamento CondicaoPagamento { get; private set; }

		private SolicitacaoCompra() { }

        public SolicitacaoCompra(string usuarioSolicitante, string nomeFornecedor)
        {
            Id = Guid.NewGuid();
            UsuarioSolicitante = new UsuarioSolicitante(usuarioSolicitante);
            NomeFornecedor = new NomeFornecedor(nomeFornecedor);
            Data = DateTime.Now;
            Situacao = Situacao.Solicitado;
            Itens = new List<Item>();
            TotalGeral = new Money(0m);
        }

        public void AdicionarItem(Produto produto, int qtde)
        {
			Itens = new List<Item>();
			Itens.Add(new Item(produto, qtde));
            
        }

        public void RegistrarCompra(IEnumerable<Item> itens)
        {
			if (!itens.Any()) throw new BusinessRuleException("Total do itens de compra deve ser maior que 0!");

			foreach (var item in itens)
			{
				TotalGeral = TotalGeral.Add(item.Subtotal);
			}

			if (TotalGeral.Value > 50000)
				CondicaoPagamento = new CondicaoPagamento(30);

			AddEvent(new CompraRegistradaEvent(Id, itens, TotalGeral.Value));

		}

	}
}
