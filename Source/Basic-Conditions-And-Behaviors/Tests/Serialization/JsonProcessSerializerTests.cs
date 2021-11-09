using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRBuilder.Core.Audio;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.Conditions;
using VRBuilder.Core.Properties;
using VRBuilder.Tests.Builder;
using VRBuilder.Core.SceneObjects;
using VRBuilder.Tests.Utils;
using VRBuilder.Tests.Utils.Mocks;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace VRBuilder.Core.Tests.Serialization
{
    public class JsonProcessSerializerTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator ObjectInRangeCondition()
        {
            // Given a training with ObjectInRangeCondition,
            ProcessSceneObject testObjectToo = TestingUtils.CreateSceneObject("TestObjectToo");
            TransformInRangeDetectorProperty detector = testObjectToo.gameObject.AddComponent<TransformInRangeDetectorProperty>();
            ProcessSceneObject testObject = TestingUtils.CreateSceneObject("TestObject");

            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddCondition(new ObjectInRangeCondition(testObject, detector, 1.5f))))
                .Build();

            // When we serialize and deserialize it
            IProcess training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));

            // Then that condition's target, detector and range should stay unchanged.
            ObjectInRangeCondition condition1 = training1.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First().Data.Conditions.First() as ObjectInRangeCondition;
            ObjectInRangeCondition condition2 = training2.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First().Data.Conditions.First() as ObjectInRangeCondition;

            Assert.IsNotNull(condition1);
            Assert.IsNotNull(condition2);
            Assert.AreEqual(condition1.Data.Range, condition2.Data.Range);
            Assert.AreEqual(condition1.Data.Target.Value, condition2.Data.Target.Value);
            Assert.AreEqual(condition1.Data.ReferenceProperty.Value, condition2.Data.ReferenceProperty.Value);

            // Cleanup
            TestingUtils.DestroySceneObject(testObjectToo);
            TestingUtils.DestroySceneObject(testObject);

            return null;
        }

        [UnityTest]
        public IEnumerator TimeoutCondition()
        {
            // Given a training with a timeout condition
            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddCondition(new TimeoutCondition(2.5f))))
                .Build();

            // When we serialize and deserialize it
            IProcess training2 = Serializer.CourseFromByteArray((Serializer.CourseToByteArray(training1)));

            // Then that condition's timeout value should stay unchanged.
            TimeoutCondition condition1 = training1.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First().Data.Conditions.First() as TimeoutCondition;
            TimeoutCondition condition2 = training2.Data.FirstChapter.Data.FirstStep.Data.Transitions.Data.Transitions.First().Data.Conditions.First() as TimeoutCondition;

            Assert.IsNotNull(condition1);
            Assert.IsNotNull(condition2);
            Assert.AreEqual(condition1.Data.Timeout, condition2.Data.Timeout);

            return null;
        }

        [UnityTest]
        public IEnumerator MoveObjectBehavior()
        {
            // Given training with MoveObjectBehavior
            ProcessSceneObject moved = TestingUtils.CreateSceneObject("moved");
            ProcessSceneObject positionProvider = TestingUtils.CreateSceneObject("positionprovider");
            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(new MoveObjectBehavior(moved, positionProvider, 24.7f))))
                .Build();

            // When that training is serialized and deserialzied
            IProcess training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));

            // Then we should have two identical move object behaviors
            MoveObjectBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as MoveObjectBehavior;
            MoveObjectBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as MoveObjectBehavior;

            Assert.IsNotNull(behavior1);
            Assert.IsNotNull(behavior2);
            Assert.IsFalse(ReferenceEquals(behavior1, behavior2));
            Assert.AreEqual(behavior1.Data.Target.Value, behavior2.Data.Target.Value);
            Assert.AreEqual(behavior1.Data.PositionProvider.Value, behavior2.Data.PositionProvider.Value);
            Assert.AreEqual(behavior1.Data.Duration, behavior2.Data.Duration);

            // Cleanup created game objects.
            TestingUtils.DestroySceneObject(moved);
            TestingUtils.DestroySceneObject(positionProvider);

            return null;
        }

        //[UnityTest]
        //public IEnumerator LocalizedString()
        //{
        //    // Given a LocalizedString
        //    LocalizedString original = new LocalizedString("Test1{0}{1}", "Test2", "Test3", "Test4");

        //    Step step = new Step("");
        //    step.Data.Behaviors.Data.Behaviors.Add(new PlayAudioBehavior(new ResourceAudio(original), BehaviorExecutionStages.Activation));
        //    ICourse course = new Course("", new Chapter("", step));

        //    // When we serialize and deserialize a training with it
        //    ICourse deserializedCourse = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(course));
        //    PlayAudioBehavior deserializedBehavior = deserializedCourse.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as PlayAudioBehavior;
        //    // ReSharper disable once PossibleNullReferenceException
        //    LocalizedString deserialized = ((ResourceAudio)deserializedBehavior.Data.AudioData).Path;

        //    // Then deserialized training should has different instance of LocalizedString but with the same values.
        //    Assert.IsFalse(ReferenceEquals(original, deserialized));
        //    Assert.AreEqual(original.Key, deserialized.Key);
        //    Assert.AreEqual(original.DefaultText, deserialized.DefaultText);
        //    Assert.IsTrue(original.FormatParams.SequenceEqual(deserialized.FormatParams));

        //    yield return null;
        //}

        [UnityTest]
        public IEnumerator BehaviorSequence()
        {
            // Given a training with a behaviors sequence
            BehaviorSequence sequence = new BehaviorSequence(true, new List<IBehavior>
            {
                new DelayBehavior(0f),
                new EmptyBehaviorMock()
            });
            IProcess course = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(sequence)))
                .Build();

            // When we serialize and deserialize it
            IProcess deserializedCourse = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(course));

            BehaviorSequence deserializedSequence = deserializedCourse.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as BehaviorSequence;

            // Then the values stay the same.
            Assert.IsNotNull(deserializedSequence);
            Assert.AreEqual(sequence.Data.PlaysOnRepeat, deserializedSequence.Data.PlaysOnRepeat);

            List<IBehavior> behaviors = sequence.Data.Behaviors;
            List<IBehavior> deserializedBehaviors = deserializedSequence.Data.Behaviors;
            Assert.AreEqual(behaviors.First().GetType(), deserializedBehaviors.First().GetType());
            Assert.AreEqual(behaviors.Last().GetType(), deserializedBehaviors.Last().GetType());
            Assert.AreEqual(behaviors.Count, deserializedBehaviors.Count);
            yield break;
        }

        [UnityTest]
        public IEnumerator DelayBehavior()
        {
            // Given we have a training with a delayed activation behavior,
            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(new DelayBehavior(7f))))
                .Build();

            // When we serialize and deserialize it,
            byte[] serialized = Serializer.CourseToByteArray(training1);
            IProcess training2 = Serializer.CourseFromByteArray(serialized);

            // Then that delayed behaviors should have the same target behaviors and delay time.
            DelayBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as DelayBehavior;
            DelayBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as DelayBehavior;

            Assert.AreEqual(behavior1.Data.DelayTime, behavior2.Data.DelayTime);

            return null;
        }

        [UnityTest]
        public IEnumerator DisableGameObjectBehavior()
        {
            // Given DisableGameObjectBehavior,
            ProcessSceneObject trainingSceneObject = TestingUtils.CreateSceneObject("TestObject");

            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicProcessStepBuilder("Step")
                        .Disable("TestObject")))
                .Build();

            // When we serialize and deserialize a training with it
            byte[] serialized = Serializer.CourseToByteArray(training1);

            IProcess training2 = Serializer.CourseFromByteArray(serialized);

            DisableGameObjectBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as DisableGameObjectBehavior;
            DisableGameObjectBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as DisableGameObjectBehavior;

            // Then it's target training scene object is still the same.
            Assert.IsNotNull(behavior1);
            Assert.IsNotNull(behavior2);
            Assert.AreEqual(behavior1.Data.Target.Value, behavior2.Data.Target.Value);

            TestingUtils.DestroySceneObject(trainingSceneObject);

            return null;
        }

        [UnityTest]
        public IEnumerator EnableGameObjectBehavior()
        {
            // Given EnableGameObjectBehavior,
            ProcessSceneObject trainingSceneObject = TestingUtils.CreateSceneObject("TestObject");

            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicProcessStepBuilder("Step")
                        .Enable("TestObject")))
                .Build();

            // When we serialize and deserialize a training course with it
            IProcess training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));

            EnableGameObjectBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as EnableGameObjectBehavior;
            EnableGameObjectBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as EnableGameObjectBehavior;

            // Then it's target training scene object is still the same.
            Assert.IsNotNull(behavior1);
            Assert.IsNotNull(behavior2);
            Assert.AreEqual(behavior1.Data.Target.Value, behavior2.Data.Target.Value);

            TestingUtils.DestroySceneObject(trainingSceneObject);

            return null;
        }

#pragma warning disable 618
        [UnityTest]
        public IEnumerator LockObjectBehavior()
        {
            // Given a training with LockObjectBehavior
            ProcessSceneObject trainingSceneObject = TestingUtils.CreateSceneObject("TestObject");

            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(new LockObjectBehavior(trainingSceneObject))))
                .Build();

            // When we serialize and deserialize it
            IProcess training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));


            // Then that's behavior target is still the same.
            LockObjectBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as LockObjectBehavior;
            LockObjectBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as LockObjectBehavior;

            Assert.IsNotNull(behavior1);
            Assert.IsNotNull(behavior2);
            Assert.AreEqual(behavior1.Data.Target.Value, behavior2.Data.Target.Value);

            // Cleanup
            TestingUtils.DestroySceneObject(trainingSceneObject);

            return null;
        }
#pragma warning restore 618

        [UnityTest]
        public IEnumerator PlayAudioOnActivationBehavior()
        {
            // Given a training with PlayAudioOnActivationBehavior with some ResourceAudio
            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(new PlayAudioBehavior(new ResourceAudio("TestPath"), BehaviorExecutionStages.Activation))))
                .Build();

            // When we serialize and deserialize it,
            IProcess training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));

            // Then path to the audiofile should not change.
            PlayAudioBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as PlayAudioBehavior;
            PlayAudioBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as PlayAudioBehavior;

            Assert.IsNotNull(behavior1);
            Assert.IsNotNull(behavior2);
            Assert.AreEqual(TestingUtils.GetField<string>(behavior1.Data.AudioData, "path"), TestingUtils.GetField<string>(behavior2.Data.AudioData, "path"));

            return null;
        }

        [UnityTest]
        public IEnumerator PlayAudioOnDectivationBehavior()
        {
            // Given a training with PlayAudioOnDeactivationBehavior and some ResourceData,
            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(new PlayAudioBehavior(new ResourceAudio("TestPath"), BehaviorExecutionStages.Activation))))
                .Build();

            // When we serialize and deserialize it,
            IProcess training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));

            PlayAudioBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as PlayAudioBehavior;
            PlayAudioBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as PlayAudioBehavior;

            // Then path to audio file should not change.
            Assert.IsNotNull(behavior1);
            Assert.IsNotNull(behavior2);
            Assert.AreEqual(TestingUtils.GetField<string>(behavior1.Data.AudioData, "path"), TestingUtils.GetField<string>(behavior2.Data.AudioData, "path"));

            return null;
        }

#pragma warning disable 618
        [UnityTest]
        public IEnumerator UnlockObjectBehavior()
        {
            // Given a training with UnlockObjectBehavior
            ProcessSceneObject trainingSceneObject = TestingUtils.CreateSceneObject("TestObject");

            IProcess training1 = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(new UnlockObjectBehavior(trainingSceneObject))))
                .Build();

            // When we serialize and deserialize it
            IProcess training2 = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(training1));

            UnlockObjectBehavior behavior1 = training1.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as UnlockObjectBehavior;
            UnlockObjectBehavior behavior2 = training2.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First() as UnlockObjectBehavior;

            // Then that behavior's target should not change.
            Assert.IsNotNull(behavior1);
            Assert.IsNotNull(behavior2);
            Assert.AreEqual(behavior1.Data.Target.Value, behavior2.Data.Target.Value);

            // Cleanup
            TestingUtils.DestroySceneObject(trainingSceneObject);

            return null;
        }

        [UnityTest]
        public IEnumerator ResourceAudio()
        {
            // Given we have a ResourceAudio instance,
            ResourceAudio audio = new ResourceAudio("TestPath");

            IProcess course = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddBehavior(new PlayAudioBehavior(audio, BehaviorExecutionStages.Activation))))
                .Build();

            // When we serialize and deserialize a training with it
            IProcess testCourse = Serializer.CourseFromByteArray(Serializer.CourseToByteArray(course));

            // Then the path to audio resource should be the same.
            string audioPath1 = TestingUtils.GetField<string>(((PlayAudioBehavior)course.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First()).Data.AudioData, "path");
            string audioPath2 = TestingUtils.GetField<string>(((PlayAudioBehavior)testCourse.Data.FirstChapter.Data.FirstStep.Data.Behaviors.Data.Behaviors.First()).Data.AudioData, "path");

            Assert.AreEqual(audioPath1, audioPath2);

            return null;
        }
    }
}
