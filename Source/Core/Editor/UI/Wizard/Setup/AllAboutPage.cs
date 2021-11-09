// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021 MindPort GmbH

using VRBuilder.Editor.Analytics;
using UnityEngine;

namespace VRBuilder.Editor.UI.Wizard
{
    internal class AllAboutPage : WizardPage
    {
        public AllAboutPage() : base("Help & Documentation", false ,false )
        {

        }

        public override void Draw(Rect window)
        {
            GUILayout.BeginArea(window);
                GUILayout.Label("Hit Play to Preview", BuilderEditorStyles.Title);
                GUILayout.Label("Have a look at How-To's and an in-depth Webinar for further information.", BuilderEditorStyles.Paragraph);
                GUILayout.Label("How-To's", BuilderEditorStyles.Header);

                BuilderGUILayout.DrawLink("How to build your VR process application", "https://developers.innoactive.de/documentation/creator/latest/articles/getting-started/designer.html", BuilderEditorStyles.IndentLarge);
                BuilderGUILayout.DrawLink("How to extend the Creator using a template", "https://developers.innoactive.de/documentation/creator/latest/articles/developer/01-introduction.html", BuilderEditorStyles.IndentLarge);

                GUILayout.Label("Need Help?", BuilderEditorStyles.Header);

                BuilderGUILayout.DrawLink("In-depth webinar on how the Creator works", "https://vimeo.com/417328541/93a752e72c", BuilderEditorStyles.IndentLarge);
                BuilderGUILayout.DrawLink("Visit our developer community", "https://innoactive.io/creator/community", BuilderEditorStyles.IndentLarge);
                BuilderGUILayout.DrawLink("Contact Us for Support", "https://www.innoactive.io/support", BuilderEditorStyles.IndentLarge);

                GUILayout.Space(BuilderEditorStyles.Indent);

                GUILayout.Label("Also, if you are facing any issues, don't hesitate to reach out to us for support", BuilderEditorStyles.Label);
            GUILayout.EndArea();
        }
    }
}
