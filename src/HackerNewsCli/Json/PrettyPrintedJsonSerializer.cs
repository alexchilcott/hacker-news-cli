using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HackerNewsCli.Json
{
    public class PrettyPrintedJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T item)
        {
            using (var jsonWriter = new StringWriter())
            {
                var serializer = new JsonSerializer
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(jsonWriter, item);
                return jsonWriter.ToString();
            }
        }
    }
}