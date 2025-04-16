using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SAFT.Core.Models
{
    [JsonSerializable(typeof(AuditFile))]
    [JsonSerializable(typeof(List<AuditFile>))]
    public partial class AuditFileContext : JsonSerializerContext
    {
        public static AuditFileContext Custom =>
            new(
                new()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            );
    }

    // Representa o ficheiro SAF-T
    [XmlRoot(Namespace = "http://www.mz.gov.tax/saft")] //Endereço fictício
    public record AuditFile
    {
        public Header? Header { get; init; }
        public MasterFiles? MasterFiles { get; init; }
        public SourceDocuments? SourceDocuments { get; init; }
    }

    // Dados gerais da empresa e do ficheiro
    public record Header
    {
        public string? AuditFileVersion { get; init; }
        public string? CompanyID { get; init; }
        public string? TaxRegistrationNumber { get; init; }
        public string? FileContentType { get; init; } // Ex.: "F" para facturação (vendas)
        public string? CompanyName { get; init; }
        public string? BusinessName { get; init; }
        public CompanyAddress? CompanyAddress { get; init; }
        public int? FiscalYear { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public string? CurrencyCode { get; init; }
        public DateTime? DateCreated { get; init; }
        public string? TaxEntity { get; init; }
        public string? ProductCompanyTaxID { get; init; }
        public string? SoftwareCertificateNumber { get; init; }
        public string? ProductID { get; init; }
        public string? ProductVersion { get; init; }
        public string? HeaderComment { get; init; }
        public string? Telephone { get; init; }
        public string? Fax { get; init; }
        public string? Email { get; init; }
        public string? Website { get; init; }
    }

    // MasterFiles
    public record MasterFiles
    {
        public List<Customer> Customers { get; init; } = [];
        public List<Product> Products { get; init; } = [];
        public List<TaxTableEntry> TaxTable { get; init; } = [];
    }

    public record Customer
    {
        public string? CustomerID { get; init; }
        public string? AccountID { get; init; }
        public string? CustomerTaxID { get; init; }
        public string? CompanyName { get; init; }
        public CustomerAddress? BillingAddress { get; init; }
        public CustomerAddress? ShipToAddress { get; init; }
        public string? Telephone { get; init; }
        public string? Fax { get; init; }
        public string? Email { get; init; }
        public string? SelfBillingIndicator { get; init; }
    }

    public record class Product
    {
        public string? ProductType { get; init; }
        public string? ProductCode { get; init; }
        public string? ProductGroup { get; init; }
        public string? ProductDescription { get; init; }
        public string? ProductNumberCode { get; init; }
    }

    public record class CustomerAddress
    {
        public string? AddressDetail { get; init; }
        public string? City { get; init; }
        public string? PostalCode { get; init; }
        public string Country { get; init; } = "MZ";
    }

    public record class TaxTableEntry
    {
        public string TaxType { get; init; } = string.Empty;
        public string TaxCountryRegion { get; init; } = "MZ";
        public string? TaxCode { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public decimal? TaxPercentage { get; init; }
        public decimal? TaxAmount { get; init; }
    }

    // Endereço da empresa
    public record CompanyAddress
    {
        public string? BuildingNumber { get; init; }
        public string? StreetName { get; init; }
        public string? AddressDetail { get; init; }
        public string? City { get; init; }
        public string? PostalCode { get; init; }
        public string? Province { get; init; }
        public string? Country { get; init; } // Deve ser "MZ"
    }

    // Seção de documentos: neste exemplo, apenas os documentos de vendas
    public record SourceDocuments
    {
        public SalesInvoices? SalesInvoices { get; init; }
    }

    // Agrupamento dos documentos de venda (faturas)
    public record SalesInvoices
    {
        public int? NumberOfEntries { get; init; }
        public decimal? TotalDebit { get; init; }
        public decimal? TotalCredit { get; init; }
        public List<Invoice>? Invoices { get; init; } = [];
    }

    // Representa um documento de venda (fatura)
    public record Invoice
    {
        public string? InvoiceNo { get; init; }  // Ex.: "FT 001/0001"
        public string? InvoiceType { get; init; }
        public DocumentStatus? DocumentStatus { get; init; } // Estado e detalhes da fatura
        public int? Hash { get; init; }
        public string? HashControl { get; init; }
        public int? Period { get; init; }  // Ex.: 1 (primeiro mês)
        public DateOnly? InvoiceDate { get; init; }
        public string? InvoiceStatus { get; init; }
        public DateTime? InvoiceStatusDate { get; init; }
        public string? SourceBilling { get; init; }
        public SpecialRegimes? SpecialRegimes { get; init; } // Regimes especiais, se houver
        public string? SourceID { get; init; }
        public string? EACCode { get; init; }  // Código CAE (se aplicável)
        public DateTime? SystemEntryDate { get; init; }
        public string? TransactionID { get; init; }
        public string? CustomerID { get; init; }
        public List<InvoiceLine>? Lines { get; init; } = [];
        public DocumentTotals? DocumentTotals { get; init; }
    }

    // Estado do documento de venda
    public record DocumentStatus
    {
        public string? InvoiceStatus { get; init; }  // Ex.: "N" = Normal, "A" = Anulado
        public DateTime? InvoiceStatusDate { get; init; }
        public string? SourceID { get; init; }
        public string? SourceBilling { get; init; }  // Ex.: "P" para documentos produzidos pelo software
    }

    // Regimes especiais aplicáveis à fatura
    public record SpecialRegimes
    {
        public int? SelfBillingIndicator { get; init; }
        public int? CashVATSchemeIndicator { get; init; }
        public int? ThirdPartiesBillingIndicator { get; init; }
    }

    // Linha (item) da fatura
    public record InvoiceLine
    {
        public int? LineNumber { get; init; }
        public string? ProductCode { get; init; }
        public string? ProductDescription { get; init; }
        public decimal? Quantity { get; init; }
        public string? UnitOfMeasure { get; init; }
        public decimal? UnitPrice { get; init; }
        public decimal? TaxBase { get; init; }
        public decimal? TaxAmount { get; init; }
        public decimal? DebitAmount { get; init; }
        public decimal? CreditAmount { get; init; }
        public DateTime? TaxPointDate { get; init; }
        public string? Description { get; init; }
    }

    // Totais do documento de venda
    public record DocumentTotals
    {
        public decimal? TaxPayable { get; init; }
        public decimal? NetTotal { get; init; }
        public decimal? GrossTotal { get; init; }
    }
}
