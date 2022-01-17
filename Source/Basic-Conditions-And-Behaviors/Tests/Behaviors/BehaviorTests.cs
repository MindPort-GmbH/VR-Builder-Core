using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using VRBuilder.Core.Behaviors;
using VRBuilder.Tests.Utils;

namespace VRBuilder.Core.Tests.Behaviors
{
    public abstract class BehaviorTests : RuntimeTests
    {
        protected abstract IBehavior CreateDefaultBehavior();       

        [UnityTest]
        public IEnumerator FastForwardInactiveBehavior()
        {
            // Given a behavior,
            IBehavior behavior = CreateDefaultBehavior();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it doesn't autocomplete because it hasn't been activated yet.
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehaviorAndActivateIt()
        {
            // Given a behavior,
            IBehavior behavior = CreateDefaultBehavior();

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then the behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingBehavior()
        {
            // Given a behavior,
            IBehavior behavior = CreateDefaultBehavior();

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then the behavior should be activated immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);

            yield break;
        }
    }
}