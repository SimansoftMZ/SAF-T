using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT.Mozambique.Models
{
    public record class FicheiroSAFT
    {

    }

    public record class DocumentoFacturao
    {
        public string TipoDocumentoId { get; init; } = string.Empty;
        public CategoriaDocumento Categoria { get; init; } = CategoriaDocumento.Factura;
        public bool? ControlaAssinatura { get; init; }
        public string? Assinatura { get; init; }

        public DateTime DataHora { get; init; }
        public int PeriodoMes { get => DataEmissao.Month; }
        public DateTime DataEmissao { get; init; }
        public OrigemDocumento OrigemDocumento { get; init; } = OrigemDocumento.ProduzidoNoSoftware;
        public string OperadorEmissao { get; init; } = string.Empty;
        public string? CodigoEAC { get; init; }
        public string? DocumentoContabilisticoId { get; init; } = string.Empty;
        public string ClienteId { get; init; } = "Consumidor Final";
        public List<DocumentoFacturacaoArtigo> Artigos { get; init; } = [];

        public decimal TotalBase { get => Artigos.Sum(s => s.PrecoTotalSemImpostos); }
        public decimal TotalDesconto { get => Artigos.Sum(s => s.ValorDesconto); }
        public decimal TotalImpostos { get => Artigos.Sum(s => s.ValorImpostos); }
        public decimal TotalGeral { get => TotalBase - TotalDesconto + TotalImpostos; }
    }

    //public record class DocumentoTotais
    //{
    //    public decimal TotalBase { get; init; }
    //    public decimal TotalDesconto { get; init; }
    //    public decimal TotalImpostos { get; init; }
    //    public decimal TotalGeral { get => TotalBase - TotalDesconto + TotalImpostos; }
    //}

    public record class DocumentoFacturacaoArtigo
    {
        public Artigo Artigo { get; init; } = new();
        public decimal Quantidade { get; init; }
        public List<Imposto> Impostos { get; init; } = [];
        public decimal PrecoTotalComImpostos { get; init; }
        public decimal ValorDesconto { get; init; }
        public decimal PercentagemDesconto { get => ValorDesconto / PrecoTotalComImpostos * 100; }
        public decimal ValorImpostos { get => Impostos.Sum(i => i.Valor); }
        public decimal PrecoTotalSemImpostos { get => PrecoTotalComImpostos - ValorImpostos; }
        public decimal PrecoUnitarioComImpostos { get => (PrecoTotalComImpostos + ValorDesconto) / Quantidade; }
        public decimal PrecoUnitarioSemImpostos { get => PrecoUnitarioComImpostos - (ValorImpostos / Quantidade); }

    }

    public record class Imposto
    {
        public string? Codigo { get; init; }
        public string? Descricao { get; init; }
        public decimal Percentagem { get; init; }
        public decimal Valor { get; init; }
    }

    public record class Artigo
    {
        public string UniqueId { get; init; } = string.Empty;
        public string? ArtigoId { get; init; }
        public string Descricao { get; init; } = string.Empty;
        public decimal? PrecoUnitario { get; init; }
        public bool? IVAIncluso { get; init; }
    }

    public enum CategoriaDocumento
    {
        Outro = 0,
        Factura = 1,
        VendaDinheiro = 2,
        NotaCredito = 3,
        NotaDebito = 4,
        Cotacao = 5,
    }

    public enum OrigemDocumento
    {
        ProduzidoNoSoftware = 1,
        IntegradoEProduzidoNoutroSoftware = 2,
        RecuperacaoOuEmissaoManual = 3
    }
}
