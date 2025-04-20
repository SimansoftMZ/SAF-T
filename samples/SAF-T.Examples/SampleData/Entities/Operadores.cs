using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Examples.SampleData.Entities
{
    public class Operadores
    {
        private static readonly List<Operador> _operadores =
        [
            new Operador { Id = "1", Nome = "Operador 1" },
            new Operador { Id = "2", Nome = "Operador 2" },
            new Operador { Id = "3", Nome = "Operador 3" }
        ];

        public static List<Operador> GetOperadores()
        {
            return _operadores;
        }

        public static Operador? GetOperador(string id)
        {
            return _operadores.FirstOrDefault(o => o.Id == id);
        }
    }
}