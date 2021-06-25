using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VPG.Core.Utils;

namespace VPG.UX
{
    /// <summary>
    /// Manages the setup of the course controller and lets the developer choose the <see cref="ICourseController"/>.
    /// </summary>
    [DefaultExecutionOrder(1000)]
    public class CourseControllerSetup : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField, SerializeReference]
        private string courseControllerQualifiedName;
        
        [SerializeField, SerializeReference]
        private bool useCustomPrefab;
        
        [SerializeField, SerializeReference]
        private GameObject customPrefab;
#pragma warning restore 0649
        
        /// <summary>
        /// Current used course controller.
        /// </summary>
        public ICourseController CurrentCourseController { get; protected set; }
        
        /// <summary>
        /// Enforced course controller will be use.
        /// </summary>
        protected static ICourseController enforcedCourseController = null;

        protected virtual void OnEnable()
        {
            Setup();
        }

        private void Setup()
        {
            CreateCourseController();
            AddComponents(CurrentCourseController.GetRequiredSetupComponents());
            CurrentCourseController.HandlePostSetup(gameObject);
        }

        private void CreateCourseController()
        {
            if (enforcedCourseController != null)
            {
                CurrentCourseController = enforcedCourseController;
            }
            else if (useCustomPrefab && customPrefab != null)
            {
                Instantiate(customPrefab);
                return;
            }
            
            ICourseController defaultCourseController = GetCourseControllerFromType();
            
            if (CurrentCourseController == null)
            {
                CurrentCourseController = defaultCourseController;
                if (CurrentCourseController == null)
                {
                    Debug.LogError("CourseControllerSetup was not configured properly.");
                    return;
                }
            }
            else
            {
                RemoveComponents(defaultCourseController.GetRequiredSetupComponents().Except(CurrentCourseController.GetRequiredSetupComponents()).ToList());
            }
            
            GameObject courseControllerPrefab = CurrentCourseController.GetCourseControllerPrefab();
            if (courseControllerPrefab != null)
            {
                Instantiate(CurrentCourseController.GetCourseControllerPrefab());
            }
        }

        private ICourseController GetCourseControllerFromType()
        {
            Type courseControllerType = RetrieveCourseControllerType();
            ICourseController courseController = ReflectionUtils.CreateInstanceOfType(courseControllerType) as ICourseController;

            return courseController;
        }

        private Type RetrieveCourseControllerType()
        {
            if (string.IsNullOrEmpty(courseControllerQualifiedName))
            {
                return RetrieveDefaultControllerType();
            }
            
            Type courseControllerType = ReflectionUtils.GetTypeFromAssemblyQualifiedName(courseControllerQualifiedName);
            return courseControllerType != null ? courseControllerType : RetrieveDefaultControllerType();
        }

        private Type RetrieveDefaultControllerType()
        {
            return ReflectionUtils.GetConcreteImplementationsOf<ICourseController>()
                .Select(c => (ICourseController) ReflectionUtils.CreateInstanceOfType(c)).OrderByDescending(controller => controller.Priority)
                .First()
                .GetType();
        }

        private void RemoveComponents(List<Type> components)
        {
            foreach (Type component in components)
            {
                DestroyImmediate(gameObject.GetComponent(component), true);
            }
        }

        private void AddComponents(List<Type> components)
        {
            if (components != null)
            {
                foreach (Type requiredComponent in components)
                {
                    if (gameObject.GetComponent(requiredComponent) == null)
                    {
                        gameObject.AddComponent(requiredComponent);
                    }
                }
            }
        }
        
        /// <summary>
        /// Enforces the given controller to be used, if possible.
        /// </summary>
        /// <param name="courseController">Controller to be used.</param>
        /// <remarks>Scene has to be reloaded to use enforced CourseController.</remarks>
        public static void SetEnforcedCourseController(ICourseController courseController)
        {
            enforcedCourseController = courseController;
        }

        /// <summary>
        /// Resets the <cref name="courseControllerQualifiedName"/> to the name of the course controller with the highest priority.
        /// </summary>
        /// <remarks>This may be used when instantiating a course controller prefab to make sure the default course controller is used.</remarks>
        public void ResetToDefault()
        {
            RemoveComponents(GetCourseControllerFromType().GetRequiredSetupComponents());
            Type courseControllerType = RetrieveDefaultControllerType();
            courseControllerQualifiedName = courseControllerType.Name;
        }
    }
}
