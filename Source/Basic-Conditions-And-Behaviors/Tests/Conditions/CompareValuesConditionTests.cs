using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine.TestTools;
using VRBuilder.Core;
using VRBuilder.Core.Conditions;
using VRBuilder.Core.ProcessUtils;
using VRBuilder.Core.Properties;
using VRBuilder.Tests.Utils;

namespace VRBuilder.Core.Tests.Conditions
{
    public abstract class CompareValuesConditionTests<T> : ConditionTests where T : IEquatable<T>, IComparable<T>
    {
        protected abstract IDataProperty<T> CreateValueProperty(string name, T value);

        // TODO Constructor tests

        [UnityTest]
        public IEnumerator ConditionIsFulfilledWhenExpected(T leftValue, T rightValue, bool isLeftConst, bool isRightConst, IProcessVariableOperation<T, bool> operationType)
        {
            // Given a condition,
            IDataProperty<T> leftProperty = CreateValueProperty("Left Property Object", leftValue);
            IDataProperty<T> rightProperty = CreateValueProperty("Left Property Object", rightValue);
            CompareValuesCondition<T> condition = new CompareValuesCondition<T>(leftProperty, rightProperty, leftValue, rightValue, isLeftConst, isRightConst, operationType);

            condition.LifeCycle.Activate();

            while (condition.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                condition.Update();
            }

            // When a value changes,
            // TODO

            while (condition.IsCompleted == false)
            {
                yield return null;
                condition.Update();
            }

            // Then the condition updates accordingly.
            Assert.IsTrue(condition.IsCompleted);
        }
    }
}