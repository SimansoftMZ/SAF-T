using Simansoft.SAFT.Core.Models;
using Simansoft.SAFT.Mozambique.Models;
using Simansoft.SAFT.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Simansoft.SAFT.Mozambique.Generators
{
    public class MozambiqueSaftGenerator : ISaftGenerator<FicheiroSAFT>
    {
        public AuditFile ConverterParaSaft(FicheiroSAFT ficheiroSAFT)
        {
            // Preenchimento distinto dos clientes
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
                            AccountID = string.IsNullOrWhiteSpace(s.Cliente.PlanoContaCorrente) ? "Desconhecido" : s.Cliente.PlanoContaCorrente,
                            CustomerTaxID = s.Cliente.EConsumidorFinal || string.IsNullOrWhiteSpace(s.Cliente.NUIT) ? "000000000" : s.Cliente.NUIT,
                            CompanyName = s.Cliente.EConsumidorFinal || string.IsNullOrWhiteSpace(s.Cliente.Nome) ? "Consumidor Final" : s.Cliente.Nome,

                            BillingAddress = s.Cliente.EConsumidorFinal ? null : new CustomerAddress
                            {
                                AddressDetail = s.Cliente.Endereco,
                                City = s.Cliente.Cidade,
                                PostalCode = s.Cliente.CodigoPostal,
                                Country = s.Cliente.Pais
                            },
// Removed commented-out ShipToAddress block to clean up the code.
                            SelfBillingIndicator = "0"
                        }).FirstOrDefault()!);

            });

            // Preenchimento distinto dos produtos
            List<Product> produtosDistintos = [];
            ficheiroSAFT.DocumentosFacturacao.SelectMany(s => s.Artigos).Select(s => s.Artigo.ArtigoId).Distinct().Order().ToList().ForEach(produtoId =>
            {
                produtosDistintos.Add(
                    ficheiroSAFT.DocumentosFacturacao
                        .OrderByDescending(o => o.DataHora)
                        .SelectMany(s => s.Artigos)
                        .Where(w => w.Artigo.ArtigoId == produtoId)
                        .Select((s) => new Product
                        {
                            ProductType = s.Artigo.ServicoId,
                            ProductCode = s.Artigo.ArtigoId,
                            ProductGroup = s.Artigo.FamiliaId,
                            ProductDescription = s.Artigo.Descricao,
                            ProductNumberCode = s.Artigo.ArtigoId
                        }).FirstOrDefault()!);
            });

            // Preenchimento distinto dos impostos
            List<TaxTableEntry> impostosDistintos = [];
            ficheiroSAFT.DocumentosFacturacao.SelectMany(s => s.Artigos).SelectMany(s => s.Artigo.Impostos).Select(s => s.Codigo).Distinct().Order().ToList().ForEach(impostoId =>
            {
                impostosDistintos.Add(
                    ficheiroSAFT.DocumentosFacturacao
                        .OrderByDescending(o => o.DataHora)
                        .SelectMany(s => s.Artigos)
                        .SelectMany(s => s.Artigo.Impostos)
                        .Where(w => w.Codigo == impostoId)
                        .Select(s => new TaxTableEntry
                        {
                            TaxType = s.Tipo,
                            TaxCountryRegion = s.Pais,
                            TaxCode = s.Codigo,
                            Description = s.Descricao,
                            TaxPercentage = s.Percentagem,
                            TaxAmount = s.Valor
                        }).FirstOrDefault()!);
            });

            AuditFile auditFile = new()
            {
                Header = new Header
                {
                    AuditFileVersion = ficheiroSAFT.VersaoFicheiro,
                    CompanyID = ficheiroSAFT.Empresa.NUIT,
                    TaxRegistrationNumber = ficheiroSAFT.Empresa.NUIT,
                    FileContentType = ficheiroSAFT.Tipo,
                    CompanyName = ficheiroSAFT.Empresa.Nome,
                    BusinessName = ficheiroSAFT.Empresa.NomeComercial,
                    CompanyAddress = new CompanyAddress
                    {
                        StreetName = ficheiroSAFT.Empresa.Endereco1,
                        AddressDetail = ficheiroSAFT.Empresa.Endereco2,
                        City = ficheiroSAFT.Empresa.Cidade,
                        PostalCode = ficheiroSAFT.Empresa.CodigoPostal,
                        Province = ficheiroSAFT.Empresa.Provincia,
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
                    Products = produtosDistintos,
                    TaxTable = impostosDistintos
                },
                SourceDocuments = new SourceDocuments
                {
                    SalesInvoices = new SalesInvoices()
                    {
                        NumberOfEntries = ficheiroSAFT.DocumentosFacturacao.Count,
                        TotalDebit = ficheiroSAFT.TotalDebito,
                        TotalCredit = ficheiroSAFT.TotalCredito,
                        Invoices = [.. ficheiroSAFT.DocumentosFacturacao.Select(doc => new Invoice
                        {
                            InvoiceNo = doc.Id,
                            Serie = doc.TipoDocumentoId,
                            DocumentStatus = new DocumentStatus
                            {
                                InvoiceStatus = doc.EstadoId,
                                InvoiceStatusDate = doc.DataHora,
                                SourceID = doc.OperadorEmissao,
                                SourceBilling = doc.OrigemDocumentoId
                            },
                            Hash = (doc.ControlaAssinatura ?? false) ? 1 : 0,
                            HashControl = (string.IsNullOrWhiteSpace(doc.Assinatura) && (doc.ControlaAssinatura ?? false))
                                ? "Não certificado"
                                : doc.Assinatura,
                            Period = doc.PeriodoMes,
                            InvoiceDate = DateOnly.FromDateTime(doc.DataHora),
                            SpecialRegimes = new SpecialRegimes
                            {
                                SelfBillingIndicator =
                                    ficheiroSAFT.TipoConteudo == ConteudoFicheiroSaft.Autofacturacao ? 1 : 0,
                                CashVATSchemeIndicator = 0,
                                ThirdPartiesBillingIndicator = 0
                            },
                            SourceID = doc.OperadorEmissao,
                            EACCode = doc.CodigoEAC,
                            SystemEntryDate = doc.DataEmissao,
                            TransactionID = doc.DocumentoContabilisticoId,
                            CustomerID = doc.Cliente.Id,
                            InvoiceType = doc.CategoriaId,

                            
                            //        City = ficheiroSAFT.Empresa.Cidade,
                            //        PostalCode = ficheiroSAFT.Empresa.CodigoPostal,
                            //        Country = ficheiroSAFT.Empresa.Pais
                            //    }
                            //},

                            Lines = [.. doc.Artigos.Select((artigo, linhaArtigo) => new InvoiceLine
                            {
                                LineNumber = linhaArtigo++,
                                ProductCode = artigo.Artigo.ArtigoId,
                                ProductDescription = artigo.Artigo.Descricao,
                                Quantity = artigo.Quantidade,
                                UnitOfMeasure = artigo.Artigo.UnidadeContagem,
                                UnitPrice = artigo.PrecoUnitarioSemImpostos,
                                TaxBase = artigo.PrecoTotalSemImpostos,
                                TaxPointDate = doc.DataHora,
                                Description = artigo.ArtigoDescricao,
                                DebitAmount = artigo.PrecoTotalComImpostos < 0 ? -artigo.PrecoTotalComImpostos : 0m,
                                CreditAmount = artigo.PrecoTotalComImpostos > 0 ? artigo.PrecoTotalComImpostos : 0m,

                                SettlementAmount = artigo.ValorDesconto,

                                Tax = [.. artigo.Artigo.Impostos.Select(imp => new TaxTableEntry
                                {
                                    TaxType = imp.Tipo,
                                    TaxCountryRegion = imp.Pais,
                                    TaxCode = imp.Codigo,
                                    TaxPercentage = imp.Percentagem,
                                    TaxAmount = imp.Valor + (artigo.PrecoTotalComImpostos / (1m + (imp.Percentagem * 0.01m))) * (imp.Percentagem * 0.01m)
                                })],

                                TaxExemptionReason = artigo.Artigo.Motivo,
                                TaxExemptionCode = artigo.Artigo.MotivoCodigo
                            })],
                            DocumentTotals = new DocumentTotals
                            {
                                TaxPayable = doc.Artigos.Sum(s => s.PrecoTotalSemImpostos),
                                NetTotal = doc.Artigos.Sum(s => s.ValorImpostos),
                                GrossTotal = doc.Artigos.Sum(s => s.PrecoTotalComImpostos),
                                Payments = [.. doc.MeiosPagamento.Select(p => new Payment
                                {
                                    PaymentMechanism = p.TipoId,
                                    PaymentAmount = p.Valor,
                                    PaymentDate = p.Data
                                })]
                            }
                        })]
                    }
                }
            };

            return auditFile;
        }

        public string GenerateJson(AuditFile auditFile)
        {
            return JsonSerializer.Serialize(auditFile, AuditFileContext.Custom.AuditFile);
        }

        public string GenerateXml(AuditFile auditFile)
        {
            try
            {
                return Encoding.UTF8.GetString(GenerateBytesXml(auditFile));
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Falha na serialização SAF-T", ex);
            }
        }

        public byte[] GenerateBytesExcel(AuditFile auditFile)
        {
            return GenerateBytesExcel(auditFile, FormatoOutput.Excel);
        }

        public byte[] GenerateBytesExcel(AuditFile auditFile, FormatoOutput formatoOutput = FormatoOutput.Excel)
        {
            try
            {
                double taxaCambio = 1d;
                if (auditFile == null)
                {
                    throw new ArgumentNullException(nameof(auditFile), "O objeto AuditFile não pode ser nulo.");
                }

                IWorkbook workbook = formatoOutput switch
                {
                    FormatoOutput.Excel => new XSSFWorkbook(),// XLSX
                    FormatoOutput.ExcelOld => new HSSFWorkbook(),// XLS
                    _ => throw new ArgumentException($"Formato de saída '{formatoOutput}' não suportado.", nameof(formatoOutput)),
                };
                ISheet sheet = workbook.CreateSheet("Faturas");

                // Cabeçalho
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Linha");
                headerRow.CreateCell(1).SetCellValue("NUIT");
                headerRow.CreateCell(2).SetCellValue("MÊS");
                headerRow.CreateCell(3).SetCellValue("ID Documento");
                headerRow.CreateCell(4).SetCellValue("Tipo");
                headerRow.CreateCell(5).SetCellValue("Série");
                headerRow.CreateCell(6).SetCellValue("Número");
                headerRow.CreateCell(7).SetCellValue("Série/Número");
                headerRow.CreateCell(8).SetCellValue("Estado");
                headerRow.CreateCell(9).SetCellValue("Referência a Documento de Origem");
                headerRow.CreateCell(10).SetCellValue("Referência a Factura");
                headerRow.CreateCell(11).SetCellValue("Data Emissão");
                headerRow.CreateCell(12).SetCellValue("Data Vencimento");
                headerRow.CreateCell(13).SetCellValue("NUIT Cliente");
                headerRow.CreateCell(14).SetCellValue("Nome do Cliente");
                headerRow.CreateCell(15).SetCellValue("Subtotal S/IVA");
                headerRow.CreateCell(16).SetCellValue("Outro S/IVA");
                headerRow.CreateCell(17).SetCellValue("IVA");
                headerRow.CreateCell(18).SetCellValue("Razão de Isenção de IVA");
                headerRow.CreateCell(19).SetCellValue("Total Retenção");
                headerRow.CreateCell(20).SetCellValue("Total Desconto");
                headerRow.CreateCell(21).SetCellValue("Total");
                headerRow.CreateCell(22).SetCellValue("Valor Recebido");
                headerRow.CreateCell(23).SetCellValue("IVA (%)");
                headerRow.CreateCell(24).SetCellValue("Valor do Imposto");
                headerRow.CreateCell(25).SetCellValue("Desconto");
                headerRow.CreateCell(26).SetCellValue("Valor Total sem Imposto");
                headerRow.CreateCell(27).SetCellValue("Total Incluindo Imposto");
                headerRow.CreateCell(28).SetCellValue("Moeda");
                headerRow.CreateCell(29).SetCellValue("Taxa Câmbio");

                IFont font = workbook.CreateFont();
                font.IsBold = true;
                ICellStyle headerStyle = workbook.CreateCellStyle();
                headerStyle.SetFont(font);
                for (int i = 0; i < 30; i++)
                {
                    headerRow.GetCell(i).CellStyle = headerStyle;
                }

                int numeroLinha = 1;
                auditFile.SourceDocuments?.SalesInvoices?.Invoices?.ForEach(factura =>
                {
                    var row = sheet.CreateRow(numeroLinha++);
                    row.CreateCell(0).SetCellValue(numeroLinha - 1);
                    row.CreateCell(1).SetCellValue(auditFile.Header!.TaxRegistrationNumber);
                    row.CreateCell(2).SetCellValue(factura.InvoiceDate!.Value.ToDateTime(new TimeOnly()).ToString("yyyy-MM"));
                    row.CreateCell(3).SetCellValue(string.Concat(factura.TipoDocumentoAbreviado, factura.InvoiceNo!.Split('/')[1]));
                    row.CreateCell(4).SetCellValue(factura.TipoDocumento);
                    row.CreateCell(5).SetCellValue(factura.Serie);
                    row.CreateCell(6).SetCellValue(factura.InvoiceNo!.Split('/')[1]);
                    row.CreateCell(7).SetCellValue(string.Concat(factura.Serie, "/", factura.InvoiceNo!.Split('/')[1]));
                    row.CreateCell(8).SetCellValue(factura.DocumentStatus!.EstadoDescricao);
                    //row.CreateCell(9).SetCellValue(factura.DocumentStatus!.ReferenciaDocumentoOrigem);
                    //row.CreateCell(10).SetCellValue(factura.DocumentStatus!.ReferenciaFactura);
                    row.CreateCell(11).SetCellValue(factura.InvoiceDate!.Value.ToString("yyyy-MM-dd"));
                    //row.CreateCell(12).SetCellValue(factura.DataVencimento?.ToString("yyyy-MM-dd"));
                    row.CreateCell(13).SetCellValue(auditFile.MasterFiles?
                        .Customers.SingleOrDefault(w => (w.CustomerID ?? string.Empty).Equals(factura.CustomerID, StringComparison.OrdinalIgnoreCase))?
                        .CustomerTaxID ?? string.Empty);

                    row.CreateCell(14).SetCellValue(auditFile.MasterFiles?
                        .Customers.SingleOrDefault(w => (w.CustomerID ?? string.Empty).Equals(factura.CustomerID, StringComparison.OrdinalIgnoreCase))?
                        .CompanyName ?? string.Empty);

                    row.CreateCell(15).SetCellValue(Convert.ToDouble(factura.DocumentTotals?.TaxPayable ?? 0m));
                    row.CreateCell(17).SetCellValue(Convert.ToDouble(factura.DocumentTotals?.NetTotal ?? 0m));
                    row.CreateCell(20).SetCellValue(Convert.ToDouble(factura.Lines?.Sum(s => s.SettlementAmount) ?? 0m));
                    row.CreateCell(21).SetCellValue(Convert.ToDouble(factura.DocumentTotals?.GrossTotal ?? 0m));
                    row.CreateCell(22).SetCellValue(Convert.ToDouble(factura.DocumentTotals?.Payments.Sum(s => s.PaymentAmount) ?? 0m));
                   
                    decimal impostos = factura.Lines?
                        .FirstOrDefault(w =>
                            w.Tax.Where(wh => wh.TaxPercentage != 0m).FirstOrDefault()?.TaxPercentage != 0m)?
                            .Tax.FirstOrDefault(w => w.TaxPercentage != 0m)?.TaxPercentage ?? 0m;

                    row.CreateCell(23).SetCellValue(Convert.ToDouble(factura.Lines?
                        .FirstOrDefault(w =>
                            w.Tax.Where(wh => wh.TaxPercentage != 0m).FirstOrDefault()?.TaxPercentage != 0m)?
                            .Tax.FirstOrDefault(w => w.TaxPercentage != 0m)?.TaxPercentage ?? 0m));

                    row.CreateCell(24).SetCellValue(Convert.ToDouble(factura.DocumentTotals?.NetTotal ?? 0m));
                    row.CreateCell(25).SetCellValue(Convert.ToDouble(factura.Lines?.Sum(s => s.SettlementAmount) ?? 0m));
                    row.CreateCell(26).SetCellValue(Convert.ToDouble(factura.DocumentTotals?.TaxPayable ?? 0m));
                    row.CreateCell(27).SetCellValue(Convert.ToDouble(factura.DocumentTotals?.GrossTotal ?? 0m));
                    row.CreateCell(28).SetCellValue(auditFile.Header?.CurrencyCode ?? string.Empty);
                    row.CreateCell(29).SetCellValue(taxaCambio);
                });

                using var stream = new MemoryStream();
                workbook.Write(stream);
                return stream.ToArray();
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("Falha ao gerar o SAF-T em formato Excel", ex);
            }
        }

        public byte[] GenerateBytesXml(AuditFile auditFile)
        {
            XmlWriterSettings settings = new()
            {
                Indent = false,
                Encoding = Encoding.UTF8 //Encoding.GetEncoding(1252)
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
                writer.WriteElementString(nameof(auditFile.Header.TaxRegistrationNumber), TratamentoStringXML(auditFile.Header.TaxRegistrationNumber));
                writer.WriteElementString(nameof(auditFile.Header.FileContentType), auditFile.Header.FileContentType);
                writer.WriteElementString(nameof(auditFile.Header.CompanyName), TratamentoStringXML(auditFile.Header.CompanyName));
                writer.WriteElementString(nameof(auditFile.Header.BusinessName), TratamentoStringXML(auditFile.Header.BusinessName));
                writer.WriteStartElement(nameof(auditFile.Header.CompanyAddress)); // Abre o elemento CompanyAddress
                //writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.StreetName), auditFile.Header.CompanyAddress!.StreetName);
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.AddressDetail), TratamentoStringXML(auditFile.Header.CompanyAddress!.AddressDetail));
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.City), TratamentoStringXML(auditFile.Header.CompanyAddress.City));
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.PostalCode), TratamentoStringXML(auditFile.Header.CompanyAddress.PostalCode));
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.Province), TratamentoStringXML(auditFile.Header.CompanyAddress.Province));
                writer.WriteElementString(nameof(auditFile.Header.CompanyAddress.Country), TratamentoStringXML(auditFile.Header.CompanyAddress.Country));
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
                if (!string.IsNullOrWhiteSpace(auditFile.Header.HeaderComment))
                {
                    writer.WriteElementString(nameof(auditFile.Header.HeaderComment), TratamentoStringXML(auditFile.Header.HeaderComment));
                }

                if (!string.IsNullOrWhiteSpace(auditFile.Header.Telephone))
                {
                    writer.WriteElementString(nameof(auditFile.Header.Telephone), TratamentoStringXML(auditFile.Header.Telephone));
                }
                if (!string.IsNullOrWhiteSpace(auditFile.Header.Fax))
                {
                    writer.WriteElementString(nameof(auditFile.Header.Fax), TratamentoStringXML(auditFile.Header.Fax));
                }
                if (!string.IsNullOrWhiteSpace(auditFile.Header.Email))
                {
                    writer.WriteElementString(nameof(auditFile.Header.Email), TratamentoStringXML(auditFile.Header.Email));
                }
                if (!string.IsNullOrWhiteSpace(auditFile.Header.Website))
                {
                    writer.WriteElementString(nameof(auditFile.Header.Website), TratamentoStringXML(auditFile.Header.Website));
                }

                writer.WriteEndElement(); // Fecha o elemento Header


                writer.WriteStartElement(nameof(auditFile.MasterFiles)); // Abre o elemento MasterFiles

                //writer.WriteStartElement(nameof(auditFile.MasterFiles.Customers)); // Abre o elemento Customers
                auditFile.MasterFiles!.Customers!.ForEach(customer =>
                {
                    writer.WriteStartElement(nameof(Customer)); // Abre o elemento Customer
                    writer.WriteElementString(nameof(customer.CustomerID), customer.CustomerID);
                    writer.WriteElementString(nameof(customer.AccountID), customer.AccountID);
                    writer.WriteElementString(nameof(customer.CustomerTaxID), customer.CustomerTaxID);
                    writer.WriteElementString(nameof(customer.CompanyName), customer.CompanyName);
                    // Faltando Contacto
                    if (customer.BillingAddress is not null)
                    {
                        writer.WriteStartElement(nameof(customer.BillingAddress)); // Abre o elemento BillingAddress
                        if (!string.IsNullOrWhiteSpace(customer.BillingAddress.AddressDetail))
                        {
                            writer.WriteElementString(nameof(customer.BillingAddress.AddressDetail), TratamentoStringXML(customer.BillingAddress.AddressDetail));
                        }
                        if (!string.IsNullOrWhiteSpace(customer.BillingAddress.City))
                        {
                            writer.WriteElementString(nameof(customer.BillingAddress.City), TratamentoStringXML(customer.BillingAddress.City));
                        }
                        if (!string.IsNullOrWhiteSpace(customer.BillingAddress.PostalCode))
                        {
                            writer.WriteElementString(nameof(customer.BillingAddress.PostalCode), TratamentoStringXML(customer.BillingAddress.PostalCode));
                        }

                        // Faltando Province
                        if (!string.IsNullOrWhiteSpace(customer.BillingAddress.Country))
                        {
                            writer.WriteElementString(nameof(customer.BillingAddress.Country), TratamentoStringXML(customer.BillingAddress.Country));
                        }
                        writer.WriteEndElement(); // Fecha o elemento BillingAddress
                    }

// Removed commented-out block for ShipToAddress to improve code readability and maintainability.
                    // Faltando Telephone
                    // Faltando Fax
                    // Faltando Email
                    // Faltando Website
                    writer.WriteElementString(nameof(customer.SelfBillingIndicator), customer.SelfBillingIndicator?.ToString());

                    writer.WriteEndElement(); // Fecha o elemento Customer
                });
                //writer.WriteEndElement(); // Fecha o elemento Customers

                //writer.WriteStartElement(nameof(auditFile.MasterFiles.Products)); // Abre o elemento Products
                auditFile.MasterFiles.Products!.ForEach(product =>
                {
                    writer.WriteStartElement(nameof(Product)); // Abre o elemento Product
                    writer.WriteElementString(nameof(product.ProductType), product.ProductType);

                    if (!string.IsNullOrWhiteSpace(product.ProductCode))
                    {
                        writer.WriteElementString(nameof(product.ProductCode), TratamentoStringXML(product.ProductCode));
                    }

                    if (!string.IsNullOrWhiteSpace(product.ProductGroup))
                    {
                        writer.WriteElementString(nameof(product.ProductGroup), TratamentoStringXML(product.ProductGroup));
                    }

                    if (!string.IsNullOrWhiteSpace(product.ProductDescription))
                    {
                        writer.WriteElementString(nameof(product.ProductDescription), TratamentoStringXML(product.ProductDescription));
                    }

                    if (!string.IsNullOrWhiteSpace(product.ProductNumberCode))
                    {
                        writer.WriteElementString(nameof(product.ProductNumberCode), TratamentoStringXML(product.ProductNumberCode));
                    }
                    writer.WriteEndElement(); // Fecha o elemento Product
                });
                //writer.WriteEndElement(); // Fecha o elemento Products

                writer.WriteStartElement(nameof(auditFile.MasterFiles.TaxTable)); // Abre o elemento TaxTable
                auditFile.MasterFiles.TaxTable!.ForEach(taxTableEntry =>
                {
                    writer.WriteStartElement(nameof(TaxTableEntry)); // Abre o elemento TaxTableEntry
                    writer.WriteElementString(nameof(taxTableEntry.TaxType), taxTableEntry.TaxType);
                    writer.WriteElementString(nameof(taxTableEntry.TaxCountryRegion), taxTableEntry.TaxCountryRegion);
                    writer.WriteElementString(nameof(taxTableEntry.TaxCode), taxTableEntry.TaxCode);
                    writer.WriteElementString(nameof(taxTableEntry.Description), taxTableEntry.Description);
                    writer.WriteElementString(nameof(taxTableEntry.TaxPercentage), taxTableEntry.TaxPercentage.ToString());
                    writer.WriteEndElement(); // Fecha o elemento TaxTableEntry
                });
                writer.WriteEndElement(); // Fecha o elemento TaxTable

                writer.WriteEndElement(); // Fecha o elemento MasterFiles

                //ESTÃO EM FALTA OS MASTER FILES

                writer.WriteStartElement(nameof(auditFile.SourceDocuments)); // Abre o elemento SourceDocuments
                writer.WriteStartElement(nameof(auditFile.SourceDocuments.SalesInvoices)); // Abre o elemento SalesInvoices
                writer.WriteElementString(nameof(auditFile.SourceDocuments.SalesInvoices.NumberOfEntries), auditFile.SourceDocuments!.SalesInvoices!.NumberOfEntries.ToString());
                writer.WriteElementString(nameof(auditFile.SourceDocuments.SalesInvoices.TotalDebit), auditFile.SourceDocuments.SalesInvoices.TotalDebit.ToString());
                writer.WriteElementString(nameof(auditFile.SourceDocuments.SalesInvoices.TotalCredit), auditFile.SourceDocuments.SalesInvoices.TotalCredit.ToString());
                //writer.WriteStartElement(nameof(auditFile.SourceDocuments.SalesInvoices.Invoices)); // Abre o elemento Invoices

                auditFile.SourceDocuments.SalesInvoices.Invoices!.ForEach(invoice =>
                {
                    writer.WriteStartElement(nameof(Invoice)); // Abre o elemento Invoice
                    writer.WriteElementString(nameof(invoice.InvoiceNo), invoice.InvoiceNo);
                    writer.WriteStartElement(nameof(invoice.DocumentStatus)); // Abre o elemento DocumentStatus
                    writer.WriteElementString(nameof(invoice.DocumentStatus.InvoiceStatus), invoice.DocumentStatus!.InvoiceStatus);
                    writer.WriteElementString(nameof(invoice.DocumentStatus.InvoiceStatusDate), invoice.DocumentStatus.InvoiceStatusDate?.ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteElementString(nameof(invoice.DocumentStatus.SourceID), invoice.DocumentStatus.SourceID);
                    writer.WriteElementString(nameof(invoice.DocumentStatus.SourceBilling), invoice.DocumentStatus.SourceBilling);
                    writer.WriteEndElement(); // Fecha o elemento DocumentStatus
                    writer.WriteElementString(nameof(invoice.Hash), invoice.Hash.ToString());
                    writer.WriteElementString(nameof(invoice.HashControl), invoice.HashControl);
                    writer.WriteElementString(nameof(invoice.Period), invoice.Period.ToString());
                    writer.WriteElementString(nameof(invoice.InvoiceDate), invoice.InvoiceDate?.ToString("yyyy-MM-dd"));
                    writer.WriteElementString(nameof(invoice.InvoiceType), invoice.InvoiceType);

                    // ATENÇÃO: Elementos "CashVATSchemeIndicator" e "ThirdPartiesBillingIndicator" em falta
                    writer.WriteStartElement(nameof(invoice.SpecialRegimes)); // Abre o elemento SpecialRegimes
                    writer.WriteElementString(nameof(invoice.SpecialRegimes.SelfBillingIndicator), invoice.SpecialRegimes!.SelfBillingIndicator.ToString());
                    writer.WriteEndElement(); // Fecha o elemento SpecialRegimes

                    writer.WriteElementString(nameof(invoice.SourceID), invoice.SourceID);
                    writer.WriteElementString(nameof(invoice.EACCode), invoice.EACCode);
                    writer.WriteElementString(nameof(invoice.SystemEntryDate), invoice.SystemEntryDate?.ToString("yyyy-MM-ddTHH:mm:ss"));
                    writer.WriteElementString(nameof(invoice.TransactionID), invoice.TransactionID);
                    writer.WriteElementString(nameof(invoice.CustomerID), invoice.CustomerID);

                    //if (invoice.ShipTo is not null)
                    //{
                    //    writer.WriteStartElement(nameof(invoice.ShipTo)); // Abre o elemento ShipTo
                    //    if (invoice.ShipTo.Address is not null)
                    //    {
                    //        writer.WriteStartElement(nameof(invoice.ShipTo.Address)); // Abre o elemento Address

                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipTo?.Address?.AddressDetail))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipTo.Address.AddressDetail), TratamentoStringXML(invoice.ShipTo?.Address?.AddressDetail));
                    //        }
                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipTo?.Address?.City))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipTo.Address.City), TratamentoStringXML(invoice.ShipTo?.Address?.City));
                    //        }
                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipTo?.Address?.PostalCode))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipTo.Address.PostalCode), TratamentoStringXML(invoice.ShipTo?.Address?.PostalCode));
                    //        }
                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipTo?.Address?.Country))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipTo.Address.Country), TratamentoStringXML(invoice.ShipTo?.Address?.Country));
                    //        }
                    //        writer.WriteEndElement(); // Fecha o elemento Address
                    //    }
                    //    writer.WriteEndElement(); // Fecha o elemento ShipTo
                    //}

                    //if (invoice.ShipFrom is not null)
                    //{
                    //    writer.WriteStartElement(nameof(invoice.ShipFrom)); // Abre o elemento ShipFrom
                    //    if (invoice.ShipFrom.Address is not null)
                    //    {
                    //        writer.WriteStartElement(nameof(invoice.ShipFrom.Address)); // Abre o elemento Address

                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipFrom?.Address?.AddressDetail))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipFrom.Address.AddressDetail), TratamentoStringXML(invoice.ShipFrom?.Address?.AddressDetail));
                    //        }

                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipFrom?.Address?.City))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipFrom.Address.City), TratamentoStringXML(invoice.ShipFrom?.Address?.City));
                    //        }

                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipFrom?.Address?.PostalCode))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipFrom.Address.PostalCode), TratamentoStringXML(invoice.ShipFrom?.Address?.PostalCode));
                    //        }

                    //        if (!string.IsNullOrWhiteSpace(invoice.ShipFrom?.Address?.Country))
                    //        {
                    //            writer.WriteElementString(nameof(invoice.ShipFrom.Address.Country), TratamentoStringXML(invoice.ShipFrom?.Address?.Country));
                    //        }
                    //        writer.WriteEndElement(); // Fecha o elemento Address
                    //    }
                    //    writer.WriteEndElement(); // Fecha o elemento ShipFrom
                    //}

                    invoice.Lines!.ForEach(artigo =>
                    {
                        writer.WriteStartElement(nameof(InvoiceLine)); // Abre o elemento artigo
                        writer.WriteElementString(nameof(artigo.LineNumber), artigo.LineNumber.ToString());
                        writer.WriteElementString(nameof(artigo.ProductCode), TratamentoStringXML(artigo.ProductCode));
                        writer.WriteElementString(nameof(artigo.ProductDescription), TratamentoStringXML(artigo.ProductDescription));
                        writer.WriteElementString(nameof(artigo.Quantity), artigo.Quantity.ToString());
                        writer.WriteElementString(nameof(artigo.UnitOfMeasure), artigo.UnitOfMeasure);
                        writer.WriteElementString(nameof(artigo.UnitPrice), artigo.UnitPrice.ToString());
                        writer.WriteElementString(nameof(artigo.TaxBase), artigo.TaxBase.ToString());
                        writer.WriteElementString(nameof(artigo.TaxPointDate), artigo.TaxPointDate?.ToString("yyyy-MM-dd"));
                        writer.WriteElementString(nameof(artigo.Description), TratamentoStringXML(artigo.Description));
                        writer.WriteElementString(nameof(artigo.DebitAmount), artigo.DebitAmount.ToString());
                        writer.WriteElementString(nameof(artigo.CreditAmount), artigo.CreditAmount.ToString());
                        writer.WriteStartElement(nameof(artigo.Tax));
                        writer.WriteElementString(nameof(TaxTableEntry.TaxType), artigo.Tax.FirstOrDefault()?.TaxType);
                        writer.WriteElementString(nameof(TaxTableEntry.TaxCountryRegion), TratamentoStringXML(artigo.Tax.FirstOrDefault()?.TaxCountryRegion));
                        writer.WriteElementString(nameof(TaxTableEntry.TaxCode), artigo.Tax.FirstOrDefault()?.TaxCode);
                        writer.WriteElementString(nameof(TaxTableEntry.TaxPercentage), artigo.Tax.FirstOrDefault()?.TaxPercentage.ToString());
                        writer.WriteElementString(nameof(TaxTableEntry.TaxAmount), artigo.Tax.FirstOrDefault()?.TaxAmount.ToString());
                        writer.WriteEndElement(); // Fecha o elemento Tax
                        writer.WriteElementString(nameof(artigo.TaxExemptionReason), TratamentoStringXML(artigo.TaxExemptionReason));
                        writer.WriteElementString(nameof(artigo.TaxExemptionCode), TratamentoStringXML(artigo.TaxExemptionCode));
                        writer.WriteElementString(nameof(artigo.SettlementAmount), artigo.SettlementAmount.ToString());

                        writer.WriteEndElement(); // Fecha o elemento artigo
                    });

                    writer.WriteStartElement(nameof(invoice.DocumentTotals)); // Abre o elemento DocumentTotals
                    writer.WriteElementString(nameof(invoice.DocumentTotals.TaxPayable), invoice.DocumentTotals?.TaxPayable.ToString());
                    writer.WriteElementString(nameof(invoice.DocumentTotals.NetTotal), invoice.DocumentTotals?.NetTotal.ToString());
                    writer.WriteElementString(nameof(invoice.DocumentTotals.GrossTotal), invoice.DocumentTotals?.GrossTotal.ToString());
                    if (invoice.DocumentTotals?.Payments != null)
                    {
                        invoice.DocumentTotals.Payments.ForEach(p =>
                        {
                            writer.WriteStartElement(nameof(Payment)); // Abre o elemento Payment
                            writer.WriteElementString(nameof(p.PaymentMechanism), p.PaymentMechanism);
                            writer.WriteElementString(nameof(p.PaymentAmount), p.PaymentAmount.ToString());
                            writer.WriteElementString(nameof(p.PaymentDate), p.PaymentDate?.ToString("yyyy-MM-dd"));
                            writer.WriteEndElement(); // Fecha o elemento Payment

                        });
                    }

                    if (invoice.DocumentTotals?.WithholdingTaxes != null)
                    {
                        invoice.DocumentTotals.WithholdingTaxes.ForEach(wt =>
                        {
                            writer.WriteStartElement(nameof(WithholdingTax)); // Abre o elemento WithholdingTax
                            writer.WriteElementString(nameof(wt.WithholdingTaxType), wt.WithholdingTaxType);
                            writer.WriteElementString(nameof(wt.WithholdingTaxAmount), wt.WithholdingTaxAmount.ToString());
                            writer.WriteEndElement(); // Fecha o elemento WithholdingTax
                        });
                    }

                    writer.WriteEndElement(); // Fecha o elemento DocumentTotals

                    writer.WriteEndElement(); // Fecha o elemento Invoice
                });
                //writer.WriteEndElement(); // Fecha o elemento Invoices

                writer.WriteEndElement(); // Fecha o elemento AuditFile
                writer.WriteEndDocument();
                writer.Flush();
            }

            //return stringWriter.ToString();
            //return Encoding.GetEncoding(1252).GetString(memoryStream.ToArray());
            return memoryStream.ToArray();
        }

        public ValidationResult Validate(FicheiroSAFT saftData)
        {
            throw new NotImplementedException();
        }

        private static string? TratamentoStringXML(string? conteudo)
        {
            if (string.IsNullOrWhiteSpace(conteudo))
            {
                return null;
            }

            StringBuilder sb = new();
            foreach (char c in conteudo!)
            {
                if (XmlConvert.IsXmlChar(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}