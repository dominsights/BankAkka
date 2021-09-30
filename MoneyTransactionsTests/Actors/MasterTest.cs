using Akka.Actor;
using Akka.TestKit.Xunit2;
using MoneyTransactions;
using MoneyTransactions.Actors;
using MoneyTransactions.Foundation;
using System;
using Xunit;

namespace MoneyTransactionsTests.Actors
{
    public class MasterTests : TestKit {
        [Fact]
        public void testParentChild() {
            var probe = CreateTestProbe();
            var subject = Sys.ActorOf(Props.Create(() => new Master(_ => probe.Ref)));
            subject.Tell(new Master.Initialize(5));
            subject.Tell("Akka is awesome!");
            probe.ExpectMsgAllOf<Master.WordCountTask>();
            probe.Send(subject, new Master.WordCountReply(0, 3));
            ExpectMsg<int>(msg => msg == 3);
            ExpectNoMsg();
        }
    }
}