﻿using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Examples.SampleData.Entities
{
    public class Clientes
    {
        private static readonly List<Cliente> _clientes =
        [
            new Cliente
            {
                Id = "Consumidor Final",
                Nome = "Consumidor Final",
                NUIT = "000000000"
            },
            new Cliente
            {
                Id = "1",
                Nome = "Cliente 1",
                NUIT = "123456789",
                Endereco = "Rua A, 123",
                Telefone = "+258-8X123456789"
            },
            new Cliente{
                Id = "2",
                Nome = "Cliente 2",
                NUIT = "987654321",
                Endereco = "Avenida B, 456",
                Telefone = "+258-8X987654321"
            }
        ];

        public static List<Cliente> GetClientes()
        {
            return _clientes;
        }

        public static Cliente? GetCliente(string id)
        {
            return _clientes.FirstOrDefault(c => c.Id == id);
        }
    }
}