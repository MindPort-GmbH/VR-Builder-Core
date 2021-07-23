using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VRBuilder.Editor.UI
{
    /// <summary>
    /// Helper editor class that allows retrieving or drawing a logo that
    /// fits the current Unity color theme.
    /// </summary>
    internal static class LogoEditorHelper
    {
        /// <summary>
        /// Filenames of light company logo (used for dark Unity theme)
        /// </summary>
        private static readonly string[] companyLogoDarkFileNames = new[] { "Mindport1_transparent_darkmode", "Mindport2_transparent_darkmode", "Mindport3_transparent_darkmode" };

        /// <summary>
        /// Filenames of light product logo (used for dark Unity theme)
        /// </summary>
        private static readonly string[] productLogoDarkFileNames = new[] { "VRBuilder1_transparent_darkmode", "VRBuilder2_transparent_darkmode", "VRBuilder3_transparent_darkmode" };

        /// <summary>
        /// Filenames of dark company logo (used for light Unity theme)
        /// </summary>
        private static readonly string[] companyLogoLightFileNames = new[] { "Mindport1_transparent_whitemode", "Mindport2_transparent_whitemode", "Mindport3_transparent_whitemode" };

        /// <summary>
        /// Filenames of dark product logo (used for light Unity theme)
        /// </summary>
        private static readonly string[] productLogoLightFileNames = new[] { "VRBuilder1_transparent_whitemode", "VRBuilder2_transparent_whitemode", "VRBuilder3_transparent_whitemode" };

        private static Texture2D textureCache = null;

        /// <summary>
        /// Returns a common texture containing the correct logo
        /// </summary>
        public static Texture2D GetProductLogoTexture(LogoStyle style)
        {
            return GetLogoTexture(GetProductLogoFilename(style));
        }

        /// <summary>
        /// Returns a common texture containing the correct logo
        /// </summary>
        public static Texture2D GetCompanyLogoTexture(LogoStyle style)
        {
            return GetLogoTexture(GetProductLogoFilename(style));
        }

        /// <summary>
        /// Draws the logo with the specified width in the current GUI context
        /// </summary>
        public static void DrawCompanyLogo(LogoStyle style, float width)
        {
            DrawLogo(GetCompanyLogoTexture(style), width);
        }

        /// <summary>
        /// Draws the logo with the specified width in the current GUI context
        /// </summary>
        public static void DrawProductLogo(LogoStyle style, float width)
        {
            DrawLogo(GetProductLogoTexture(style), width);
        }

        /// <summary>
        /// Returns the file name of the correct company logo
        /// </summary>
        public static string GetCompanyLogoFilename(LogoStyle style)
        {
            if (EditorGUIUtility.isProSkin)
            {
                return companyLogoDarkFileNames[(int)style];
            }
            else
            {
                return companyLogoLightFileNames[(int)style];
            }
        }

        /// <summary>
        /// Returns the file name of the correct product logo
        /// </summary>
        public static string GetProductLogoFilename(LogoStyle style)
        {
            if (EditorGUIUtility.isProSkin)
            {
                return productLogoDarkFileNames[(int)style];
            }
            else
            {
                return productLogoLightFileNames[(int)style];
            }
        }

        /// <summary>
        /// Returns the asset path of the correct logo
        /// </summary>
        public static string GetLogoAssetPath(string filename)
        {
            string[] results = AssetDatabase.FindAssets(filename);
            if (results != null && results.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(results.First());
            }
            return null;
        }

        private static void DrawLogo(Texture2D logo, float width)
        {
            if (logo)
            {
                Rect rect = GUILayoutUtility.GetRect(width, 150, GUI.skin.box);
                GUI.DrawTexture(rect, logo, ScaleMode.ScaleToFit);
            }
        }

        private static Texture2D GetLogoTexture(string filename)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(GetLogoAssetPath(filename));
            //if (textureCache == null)
            //{
            //    textureCache = AssetDatabase.LoadAssetAtPath<Texture2D>(GetLogoAssetPath());
            //}
            //return textureCache;
        }
    }
}
