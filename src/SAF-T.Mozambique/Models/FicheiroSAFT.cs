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
