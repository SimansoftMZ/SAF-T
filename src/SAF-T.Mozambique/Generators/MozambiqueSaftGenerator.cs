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

        //public string GenerateXml(FicheiroSAFT saftData)
        public string GenerateXml(FicheiroSAFT saftData)
        {
            try
            {

                var xml = retornaXml(saftData.DocumentosFacturacao.FirstOrDefault()!);

                return xml;

            }
            catch (InvalidOperationException ex)
            {
                // Log de erros de serialização
                throw new InvalidOperationException("Falha na serialização SAF-T", ex);
            }
        }

        public byte[] GenerateXmlBytes(FicheiroSAFT saftData)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(FicheiroSAFT saftData)
        {
            throw new NotImplementedException();
        }

        private string retornaXml(DocumentoFacturacao ficheiroSAFT)
        {
            var settings = new XmlWriterSettings { Indent = true };
            using var stringWriter = new StringWriter();
            using var writer = XmlWriter.Create(stringWriter, settings);

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
