// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;

namespace VRBuilder.Core.Exceptions
{
    public class TrainingException : Exception
    {
        public TrainingException()
        {
        }

        public TrainingException(string message) : base(message)
        {
        }

        public TrainingException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
