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
    [Register ("vHome")]
    partial class vHome
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton crewStatusButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton logoutButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TaggingButton { get; set; }

        [Action ("LogoutButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LogoutButton_TouchUpInside (UIKit.UIButton sender);

        [Action("TaggingButton_TouchUpInside:")]
        [GeneratedCode("iOS Designer", "1.0")]
        partial void TaggingButton_TouchUpInside(UIKit.UIButton sender);


        [Action ("CrewStatusButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CrewStatusButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (crewStatusButton != null) {
                crewStatusButton.Dispose ();
                crewStatusButton = null;
            }

            if (logoutButton != null) {
                logoutButton.Dispose ();
                logoutButton = null;
            }

            if (TaggingButton != null) {
                TaggingButton.Dispose ();
                TaggingButton = null;
            }
        }
    }
}