using System;
using VPG.Core.Input;
using UnityEditor;

[InitializeOnLoad]
public static class InputSystemChecker
{
    private const string message =
        "VR Process Gizmo requires Unity's new Input System." +
        "\n\nTo switch from the legacy input system to the new one, open the 'Player Settings' and set the " +
        "option 'Active Input Handling' to 'Both' or 'Input System Package (New)'.";

    /// <summary>
    /// This is a check if the new input system is active OR another concrete implementation of the InputController exists.
    /// </summary>
    static InputSystemChecker()
    {
        try
        {
            // This will throw an InvalidOperationException when no concrete implementation is found.
            Type type = InputController.ConcreteType;
        }
        catch (InvalidOperationException)
        {
            if (VPGProjectSettings.Load().IsFirstTimeStarted == false)
            {
                EditorUtility.DisplayDialog("Attention required!", message, "Understood");
            }
        }
    }
}
