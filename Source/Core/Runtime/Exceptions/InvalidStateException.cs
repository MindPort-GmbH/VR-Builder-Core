﻿namespace VRBuilder.Core.Exceptions
{
    public class InvalidStateException : TrainingException
    {
        public InvalidStateException(string message) : base(message)
        {
        }
    }
}