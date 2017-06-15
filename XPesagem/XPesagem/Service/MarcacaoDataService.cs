using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XPesagem.Interface;
using XPesagem.Model;

namespace XPesagem.Service
{
    public class MarcacaoDataService
    {
        SQLiteConnection database;

        public MarcacaoDataService() {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Marcacao>();
        }

        public bool SalvaItem(Marcacao marcacao)
        {
            try
            {
                if (!String.IsNullOrEmpty(marcacao.Id))
                {
                    database.Update(marcacao);
                }
                else
                {
                    database.Insert(marcacao);
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.GetBaseException();
                return false;
            }
        }
    }
}
