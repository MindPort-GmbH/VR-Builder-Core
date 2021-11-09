// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using VRBuilder.Core;
using VRBuilder.Editor.UndoRedo;
using VRBuilder.Core.Configuration;
using VRBuilder.Editor.Configuration;

namespace VRBuilder.Editor.UI.Windows
{
    /// <summary>
    /// This class draws the Workflow window..
    /// </summary>
    public class ProcessWindow : EditorWindow
    {
        private IProcess activeCourse;

        [SerializeField]
        private Vector2 currentScrollPosition;

        private EditorIcon titleIcon;

        [SerializeField]
        private ProcessMenuView chapterMenu;

        private ChapterRepresentation chapterRepresentation;

        private bool isPanning;
        private Vector2 mousePosition;

        /// <summary>
        /// Sets the <paramref name="course"/> to be displayed and edited in this window.
        /// </summary>
        public void SetCourse(IProcess course)
        {
            RevertableChangesHandler.FlushStack();

            activeCourse = course;

            if (course == null)
            {
                return;
            }

            chapterMenu.Initialise(course, this);
            chapterMenu.ChapterChanged += (sender, args) =>
            {
                chapterRepresentation.SetChapter(args.CurrentChapter);
            };

            chapterRepresentation.SetChapter(course.Data.FirstChapter);
        }

        /// <summary>
        /// Returns currently edited course.
        /// </summary>
        internal IProcess GetCourse()
        {
            return activeCourse;
        }

        /// <summary>
        /// Updates the chapter representation to the selected chapter.
        /// </summary>
        internal void RefreshChapterRepresentation()
        {
            if (activeCourse != null)
            {
                chapterRepresentation.SetChapter(chapterMenu.CurrentChapter);
            }
        }

        /// <summary>
        /// Returns currently selected chapter.
        /// </summary>
        internal IChapter GetChapter()
        {
            return activeCourse == null ? null : chapterMenu.CurrentChapter;
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            wantsMouseMove = true;
            if (chapterMenu == null)
            {
                chapterMenu = CreateInstance<ProcessMenuView>();
            }

            if (chapterRepresentation == null)
            {
 #if CREATOR_PRO
                chapterRepresentation = new ProChapterRepresentation();
 #else
                chapterRepresentation = new ChapterRepresentation();
 #endif
                chapterRepresentation.Graphics.Canvas.PointerDrag += (o, eventArgs) => currentScrollPosition -= eventArgs.PointerDelta;
            }

            if (titleIcon == null)
            {
                titleIcon = new EditorIcon("icon_process_editor");
            }

            EditorSceneManager.newSceneCreated += OnNewScene;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            GlobalEditorHandler.CourseWindowOpened(this);
        }

        private void OnDestroy()
        {
            EditorSceneManager.newSceneCreated -= OnNewScene;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            GlobalEditorHandler.CourseWindowClosed(this);
        }

        private void SetTabName()
        {
            titleContent = new GUIContent("Workflow", titleIcon.Texture);
        }

        private void OnGUI()
        {
            if (activeCourse == null)
            {
                return;
            }

            SetTabName();

            float width = chapterMenu.IsExtended ? ProcessMenuView.ExtendedMenuWidth : ProcessMenuView.MinimizedMenuWidth;
            Rect scrollRect = new Rect(width, 0f, position.size.x - width, position.size.y);

            Vector2 centerViewpointOnCanvas = currentScrollPosition + scrollRect.size / 2f;

            HandleEditorCommands(centerViewpointOnCanvas);
            chapterMenu.Draw();
            DrawChapterWorkflow(scrollRect);
        }

        private void OnFocus()
        {
            if (EditorConfigurator.Instance.Validation.IsAllowedToValidate() && activeCourse != null)
            {
                EditorConfigurator.Instance.Validation.Validate(activeCourse.Data, GlobalEditorHandler.GetCurrentCourse());
            }
        }

        private void HandleEditorCommands(Vector2 centerViewpointOnCanvas)
        {
            if (Event.current.type != EventType.ValidateCommand)
            {
                return;
            }

            bool used = false;
            switch (Event.current.commandName)
            {
                case "Copy":
                    used = chapterRepresentation.CopySelected();
                    break;
                case "Cut":
                    used = chapterRepresentation.CutSelected();
                    break;
                case "Paste":
                    used = chapterRepresentation.Paste(centerViewpointOnCanvas);
                    break;
                case "Delete":
                case "SoftDelete":
                    used = chapterRepresentation.DeleteSelected();
                    break;
                case "Duplicate":
                    break;
                case "FrameSelected":
                    break;
                case "FrameSelectedWithLock":
                    break;
                case "SelectAll":
                    break;
                case "Find":
                    break;
                case "FocusProjectWindow":
                    break;
                default:
                    break;
            }

            if (used)
            {
                Event.current.Use();
            }
        }

        private void DrawChapterWorkflow(Rect scrollRect)
        {
            Event current = Event.current;

            if (current.type == EventType.MouseDown && current.button == 2)
            {
                mousePosition = current.mousePosition;
                isPanning = true;
            }
            else if (current.type == EventType.MouseUp && current.button == 2)
            {
                isPanning = false;
            }

            if (isPanning && current.type == EventType.MouseDrag)
            {
                currentScrollPosition += (mousePosition - current.mousePosition);
                mousePosition = current.mousePosition;
            }

            currentScrollPosition = GUI.BeginScrollView(new Rect(scrollRect.position, scrollRect.size), currentScrollPosition - chapterRepresentation.BoundingBox.min, chapterRepresentation.BoundingBox, true, true) + chapterRepresentation.BoundingBox.min;
            {
                Rect controlRect = new Rect(currentScrollPosition, scrollRect.size);
                chapterRepresentation.HandleEvent(Event.current, controlRect);

                if (Event.current.type == EventType.Used || isPanning)
                {
                    Repaint();
                }
            }
            GUI.EndScrollView();
        }

        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (BuildPipeline.isBuildingPlayer == false && RuntimeConfigurator.Exists == false)
            {
                Close();
            }
        }

        private void OnNewScene(Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            if (BuildPipeline.isBuildingPlayer == false)
            {
                Close();
            }
        }
    }
}
