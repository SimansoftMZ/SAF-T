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
        public string GenerateJson(FicheiroSAFT saftData)
        {
            return JsonSerializer.Serialize(saftData, FicheiroSaftContext.Custom.FicheiroSAFT);
        }

        public string GenerateXml(FicheiroSAFT saftData)
        {
            try
            {
                var xml = RetornaXml(saftData.DocumentosFacturacao.FirstOrDefault()!);

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
                }
            };
            return auditFile;
        }

        private string RetornaXml(DocumentoFacturacao ficheiroSAFT)
        {
            var settings = new XmlWriterSettings { Indent = true };
            using StringWriter stringWriter = new();
            using XmlWriter writer = XmlWriter.Create(stringWriter, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("FicheiroSAFT");

            // Escreve os campos manualmente
            writer.WriteElementString(nameof(ficheiroSAFT.NumeroDocumento), ficheiroSAFT.NumeroDocumento);
            writer.WriteElementString(nameof(ficheiroSAFT.Id), ficheiroSAFT.Id);
            // ...

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return stringWriter.ToString();
        }
    }
}
