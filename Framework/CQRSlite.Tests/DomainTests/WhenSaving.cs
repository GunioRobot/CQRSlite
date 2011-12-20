﻿using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Domain;
using CQRSlite.Eventing;
using CQRSlite.Tests.TestSubstitutes;
using NUnit.Framework;

namespace CQRSlite.Tests.DomainTests
{
	[TestFixture]
    public class When_saving
    {
        private TestEventStore _eventStore;
        private TestAggregateNoParameterLessConstructor _aggregate;
        private TestEventPublisher _eventPublisher;
        private Repository<TestAggregateNoParameterLessConstructor> _rep;

		[SetUp]
        public void Setup()
        {
            _eventStore = new TestEventStore();
            _eventPublisher = new TestEventPublisher();
            var snapshotstore = new NullSnapshotStore();
            _rep = new Repository<TestAggregateNoParameterLessConstructor>(_eventStore, snapshotstore, _eventPublisher);
            _aggregate = new TestAggregateNoParameterLessConstructor(2);

        }

        [Test]
        public void Should_save_uncommited_changes()
        {
            _aggregate.DoSomething();
            _rep.Save(_aggregate, 0);
            Assert.AreEqual(1, _eventStore.SavedEvents);
        }

        [Test]
        public void Should_mark_commited_after_save()
        {
            _aggregate.DoSomething();
            _rep.Save(_aggregate, 0);
            Assert.AreEqual(0, _aggregate.GetUncommittedChanges().Count());
        }

        [Test]
        public void ShouldThrowConcurrencyException()
        {
            Assert.Throws<ConcurrencyException>(() =>  _rep.Save(_aggregate, 1));
        }

        [Test]
        public void ShouldPublishEvents()
        {
            _aggregate.DoSomething();
            _rep.Save(_aggregate, 0);
            Assert.AreEqual(1, _eventPublisher.Published);
        }

        [Test]
        public void ShouldAddNewAggregate()
        {
            var agg = new TestAggregateNoParameterLessConstructor(1,Guid.Empty);
            agg.DoSomething();
            _rep.Save(agg,0);
            Assert.AreEqual(1, _eventStore.SavedEvents);
        }
    }
}
