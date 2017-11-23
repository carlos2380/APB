using Microsoft.WindowsAzure.MobileServices;
using System;

namespace Xamagram.Models
{
    public class Aviso
    {
        public string Id { get; set; }

        public string Tipo { get; set; }

        public string Descripcion { get; set; }

        public string UriFoto { get; set; }

        public double lon { get; set; }

        public double lat { get; set; }

        public bool cerrado { get; set; }

        [Version]
        public string AzureVersion { get; set; }
    }
}
