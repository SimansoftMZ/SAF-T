using SAFT.Mozambique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT.Examples.SampleData.Invoices
{
    public static class MozambiqueInvoices
    {
        // Fatura básica válida
        public static DocumentoFacturao GetValidInvoice()
        {
            return new DocumentoFacturao
            {
                TipoDocumentoId = "1",
                Categoria = CategoriaDocumento.Factura,
                NumeroDocumento = "1",
                DataHora = DateTime.Now.AddDays(-5),
                DataEmissao = DateTime.Now.AddDays(-5),
                OperadorEmissao = "Operador 1",
                ClienteId = "Cliente 1",

                Id = "INV-MZ-2024-001",
                Date = DateTime.Now.AddDays(-5),
                TotalAmount = 15000.50m,
                TaxAmount = 3000.10m,
                NUIT = "123456789", // NUIT válido (9 dígitos)
                Customer = Companies.GetMozCustomer("Acme Ltd")
            };
        }

        //// Fatura inválida (para testes de validação)
        //public static DocumentoFacturao GetInvalidInvoice()
        //{
        //    return new DocumentoFacturao
        //    {
        //        Id = "", // Campo obrigatório faltando
        //        NUIT = "123" // NUIT inválido (apenas 3 dígitos)
        //    };
        //}

        //// Lista de faturas (exemplo de lote)
        //public static List<DocumentoFacturao> GetInvoiceBatch(int count = 5)
        //{
        //    var invoices = new List<DocumentoFacturao>();
        //    for (int i = 1; i <= count; i++)
        //    {
        //        invoices.Add(new MozInvoice
        //        {
        //            Id = $"INV-MZ-2024-{i:000}",
        //            Date = DateTime.Now.AddDays(-i),
        //            TotalAmount = 1000.00m * i,
        //            NUIT = "987654321"
        //        });
        //    }
        //    return invoices;
        //}
    }
}
