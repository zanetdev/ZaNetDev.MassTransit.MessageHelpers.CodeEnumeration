using System;
using Newtonsoft.Json;

namespace ZaNetDev.MassTransit.MessageHelpers.CodeEnumeration
{
    /// <summary>
    /// This can be used with the JsonConvertor attribute on a CodeEnumeration type
    /// eg [JsonConverter(typeof(CodeEnumerationJsonConverter<MyStatusEnum>))]
    /// where MyStatusEnum is a CodeEnumeration
    /// This will have Masstransit serialize the type as a string code and deserialize the string code to the CodeEnumeration
    /// This is usefull where you have a field in a json object that must be constrained to certain predefined values. 
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class CodeEnumerationJsonConverter<TEnum> : JsonConverter<CodeEnumeration<TEnum>> where TEnum : CodeEnumeration<TEnum>
    {
        public override void WriteJson(JsonWriter writer, CodeEnumeration<TEnum> value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Code);
        }

        public override CodeEnumeration<TEnum> ReadJson(JsonReader reader, Type objectType, CodeEnumeration<TEnum> existingValue,bool hasExistingValue, JsonSerializer serializer)
        {
            var code = (string) reader.Value;
            return CodeEnumeration<TEnum>.FromCode(code);
        }
        
    }
}