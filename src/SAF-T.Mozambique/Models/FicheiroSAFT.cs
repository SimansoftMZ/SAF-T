using System.Text.Json;
using System.Text.Json.Serialization;

namespace SAFT.Mozambique.Models
{
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
        public string VersaoFicheiro { get; init; } = "1.0";
        public ConteudoFicheiroSaft TipoConteudo { get; init; } = ConteudoFicheiroSaft.Vendas;

        public int AnoFiscal { get; init; } = DateTime.Now.Year;
        public DateTime DataInicial { get; init; }
        public DateTime DataFinal { get; init; }
        public string Moeda { get; init; } = "MZN";
        public DateTime? DataCriacao { get; init; } = DateTime.Now;

        public string? ComentariosCabecario { get; init; }

        public Empresa Empresa { get; init; } = new();
        public FabricanteSoftware FabricanteSoftware { get; init; } = new();

        public decimal TotalDebito { get => DocumentosFacturacao.Where(w => w.TotalGeral < 0).Sum(s => -s.TotalGeral); }
        public decimal TotalCredito { get => DocumentosFacturacao.Where(w => w.TotalGeral > 0).Sum(s => s.TotalGeral); }

        public List<DocumentoFacturacao> DocumentosFacturacao { get; init; } = [];
    }

    public record class Empresa
    {
        public string Nome { get; init; } = string.Empty;
        public string NomeComercial { get; init; } = string.Empty;
        public string EstabelecimentoId { get; init; } = "Global";
        public string NUIT { get; init; } = string.Empty;
        public string EACCode { get; init; } = string.Empty;
        public string Endereco1 { get; init; } = string.Empty;
        public string Endereco2 { get; init; } = string.Empty;
        public string EdificioNumero { get; init; } = string.Empty;
        public string Cidade { get; init; } = string.Empty;
        public string Distrito { get; init; } = string.Empty;
        public string Provincia { get; init; } = string.Empty;
        public string Pais { get; init; } = "MZ";
        public string CodigoPostal { get; init; } = string.Empty;
        public string Telefone { get; init; } = string.Empty;
        public string Fax { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Website { get; init; } = string.Empty;
    }

    public record class FabricanteSoftware
    {
        public string Nome { get; init; } = string.Empty;
        public string NUIT { get; init; } = string.Empty;
        public string SoftwareProdutoId { get; init; } = string.Empty;

        public string SoftwareProdutoIdCompleto { get => $"{Nome}/{SoftwareProdutoId}"; }

        public string SoftwareProdutoVersao { get; init; } = string.Empty;
        public string SoftwareNumeroCertificacao { get; init; } = string.Empty;
    }

    public record class DocumentoFacturacao
    {
        public string TipoDocumentoId { get; init; } = string.Empty;
        public CategoriaDocumento Categoria { get; init; } = CategoriaDocumento.Factura;
        public string NumeroDocumento { get; init; } = string.Empty;
        
        public string TipoDocumento
        {
            get => Categoria switch
            {
                CategoriaDocumento.Factura => "FT",
                CategoriaDocumento.VendaDinheiro => "FR",
                CategoriaDocumento.NotaCredito => "NC",
                CategoriaDocumento.NotaDebito => "ND",
                CategoriaDocumento.Cotacao => "CT",
                _ => string.Empty,
            };
        }

        public string Id { get => $"{TipoDocumento} {string.Concat("000", TipoDocumentoId)[^3..]}/{NumeroDocumento}"; }

        public bool? ControlaAssinatura { get; init; }
        public string? Assinatura { get; init; }

        public DateTime DataHora { get; init; }
        public int PeriodoMes { get => DataEmissao.Month; }
        public DateTime DataEmissao { get; init; }
        public OrigemDocumento OrigemDocumento { get; init; } = OrigemDocumento.ProduzidoNoSoftware;
        public string OrigemDocumentoId { get => OrigemDocumento switch
        {
            OrigemDocumento.ProduzidoNoSoftware => "P",
            OrigemDocumento.IntegradoEProduzidoNoutroSoftware => "I",
            OrigemDocumento.RecuperacaoOuEmissaoManual => "M",
            _ => string.Empty,
        };
        }
        public string OperadorEmissao { get; init; } = string.Empty;
        public string? CodigoEAC { get; init; }
        public string? DocumentoContabilisticoId { get; init; } = string.Empty;
        //public string ClienteId { get; init; } = "Consumidor Final";
        public Cliente Cliente { get; init; } = new()
        {
            Nome = "Consumidor Final",
            EConsumidorFinal = true
        };
        public List<DocumentoFacturacaoArtigo> Artigos { get; init; } = [];

        public decimal TotalBase { get => Artigos.Sum(s => s.PrecoTotalSemImpostos); }
        public decimal TotalDesconto { get => Artigos.Sum(s => s.ValorDesconto); }
        public decimal TotalImpostos { get => Artigos.Sum(s => s.ValorImpostos); }
        public decimal TotalGeral { get => TotalBase - TotalDesconto + TotalImpostos; }
    }

    public record class DocumentoFacturacaoArtigo
    {
        public Artigo Artigo { get; init; } = new();
        public string? ArtigoDescricao { get; init; }
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
        public string? UnidadeContagem { get; init; }
        public decimal? PrecoUnitario { get; init; }
        public bool? IVAIncluso { get; init; }
        public bool? Servico { get; init; }
        public List<Imposto> Impostos { get; init; } = [];
    }

    public record class Cliente
    {
        public string Id { get; init; } = string.Empty;
        public string? Nome { get; init; } = string.Empty;
        public string? NUIT { get; init; } = string.Empty;
        public string? Endereco { get; init; } = string.Empty;
        public string? Cidade { get; init; } = string.Empty;
        public string? Telefone { get; init; } = string.Empty;
        public string? Email { get; init; } = string.Empty;
        public string Pais { get; init; } = "MZ";
        public string? CodigoPostal { get; init; } = string.Empty;
        public string? PlanoContaCorrente { get; init; } = string.Empty;
        public bool EConsumidorFinal { get; init; } = false;
    }

    public record class Operador
    {
        public string Id { get; init; } = string.Empty;
        public string? Nome { get; init; } = string.Empty;
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

    public enum ConteudoFicheiroSaft
    {
        Compras = 1,
        Vendas = 2,
        Transporte = 3,
        Contabilidade = 4,
        Inventario = 5,
        Autofacturacao = 6
    }
}