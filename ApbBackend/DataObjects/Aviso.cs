using Microsoft.Azure.Mobile.Server;
namespace ApbBackend.DataObjects
{
    public class Aviso : EntityData
    {
        public string Tipo { get; set; }

        public string Descripcion { get; set; }

        public string UriFoto { get; set; }

        public double lon { get; set; }

        public double lat { get; set; }

        public bool cerrado { get; set; }

    }
}