﻿// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="Asynkron HB">
//      Copyright (C) 2015-2016 Asynkron HB All rights reserved
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Proto;
using Proto.Remoting;
using Messages;
using ProtosReflection = Messages.ProtosReflection;

namespace Node2
{
    public class EchoActor : IActor
    {
        private PID _sender;

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case StartRemote sr:
                    Console.WriteLine("Starting");
                    _sender = sr.Sender;
                    context.Respond(new Start());
                    return Actor.Done;
                case Ping _:
                    _sender.Tell(new Pong());
                    return Actor.Done;
                default:
                    return Actor.Done;
            }            
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Serialization.RegisterFileDescriptor(ProtosReflection.Descriptor);
            RemotingSystem.Start("127.0.0.1", 8080);
            var props = Actor.FromProducer(() => new EchoActor());
            var pid = Actor.SpawnNamed(props, "remote");
            Console.ReadLine();
        }
    }
}