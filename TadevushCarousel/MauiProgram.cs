using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using CommunityToolkit.Maui;
using TadevushCarousel.DataAccess;
using TadevushCarousel.Domain;

namespace TadevushCarousel;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            //.UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        string connectionString = Environment.GetEnvironmentVariable("BlobConnectionString");
        
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        builder.Services
            .AddSingleton<IRandomizer, Randomizer>(s =>
            {
                var blobContainer = new BlobContainerClient(connectionString, "carmash-map");

                var blob = blobContainer.GetBlockBlobClient("carmash-map.txt");
                var stream = blob.DownloadStreaming();

                var files = new List<string>();
                using (var streamReader = new StreamReader(stream.Value.Content))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        files.Add(line);
                    }
                }

                return new Randomizer(files);
            })
            .AddSingleton<ICarStore, CarStore>(s =>
            {
                var blobContainer = new BlobContainerClient(connectionString, "carmash");

                var tableStorage = new TableServiceClient(
                    new Uri("https://satkarouseldev.table.core.windows.net"),
                    new TableSharedKeyCredential("satkarouseldev",
                    Environment.GetEnvironmentVariable("StorageAccountToken")));

                var tableClient = tableStorage.GetTableClient("cars");

                return new CarStore(blobContainer, tableClient);
            });

		return builder.Build();
	}
}
