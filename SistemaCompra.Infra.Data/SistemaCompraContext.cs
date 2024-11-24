using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaCompra.Domain.Core;
using SistemaCompra.Domain.Core.Model;
using SistemaCompra.Infra.Data.Produto;
using System.Reflection;
using ProdutoAgg = SistemaCompra.Domain.ProdutoAggregate;
using SolicitacaoAgg = SistemaCompra.Domain.SolicitacaoCompraAggregate;

namespace SistemaCompra.Infra.Data
{
    public class SistemaCompraContext : DbContext
    {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

		public SistemaCompraContext(DbContextOptions options) : base(options) { }
		public DbSet<ProdutoAgg.Produto> Produtos { get; set; }
		public DbSet<SolicitacaoAgg.SolicitacaoCompra> SolicitacaoCompras { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			var seedProduto = new ProdutoAgg.Produto("Produto01", "Descricao01", "Madeira", 100);
			var seedMoney = new Money(100m);

			modelBuilder.Entity<ProdutoAgg.Produto>().HasData(new
			{
				Id = seedProduto.Id,
				Nome = seedProduto.Nome,
				Descricao = seedProduto.Descricao,
				Categoria = seedProduto.Categoria,
				Situacao = seedProduto.Situacao
			});

			modelBuilder.Entity<ProdutoAgg.Produto>()
				.OwnsOne(x => x.Preco, preco =>
				{
					preco.Property<decimal>("Value")
					.HasColumnType("decimal(18,2)")
					.HasColumnName("Preco");
				});

			modelBuilder.Entity<SolicitacaoAgg.SolicitacaoCompra>()
				.OwnsOne(x => x.TotalGeral, totalGeral =>
				{
					totalGeral.Property<decimal>("Value")
					.HasColumnType("decimal(18,2)")
					.HasColumnName("TotalGeral");
				});

			modelBuilder.Ignore<Event>();

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory)  
                .EnableSensitiveDataLogging()
                .UseSqlServer(@"Server=VAIOANTENOR\SQLEXPRESS;Database=SistemaCompraDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
