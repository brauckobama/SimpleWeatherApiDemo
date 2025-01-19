namespace SimpleWeatherApiDemo.Interfaces
{
    public interface IWeatherCacheService
    {
        public void Add(object key, object value, DateTime? timeToExpire);
        public object? Get(object key);
    }
}
