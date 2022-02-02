﻿using System.Text.Json;
using Confluent.Kafka;

namespace KafkaServiceApi.Serializers;

public class SimpleKafKaSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}