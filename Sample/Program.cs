﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MsgPack;

class Program
{
    static async Task Main()
    {
        var configuration = new EndpointConfiguration("MsgPackSerializerSample");
        configuration.UseSerialization<MsgPackSerializer>();
        configuration.EnableInstallers();
        configuration.SendFailedMessagesTo("error");
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.UseTransport<LearningTransport>();

        var endpoint = await Endpoint.Start(configuration);

        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await endpoint.SendLocal(message);
        var startSaga = new StartSaga
        {
            TheId = Guid.NewGuid(),
        };
        await endpoint.SendLocal(startSaga);
        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.Read();
        await endpoint.Stop();
    }
}