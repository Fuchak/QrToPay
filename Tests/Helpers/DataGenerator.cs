namespace Tests.Helpers
{
    public static class DataGenerator
    {
        private static readonly Random Random = new();

        private static readonly List<string> ActivationTokens =
        [
            "A007731C-90FD-4BA0-95D4-E4DE342F0A54",
            "E0920CBD-F501-472D-B166-F175325A2376",
            "D4270617-0A1C-4BFD-B696-148A1DAACFB8"
        ];

        private static readonly List<string> ServiceTypes =
        [
            "SkiResort",
            "FunFair"
        ];

        private static readonly List<string> QRCodes =
        [
            "ZAKFA123456", "KRAFA606060", "WAWFA232323", "ZAKFA787878",
            "KRAFA666666", "WAWFA131313", "ZAKSL404404", "KRYSL292929",
            "SZCSL95959", "ZAKSL76767", "KRYSL13131", "SZCSL37373"
        ];

        public static string GetRandomActivationToken() => ActivationTokens[Random.Next(ActivationTokens.Count)];

        public static string GetRandomServiceType() => ServiceTypes[Random.Next(ServiceTypes.Count)];

        public static string GetRandomQRCode() => QRCodes[Random.Next(QRCodes.Count)];
    }
}