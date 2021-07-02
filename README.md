CodeEnumeration
================================

This simple utility base class is intended for use with MassTransit. Its purpose is to allow the addition of a field to a message contract (interface) that will be constrained to a set of predefined values.

One could use a regular enum however it is serialised as an integer. This is not always desired, and there is also the danger of having values shift if the order is changed.

The "enumeration class" is an idea I first encountered many years ago when I first began following the work of Steve Smith (Ardalis), and the code included here is largely based off of that initial idea. 

## Documentation
By way of example, below is a possible interface that could be published or sent as a message from MassTransit
```c#
public interface SubmitOrder
{
    Guid OrderId { get; }
    DateTimeOffset OrderDateTime { get; }
    MyStatusEnum Status { get; }
}
```

In the interface above you will note the last field (Status) is of type MyStatusEnum. This is defined below and is the Message Enumeration Class. As can be seen values are specified by declaring static fields with the same return type as the enclosing one. Note also the JsonConverter attribute being applied to the class. This is what allows the class to be serialised as its own code value, instead of as an object.  
```c#
[JsonConverter(typeof(CodeEnumerationJsonConverter<MyStatusEnum>))]
public class MyStatusEnum : CodeEnumeration<MyStatusEnum>
{
    public static MyStatusEnum Started = new MyStatusEnum("st");
    public static MyStatusEnum Finished = new MyStatusEnum("fn");

    public MyStatusEnum(string code) : base(code: code)
    {
    }
}
```

If you publish or send a message for the "SubmitOrder" interface:
```c#
await bus.Send<SubmitOrder>(
    new
    {
        OrderId = orderId,
        OrderDateTime = DateTimeOffset.Now,
        Status = MyStatusEnum.Started
    }
    );
```

This would be the resultant json message payload

```json
{
  "message": {
    "orderId": "7fb80000-5dc3-0015-0c2d-08d9334eacb9",
    "orderDateTime": "2021-06-19T20:18:44.7920189+02:00",
    "status": "st"
  }
}
```

Take note that the "status" field is simply a string "st" which is the code declared in the MyStatusEnum class.