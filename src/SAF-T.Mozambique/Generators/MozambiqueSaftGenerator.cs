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
            var auditFile = new AuditFile
            {
                
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
