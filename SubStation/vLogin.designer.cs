// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tagging
{
    [Register ("vLogin")]
    partial class vLogin
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField fillPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField fillUsername { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton showPasswordButton { get; set; }

        [Action ("ShowPasswordButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ShowPasswordButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("UIButton11159_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton11159_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (fillPassword != null) {
                fillPassword.Dispose ();
                fillPassword = null;
            }

            if (fillUsername != null) {
                fillUsername.Dispose ();
                fillUsername = null;
            }

            if (showPasswordButton != null) {
                showPasswordButton.Dispose ();
                showPasswordButton = null;
            }
        }
    }
}