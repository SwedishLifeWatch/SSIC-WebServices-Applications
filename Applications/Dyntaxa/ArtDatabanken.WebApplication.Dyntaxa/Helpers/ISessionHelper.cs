namespace ArtDatabanken.WebApplication.Dyntaxa.Helpers
{
    public interface ISessionHelper
    {
         T GetFromSession<T>(string key);
         void SetInSession<T>(string key, T value);

         T GetFromCache<T>(string key);
         void SetInCache<T>(string key, T value);
    }
}