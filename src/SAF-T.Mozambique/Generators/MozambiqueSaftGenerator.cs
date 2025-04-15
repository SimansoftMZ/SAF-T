using SAFT.Core.Interfaces;
using SAFT.Mozambique.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FicheiroSAFT));
            using StringWriter stringWriter = new();
            xmlSerializer.Serialize(stringWriter, saftData);
            return stringWriter.ToString();
        }

        public byte[] GenerateXmlBytes(FicheiroSAFT saftData)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(FicheiroSAFT saftData)
        {
            throw new NotImplementedException();
        }
    }
}
