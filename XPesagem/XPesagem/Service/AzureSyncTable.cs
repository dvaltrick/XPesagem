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

[assembly: Dependency(typeof(AzureSyncTable))]
namespace XPesagem.Service
{
    public partial class AzureSyncTable
    {
        static AzureSyncTable defaultInstance = new AzureSyncTable();

        IMobileServiceClient client;
        IMobileServiceSyncTable<Marcacao> table;

        public AzureSyncTable() {
            this.client = new MobileServiceClient("https://xpesagem.azurewebsites.net");
            var store = new MobileServiceSQLiteStore("xpesagemdata.db");
            store.DefineTable<Marcacao>();
            this.client.SyncContext.InitializeAsync(store);
            this.table = client.GetSyncTable<Marcacao>();
        }


        public async Task Intialize()
        {
            //Create our client
            client = new MobileServiceClient("https://xpesagem.azurewebsites.net");

            const string path = "xpesagemdata.db";
            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Marcacao>();
            await client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            table = client.GetSyncTable<Marcacao>();
        }

        public async Task<IEnumerable> GetReg()
        {
            await SyncAsync();
            return await table.OrderByDescending(c => c.Data).ToEnumerableAsync();
        }

        public async Task AddReg(Marcacao marcacao)
        {
            await table.InsertAsync(marcacao);

            //Synchronize coffee
            await SyncAsync();
        }

        public async Task SyncAsync() {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                // Use a different query name for each unique query in your program.
                await this.table.PullAsync("todasMarcacoes", this.table.CreateQuery());
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
