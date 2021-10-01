using Akka.Actor;
using System.Collections.Generic;
using System.Linq;
using Akka.Event;
using System;

public class Master : ReceiveActor {
    public record Initialize(int nChildren);
    public record Work(int id, string text);
    public record WordCountReply(int id, int count);
    public record WordCountTask(int id, string text);

    private List<IActorRef> children;
    private int childIndex;
    private int taskId;
    Dictionary<int, IActorRef> requestsMapping;
    private readonly ILoggingAdapter log = Logging.GetLogger(Context);

    public Master() {
        requestsMapping = new Dictionary<int, IActorRef>();
        children = new List<IActorRef>();
        
        Receive<Initialize>(msg => {
            children = Enumerable.Range(1, msg.nChildren)
                        .Select(_ => Context.ActorOf(Props.Create(() => new Worker()))).ToList();
            Become(Initialized);
        });
    }

    public void Initialized() {
        Receive<string>(msg => {
            children[childIndex].Tell(new WordCountTask(taskId, msg));
            requestsMapping.Add(taskId, Sender);
            childIndex = (childIndex + 1) % children.Count;
            taskId = taskId + 1;
        });

        Receive<WordCountReply>(msg => {
            requestsMapping[msg.id].Tell(msg.count);
            requestsMapping.Remove(msg.id);
            log.Info($"[Reply received] Word count: {msg.count}");
        });
    }
}

public class Worker : ReceiveActor {
    public Worker() {
        Receive<Master.WordCountTask>(msg => 
            Sender.Tell(new Master.WordCountReply(msg.id, msg.text.Split(" ").Count())));
    }
}