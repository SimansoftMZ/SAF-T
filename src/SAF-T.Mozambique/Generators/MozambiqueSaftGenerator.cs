using SAFT.Core.Interfaces;
using SAFT.Core.Models;
using SAFT.Mozambique.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Xml;

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
                string xml = RetornaXml(auditFile);

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
            List<Customer> clientesDistintos = [];
            ficheiroSAFT.DocumentosFacturacao.Select(s => s.Cliente.Id).Distinct().Order().ToList().ForEach(clienteId =>
            {
                clientesDistintos.Add(
                    ficheiroSAFT.DocumentosFacturacao
                        .Where(w => w.Cliente.Id == clienteId)
                        .OrderByDescending(o => o.DataHora)
                        .Select(s => new Customer
                        {
                            CustomerID = s.Cliente.Id,
                            CustomerTaxID = s.Cliente.NUIT,
                            AccountID = s.Cliente.PlanoContaCorrente,
                            CompanyName = s.Cliente.Nome,
                            
                            BillingAddress = new CustomerAddress
                            {
                                AddressDetail = s.Cliente.Endereco,
                                City = s.Cliente.Cidade,
                                PostalCode = s.Cliente.CodigoPostal,
                                Country = s.Cliente.Pais
                            }
                        }).FirstOrDefault()!);

            });

            List<Product> produtosDistintos = [];
            ficheiroSAFT.DocumentosFacturacao.SelectMany(s => s.Artigos).Select(s => s.Artigo.ArtigoId).Distinct().Order().ToList().ForEach(produtoId =>
            {
                produtosDistintos.Add(
                    ficheiroSAFT.DocumentosFacturacao
                        .OrderByDescending(o => o.DataHora)
                        .SelectMany(s => s.Artigos)                        
                        .Where(w => w.Artigo.ArtigoId == produtoId)                        
                        .Select(s => new Product
                        {
                            ProductCode = s.Artigo.ArtigoId,
                            ProductDescription = s.Artigo.Descricao,
                            ProductNumberCode = s.Artigo.ArtigoId,
                            ProductGroup = s.Artigo.FamiliaId
                        }).FirstOrDefault()!);
            });

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
                    ProductID = ficheiroSAFT.FabricanteSoftware.SoftwareProdutoIdCompleto,
                    ProductVersion = ficheiroSAFT.FabricanteSoftware.SoftwareProdutoVersao,
                    HeaderComment = ficheiroSAFT.ComentariosCabecario,
                    Telephone = ficheiroSAFT.Empresa.Telefone,
                    Fax = ficheiroSAFT.Empresa.Fax,
                    Email = ficheiroSAFT.Empresa.Email,
                    Website = ficheiroSAFT.Empresa.Website
                },
                MasterFiles = new MasterFiles()
                {
                    Customers = clientesDistintos,
                    Products = produtosDistintos
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
                            CustomerID =  doc.Cliente.EConsumidorFinal ? "Consumidor Final" : doc.Cliente.Id,

                            Lines = [.. doc.Artigos.Select((artigo, linhaArtigo) => new InvoiceLine
                            {
                                LineNumber = linhaArtigo + 1,
                                ProductCode = artigo.Artigo.ArtigoId,
                                ProductDescription = artigo.Artigo.Descricao,
                                Description = artigo.ArtigoDescricao,
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
            XmlWriterSettings settings = new()
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };
            //using StringWriter stringWriter = new();
            //using XmlWriter writer = XmlWriter.Create(stringWriter, settings);

            using MemoryStream memoryStream = new();
            using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(nameof(AuditFile), "SAF-T_MZ");

                // Escreve os campos manualmente
                writer.WriteStartElement(nameof(auditFile.Header)); // Abre o elemento Header
                writer.WriteElementString(nameof(auditFile.Header.AuditFileVersion), auditFile.Header!.AuditFileVersion);
                writer.WriteElementString(nameof(auditFile.Header.CompanyID), auditFile.Header.CompanyID);
                writer.WriteElementString(nameof(auditFile.Header.TaxRegistrationNumber), auditFile.Header.TaxRegistrationNumber);
                writer.WriteElementString(nameof(auditFile.Header.FileContentType), auditFile.Header.FileContentType);
                writer.WriteElementString(nameof(auditFile.Header.CompanyName), auditFile.Header.CompanyName);
                writer.WriteElementString(nameof(auditFile.Header.BusinessName), auditFile.Header.BusinessName);
                writer.WriteStartElement(nameof(auditFile.Header.CompanyAddress)); // Abre o elemento CompanyAddress
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.StreetName), auditFile.Header.CompanyAddress!.StreetName);
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.AddressDetail), auditFile.Header.CompanyAddress.AddressDetail);
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.City), auditFile.Header.CompanyAddress.City);
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.Province), auditFile.Header.CompanyAddress.Province);
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.PostalCode), auditFile.Header.CompanyAddress.PostalCode);
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.Country), auditFile.Header.CompanyAddress.Country);
                writer.WriteEndElement(); // Fecha o elemento CompanyAddress
                writer.WriteElementString(nameof(auditFile.Header.FiscalYear), auditFile.Header.FiscalYear.ToString());
                writer.WriteElementString(nameof(auditFile.Header.StartDate), auditFile.Header.StartDate?.ToString("yyyy-MM-dd"));
                writer.WriteElementString(nameof(auditFile.Header.EndDate), auditFile.Header.EndDate?.ToString("yyyy-MM-dd"));
                writer.WriteElementString(nameof(auditFile.Header.CurrencyCode), auditFile.Header.CurrencyCode);
                writer.WriteElementString(nameof(auditFile.Header.DateCreated), auditFile.Header.DateCreated?.ToString("yyyy-MM-dd"));
                writer.WriteElementString(nameof(auditFile.Header.TaxEntity), auditFile.Header.TaxEntity);
                writer.WriteElementString(nameof(auditFile.Header.ProductCompanyTaxID), auditFile.Header.ProductCompanyTaxID);
                writer.WriteElementString(nameof(auditFile.Header.SoftwareCertificateNumber), auditFile.Header.SoftwareCertificateNumber);
                writer.WriteElementString(nameof(auditFile.Header.ProductID), auditFile.Header.ProductID);
                writer.WriteElementString(nameof(auditFile.Header.ProductVersion), auditFile.Header.ProductVersion);
                writer.WriteElementString(nameof(auditFile.Header.HeaderComment), auditFile.Header.HeaderComment);
                writer.WriteElementString(nameof(auditFile.Header.Telephone), auditFile.Header.Telephone);
                writer.WriteElementString(nameof(auditFile.Header.Fax), auditFile.Header.Fax);
                writer.WriteElementString(nameof(auditFile.Header.Email), auditFile.Header.Email);
                writer.WriteElementString(nameof(auditFile.Header.Website), auditFile.Header.Website);
                writer.WriteEndElement(); // Fecha o elemento Header

                //ESTÃO EM FALTA OS MATER FILES
                //writer.WriteStartElement(nameof(auditFile.)); // Abre o elemento MasterFiles

                writer.WriteStartElement(nameof(auditFile.SourceDocuments)); // Abre o elemento SourceDocuments
                writer.WriteStartElement(nameof(auditFile.SourceDocuments.SalesInvoices)); // Abre o elemento SalesInvoices
                writer.WriteElementString(nameof(auditFile.SourceDocuments.SalesInvoices.NumberOfEntries), auditFile.SourceDocuments!.SalesInvoices!.NumberOfEntries.ToString());
                writer.WriteElementString(nameof(auditFile.SourceDocuments.SalesInvoices.TotalCredit), auditFile.SourceDocuments.SalesInvoices.TotalCredit.ToString());
                writer.WriteElementString(nameof(auditFile.SourceDocuments.SalesInvoices.TotalDebit), auditFile.SourceDocuments.SalesInvoices.TotalDebit.ToString());
                //writer.WriteStartElement(nameof(auditFile.SourceDocuments.SalesInvoices.Invoices)); // Abre o elemento Invoices

                auditFile.SourceDocuments.SalesInvoices.Invoices!.ForEach(invoice =>
                {
                    writer.WriteStartElement(nameof(Invoice)); // Abre o elemento Invoice
                    writer.WriteElementString(nameof(invoice.InvoiceNo), invoice.InvoiceNo);
                    writer.WriteElementString(nameof(invoice.InvoiceType), invoice.InvoiceType);
                    writer.WriteStartElement(nameof(invoice.DocumentStatus)); // Abre o elemento DocumentStatus
                    writer.WriteElementString(nameof(invoice.DocumentStatus.InvoiceStatus), invoice.DocumentStatus!.InvoiceStatus);
                    writer.WriteElementString(nameof(invoice.DocumentStatus.InvoiceStatusDate), invoice.DocumentStatus.InvoiceStatusDate?.ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteElementString(nameof(invoice.DocumentStatus.SourceID), invoice.DocumentStatus.SourceID);
                    writer.WriteElementString(nameof(invoice.DocumentStatus.SourceBilling), invoice.DocumentStatus.SourceBilling);
                    writer.WriteEndElement(); // Fecha o elemento DocumentStatus
                    writer.WriteElementString(nameof(invoice.Hash), invoice.Hash.ToString());
                    writer.WriteElementString(nameof(invoice.HashControl), invoice.HashControl);
                    writer.WriteElementString(nameof(invoice.EACCode), invoice.EACCode);
                    writer.WriteElementString(nameof(invoice.Period), invoice.Period.ToString());
                    writer.WriteElementString(nameof(invoice.InvoiceDate), invoice.InvoiceDate?.ToString("yyyy-MM-dd"));
                    writer.WriteElementString(nameof(invoice.InvoiceStatus), invoice.InvoiceStatus);
                    writer.WriteElementString(nameof(invoice.InvoiceStatusDate), invoice.InvoiceStatusDate?.ToString("yyyy-MM-dd"));
                    writer.WriteElementString(nameof(invoice.SourceBilling), invoice.SourceBilling);
                    writer.WriteStartElement(nameof(invoice.SpecialRegimes)); // Abre o elemento SpecialRegimes
                    writer.WriteElementString(nameof(invoice.SpecialRegimes.SelfBillingIndicator), invoice.SpecialRegimes!.SelfBillingIndicator.ToString());
                    writer.WriteEndElement(); // Fecha o elemento SpecialRegimes
                    writer.WriteElementString(nameof(invoice.SourceID), invoice.SourceID);
                    writer.WriteElementString(nameof(invoice.SystemEntryDate), invoice.SystemEntryDate?.ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteElementString(nameof(invoice.TransactionID), invoice.TransactionID);
                    writer.WriteElementString(nameof(invoice.CustomerID), invoice.CustomerID);

                    invoice.Lines!.ForEach(artigo =>
                    {
                        writer.WriteStartElement(nameof(InvoiceLine)); // Abre o elemento artigo
                        writer.WriteElementString(nameof(artigo.LineNumber), artigo.LineNumber.ToString());
                        writer.WriteElementString(nameof(artigo.ProductCode), artigo.ProductCode);
                        writer.WriteElementString(nameof(artigo.ProductDescription), artigo.ProductDescription);
                        writer.WriteElementString(nameof(artigo.Description), artigo.Description);
                        writer.WriteElementString(nameof(artigo.Quantity), artigo.Quantity.ToString());
                        writer.WriteElementString(nameof(artigo.UnitOfMeasure), artigo.UnitOfMeasure);
                        writer.WriteElementString(nameof(artigo.UnitPrice), artigo.UnitPrice.ToString());
                        writer.WriteElementString(nameof(artigo.TaxBase), artigo.TaxBase.ToString());
                        writer.WriteElementString(nameof(artigo.TaxAmount), artigo.TaxAmount.ToString());
                        writer.WriteElementString(nameof(artigo.CreditAmount), artigo.CreditAmount.ToString());
                        writer.WriteElementString(nameof(artigo.DebitAmount), artigo.DebitAmount.ToString());
                        writer.WriteElementString(nameof(artigo.TaxPointDate), artigo.TaxPointDate?.ToString("yyyy-MM-dd"));
                        writer.WriteEndElement(); // Fecha o elemento artigo
                    });
                    writer.WriteEndElement(); // Fecha o elemento Invoice
                });
                //writer.WriteEndElement(); // Fecha o elemento Invoices

                writer.WriteEndElement(); // Fecha o elemento AuditFile
                writer.WriteEndDocument();
                writer.Flush();
            }

            //return stringWriter.ToString();
            //return Encoding.UTF8.GetString(memoryStream.ToArray());
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}
