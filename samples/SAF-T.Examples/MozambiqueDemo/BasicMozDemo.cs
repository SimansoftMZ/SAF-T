using Simansoft.SAFT.Core.Models;
using Simansoft.SAFT.Examples.SampleData.Entities;
using Simansoft.SAFT.Examples.SampleData.Invoices;
using Simansoft.SAFT.Mozambique.Generators;
using Simansoft.SAFT.Mozambique.Models;
using Simansoft.SAFT.Mozambique.Utils;

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

            // Console.WriteLine(xml);
            // Console.ReadKey(true);
            // Console.Clear();


            //var verificaDocumento1 = VerificarAssinatura(
            //    "id composto do documento",
            //    "assinatura do documento"
            //    , "-----BEGIN PUBLIC KEY----------END PUBLIC KEY-----");

            //var verificaDocumento2 = VerificarAssinatura(
            //    "id composto do documento",
            //    "assinatura do documento"
            //    , "-----BEGIN PUBLIC KEY----------END PUBLIC KEY-----");



            // var documento = documentosFacturacao.Select(s => new DocumentoParaHash
            // {
            //     // NumeroCertificadoAplicacaoEmissora = s.NumeroCertificadoAplicacaoEmissora,
            //     // VersaoChave = s.VersaoChave,
            //     DocumentoFacturacaoData = s.DataHora,
            //     DocumentoFacturacaoDataRegisto = s.DataEmissao,
            //     Categoria = s.Categoria,
            //     TipoDocumentoId = s.TipoDocumentoId,
            //     NumeroDocumento = s.NumeroDocumento,
            //     DocumentoFacturacaoTotal = s.TotalGeral,
            //     HashDocumentoAnterior = string.Empty
            // }).FirstOrDefault();

            var signerChavePrivada = new SaftDocumentSigner();

            //string chavePrivadaPEM = "-----BEGIN RSA PRIVATE KEY----- -----END RSA PRIVATE KEY-----";
            //signerChavePrivada.LoadPrivateKey(chavePrivadaPEM);

            // Documento 1
            var documento1 = new DocumentoParaHash
            {
                DocumentoFacturacaoData = new DateTime(2024, 12, 9),
                DocumentoFacturacaoDataRegisto = new DateTime(2024, 12, 9, 11, 22, 19),
                TipoDocumentoId = "2024",
                NumeroDocumento = "20",
                DocumentoFacturacaoTotal = 100.02m,
                Categoria = CategoriaDocumento.Factura,
                HashDocumentoAnterior = string.Empty
            };

            string hash1 = signerChavePrivada.SignSaftDocument(documento1.DadosCompostosParaHash);

            // Documento 2
            var documento2 = new DocumentoParaHash
            {
                DocumentoFacturacaoData = new DateTime(2024, 12, 9),
                DocumentoFacturacaoDataRegisto = new DateTime(2024, 12, 9, 15, 43, 25),
                TipoDocumentoId = "2024",
                NumeroDocumento = "21",
                DocumentoFacturacaoTotal = 200.34m,
                Categoria = CategoriaDocumento.Factura,
                HashDocumentoAnterior = hash1
            };

            string hash2 = signerChavePrivada.SignSaftDocument(documento2.DadosCompostosParaHash);

            //string hash2 = documento2.Assinar(rsa);
            // Cria um documento semelhante ao que está presente no regulamento publicado pelo governo de Moçambique



            // var documento2 = new DocumentoParaHash
            // {
            //     DocumentoFacturacaoData = new DateTime(2013, 7, 1),
            //     DocumentoFacturacaoDataRegisto = new DateTime(2023, 01, 01, 11, 7, 28),
            //     TipoDocumentoId = "001",
            //     NumeroDocumento = "0009",
            //     DocumentoFacturacaoTotal = 200.00m,
            //     Categoria = CategoriaDocumento.FacturaSimplificada,
            //     HashDocumentoAnterior = "mYJEv4iGwLcnQbRD7dPs2uD1mX08XjXIKcGg3GEHmwMhmmGYusffIJjTdSITLX+uujTwzqmL/U5nvt6S9s8ijN3LwkJXsiEpt099e1MET/8y3+Y1bN+K+YPJQiVmlQS0fXETsOPo8SwUZdBALt0vTo1VhUZKejACcjEYJ9G6nI="
            // };

            Console.WriteLine($"Dados compostos para hash 1: {documento1.DadosCompostosParaHash}");
            Console.WriteLine($"Dados compostos para hash 2: {documento2.DadosCompostosParaHash}");
            Console.WriteLine(string.Empty);
            Console.WriteLine(string.Empty);
            Console.WriteLine("==== Hashes ====");
            Console.WriteLine($"Hash 1: {hash1}");
            Console.WriteLine($"Dados 1: {documento1.DadosCompostosParaHash}");
            Console.WriteLine(string.Empty);
            Console.WriteLine($"Hash 2: {hash2}");
            Console.WriteLine($"Dados 1: {documento2.DadosCompostosParaHash}");

            // Console.WriteLine($"Dados compostos para hash 2: {documento2.DadosCompostosParaHash}");


            // if (documento2 != null)
            // {
            //     // Gerar o hash do documento
            //     var hashGerado = documento2.GerarHash();
            //     Console.WriteLine($"Dados compostos para hash: {documento2.DadosCompostosParaHash}");
            //     Console.WriteLine($"Hash gerado: {hashGerado}");
            // }
            // else
            // {
            //     Console.WriteLine("Nenhum documento encontrado. Não foi possível gerar o hash 2.");
            // }
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("===== Validando as assinaturas =====");
            string chavePublicaPEM = signerChavePrivada.ExportPublicKey();

            var signerChavePublica = new SaftDocumentSigner();

            signerChavePublica.LoadPublicKey(chavePublicaPEM);

            Console.WriteLine("Verificando assinatura do documento 1...");
            bool doc1BemAssinado = signerChavePublica.Verify(documento1.DadosCompostosParaHash, hash1);
            Console.WriteLine("Assinatura do documento 1: " + (doc1BemAssinado ? "Válida" : "Inválida"));
            Console.WriteLine(string.Empty);
            Console.WriteLine("Verificando assinatura do documento 2...");
            bool doc2BemAssinado = signerChavePublica.Verify(documento2.DadosCompostosParaHash, hash2);
            Console.WriteLine("Assinatura do documento 2: " + (doc2BemAssinado ? "Válida" : "Inválida"));
            Console.ReadKey(true);
        }
    }
}