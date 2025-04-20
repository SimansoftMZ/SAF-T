using SAFT.Examples.SampleData.Entities;
using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Examples.SampleData.Invoices
{
    public class MozambiqueInvoices
    {
        private static readonly List<DocumentoFacturacao> _documentosFacturacao =
            [
                new DocumentoFacturacao
                {
                    TipoDocumentoId = "1",
                    Categoria = CategoriaDocumento.Factura,
                    NumeroDocumento = "1",
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("1")!.Id,
                    Cliente = Clientes.GetCliente("1")!,
                    CodigoEAC = "8310",
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("1")!,
                                Quantidade = 3,
                                ValorDesconto = 50m,
                                PrecoTotalComImpostos = 315,
                            },
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("2")!,
                                Quantidade = 2,
                                ValorDesconto = 0m,
                                PrecoTotalComImpostos = 210
                            }
                        ]
                },
                new DocumentoFacturacao
                {
                    TipoDocumentoId = "1",
                    Categoria = CategoriaDocumento.Factura,
                    NumeroDocumento = "2",
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("2")!.Id,
                    Cliente = Clientes.GetCliente("1")!,
                    CodigoEAC = "8311",
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("3")!,
                                Quantidade = 3,
                                ValorDesconto = 50m,
                                PrecoTotalComImpostos = 315
                            },
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("4")!,
                                Quantidade = 2,
                                ValorDesconto = 0m,
                                PrecoTotalComImpostos = 600
                            },
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("5")!,
                                Quantidade = 1,
                                ValorDesconto = 0m,
                                PrecoTotalComImpostos = 150
                            }
                        ]
                },
                new DocumentoFacturacao
                {
                    TipoDocumentoId = "1",
                    Categoria = CategoriaDocumento.Factura,
                    NumeroDocumento = "3",
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("1")!.Id,
                    Cliente = Clientes.GetCliente("1")!,
                    CodigoEAC = "8311",
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("5")!,
                                Quantidade = 1,
                                ValorDesconto = 0m,
                                PrecoTotalComImpostos = 468
                            }
                        ]
                },
                new DocumentoFacturacao
                {
                    TipoDocumentoId = "1",
                    Categoria = CategoriaDocumento.Factura,
                    NumeroDocumento = "4",
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("3")!.Id,
                    Cliente = Clientes.GetCliente("2")!,
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("1")!,
                                Quantidade = 1,
                                ValorDesconto = 0m,
                                PrecoTotalComImpostos = 200
                            }
                        ]
                },
                new DocumentoFacturacao
                {
                    TipoDocumentoId = "24",
                    Categoria = CategoriaDocumento.VendaDinheiro,
                    NumeroDocumento = "1",
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("3")!.Id,
                    Cliente = Clientes.GetCliente("Consumidor Final")!,
                    CodigoEAC = "8310",
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("2")!,
                                Quantidade = 4,
                                ValorDesconto = 0m,
                                PrecoTotalComImpostos = 420
                            }
                        ]
                },
                new DocumentoFacturacao
                {
                    TipoDocumentoId = "24",
                    Categoria = CategoriaDocumento.VendaDinheiro,
                    NumeroDocumento = "2",
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("1")!.Id,
                    Cliente = Clientes.GetCliente("Consumidor Final")!,
                    CodigoEAC = "8311",
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("3")!,
                                Quantidade = 5,
                                ValorDesconto = 0m,
                                PrecoTotalComImpostos = 360
                            }
                        ],
                    MeiosPagamento =
                    [
                        new MeioPagamento()
                        {
                            Tipo = TipoMeioPagamento.Numerario,
                            Valor = 60
                        },
                        new MeioPagamento()
                        {
                            Tipo = TipoMeioPagamento.CartaoDebito,
                            Valor = 300
                        }
                    ]
                }
            ];

        private static readonly List<DocumentoFacturacao> _documentosFacturacaoInvalidos =
            [
            new DocumentoFacturacao
                {
                    TipoDocumentoId = "1",
                    Categoria = CategoriaDocumento.Factura,
                    NumeroDocumento = "", //Não possui o número do documento de facturação
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("1")!.Id,
                    Cliente = Clientes.GetCliente("1")!,
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("1")!,
                                Quantidade = 3,
                                ValorDesconto = 50m,
                                PrecoTotalComImpostos = 315
                            }
                        ]
                },
            new DocumentoFacturacao
                {
                    TipoDocumentoId = "24",
                    Categoria = CategoriaDocumento.VendaDinheiro,
                    NumeroDocumento = "1", //Não possui o número do documento de facturação
                    //Não possui dataHora
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("1")!.Id,
                    Cliente = Clientes.GetCliente("Consumidor Final")!,
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("1")!,
                                Quantidade = 3,
                                ValorDesconto = 50m,
                                PrecoTotalComImpostos = 315
                            }
                        ]
                },
            new DocumentoFacturacao
                {
                    TipoDocumentoId = "24",
                    Categoria = CategoriaDocumento.VendaDinheiro,
                    NumeroDocumento = "2", //Não possui o número do documento de facturação
                    //Não possui dataHora
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("1")!.Id,
                    Cliente = Clientes.GetCliente("Consumidor Final")!,
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("1")!,
                                Quantidade = 3,
                                ValorDesconto = 50m,
                                PrecoTotalComImpostos = 315
                            }
                        ]
                },
            new DocumentoFacturacao
                {
                    TipoDocumentoId = "1",
                    Categoria = CategoriaDocumento.VendaDinheiro,
                    NumeroDocumento = "3", //Não possui o número do documento de facturação
                    DataHora = DateTime.Now.AddDays(-5),
                    DataEmissao = DateTime.Now.AddDays(-5),
                    OperadorEmissao = Operadores.GetOperador("1")!.Id,
                    Cliente = Clientes.GetCliente("XX") ?? new Cliente(), //Cliente inválido
                    Artigos =
                        [
                            new DocumentoFacturacaoArtigo
                            {
                                Artigo = Artigos.GetArtigo("1")!,
                                Quantidade = 3,
                                ValorDesconto = 50m,
                                PrecoTotalComImpostos = 315
                            }
                        ]
                }
            ];

        public static List<DocumentoFacturacao> GetInvoices()
        {
            return _documentosFacturacao;
        }

        public static List<DocumentoFacturacao> GetInvalidInvoices()
        {
            return _documentosFacturacaoInvalidos;
        }

        public static DocumentoFacturacao GetInvoice(string id)
        {
            return _documentosFacturacao.FirstOrDefault(i => i.NumeroDocumento == id) ?? new DocumentoFacturacao();
        }

        public static DocumentoFacturacao GetValidInvoice(string id)
        {
            return _documentosFacturacaoInvalidos.FirstOrDefault(i => i.NumeroDocumento == id) ?? new DocumentoFacturacao();
        }
    }
}
