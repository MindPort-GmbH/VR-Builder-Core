﻿// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System.Collections.Generic;
using System.Linq;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.Properties;
using VRBuilder.Unity;

namespace VRBuilder.Core.RestrictiveEnvironment
{
    /// <summary>
    /// Restricts interaction with scene objects by using LockableProperties, which are extracted from the <see cref="IStepData"/>.
    /// </summary>
    public class DefaultStepLockHandling : StepLockHandlingStrategy
    {
        private bool lockOnCourseStart = true;
        private bool lockOnCourseFinished = true;

        /// <inheritdoc />
        public override void Unlock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {
            IEnumerable<LockablePropertyData> unlockList = PropertyReflectionHelper.ExtractLockablePropertiesFromStep(data);
            unlockList = unlockList.Union(manualUnlocked);

            foreach (LockablePropertyData lockable in unlockList)
            {
                lockable.Property.SetLocked(false);
            }
        }

        /// <inheritdoc />
        public override void Lock(IStepData data, IEnumerable<LockablePropertyData> manualUnlocked)
        {
            // All properties which should be locked
            IEnumerable<LockablePropertyData> lockList = PropertyReflectionHelper.ExtractLockablePropertiesFromStep(data);
            lockList = lockList.Union(manualUnlocked);

            ITransition completedTransition = data.Transitions.Data.Transitions.FirstOrDefault(transition => transition.IsCompleted);
            if (completedTransition != null)
            {
                IStepData nextStepData = GetNextStep(completedTransition);
                IEnumerable<LockablePropertyData> nextStepProperties = PropertyReflectionHelper.ExtractLockablePropertiesFromStep(nextStepData);
                if (nextStepData != null && nextStepData is ILockableStepData lockableStepData)
                {
                    IEnumerable<LockablePropertyData> toUnlock = lockableStepData.ToUnlock.Select(reference => new LockablePropertyData(reference.GetProperty()));
                    nextStepProperties = nextStepProperties.Union(toUnlock);
                }

                if (completedTransition is ILockablePropertiesProvider completedLockableTransition)
                {
                    IEnumerable<LockablePropertyData> transitionLockList = completedLockableTransition.GetLockableProperties();
                    foreach (LockablePropertyData lockable in transitionLockList)
                    {
                        lockable.Property.SetLocked(lockable.EndStepLocked && nextStepProperties.Contains(lockable) == false);
                    }

                    // Remove all lockable from completed transition
                    lockList = lockList.Except(transitionLockList);
                }
                // Remove all lockable from
                lockList = lockList.Except(nextStepProperties);
            }

            foreach (LockablePropertyData lockable in lockList)
            {
                lockable.Property.SetLocked(true);
            }
        }

        private IStepData GetNextStep(ITransition completedTransition)
        {
            if (completedTransition.Data.TargetStep != null)
            {
                return completedTransition.Data.TargetStep.Data;
            }

            if (ProcessRunner.IsRunning == false)
            {
                return null;
            }

            IProcessData course = ProcessRunner.Current.Data;
            // Test all chapters, but the last.
            for (int i = 0; i < course.Chapters.Count - 1; i++)
            {
                if (course.Chapters[i] == course.Current)
                {
                    if (course.Chapters[i + 1].Data.FirstStep != null)
                    {
                        return course.Chapters[i + 1].Data.FirstStep.Data;
                    }
                    break;
                }
            }
            // No next step found, seems to be the last.
            return null;
        }

        /// <inheritdoc />
        public override void Configure(IMode mode)
        {
            if (mode.ContainsParameter<bool>("LockOnCourseStart"))
            {
                lockOnCourseStart = mode.GetParameter<bool>("LockOnCourseStart");
            }

            if (mode.ContainsParameter<bool>("LockOnCourseFinished"))
            {
                lockOnCourseFinished = mode.GetParameter<bool>("LockOnCourseFinished");
            }
        }

        /// <inheritdoc />
        public override void OnCourseStarted(IProcess course)
        {
            if (lockOnCourseStart)
            {
                foreach (LockableProperty prop in SceneUtils.GetActiveAndInactiveComponents<LockableProperty>())
                {
                    prop.SetLocked(true);
                }
            }
        }

        /// <inheritdoc />
        public override void OnCourseFinished(IProcess course)
        {
            if (lockOnCourseFinished)
            {
                foreach (LockableProperty prop in SceneUtils.GetActiveAndInactiveComponents<LockableProperty>())
                {
                    prop.SetLocked(true);
                }
            }
        }
    }
}
