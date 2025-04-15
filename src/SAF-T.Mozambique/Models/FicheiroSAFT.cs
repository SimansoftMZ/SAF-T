using SAFT.Core.Models;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
//using System.Xml.Serialization;

namespace SAFT.Mozambique.Models
{
    //[XmlRoot("FicheiroSAFT")]
    [JsonSerializable(typeof(FicheiroSAFT))]
    public partial class FicheiroSaftContext : JsonSerializerContext
    {
        public static FicheiroSaftContext Custom =>
            new(
                new()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            );
    }

    public record class FicheiroSAFT
    {
        public List<DocumentoFacturacao> DocumentosFacturacao { get; init; } = [];
    }

    public record class DocumentoFacturacao
    {
        public string TipoDocumentoId { get; init; } = string.Empty;
        public CategoriaDocumento Categoria { get; init; } = CategoriaDocumento.Factura;
        public string NumeroDocumento { get; init; } = string.Empty;
        public string Id
        {
            get
            {
                string tipoDoc = Categoria switch
                {
                    CategoriaDocumento.Factura => "FT",
                    CategoriaDocumento.VendaDinheiro => "VD",
                    CategoriaDocumento.NotaCredito => "NC",
                    CategoriaDocumento.NotaDebito => "ND",
                    CategoriaDocumento.Cotacao => "CT",
                    _ => string.Empty,
                };

                return $"{tipoDoc} {string.Concat("000", TipoDocumentoId)[^3..]}/{NumeroDocumento}";
            }
        }
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

    public record class DocumentoFacturacaoArtigo
    {
        public Artigo Artigo { get; init; } = new();
        public decimal Quantidade { get; init; }
        public decimal PrecoTotalComImpostos { get; init; }
        public decimal ValorDesconto { get; init; }
        public decimal PercentagemDesconto { get => ValorDesconto / PrecoTotalComImpostos * 100; }
        public decimal ValorImpostos { get => Artigo.Impostos.Sum(i => i.Valor + (PrecoTotalComImpostos / (1m + i.Percentagem * 0.01m) * i.Percentagem * 0.01m)); }
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
        public bool? Servico { get; init; }
        public List<Imposto> Impostos { get; init; } = [];
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
