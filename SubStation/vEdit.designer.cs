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
    [Register ("vEdit")]
    partial class vEdit
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CloseTagButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView fillComments { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel fillDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView fillEquipment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField fillPole1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField fillPole2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView fillPurpose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField fillRequest { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField fillRequestFor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView fillRequiredNotifications { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel fillTagNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField fillTruck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView fillType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubmitButton { get; set; }

        [Action ("CloseTagButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CloseTagButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("SubmitButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SubmitButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CloseTagButton != null) {
                CloseTagButton.Dispose ();
                CloseTagButton = null;
            }

            if (fillComments != null) {
                fillComments.Dispose ();
                fillComments = null;
            }

            if (fillDate != null) {
                fillDate.Dispose ();
                fillDate = null;
            }

            if (fillEquipment != null) {
                fillEquipment.Dispose ();
                fillEquipment = null;
            }

            if (fillPole1 != null) {
                fillPole1.Dispose ();
                fillPole1 = null;
            }

            if (fillPole2 != null) {
                fillPole2.Dispose ();
                fillPole2 = null;
            }

            if (fillPurpose != null) {
                fillPurpose.Dispose ();
                fillPurpose = null;
            }

            if (fillRequest != null) {
                fillRequest.Dispose ();
                fillRequest = null;
            }

            if (fillRequestFor != null) {
                fillRequestFor.Dispose ();
                fillRequestFor = null;
            }

            if (fillRequiredNotifications != null) {
                fillRequiredNotifications.Dispose ();
                fillRequiredNotifications = null;
            }

            if (fillTagNumber != null) {
                fillTagNumber.Dispose ();
                fillTagNumber = null;
            }

            if (fillTruck != null) {
                fillTruck.Dispose ();
                fillTruck = null;
            }

            if (fillType != null) {
                fillType.Dispose ();
                fillType = null;
            }

            if (SubmitButton != null) {
                SubmitButton.Dispose ();
                SubmitButton = null;
            }
        }
    }
}