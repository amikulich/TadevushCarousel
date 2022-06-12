using Azure;
using Azure.Data.Tables;

namespace TadevushCarousel.Domain
{
    public class Car : ITableEntity
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string Manufacturer { get; set; }

        public int NumOfGames { get; set; }

        public int Wins { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }
}
