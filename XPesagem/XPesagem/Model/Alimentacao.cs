using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPesagem.Model
{
    [DataTable("Alimentacao")]
    public class Alimentacao
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }
        public string Usuario { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        //public int Tipo { get; set; } //1 - Café 2 Lanche 3 Almoço 4 Lanche 5 Jantar 6 Lanche 

        [Microsoft.WindowsAzure.MobileServices.Version]
        public String AzureVersion { get; set; }
    }
}
