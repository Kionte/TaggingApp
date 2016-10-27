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
    [Register ("vThreeButtonView")]
    partial class vThreeButtonView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AllTagButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView bottomBackGround { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem CancelTagging { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MyTagsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NewTagButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UINavigationItem Tagging { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView topBackground { get; set; }

        [Action("AllTagButton_TouchUpInside:")]
        [GeneratedCode("iOS Designer", "1.0")]
        partial void AllTagButton_TouchUpInside(UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AllTagButton != null) {
                AllTagButton.Dispose ();
                AllTagButton = null;
            }

            if (bottomBackGround != null) {
                bottomBackGround.Dispose ();
                bottomBackGround = null;
            }

            if (CancelTagging != null) {
                CancelTagging.Dispose ();
                CancelTagging = null;
            }

            if (MyTagsButton != null) {
                MyTagsButton.Dispose ();
                MyTagsButton = null;
            }

            if (NewTagButton != null) {
                NewTagButton.Dispose ();
                NewTagButton = null;
            }

            if (Tagging != null) {
                Tagging.Dispose ();
                Tagging = null;
            }

            if (topBackground != null) {
                topBackground.Dispose ();
                topBackground = null;
            }
        }
    }
}