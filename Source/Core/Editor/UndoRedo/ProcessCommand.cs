// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using System;
using VRBuilder.Editor.UI.Windows;

namespace VRBuilder.Editor.UndoRedo
{
    /// <summary>
    /// A <see cref="CallbackCommand"/> which notifies the <seealso cref="GlobalEditorHandler"/> class that the current process was modified.
    /// </summary>
    public class ProcessCommand : CallbackCommand
    {
        /// <inheritdoc />
        public override void Do()
        {
            base.Do();

            GlobalEditorHandler.CurrentProcessModified();
        }

        /// <inheritdoc />
        public override void Undo()
        {
            base.Undo();

            GlobalEditorHandler.CurrentProcessModified();
        }

        public ProcessCommand(Action doCallback, Action undoCallback) : base(doCallback, undoCallback) { }
    }
}
