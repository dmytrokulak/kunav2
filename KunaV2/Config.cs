using Microsoft.Extensions.Configuration;

namespace KunaV2
{
    internal static class Config
    {
        //ToDo:: use DI
        static readonly IConfiguration Settings = new ConfigurationBuilder().AddJsonFile("kunasettings.json").Build();

        internal static class ApiKey
        {
            internal static string Public => Settings.GetSection("apiKey")["public"];
            internal static string Private => Settings.GetSection("apiKey")["private"];
        }

        internal static class Fee
        {
            internal static decimal Maker = 0.0025M;
            internal static decimal Taker = 0.0025M;
        }
    }
}
