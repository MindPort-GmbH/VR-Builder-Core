using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Tests.Behaviors
{
    [TestFixture]    
    public class SetParentBehaviorTests : BehaviorTests
    {
        List<GameObject> spawnedObjects = new List<GameObject>();

        protected override IBehavior CreateDefaultBehavior()
        {
            ISceneObject target = SpawnTestObject("Target", Vector3.zero, Quaternion.identity, Vector3.one);
            ISceneObject parent = SpawnTestObject("Parent", Vector3.zero, Quaternion.identity, Vector3.one);
            return new SetParentBehavior(target, parent);
        }

        public ProcessSceneObject SpawnTestObject(string name, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent = null)
        {
            GameObject spawnedObject = new GameObject(name);
            spawnedObject.transform.SetPositionAndRotation(position, rotation);
            spawnedObject.transform.localScale = scale;
            spawnedObjects.Add(spawnedObject);            
            return spawnedObject.AddComponent<ProcessSceneObject>();
        }

        private static TestCaseData[] snapTestCases = new TestCaseData[]
        {
            new TestCaseData(new Vector3(3, -2, 6), Quaternion.Euler(-5, 7, 43)).Returns(null),
            new TestCaseData(new Vector3(75, 2, 8), Quaternion.Euler(123, 65, 41)).Returns(null),
            new TestCaseData(new Vector3(0, 0, 0), Quaternion.Euler(45, 2, -12)).Returns(null),
            new TestCaseData(new Vector3(5, -6, 2), Quaternion.Euler(0, 0, 0)).Returns(null),
        };

        [TearDown]
        public void DeleteAllObjects()
        {
            foreach(GameObject spawnedObject in spawnedObjects)
            {
                GameObject.DestroyImmediate(spawnedObject);
            }

            spawnedObjects.Clear();
        }

        [UnityTest]
        public IEnumerator ObjectIsParented()
        {
            // Given a set parent behavior,
            ProcessSceneObject target = SpawnTestObject("Target", Vector3.zero, Quaternion.identity, Vector3.one);
            ProcessSceneObject parent = SpawnTestObject("Parent", Vector3.zero, Quaternion.identity, Vector3.one);
            IBehavior behavior = new SetParentBehavior(target, parent);

            // When the behavior completes,
            behavior.LifeCycle.Activate();

            while(behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // Then the target object has been parented.
            Assert.AreEqual(parent.transform, target.transform.parent);
        }

        [UnityTest]        
        [TestCaseSource(nameof(snapTestCases))]
        public IEnumerator ObjectSnapsToParentIfSet(Vector3 parentPosition, Quaternion parentRotation)
        {
            // Given a set parent behavior,
            ProcessSceneObject target = SpawnTestObject("Target", Vector3.zero, Quaternion.identity, Vector3.one);
            ProcessSceneObject parent = SpawnTestObject("Parent", parentPosition, parentRotation, Vector3.one);
            IBehavior behavior = new SetParentBehavior(target, parent, true);

            // When the behavior completes,
            behavior.LifeCycle.Activate();

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // Then the target object has been parented, and it snaps to the parent's position.
            Assert.AreEqual(parent.transform, target.transform.parent);
            Assert.IsTrue(parent.transform.position == target.transform.position);
            Assert.IsTrue((parent.transform.rotation == target.transform.rotation));
        }

        [UnityTest]
        [TestCaseSource(nameof(snapTestCases))]
        public IEnumerator ObjectDoesNotSnapToParentIfNotSet(Vector3 parentPosition, Quaternion parentRotation)
        {
            // Given a set parent behavior,
            Vector3 originalPosition = new Vector3(456, 42, - 22);
            Quaternion originalRotation = Quaternion.Euler(34, -56, 190);
            ProcessSceneObject target = SpawnTestObject("Target", originalPosition, originalRotation, Vector3.one);
            ProcessSceneObject parent = SpawnTestObject("Parent", parentPosition, parentRotation, Vector3.one);
            IBehavior behavior = new SetParentBehavior(target, parent, false);

            // When the behavior completes,
            behavior.LifeCycle.Activate();

            while (behavior.LifeCycle.Stage != Stage.Active)
            {
                yield return null;
                behavior.Update();
            }

            // Then the target object has been parented, and it snaps to the parent's position.
            Assert.AreEqual(parent.transform, target.transform.parent);
            Assert.IsTrue(originalPosition == target.transform.position);
            Assert.IsTrue((originalRotation == target.transform.rotation));
        }
    }
}