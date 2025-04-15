using SAFT.Examples.SampleData.Invoices;
using SAFT.Mozambique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT.Examples.MozambiqueDemo
{
    public class BasicMozDemo
    {
        public static void Run()
        {
            // 1. Criar uma fatura de Moçambique
            var documentosFacturacao = MozambiqueInvoices.GetInvoices();
            

            Console.WriteLine("Demo Mozambique.");

            // 2. Gerar XML
            var generator = new MozambiqueSaftGenerator();
            string xml = generator.GenerateXml(invoice);

            //// 3. Gerar hash
            //string hash = HashUtility.GenerateSha256Hash(xml);

            //Console.WriteLine("=== SAF-T para Moçambique ===");
            //Console.WriteLine(xml);
            //Console.WriteLine($"\nHash: {hash}");

        }
    }
}
