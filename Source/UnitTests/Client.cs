﻿using System;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Permissions;

namespace UnitTests
{
    public class Client
    {
        [SecurityPermission(SecurityAction.Demand)]
        public static void Main(string[] args)
        {
            // Create the channel.
            IpcChannel channel = new IpcChannel();

            // Register the channel.
            System.Runtime.Remoting.Channels.ChannelServices.
                RegisterChannel(channel);

            // Register as client for remote object.
            System.Runtime.Remoting.WellKnownClientTypeEntry remoteType =
                new System.Runtime.Remoting.WellKnownClientTypeEntry(
                    typeof(RemoteObject),
                    "ipc://localhost:9090/RemoteObject.rem");
            System.Runtime.Remoting.RemotingConfiguration.
                RegisterWellKnownClientType(remoteType);

            // Create a message sink. 
            string objectUri;
            System.Runtime.Remoting.Messaging.IMessageSink messageSink =
                channel.CreateMessageSink(
                    "ipc://localhost:9090/RemoteObject.rem", null,
                    out objectUri);
            Console.WriteLine("The URI of the message sink is {0}.",
                objectUri);
            if (messageSink != null)
            {
                Console.WriteLine("The type of the message sink is {0}.",
                    messageSink.GetType().ToString());
            }

            // Create an instance of the remote object.
            RemoteObject service = new RemoteObject();

            // Invoke a method on the remote object.
            Console.WriteLine("The client is invoking the remote object.");
            Console.WriteLine("The remote object has been called {0} times.",
                service.GetCount());
        }
    }
}