using Newtonsoft.Json;

namespace MyCountsTest.Tools
{
    public static class MySessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            //извлечи стойностите и ги десериализирай
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            //съхрани в сесията серализираните стойности
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
