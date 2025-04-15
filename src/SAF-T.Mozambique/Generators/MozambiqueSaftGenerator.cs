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
                    AuditFileVersion = "1.0",
                    CompanyID = ficheiroSAFT.CompanyID,
                    TaxRegistrationNumber = ficheiroSAFT.TaxRegistrationNumber,
                    FileContentType = ficheiroSAFT.FileContentType,
                    CompanyName = ficheiroSAFT.CompanyName,
                    BusinessName = ficheiroSAFT.BusinessName,
                    CompanyAddress = new CompanyAddress
                    {
                        BuildingNumber = ficheiroSAFT.BuildingNumber,
                        City = ficheiroSAFT.City,
                        Country = ficheiroSAFT.Country,
                        District = ficheiroSAFT.District,
                        PostalCode = ficheiroSAFT.PostalCode,
                        StreetName = ficheiroSAFT.StreetName
                    },
                    FiscalYear = ficheiroSAFT.FiscalYear,
                    StartDate = ficheiroSAFT.StartDate,
                    EndDate = ficheiroSAFT.EndDate,
                    CurrencyCode = ficheiroSAFT.CurrencyCode,
                    DateCreated = DateTime.UtcNow, // Data de criação do arquivo
                    TaxEntity = "Mozambique",
                    ProductCompanyTaxID = "123456789", // Exemplo fictício
                    SoftwareCertificateNumber = "987654321", // Exemplo fictício
                    ProductID = "MySoftware",
                    ProductVersion = "1.0",
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
