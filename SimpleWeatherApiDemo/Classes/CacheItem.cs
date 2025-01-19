namespace SimpleWeatherApiDemo.Classes
{
    public class CacheItem
    {
        public required object Value { get; set; }
        public DateTime? TimeToExpire { get; set; }
        public bool IsExpired { get {  return TimeToExpire < DateTime.UtcNow; } }
    }
}
