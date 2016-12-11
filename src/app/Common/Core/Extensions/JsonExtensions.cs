using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Clinic.Common.Core.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            TextWriter writer = new StringWriter();
            var serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializer.Serialize(writer, obj);
            return writer.ToString();
            //return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
