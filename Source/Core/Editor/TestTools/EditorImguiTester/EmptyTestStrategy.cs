// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Core;
using VRBuilder.Editor.UI.Windows;
using UnityEngine;

namespace VRBuilder.Editor.TestTools
{
    /// <summary>
    /// An editing strategy that does nothing. Use it to isolate windows logic during testing.
    /// </summary>
    internal class EmptyTestStrategy : IEditingStrategy
    {
        public ICourse CurrentCourse { get; }
        public IChapter CurrentChapter { get; private set; }

        /// <inheritdoc/>
        public void HandleNewCourseWindow(CourseWindow window)
        {
        }

        /// <inheritdoc/>
        public void HandleNewStepWindow(StepWindow window)
        {
        }

        /// <inheritdoc/>
        public void HandleCurrentCourseModified()
        {
        }

        /// <inheritdoc/>
        public void HandleCourseWindowClosed(CourseWindow window)
        {
        }

        /// <inheritdoc/>
        public void HandleStepWindowClosed(StepWindow window)
        {
        }

        /// <inheritdoc/>
        public void HandleStartEditingCourse()
        {
        }

        /// <inheritdoc/>
        public void HandleCurrentCourseChanged(string courseName)
        {
        }

        /// <inheritdoc/>
        public void HandleCurrentStepModified(IStep step)
        {
        }

        /// <inheritdoc/>
        public void HandleStartEditingStep()
        {
        }

        /// <inheritdoc/>
        public void HandleCurrentStepChanged(IStep step)
        {
        }

        public void HandleCurrentChapterChanged(IChapter chapter)
        {
            CurrentChapter = chapter;
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToUnload()
        {
        }

        /// <inheritdoc/>
        public void HandleProjectIsGoingToSave()
        {
        }

        /// <inheritdoc/>
        public void HandleExitingPlayMode()
        {
        }

        /// <inheritdoc/>
        public void HandleEnterPlayMode()
        {
        }
    }
}
