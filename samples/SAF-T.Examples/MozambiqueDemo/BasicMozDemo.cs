using Simansoft.SAFT.Core.Models;
using Simansoft.SAFT.Examples.SampleData.Entities;
using Simansoft.SAFT.Examples.SampleData.Invoices;
using Simansoft.SAFT.Mozambique.Generators;
using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Examples.MozambiqueDemo
{
    public class BasicMozDemo
    {
        public static void Run()
        {
            // 1. Criar uma fatura de Moçambique
            List<DocumentoFacturacao> documentosFacturacao = MozambiqueInvoices.GetInvoices();

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

            AuditFile auditFile = gerador.ConverterParaSaft(ficheiroSAFT);

            // Serialize para JSON (mas com estrutura XML)
            //var json = gerador.GenerateJson(auditFile);
            string xml = gerador.GenerateXml(auditFile);

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