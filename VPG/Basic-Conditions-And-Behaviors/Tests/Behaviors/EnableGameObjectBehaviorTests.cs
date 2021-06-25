using UnityEngine;
using System.Collections;
using VPG.Core.Behaviors;
using VPG.Tests.Builder;
using VPG.Core.Configuration;
using VPG.Core.SceneObjects;
using VPG.Tests.Utils;
using VPG.Tests.Utils.Mocks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace VPG.Core.Tests.Behaviors
{
    public class EnableGameObjectBehaviorTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator GameObjectIsEnabledAfterActivation()
        {
            // Given an active training scene object and a training with enable game object behavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("toEnable");
            toEnable.GameObject.SetActive(false);

            EndlessConditionMock trigger = new EndlessConditionMock();

            ICourse course = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicCourseStepBuilder("Step")
                        .Enable(toEnable)
                        .AddCondition(trigger)))
                .Build();

            course.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

            CourseRunner.Initialize(course);
            CourseRunner.Run();

            // When the behavior is activated
            CourseRunner.Initialize(course);
            CourseRunner.Run();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Active);

            // Then the training scene object is enabled.
            Assert.True(toEnable.GameObject.activeSelf);

            // Cleanup
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }

        [UnityTest]
        public IEnumerator GameObjectStaysEnabled()
        {
            // Given an active training scene object and a training with enalbe game object condition,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("toEnable");
            toEnable.GameObject.SetActive(false);

            EndlessConditionMock trigger = new EndlessConditionMock();

            ICourse course = new LinearTrainingBuilder("Training")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicCourseStepBuilder("Step")
                        .Enable(toEnable))
                    .AddStep(new BasicCourseStepBuilder("Step")
                        .AddCondition(trigger)))
                .Build();

            course.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

            // When the behavior is activated and after the step is completed
            CourseRunner.Initialize(course);
            CourseRunner.Run();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Active);

            trigger.Autocomplete();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Inactive);

            // Then the training scene object stays enabled.
            Assert.True(toEnable.GameObject.activeSelf);

            // Cleanup
            TestingUtils.DestroySceneObject(toEnable);
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehavior()
        {
            // Given an inactive training scene object and an EnableGameObjectBehavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("ToEnable");
            toEnable.GameObject.SetActive(false);

            EnableGameObjectBehavior behavior = new EnableGameObjectBehavior(toEnable);

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it doesn't autocomplete because it weren't activated yet.
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);

            // Cleanup.
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehaviorAndActivateIt()
        {
            // Given an inactive training scene object and a EnableGameObjectBehavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("ToEnable");
            toEnable.GameObject.SetActive(false);

            EnableGameObjectBehavior behavior = new EnableGameObjectBehavior(toEnable);

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then it should work without any differences because the behavior is done immediately anyways.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.IsTrue(toEnable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingBehavior()
        {
            // Given an inactive training scene object and an active EnableGameObjectBehavior,
            TrainingSceneObject toEnable = TestingUtils.CreateSceneObject("ToEnable");
            toEnable.GameObject.SetActive(false);

            EnableGameObjectBehavior behavior = new EnableGameObjectBehavior(toEnable);

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it should work without any differences because the behavior is done immediately anyways.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.IsTrue(toEnable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toEnable);

            yield break;
        }
    }
}
