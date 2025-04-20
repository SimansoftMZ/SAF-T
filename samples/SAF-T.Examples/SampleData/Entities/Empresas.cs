using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Examples.SampleData.Entities
{
    public class Empresas
    {
        private static List<Empresa> _empresas =
        [
            new Empresa
            {
                Nome = "Empresa 1",
                NomeComercial = "EMP 1",
                EstabelecimentoId = "Global",
                NUIT = "123456789",
                EACCode = "8610",
                Endereco1 = "Rua 1",
                Endereco2 = "Edificio 1",
                Cidade = "Cidade 1",
                Provincia = "Provincia 1",
                Pais = "MZ",
                CodigoPostal = "1234",
                Telefone = "123456789",
                Fax = "123456789",
                EdificioNumero = "123",
                Email = "info@empresa.com",
                Website = "www.empresa.com"
            },
            new Empresa
            {
                Nome = "Empresa 2",
                NomeComercial = "EMP 2",
                EstabelecimentoId = "Global",
                NUIT = "987654321",
                EACCode = "8611",
                Endereco1 = "Rua 2",
                Endereco2 = "Edificio 2",
                Cidade = "Cidade 2",
                Provincia = "Provincia 2",
                Pais = "MZ",
                CodigoPostal = "5678",
                Telefone = "987654321",
                Fax = "987654321",
                EdificioNumero = "456",
                Email = "info@empresa2.com",
                Website = "www.empresa2.com"
            }
        ];

        public static List<Empresa> GetEmpresas()
        {
            return _empresas;
        }

        public static Empresa GetEmpresaById(string id)
        {
            return _empresas.FirstOrDefault(e => e.NUIT == id)!;
        }
    }
}