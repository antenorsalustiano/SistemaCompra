using MediatR;
using SistemaCompra.Domain.ProdutoAggregate;
using SistemaCompra.Domain.SolicitacaoCompraAggregate;
using SistemaCompra.Infra.Data.UoW;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Solicita = SistemaCompra.Domain.SolicitacaoCompraAggregate;
using Prod = SistemaCompra.Domain.ProdutoAggregate;

namespace SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra
{
    public class RegistrarCompraCommandHandler: CommandHandler, IRequestHandler<RegistrarCompraCommand,bool>
    {
        private readonly ISolicitacaoCompraRepository _solicitacaoCompraRepository;
        public RegistrarCompraCommandHandler(ISolicitacaoCompraRepository solicitacaoCompraRepository, IUnitOfWork uow, IMediator mediator) : base(uow, mediator)
        {
            _solicitacaoCompraRepository = solicitacaoCompraRepository;
        }

        public Task<bool> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {            
            var produto = new Prod.Produto(request.NomeProduto, request.Descricao, request.Categoria.ToString(), request.Valor);
            produto.AtualizarPreco(request.Valor);

            var solicitacaoCompra = new Solicita.SolicitacaoCompra(request.Solicitante, request.Fornecedor);
            solicitacaoCompra.AdicionarItem(produto, request.Qtd);            
            solicitacaoCompra.RegistrarCompra(solicitacaoCompra.Itens);


            _solicitacaoCompraRepository.RegistrarCompra(solicitacaoCompra);

            Commit();
            PublishEvents(solicitacaoCompra.Events);

            return Task.FromResult(true);
        }
    }
}
