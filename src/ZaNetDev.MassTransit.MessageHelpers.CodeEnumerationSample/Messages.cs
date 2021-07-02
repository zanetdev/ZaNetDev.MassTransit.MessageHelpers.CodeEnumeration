using System;
using Newtonsoft.Json;
using ZaNetDev.MassTransit.MessageHelpers.CodeEnumeration;

namespace ZaNetDev.MassTransit.Helpers.MessageEnumerationClassSample
{
    public interface SubmitOrder
    {
        Guid OrderId { get; }
        DateTimeOffset OrderDateTime { get; }
        MyStatusEnum Status { get; }
    }

    [JsonConverter(typeof(CodeEnumerationJsonConverter<MyStatusEnum>))]
    public class MyStatusEnum : CodeEnumeration<MyStatusEnum>
    {
        public static MyStatusEnum Started = new MyStatusEnum("st");
        public static MyStatusEnum Finished = new MyStatusEnum("fn");
        public static MyStatusEnum Status1 = new MyStatusEnum("s1");
        public static MyStatusEnum Status2 = new MyStatusEnum("s2");
        public static MyStatusEnum Status3 = new MyStatusEnum("s3");
        public static MyStatusEnum NullStatus = null;

        public MyStatusEnum(string code) : base(code: code)
        {
        }
    }
}