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

        //public string GenerateXml(FicheiroSAFT saftData)
        public string GenerateXml(FicheiroSAFT saftData)
        {
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(FicheiroSAFT));
            //using StringWriter stringWriter = new();
            //xmlSerializer.Serialize(stringWriter, saftData);
            //return stringWriter.ToString();

            try
            {
                var serializer = new XmlSerializer(typeof(FicheiroSAFT),
                    new Type[] { typeof(FicheiroSaftContext) });
                using var stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, saftData);
                return stringWriter.ToString();

            }
            catch (InvalidOperationException ex)
            {
                // Log de erros de serialização
                throw new NotImplementedException("Falha na serialização SAF-T", ex);
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
    }
}
