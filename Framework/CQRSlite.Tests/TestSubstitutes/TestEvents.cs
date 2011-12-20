﻿using CQRSlite.Eventing;

namespace CQRSlite.Tests.TestSubstitutes
{
    public class TestAggregateDidSomething : Event
    {

    }
    public class TestAggregateDidSomeethingElse : Event
    {

    }

    public class TestAggregateDidSomethingHandler : IHandles<TestAggregateDidSomething>
    {
        public void Handle(TestAggregateDidSomething message)
        {
            lock (message)
            {
                TimesRun++;
            }
        }

        public int TimesRun { get; set; }
    }
}
