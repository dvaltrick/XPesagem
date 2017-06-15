using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPesagem.Model;

namespace XPesagem.Service
{
    public class Repositorio
    {
        public async Task<List<Marcacao>> GetMarcacoes()
        {
            var service = new Service.AzureService<Marcacao>();
            var Items = await service.GetTable();

            return Items.ToList();
        }
    }
}
