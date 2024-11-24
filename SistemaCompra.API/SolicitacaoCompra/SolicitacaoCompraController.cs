using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SistemaCompra.Application.Produto.Command.RegistrarProduto;
using SistemaCompra.Application.SolicitacaoCompra.Command.RegistrarCompra;
using System;
using System.Threading.Tasks;

namespace SistemaCompra.API.SolicitacaoCompra
{
    public class SolicitacaoCompraController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SolicitacaoCompraController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("solicitacaoCompra/solicitar")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult SolicitarCompra([FromBody] RegistrarCompraCommand registrarCompraCommand)
        {
            try
            {
                _mediator.Send(registrarCompraCommand);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {Message = "Erro no servidor.", Detalhes = ex.Message});
            }
        }

		[HttpPost("registrar")]
		public async Task<IActionResult> RegistrarCompra([FromBody] RegistrarCompraCommand command)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var result = await _mediator.Send(command);

				if (result)
				{
					return Ok(new { Message = "Compra registrada com sucesso." });
				}

				return BadRequest(new { Message = "Falha ao registrar a compra." });
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
			catch (Exception ex)
			{
				// Para fins de debug, retorna o erro. Em produção, logue-o.
				return StatusCode(500, new { Message = "Erro interno no servidor.", Detalhes = ex.Message });
			}
		}

	}
}
