// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;

namespace VRBuilder.Core
{
    /// <summary>
    /// EventArgs for fast forward course events.
    /// </summary>
    public class FastForwardCourseEventArgs : CourseEventArgs
    {
        /// <summary>
        /// Completed transition
        /// </summary>
        public readonly ITransition CompletedTransition;

        public FastForwardCourseEventArgs(ITransition transition, ICourse course) : base(course)
        {
            CompletedTransition = transition;
        }
    }
}
