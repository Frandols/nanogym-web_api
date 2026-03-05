namespace Domain.ValueObjects
{
    public sealed record Location
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public string Address { get; }

        public Location(double latitude, double longitude, string address)
        {
            if (latitude is < -90 or > 90)
                throw new ArgumentOutOfRangeException(nameof(latitude));

            if (longitude is < -180 or > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude));

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address cannot be null or empty.", nameof(address));

            Latitude = latitude;
            Longitude = longitude;
            Address = address;
        }
    }
}
