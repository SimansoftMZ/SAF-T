using Simansoft.SAFT.Mozambique.Models;

namespace Simansoft.SAFT.Examples.SampleData.Entities
{
    public class FabricantesSoftware
    {
        private static List<FabricanteSoftware> _fabricantesSoftware =
        [
            new FabricanteSoftware()
            {
                Nome = "Fabricante Software 1",
                NUIT = "132457689",
                SoftwareProdutoId = "SaftGen",
                SoftwareProdutoVersao = "1.0.0",
                SoftwareNumeroCertificacao = "ATCert0101"
            },
            new FabricanteSoftware()
            {
                Nome = "Fabricante Software 2",
                NUIT = "987654321",
                SoftwareProdutoId = "GenSaft",
                SoftwareProdutoVersao = "1.0.0",
                SoftwareNumeroCertificacao = "ATCert0102"
            },
        ];

        public static List<FabricanteSoftware> GetFabricantesSoftware()
        {
            return _fabricantesSoftware;
        }

        public static FabricanteSoftware GetFabricanteSoftwareById(string id)
        {
            return _fabricantesSoftware.FirstOrDefault(f => f.NUIT == id) ?? new FabricanteSoftware();
        }
    }
}