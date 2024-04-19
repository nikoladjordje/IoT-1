using Grpc.Net.Client;
using GrpcClientIoT;

class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5240");
        
        var client = new Beehive.BeehiveClient(channel);

        var beehiveData = new BeehiveValue
        {
            Device = 1,
            HiveNumber = 5,
            Date = "2022-06-08 14:52:28",
            HiveTemp = 36.42f,
            HiveHumidity = 30.29f,
            HivePressure = 1007.45f,
            WeatherTemp = 26.68f,
            WeatherHumidity = 52,
            WeatherPressure = 1013,
            WindSpeed = 8.75f,
            WeatherID = 711,
            CloudCoverage = 75,
            Rain = 0,
            Lat = 37.29f,
            Lon = -121.95f,
            FileName = "2022-06-08--14-52-28_1.raw",
            QueenPresence = 1,
            QueenAcceptance = 2,
            Frames = 8,
            Target = 0,
            Time = 0.583f,
            QueenStatus = 0
        };

        try
        {
            var response = await client.AddBeehiveValueAsync(beehiveData);
            Console.WriteLine($"Message: {response.Message}");
            Console.WriteLine($"ID: {response.Id}");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.ReadLine();
    }
}
