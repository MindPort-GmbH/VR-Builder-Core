using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.ProcessUtils;
using VRBuilder.Core.Properties;
using VRBuilder.Tests.Utils;

namespace VRBuilder.Core.Tests.Behaviors
{
    public class ArithmeticOperationBehaviorTests : BehaviorTests
    {
        protected override IBehavior CreateDefaultBehavior()
        {
            IDataProperty<float> property = CreatePropertyObject("Default Property Object", -67.5f);
            return new ArithmeticOperationBehavior(property, null, 5.6f, true, new MultiplyOperation());
        }

        protected IDataProperty<float> CreatePropertyObject(string name, float value)
        {
            GameObject propertyObject = new GameObject(name);
            NumberDataProperty property = propertyObject.AddComponent<NumberDataProperty>();
            property.SetValue(value);
            return property;

        }

        protected static TestCaseData[] ArithmeticOperationTestCases = new TestCaseData[]
        {
            new TestCaseData(34f, 84.3f, new SumOperation(), 118.3f).Returns(null),
            new TestCaseData(5f, 3f, new SubtractOperation(), 2f).Returns(null),
            new TestCaseData(6f, 4f, new MultiplyOperation(), 24f).Returns(null),
            new TestCaseData(7f, 2f, new DivideOperation(), 3.5f).Returns(null),
            new TestCaseData(34f, -84f, new SumOperation(), -50f).Returns(null),
            new TestCaseData(34f, 0, new MultiplyOperation(), 0f).Returns(null),
            new TestCaseData(34f, -2, new DivideOperation(), -17f).Returns(null),
            new TestCaseData(0f, 84.3f, new DivideOperation(), 0f).Returns(null),

        };

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator CreateByReference(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            // Given the necessary parameters,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", rightValue);

            // When we create the behavior passing process objects by reference,
            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftProperty, rightProperty, 5.6f, true, operationType);

            // Then all properties of the behavior are properly assigned.
            Assert.AreEqual(leftProperty, behavior.Data.ModifiedProperty.Value);
            Assert.AreEqual(rightProperty, behavior.Data.ModifierProperty.Value);
            Assert.AreEqual(5.6f, behavior.Data.ModifierConst);
            Assert.AreEqual(true, behavior.Data.IsModifierConst);
            Assert.AreEqual(operationType, behavior.Data.Operation);

            yield break;
        }

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator CreateByName(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            // Given the necessary parameters,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", rightValue);
            string leftPropertyName = leftProperty.SceneObject.UniqueName;
            string rightPropertyName = rightProperty.SceneObject.UniqueName;

            // When we create the behavior passing process objects by name,
            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftPropertyName, rightPropertyName, 5.6f, true, operationType);

            // Then all properties of the behavior are properly assigned.
            Assert.AreEqual(leftProperty, behavior.Data.ModifiedProperty.Value);
            Assert.AreEqual(rightProperty, behavior.Data.ModifierProperty.Value);
            Assert.AreEqual(5.6f, behavior.Data.ModifierConst);
            Assert.AreEqual(true, behavior.Data.IsModifierConst);
            Assert.AreEqual(operationType, behavior.Data.Operation);

            yield break;
        }

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator OperationOnProperty(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            // Given an arithmetic operation behavior,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", rightValue);
            Assert.AreEqual(leftValue, leftProperty.GetValue());
            Assert.AreEqual(rightValue, rightProperty.GetValue());

            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftProperty, rightProperty, 0f, false, operationType);

            // When it completes,
            behavior.LifeCycle.Activate();


            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // Then the target property is changed as expected.
            Assert.AreEqual(expectedResult, leftProperty.GetValue());
        }

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator OperationOnConstant(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            // Given an arithmetic operation behavior,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", 0f);

            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftProperty, rightProperty, rightValue, true, operationType);

            // When it completes,
            behavior.LifeCycle.Activate();

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // Then the target property is changed as expected.
            Assert.AreEqual(expectedResult, leftProperty.GetValue());
        }

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator FastForwardInactiveBehaviorAndActivateIt(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            //Given a behavior,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", rightValue);

            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftProperty, rightProperty, 5.6f, false, operationType);

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then it autocompletes immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.AreEqual(expectedResult, leftProperty.GetValue());

            yield break;
        }

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator FastForwardInactiveBehaviorAndDeactivateIt(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            //Given a behavior,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", rightValue);

            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftProperty, rightProperty, 5.6f, false, operationType);

            // When we mark it to fast-forward, activate and immediately deactivate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            behavior.LifeCycle.Deactivate();

            // Then it autocompletes immediately.
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);
            Assert.AreEqual(expectedResult, leftProperty.GetValue());
        }

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator FastForwardActivatingBehavior(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            //Given a behavior,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", rightValue);

            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftProperty, rightProperty, 5.6f, false, operationType);

            behavior.LifeCycle.Activate();

            while (behavior.LifeCycle.Stage != Stage.Activating)
            {
                yield return null;
                behavior.Update();
            }

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it autocompletes immediately.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.AreEqual(expectedResult, leftProperty.GetValue());
        }

        [UnityTest]
        [TestCaseSource(nameof(ArithmeticOperationTestCases))]
        public IEnumerator FastForwardDeactivatingBehavior(float leftValue, float rightValue, IProcessVariableOperation<float, float> operationType, float expectedResult)
        {
            //Given a behavior,
            IDataProperty<float> leftProperty = CreatePropertyObject("Left Property Object", leftValue);
            IDataProperty<float> rightProperty = CreatePropertyObject("Right Property Object", rightValue);

            ArithmeticOperationBehavior behavior = new ArithmeticOperationBehavior(leftProperty, rightProperty, 5.6f, false, operationType);

            behavior.LifeCycle.Activate();

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            behavior.LifeCycle.Deactivate();

            while (behavior.LifeCycle.Stage != Stage.Deactivating)
            {
                yield return null;
                behavior.Update();
            }

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it autocompletes immediately.
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);
            Assert.AreEqual(expectedResult, leftProperty.GetValue());
        }
    }
}