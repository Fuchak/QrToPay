namespace QrToPay.Models.Common
{
    public sealed class AppState
    {
        public string? CityName { get; private set; }
        public Guid ServiceId { get; private set; }
        public int AttractionId { get; private set; }
        public string? ResortName { get; private set; }
        public decimal Price { get; private set; }
        public int Points { get; private set; }

        public AppState() { }

        public void UpdateCityName(string cityName) => CityName = cityName;
        public void UpdateServiceId(Guid serviceId) => ServiceId = serviceId;
        public void UpdateAttractionId(int attractionId) => AttractionId = attractionId;
        public void UpdateResortName(string resortName) => ResortName = resortName;
        public void UpdatePrice(decimal price) => Price = price;
        public void UpdatePoints(int points) => Points = points;
    }
}