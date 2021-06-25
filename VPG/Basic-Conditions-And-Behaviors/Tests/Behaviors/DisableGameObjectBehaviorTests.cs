using System.Collections;
using VPG.Core.Behaviors;
using VPG.Tests.Builder;
using VPG.Core.Configuration;
using VPG.Core.SceneObjects;
using VPG.Tests.Utils;
using VPG.Tests.Utils.Mocks;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace VPG.Core.Tests.Behaviors
{
    public class DisableGameObjectBehaviorTests : RuntimeTests
    {
        [UnityTest]
        public IEnumerator GameObjectIsDisabledAfterActivation()
        {
            // Given an active training scene object and a training course with disable game object behavior,
            TrainingSceneObject toDisable = TestingUtils.CreateSceneObject("ToDisable");
            EndlessConditionMock trigger = new EndlessConditionMock();
            
            BasicCourseStepBuilder basicStepBuilder = new BasicCourseStepBuilder("Step");

            ICourse course = new LinearTrainingBuilder("Course")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(basicStepBuilder
                        .Disable(toDisable)
                        .AddCondition(trigger)))
                .Build();

            course.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

            // When the behavior is activated
            CourseRunner.Initialize(course);
            CourseRunner.Run();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Active);

            trigger.Autocomplete();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[0].LifeCycle.Stage == Stage.Inactive);

            // Then the training scene object is disabled.
            Assert.False(toDisable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toDisable);

            yield break;
        }

        [UnityTest]
        public IEnumerator GameObjectStaysDisabled()
        {
            // Given an active training scene object and a training course with disable game object behavior,
            TrainingSceneObject toDisable = TestingUtils.CreateSceneObject("ToDisable");
            EndlessConditionMock trigger = new EndlessConditionMock();

            ICourse course = new LinearTrainingBuilder("Course")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicCourseStepBuilder("Step")
                        .Disable(toDisable))
                    .AddStep(new BasicCourseStepBuilder("Step")
                        .AddCondition(trigger)))
                .Build();

            course.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

            // When the behavior is activated and after the step is completed
            CourseRunner.Initialize(course);
            CourseRunner.Run();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[1].LifeCycle.Stage == Stage.Active);

            trigger.Autocomplete();

            yield return new WaitUntil(()=> course.Data.FirstChapter.Data.Steps[1].LifeCycle.Stage == Stage.Inactive);

            // Then the training scene object stays disabled.
            Assert.False(toDisable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toDisable);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehavior()
        {
            // Given an active training scene object and a DisableGameObjectBehavior,
            TrainingSceneObject toDisable = TestingUtils.CreateSceneObject("ToDisable");

            DisableGameObjectBehavior behavior = new DisableGameObjectBehavior(toDisable);

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it doesn't autocomplete because it weren't activated yet.
            Assert.AreEqual(Stage.Inactive, behavior.LifeCycle.Stage);

            // Cleanup.
            TestingUtils.DestroySceneObject(toDisable);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveBehaviorAndActivateIt()
        {
            // Given an active training scene object and a DisableGameObjectBehavior,
            TrainingSceneObject toDisable = TestingUtils.CreateSceneObject("ToDisable");

            DisableGameObjectBehavior behavior = new DisableGameObjectBehavior(toDisable);

            // When we mark it to fast-forward and activate it,
            behavior.LifeCycle.MarkToFastForward();
            behavior.LifeCycle.Activate();

            // Then it should work without any differences because the behavior is done immediately anyways.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.IsFalse(toDisable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toDisable);

            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingBehavior()
        {
            // Given an active training scene object and an active DisableGameObjectBehavior,
            TrainingSceneObject toDisable = TestingUtils.CreateSceneObject("ToDisable");

            DisableGameObjectBehavior behavior = new DisableGameObjectBehavior(toDisable);

            behavior.LifeCycle.Activate();

            // When we mark it to fast-forward,
            behavior.LifeCycle.MarkToFastForward();

            // Then it should work without any differences because the behavior is done immediately anyways.
            Assert.AreEqual(Stage.Active, behavior.LifeCycle.Stage);
            Assert.IsFalse(toDisable.GameObject.activeSelf);

            // Cleanup.
            TestingUtils.DestroySceneObject(toDisable);

            yield break;
        }
    }
}
