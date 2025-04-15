using SAFT.Core.Interfaces;
using SAFT.Core.Models;
using SAFT.Mozambique.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SAFT.Mozambique.Generators
{
    public class MozambiqueSaftGenerator : ISaftGenerator<FicheiroSAFT>
    {
        public string GenerateJson(AuditFile auditFile)
        {
            return JsonSerializer.Serialize(auditFile, AuditFileContext.Custom.AuditFile);
        }

        public string GenerateXml(AuditFile auditFile)
        {
            try
            {
                var xml = RetornaXml(auditFile);

                return xml;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Falha na serialização SAF-T", ex);
            }
        }

        public ValidationResult Validate(FicheiroSAFT saftData)
        {
            throw new NotImplementedException();
        }

        public AuditFile ConverterParaSaft(FicheiroSAFT ficheiroSAFT)
        {
            AuditFile auditFile = new()
            {
                Header = new Header
                {
                    AuditFileVersion = ficheiroSAFT.VersaoFicheiro,
                    CompanyID = ficheiroSAFT.Empresa.NUIT,
                    TaxRegistrationNumber = ficheiroSAFT.Empresa.NUIT,
                    FileContentType = ficheiroSAFT.TipoConteudo switch
                    {
                        ConteudoFicheiroSaft.Vendas => "F",
                        ConteudoFicheiroSaft.Compras => "A",
                        ConteudoFicheiroSaft.Contabilidade => "C",
                        ConteudoFicheiroSaft.Inventario => "I",
                        ConteudoFicheiroSaft.Autofacturacao => "S",                        
                        ConteudoFicheiroSaft.Transporte => "T",
                        
                        _ => throw new ArgumentOutOfRangeException(nameof(ficheiroSAFT), nameof(ficheiroSAFT.TipoConteudo),
                                                                   "Tipo de conteúdo inválido.")
                    },
                    CompanyName = ficheiroSAFT.Empresa.Nome,
                    BusinessName = ficheiroSAFT.Empresa.NomeComercial,
                    CompanyAddress = new CompanyAddress
                    {
                        StreetName = ficheiroSAFT.Empresa.Endereco1,
                        AddressDetail = ficheiroSAFT.Empresa.Endereco2,
                        City = ficheiroSAFT.Empresa.Cidade,
                        Province = ficheiroSAFT.Empresa.Provincia,
                        PostalCode = ficheiroSAFT.Empresa.CodigoPostal,
                        Country = ficheiroSAFT.Empresa.Pais
                    },
                    FiscalYear = ficheiroSAFT.AnoFiscal,
                    StartDate = ficheiroSAFT.DataInicial,
                    EndDate = ficheiroSAFT.DataFinal,
                    CurrencyCode = ficheiroSAFT.Moeda,
                    DateCreated = ficheiroSAFT.DataCriacao,
                    TaxEntity = ficheiroSAFT.Empresa.EstabelecimentoId,

                    ProductCompanyTaxID = ficheiroSAFT.Empresa.NUIT,
                    SoftwareCertificateNumber = ficheiroSAFT.FabricanteSoftware.SoftwareNumeroCertificacao,
                    ProductID = ficheiroSAFT.FabricanteSoftware.SoftwareProdutoId,
                    ProductVersion = ficheiroSAFT.FabricanteSoftware.SoftwareProdutoVersao,
                    HeaderComment = ficheiroSAFT.ComentariosCabecario,
                    Telephone = ficheiroSAFT.Empresa.Telefone,
                    Fax = ficheiroSAFT.Empresa.Fax,
                    Email = ficheiroSAFT.Empresa.Email,
                    Website = ficheiroSAFT.Empresa.Website
                },
                SourceDocuments = new SourceDocuments
                {
                    SalesInvoices = new SalesInvoices()
                    {
                        NumberOfEntries = ficheiroSAFT.DocumentosFacturacao.Count,
                        TotalCredit = ficheiroSAFT.TotalCredito,
                        TotalDebit = ficheiroSAFT.TotalDebito,
                        Invoices = [.. ficheiroSAFT.DocumentosFacturacao.Select(doc => new Invoice
                        {
                            InvoiceNo = doc.Id,
                            InvoiceType = doc.TipoDocumento,
                            DocumentStatus = new DocumentStatus
                            {
                                InvoiceStatus = "N",
                                InvoiceStatusDate = doc.DataHora,
                                SourceID = doc.OperadorEmissao,
                                SourceBilling = doc.OrigemDocumentoId
                            },
                            Hash = (doc.ControlaAssinatura ?? false) ? 1 : 0,
                            HashControl = doc.Assinatura,
                            EACCode = doc.CodigoEAC,
                            Period = doc.PeriodoMes,
                            InvoiceDate = DateOnly.FromDateTime(doc.DataHora),
                            InvoiceStatus = "N",
                            InvoiceStatusDate = doc.DataHora,
                            SourceBilling = doc.OrigemDocumentoId,
                            SpecialRegimes = new SpecialRegimes
                            {
                                SelfBillingIndicator =
                                    ficheiroSAFT.TipoConteudo == ConteudoFicheiroSaft.Autofacturacao ? 1 : 0
                            },
                            SourceID = doc.OperadorEmissao,

                            SystemEntryDate = doc.DataEmissao,
                            TransactionID = doc.DocumentoContabilisticoId,
                            CustomerID = doc.ClienteId,

                            Lines = [.. doc.Artigos.Select((artigo, linhaArtigo) => new InvoiceLine
                            {
                                LineNumber = linhaArtigo + 1,
                                ProductCode = artigo.Artigo.ArtigoId,
                                ProductDescription = artigo.Artigo.Descricao,
                                Quantity = artigo.Quantidade,
                                UnitOfMeasure = artigo.Artigo.UnidadeContagem,
                                UnitPrice = artigo.PrecoTotalSemImpostos,
                                TaxBase = artigo.PrecoTotalSemImpostos,
                                TaxAmount = artigo.ValorImpostos,
                                CreditAmount = artigo.PrecoTotalComImpostos > 0 ? artigo.PrecoTotalComImpostos : 0m,
                                DebitAmount = artigo.PrecoTotalComImpostos < 0 ? -artigo.PrecoTotalComImpostos : 0m,
                                TaxPointDate = doc.DataHora
                            })]
                        })]
                    }
                }
            };
            return auditFile;
        }

        private string RetornaXml(AuditFile auditFile)
        {
            var settings = new XmlWriterSettings { Indent = true };
            using StringWriter stringWriter = new();
            using XmlWriter writer = XmlWriter.Create(stringWriter, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement(nameof(AuditFile));

            // Escreve os campos manualmente
            writer.WriteStartElement(nameof(auditFile.Header));
            writer.WriteElementString(nameof(auditFile.Header.AuditFileVersion), auditFile.Header!.AuditFileVersion);
            writer.WriteElementString(nameof(auditFile.Header.CompanyID), auditFile.Header.CompanyID);

            writer.WriteEndElement(); // Fecha o elemento Header
            writer.WriteEndElement(); // Fecha o elemento AuditFile
            writer.WriteEndDocument();
            writer.Flush();

            return stringWriter.ToString();
        }
    }
}
