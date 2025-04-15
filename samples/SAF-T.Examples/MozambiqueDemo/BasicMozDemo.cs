using SAFT.Examples.SampleData.Invoices;
using SAFT.Mozambique.Generators;
using SAFT.Mozambique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace SAFT.Examples.MozambiqueDemo
{
    public class BasicMozDemo
    {
        public static void Run()
        {
            // 1. Criar uma fatura de Moçambique
            var documentosFacturacao = MozambiqueInvoices.GetInvoices();

            MozambiqueSaftGenerator gerador = new();

            FicheiroSAFT ficheiroSAFT = new()
            {
                //DocumentosFacturacao = documentosFacturacao
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                TypeInfoResolver = FicheiroSaftContext.Default
            };

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            // Serialize para JSON (mas com estrutura XML)
            JsonSerializer.Serialize(writer, ficheiroSAFT, options);


            var resultXML = Encoding.UTF8.GetString(stream.ToArray());

            Console.WriteLine("Demo Mozambique.");
            Console.WriteLine(resultXML);
            Console.ReadKey(false);

            // 2. Gerar XML
            //var generator = new MozambiqueSaftGenerator();
            //string xml = generator.GenerateXml(invoice);

            //// 3. Gerar hash
            //string hash = HashUtility.GenerateSha256Hash(xml);

            //Console.WriteLine("=== SAF-T para Moçambique ===");
            //Console.WriteLine(xml);
            //Console.WriteLine($"\nHash: {hash}");

        }
    }
}
