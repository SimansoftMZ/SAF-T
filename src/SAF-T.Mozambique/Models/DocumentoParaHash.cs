namespace Simansoft.SAFT.Mozambique.Models
{
    public record class DocumentoParaHash
    {
        public string NumeroCertificadoAplicacaoEmissora { get; init; } = string.Empty;
        public string VersaoChave { get; init; } = string.Empty;


        public DateTime DocumentoFacturacaoData { get; init; }
        public DateTime DocumentoFacturacaoDataRegisto { get; init; }

        public CategoriaDocumento Categoria { get; init; } = CategoriaDocumento.Factura;
        public string CategoriaId
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

        public string TipoDocumentoId { get; init; } = string.Empty;
        
        public string NumeroDocumento { get; init; } = string.Empty;

        public string DocumentoFacturacaoId { get => $"{CategoriaId} {string.Concat("000", TipoDocumentoId)[^3..]}/{NumeroDocumento}"; }

        public decimal DocumentoFacturacaoTotal { get; init; }
        public string? HashDocumentoAnterior { get; init; }

        public string DadosCompostosParaHash
        {
            get =>
                $"{DocumentoFacturacaoData:yyyy-MM-dd};{DocumentoFacturacaoDataRegisto:yyyy-MM-ddTHH:mm:ss};{DocumentoFacturacaoId};" +
                $"{DocumentoFacturacaoTotal:F2};{HashDocumentoAnterior}";
        }
    }
}
