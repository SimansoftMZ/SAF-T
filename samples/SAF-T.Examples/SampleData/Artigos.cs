using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Examples.SampleData
{
    public class Artigos
    {
        private static readonly List<Artigo> _artigos =
        [
            new Artigo {
                UniqueId = "X1",
                ArtigoId = "1",
                Descricao = "Artigo 1",
                PrecoUnitario = 100.0m,
                IVAIncluso = false,
                Servico = false
            },
            new Artigo {
                UniqueId = "X2",
                ArtigoId = "2",
                Descricao = "Servico 1",
                PrecoUnitario = 200.0m,
                IVAIncluso = false,
                Servico = true,
                Impostos =
                [
                    new Imposto
                    {
                        Tabela = 6,
                        CodigoTaxa = CodigoTaxa.TaxaReduzida,
                        TipoTaxa = TipoTaxa.IVA,
                        Descricao = "IVA",
                        Percentagem = 5.0m
                    }
                ]
            },
            new Artigo {
                UniqueId = "X3",
                ArtigoId = "3",
                Descricao = "Servico 2",
                PrecoUnitario = 300.0m,
                IVAIncluso = true,
                Servico = true,
                Impostos =
                [
                    new Imposto
                    {
                        Tabela = 6,
                        CodigoTaxa = CodigoTaxa.TaxaReduzida,
                        TipoTaxa = TipoTaxa.IVA,
                        Descricao = "IVA",
                        Percentagem = 5.0m
                    }
                ]
            },
            new Artigo {
                UniqueId = "X4",
                ArtigoId = "4",
                Descricao = "Artigo 2",
                PrecoUnitario = 500.0m,
                IVAIncluso = false,
                Servico = false
            },
            new Artigo {
                UniqueId = "X5",
                ArtigoId = "5",
                Descricao = "Artigo 3",
                PrecoUnitario = 500.0m,
                IVAIncluso = false,
                Servico = false
            }
        ];

        public static List<Artigo> GetArtigos()
        {
            return _artigos;
        }

        public static Artigo? GetArtigo(string id)
        {
            return _artigos.FirstOrDefault(a => a.ArtigoId == id);
        }
    }
}