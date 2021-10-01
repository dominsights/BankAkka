using Akka.Actor;
using Akka.TestKit.Xunit2;
using System.Text.RegularExpressions;
using Xunit;

namespace MoneyTransactionsTests.Actors
{
    public class MasterTests : TestKit {
        [Fact]
        public void testParentChild() {
            var probe = CreateTestProbe();
            var subject = Sys.ActorOf(Props.Create(() => new Master()));
            subject.Tell(new Master.Initialize(5));
            
            Regex regex = new Regex(@"\[Reply received] Word count: [0-9]+");

            EventFilter.Info(regex).Expect(3, () => {
                subject.Tell("Akka is awesome!");
                subject.Tell("Akka!");
                subject.Tell("Scala!");
            });
        }
    }
}