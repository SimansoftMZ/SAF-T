using SAFT.Examples.SampleData.Entities;
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
                Empresa = Empresas.GetEmpresaById("123456789"),
                AnoFiscal = DateTime.Now.Year,
                DataInicial = DateTime.Now.AddMonths(-1),
                DataFinal = DateTime.Now,
                DataCriacao = DateTime.Now,
                Moeda = "MZN",
                TipoConteudo = ConteudoFicheiroSaft.Vendas,
                FabricanteSoftware = FabricantesSoftware.GetFabricanteSoftwareById("132457689"),
                ComentariosCabecario = "Submissão apenas para testes",
                DocumentosFacturacao = documentosFacturacao
            };

            var auditFile = gerador.ConverterParaSaft(ficheiroSAFT);

            // Serialize para JSON (mas com estrutura XML)
            //var json = gerador.GenerateJson(auditFile);
            var xml = gerador.GenerateXml(auditFile);

            // Gravar o XML num ficheiro
            //File.WriteAllText("saft_mozambique.xml", xml);

            Console.WriteLine("Demo Mozambique.");
            Console.WriteLine("=== SAF-T para Moçambique ===");
            //Console.WriteLine("=== JSON ===");
            //Console.WriteLine(json);
            //Console.ReadKey(false);
            Console.WriteLine("=== XML ===");

            Console.WriteLine(xml);
            Console.ReadKey(false);
        }
    }
}
