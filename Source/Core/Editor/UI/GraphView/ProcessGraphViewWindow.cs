using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace VRBuilder.Editor.UI.Graphics
{
    public class ProcessGraphViewWindow : EditorWindow
    {
        private ProcessGraphView graphView;

        [MenuItem("Window/Process Graph")]
        public static void OpenProcessGraphView()
        {
            EditorWindow window = GetWindow<ProcessGraphViewWindow>();
            window.titleContent = new GUIContent("Process Graph");
        }

        private void ConstructGraphView()
        {
            graphView = new ProcessGraphView()
            {
                name = "Process Graph"
            };

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            Button newStepButton = new Button(() => { graphView.CreateNode("New Step"); });
            newStepButton.text = "New Step";
            toolbar.Add(newStepButton);

            rootVisualElement.Add(toolbar);
        }

        private void OnEnable()
        {
            ConstructGraphView();
            CreateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(graphView);
        }
    }
}
