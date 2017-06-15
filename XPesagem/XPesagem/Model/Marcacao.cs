using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPesagem.Model
{
    [DataTable("Marcacoes")]
    public class Marcacao
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }
        public string Usuario { get; set; }
        public DateTime Data { get; set; }
        public float Peso { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public String AzureVersion { get; set; }
    }
}
