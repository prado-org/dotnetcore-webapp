using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstProject.TodoApi
{
    public class AzureBlobStorage
    {
        private BlobContainerClient _containerClient;

        public AzureBlobStorage()
        {
            //teste
            string connectionString = "+LXpHumptdfyu/45QHCIPT1YsbyAoMNAwI0lIgjPFuJEl2LyflvnIvLHbjJxb73X7x7thzAcdyHE+AStjy9ClA==";
            string containerName = "myfirstproject";
            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            var blobList = new List<string>();
            await foreach (var blobItem in _containerClient.GetBlobsAsync())
            {
                blobList.Add(blobItem.Name);
            }
            return blobList;
        }


        
        
    }
}
