using Akka.Actor;
using System.Collections.Generic;
using System.Linq;
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

    public Master(Func<IActorRefFactory, IActorRef> maker) {
        requestsMapping = new Dictionary<int, IActorRef>();
        children = new List<IActorRef>();
        
        Receive<Initialize>(msg => {
            children = Enumerable.Range(1, msg.nChildren)
                        .Select(_ => maker(Context)).ToList();
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
        });
    }
}

public class Worker : ReceiveActor {
    public Worker() {
        Receive<Master.WordCountTask>(msg => 
            Sender.Tell(new Master.WordCountReply(msg.id, msg.text.Split(" ").Count())));
    }
}