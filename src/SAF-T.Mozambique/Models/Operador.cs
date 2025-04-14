using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT.Mozambique.Models
{
    public record class Operador
    {
        public string Id { get; init; } = string.Empty;
        public string? Nome { get; init; } = string.Empty;
    }
}
