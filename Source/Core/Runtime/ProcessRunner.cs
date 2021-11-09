// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using System.Collections.Generic;
using System.Linq;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Configuration.Modes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRBuilder.Core
{
    /// <summary>
    /// Runs a <see cref="IProcess"/>, expects to be run only once.
    /// </summary>
    public static class ProcessRunner
    {
        public class CourseEvents
        {
            /// <summary>
            /// Will be called before the course is setup internally.
            /// </summary>
            public EventHandler<ProcessEventArgs> CourseSetup;

            /// <summary>
            /// Will be called on course start.
            /// </summary>
            public EventHandler<ProcessEventArgs> CourseStarted;

            /// <summary>
            /// Will be called each time a chapter activates.
            /// </summary>
            public EventHandler<ProcessEventArgs> ChapterStarted;

            /// <summary>
            /// Will be called each time a step activates.
            /// </summary>
            public EventHandler<ProcessEventArgs> StepStarted;

            /// <summary>
            /// Will be called when the course finishes.
            /// </summary>
            public EventHandler<ProcessEventArgs> CourseFinished;

            /// <summary>
            /// Will be called when manual fast forward is triggered.
            /// </summary>
            public EventHandler<FastForwardProcessEventArgs> FastForwardStep;
        }

        private class CourseRunnerInstance : MonoBehaviour
        {
            /// <summary>
            /// Reference to the currently used <see cref="IProcess"/>.
            /// </summary>
            public IProcess course = null;

            private void HandleModeChanged(object sender, ModeChangedEventArgs args)
            {
                if (course != null)
                {
                    course.Configure(args.Mode);
                    RuntimeConfigurator.Configuration.StepLockHandling.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);
                }
            }

            private void HandleCourseStageChanged(object sender, ActivationStateChangedEventArgs e)
            {
                if (e.Stage == Stage.Inactive)
                {
                    RuntimeConfigurator.ModeChanged -= HandleModeChanged;
                    Destroy(gameObject);
                }
            }

            private void Update()
            {
                if (course == null)
                {
                    return;
                }

                if (course.LifeCycle.Stage == Stage.Inactive)
                {
                    return;
                }

                course.Update();

                if (course.Data.Current?.LifeCycle.Stage == Stage.Activating)
                {
                    Events.ChapterStarted?.Invoke(this, new ProcessEventArgs(course));
                }

                if (course.Data.Current?.Data.Current?.LifeCycle.Stage == Stage.Activating)
                {
                    Events.StepStarted?.Invoke(this, new ProcessEventArgs(course));
                }

                if (course.LifeCycle.Stage == Stage.Active)
                {
                    course.LifeCycle.Deactivate();
                    RuntimeConfigurator.Configuration.StepLockHandling.OnCourseFinished(course);
                    Events.CourseFinished?.Invoke(this, new ProcessEventArgs(course));
                }
            }

            /// <summary>
            /// Starts the <see cref="IProcess"/>.
            /// </summary>
            public void Execute()
            {
                Events.CourseSetup?.Invoke(this, new ProcessEventArgs(course));

                RuntimeConfigurator.ModeChanged += HandleModeChanged;

                course.LifeCycle.StageChanged += HandleCourseStageChanged;
                course.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);

                RuntimeConfigurator.Configuration.StepLockHandling.Configure(RuntimeConfigurator.Configuration.Modes.CurrentMode);
                RuntimeConfigurator.Configuration.StepLockHandling.OnCourseStarted(course);
                course.LifeCycle.Activate();

                Events.CourseStarted?.Invoke(this, new ProcessEventArgs(course));
            }
        }

        private static CourseRunnerInstance instance;

        private static CourseEvents events;

        /// <summary>
        /// Returns all course events for the current scene.
        /// </summary>
        public static CourseEvents Events
        {
            get
            {
                if (events == null)
                {
                    events = new CourseEvents();
                    SceneManager.sceneUnloaded += OnSceneUnloaded;
                }
                return events;
            }
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            events = null;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        /// <summary>
        /// Currently running <see cref="IProcess"/>
        /// </summary>
        public static IProcess Current
        {
            get
            {
                return instance == null ? null : instance.course;
            }
        }

        /// <summary>
        /// Returns true if the current <see cref="IProcess"/> is running.
        /// </summary>
        public static bool IsRunning
        {
            get
            {
                return Current != null && Current.LifeCycle.Stage != Stage.Inactive;
            }
        }

        /// <summary>
        /// Initializes the training runner by creating all required component in scene.
        /// </summary>
        /// <param name="course">The course which should be run.</param>
        public static void Initialize(IProcess course)
        {
            instance = instance == null ? new GameObject("[TRAINING_RUNNER]").AddComponent<CourseRunnerInstance>() : instance;
            instance.course = course;
        }

        /// <summary>
        /// Skips the given amount of chapters.
        /// </summary>
        /// <param name="numberOfChapters">Number of chapters.</param>
        public static void SkipChapters(int numberOfChapters)
        {
            IList<IChapter> chapters = Current.Data.Chapters;

            foreach (IChapter currentChapter in chapters.Skip(chapters.IndexOf(Current.Data.Current)).Take(numberOfChapters))
            {
                currentChapter.LifeCycle.MarkToFastForward();
            }
        }

        /// <summary>
        /// Skips the current step and uses given transition.
        /// </summary>
        /// <param name="transition">Transition which should be used.</param>
        public static void SkipStep(ITransition transition)
        {
            if (IsRunning == false)
            {
                return;
            }

            Current.Data.Current.Data.Current.LifeCycle.MarkToFastForward();
            transition.Autocomplete();

            Events.FastForwardStep?.Invoke(instance, new FastForwardProcessEventArgs(transition, Current));
        }

        /// <summary>
        /// Starts the <see cref="IProcess"/>.
        /// </summary>
        public static void Run()
        {
            if (IsRunning)
            {
                return;
            }

            instance.Execute();
        }
    }
}
