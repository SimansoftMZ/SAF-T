using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT.Mozambique.Models
{
    public record class Cliente
    {
        public string Id { get; init; } = string.Empty;
        public string? Nome { get; init; } = string.Empty;
        public string? NUIT { get; init; } = string.Empty;
        public string? Endereco { get; init; } = string.Empty;
        public string? Telefone { get; init; } = string.Empty;
        public string? Email { get; init; } = string.Empty;
        public string? Pais { get; init; } = "MZ";
        public bool IsConsumidorFinal { get => Id == "Consumidor Final"; }
    }
}
