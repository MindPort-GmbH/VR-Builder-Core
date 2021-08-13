// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;

namespace VRBuilder.Core
{
    /// <summary>
    /// EventArgs for course events.
    /// </summary>
    public class CourseEventArgs : EventArgs
    {
        /// <summary>
        /// Active course.
        /// </summary>
        public readonly ICourse Course;

        /// <summary>
        /// Active Chapter.
        /// </summary>
        public readonly IChapter Chapter;

        /// <summary>
        /// Active Step.
        /// </summary>
        public readonly IStep Step;

        public CourseEventArgs(ICourse course)
        {
            Course = course;
            Chapter = course.Data.Current;
            if (Chapter != null)
            {
                Step = Chapter.Data.Current;
            }
        }
    }
}
