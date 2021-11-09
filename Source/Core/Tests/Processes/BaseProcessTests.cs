// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.Collections;
using System.Collections.Generic;
using VRBuilder.Core;
using VRBuilder.Core.Configuration;
using VRBuilder.Tests.Utils;
using VRBuilder.Tests.Utils.Mocks;
using VRBuilder.Tests.Builder;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace VRBuilder.Tests.Courses
{
    public class BaseProcessTests : RuntimeTests
    {
        [Test]
        public void CanBeSetup()
        {
            Chapter chapter1 = TestLinearChapterBuilder.SetupChapterBuilder(3, false).Build();
            Chapter chapter2 = TestLinearChapterBuilder.SetupChapterBuilder().Build();
            Process course = new Process("MyCourse", new List<IChapter>
            {
                chapter1,
                chapter2
            });

            Assert.AreEqual(chapter1, course.Data.FirstChapter);
        }

        [UnityTest]
        public IEnumerator OneChapterCourse()
        {
            Chapter chapter1 = TestLinearChapterBuilder.SetupChapterBuilder(3, false).Build();
            Process course = new Process("MyCourse", chapter1);

            ProcessRunner.Initialize(course);
            ProcessRunner.Run();

            Debug.Log(chapter1.LifeCycle.Stage);
            yield return null;

            Assert.AreEqual(Stage.Activating, chapter1.LifeCycle.Stage);

            while (chapter1.LifeCycle.Stage != Stage.Inactive)
            {
                Debug.Log(chapter1.LifeCycle.Stage);
                yield return null;
            }

            Assert.AreEqual(Stage.Inactive, chapter1.LifeCycle.Stage);
        }

        [UnityTest]
        public IEnumerator TwoChapterCourse()
        {
            Chapter chapter1 = TestLinearChapterBuilder.SetupChapterBuilder(3, false).Build();
            Chapter chapter2 = TestLinearChapterBuilder.SetupChapterBuilder().Build();
            Process course = new Process("MyCourse", new List<IChapter>
            {
                chapter1,
                chapter2
            });

            ProcessRunner.Initialize(course);
            ProcessRunner.Run();

            yield return new WaitUntil(() => chapter1.LifeCycle.Stage == Stage.Activating);

            Assert.AreEqual(Stage.Inactive, chapter2.LifeCycle.Stage);

            yield return new WaitUntil(() => chapter2.LifeCycle.Stage == Stage.Activating);

            Assert.AreEqual(Stage.Inactive, chapter1.LifeCycle.Stage);
            Assert.AreEqual(Stage.Activating, chapter2.LifeCycle.Stage);
        }

        [UnityTest]
        public IEnumerator EventsAreThrown()
        {
            Chapter chapter1 = TestLinearChapterBuilder.SetupChapterBuilder(3, false).Build();
            Chapter chapter2 = TestLinearChapterBuilder.SetupChapterBuilder(3, false).Build();
            Process course = new Process("MyCourse", new List<IChapter>
            {
                chapter1,
                chapter2
            });

            bool wasStarted = false;
            bool wasCompleted = false;

            course.LifeCycle.StageChanged += (obj, args) =>
            {
                if (args.Stage == Stage.Activating)
                {
                    wasStarted = true;
                }
                else if (args.Stage == Stage.Active)
                {
                    wasCompleted = true;
                }
            };

            ProcessRunner.Initialize(course);
            ProcessRunner.Run();

            while (course.LifeCycle.Stage != Stage.Inactive)
            {
                yield return null;
            }

            Assert.IsTrue(wasStarted);
            Assert.IsTrue(wasCompleted);
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveCourse()
        {
            // Given a process
            Process course = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddCondition(new EndlessConditionMock())))
                .Build();

            course.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

            // When you mark it to fast-forward,
            course.LifeCycle.MarkToFastForward();

            // Then it doesn't autocomplete because it weren't activated yet.
            Assert.AreEqual(Stage.Inactive, course.LifeCycle.Stage);
            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardInactiveCourseAndActivateIt()
        {
            // Given a process
            Process course = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddCondition(new EndlessConditionMock())))
                .Build();

            // When you mark it to fast-forward and activate it,
            course.LifeCycle.MarkToFastForward();

            ProcessRunner.Initialize(course);
            ProcessRunner.Run();

            yield return null;

            // Then it autocompletes.
            Assert.AreEqual(Stage.Inactive, course.LifeCycle.Stage);
            yield break;
        }

        [UnityTest]
        public IEnumerator FastForwardActivatingCourse()
        {
            // Given an activated process
            Process course = new LinearProcessBuilder("Process")
                .AddChapter(new LinearChapterBuilder("Chapter")
                    .AddStep(new BasicStepBuilder("Step")
                        .AddCondition(new EndlessConditionMock())))
                .Build();

            ProcessRunner.Initialize(course);
            ProcessRunner.Run();

            // When you mark it to fast-forward,
            course.LifeCycle.MarkToFastForward();

            // Then it finishes activation.
            Assert.AreEqual(Stage.Active, course.LifeCycle.Stage);
            yield break;
        }
    }
}
