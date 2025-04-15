using SAFT.Examples.SampleData.Invoices;
using SAFT.Mozambique.Generators;
using SAFT.Mozambique.Models;
using System;
using System.Buffers;
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
                DocumentosFacturacao = documentosFacturacao
            };

            var auditFile = gerador.ConverterParaSaft(ficheiroSAFT);

            // Serialize para JSON (mas com estrutura XML)
            var json = gerador.GenerateJson(auditFile);
            var xml = gerador.GenerateXml(auditFile);

            Console.WriteLine("Demo Mozambique.");
            Console.WriteLine("=== SAF-T para Moçambique ===");
            Console.WriteLine("=== JSON ===");
            Console.WriteLine(json);
            Console.ReadKey(false);
            Console.WriteLine("=== XML ===");

            Console.WriteLine(xml);
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
