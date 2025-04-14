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
            var invoice = new DocumentoFacturao
            {
                Categoria = CategoriaDocumento.Factura,
                TipoDocumentoId = "1",
                NumeroDocumento = "1",
                DataHora = DateTime.Now,
                DataEmissao = DateTime.Now,
                OperadorEmissao = "Operador 1",
                ClienteId = "Cliente 1",
                Artigos = 
                [
                    new DocumentoFacturacaoArtigo
                    {
                        Artigo = new()
                        {
                            ArtigoId = "P001",
                            Descricao = "Produto 1",
                            PrecoUnitario = 100
                        },
                        Impostos = [
                        
                            new Imposto
                            {
                                Codigo = "6",
                                Descricao = "IVA",
                                Percentagem = 5
                            }
                        ],
                        Quantidade = 1,
                        PrecoTotalComImpostos = 105
                    },
                    new DocumentoFacturacaoArtigo
                    {
                        Artigo = new()
                        {
                            ArtigoId = "P002",
                            Descricao = "Produto 2",
                            PrecoUnitario = 200
                        },
                        Impostos = [

                            new Imposto
                            {
                                Codigo = "1",
                                Descricao = "IVA",
                                Percentagem = 0
                            }
                        ],
                        Quantidade = 2,
                        PrecoTotalComImpostos = 400
                    }
                ]
            };

            Console.WriteLine("Demo Mozambique.");

            //// 2. Gerar XML
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
