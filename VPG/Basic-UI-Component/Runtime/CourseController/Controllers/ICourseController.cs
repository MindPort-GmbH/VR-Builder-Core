using System;
using System.Collections.Generic;
using UnityEngine;

namespace VPG.UX
{
    /// <summary>
    /// Controller for managing the course.
    /// Can for example instantiate a controller menu and a spectator camera.
    /// </summary>
    public interface ICourseController
    {
        /// <summary>
        /// Prettified name.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Priority of the controller.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets a course controller game object.
        /// </summary>
        GameObject GetCourseControllerPrefab();

        /// <summary>
        /// List of component types which are required on the setup object.
        /// </summary>
        /// <returns>List of component types.</returns>
        List<Type> GetRequiredSetupComponents();

        /// <summary>
        /// Handles post-setup logic.
        /// Should be called after all components are added and object is initialized.
        /// </summary>
        /// <param name="courseControllerObject">Actual course controller object</param>
        void HandlePostSetup(GameObject courseControllerObject);
    }
}