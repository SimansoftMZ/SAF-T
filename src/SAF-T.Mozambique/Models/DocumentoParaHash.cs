using System.Security.Cryptography;
using System.Text;

namespace Simansoft.SAFT.Mozambique.Models
{
    public record class DocumentoParaHash
    {
        // public string NumeroCertificadoAplicacaoEmissora { get; init; } = string.Empty;
        // public string VersaoChave { get; init; } = string.Empty;


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
                CategoriaDocumento.FacturaSimplificada => "FS",
                CategoriaDocumento.Cotacao => "CT",
                _ => string.Empty,
            };
        }

        public string TipoDocumentoId { get; init; } = string.Empty;

        public string NumeroDocumento { get; init; } = string.Empty;

        public string DocumentoFacturacaoId { get => $"{CategoriaId} {TipoDocumentoId}/{NumeroDocumento}"; }

        public decimal DocumentoFacturacaoTotal { get; init; }
        public string? HashDocumentoAnterior { get; init; }

        public string DadosCompostosParaHash
        {
            get =>
                $"{DocumentoFacturacaoData:yyyy-MM-dd};{DocumentoFacturacaoDataRegisto:yyyy-MM-ddTHH:mm:ss};{DocumentoFacturacaoId};" +
                $"{DocumentoFacturacaoTotal:F2};{((DocumentoFacturacaoId != DefaultDocumentoFacturacaoId && !string.IsNullOrWhiteSpace(HashDocumentoAnterior))
                    ? HashDocumentoAnterior : string.Empty)}";
        }

        //private string GerarHash()
        //{
        //    return Convert.ToBase64String(Encoding.UTF8.GetBytes(DadosCompostosParaHash));
        //}
        
        //public string Assinar(RSA privateKey)
        //{
        //    byte[] dataBytes = Encoding.UTF8.GetBytes(DadosCompostosParaHash);

        //    byte[] signedBytes = privateKey.SignData(dataBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

        //    return Convert.ToBase64String(signedBytes);
        //}
    }
}