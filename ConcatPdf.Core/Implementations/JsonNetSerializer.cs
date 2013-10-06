using ConcatPdf.Core.Interfaces;
using Newtonsoft.Json;

namespace ConcatPdf.Core.Implementations
{
    public class JsonNetSerializer : IJsonSerializer
    {
        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
