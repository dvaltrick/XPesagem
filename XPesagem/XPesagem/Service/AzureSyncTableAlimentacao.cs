using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XPesagem.Model;
using XPesagem.Service;

[assembly: Dependency(typeof(AzureSyncTableAlimentacao))]
namespace XPesagem.Service
{
    public partial class AzureSyncTableAlimentacao
    {
        static AzureSyncTable defaultInstance = new AzureSyncTable();

        IMobileServiceClient client;
        IMobileServiceSyncTable<Alimentacao> table;

        public AzureSyncTableAlimentacao()
        {
            this.client = new MobileServiceClient("https://xpesagem.azurewebsites.net");
            var store = new MobileServiceSQLiteStore("xpesagemdieta.db");
            store.DefineTable<Alimentacao>();
            this.client.SyncContext.InitializeAsync(store);
            this.table = client.GetSyncTable<Alimentacao>();
        }

        public async Task<IEnumerable> GetReg()
        {
            await SyncAsync();
            return await table.OrderBy(c => c.Data).ToEnumerableAsync();
        }

        public async Task<IEnumerable> GetReg(DateTime dataFiltro)
        {
            await SyncAsync();
            return await table.Where(c => (c.Data.Day == dataFiltro.Day)).OrderBy(c => c.Data).ToEnumerableAsync();
        }

        public async Task AddReg(Alimentacao dieta)
        {
            await table.InsertAsync(dieta);

            //Synchronize coffee
            await SyncAsync();
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                // Use a different query name for each unique query in your program.
                await this.table.PullAsync("todaDieta", this.table.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        // Update failed, revert to server's copy
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }

        }

    }
}
