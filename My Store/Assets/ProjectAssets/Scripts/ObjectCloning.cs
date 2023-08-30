using Newtonsoft.Json;

namespace Code.Helpers
{
    public static class ObjectCloning
    {
        public static T Clone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}