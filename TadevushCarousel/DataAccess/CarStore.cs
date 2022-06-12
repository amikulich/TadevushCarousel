using Azure.Data.Tables;
using Azure.Storage.Blobs;
using TadevushCarousel.Domain;

namespace TadevushCarousel.DataAccess
{
    public class CarStore : ICarStore
    {
        private readonly BlobContainerClient _container;
        private readonly TableClient _carsTable;

        public CarStore(BlobContainerClient container, TableClient carsTable)
        {
            _container = container;
            _carsTable = carsTable;
        }

        public Car GetCar(string id)
        {
            var fileName = $"{id}.jpg";
            var blob = _container.GetBlobClient(fileName);

            if (!blob.Exists())
            {
                throw new InvalidOperationException($"The image {fileName} doesn't exist");
            }

            Car car = null;
            var dbEntry = _carsTable.Query<Car>(filter: $"PartitionKey eq '{id}' and RowKey eq '{id}'");

            if (!dbEntry.Any<Car>())
            {
                car = new Car()
                {
                    Id = id,
                    ImageUrl = blob.Uri.AbsoluteUri.ToString(),
                    Wins = 0,
                    NumOfGames = 0,
                    PartitionKey = id,
                    RowKey = id,
                    Timestamp = DateTime.UtcNow,
                };
            }
            else
            {
                car = dbEntry.First<Car>();
            }

            return car;
        }

        public void SaveCar(Car car)
        {
            _carsTable.UpsertEntity(car, TableUpdateMode.Replace);
        }
    }
}