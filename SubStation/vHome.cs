using Foundation;
using System;
using SQLite;
using UIKit;
using CoreGraphics;

namespace Tagging
{
    public partial class vHome : UIViewController
    {
        /// <summary>
        /// ctor for segues
        /// </summary>
        /// <param name="handle"></param>
        public vHome(IntPtr handle) : base(handle) { }
        /// <summary>
        /// On load makes buttons look good.
        /// </summary>
        public override void ViewDidLoad()
        {

            TaggingButton.Layer.CornerRadius = 20;
            TaggingButton.Layer.ShadowColor = new CGColor(90f / 255f, 144f / 255f, 192f / 255f, 1f);
            TaggingButton.Layer.ShadowOpacity = 1;
            TaggingButton.Layer.ShadowRadius = 1;
            TaggingButton.Layer.ShadowOffset = new CGSize(3, 3);


            crewStatusButton.Layer.CornerRadius = 15;
            crewStatusButton.Layer.ShadowColor = new CGColor(0f / 255f, 0, 100f / 255f, 1f);
            crewStatusButton.Layer.ShadowOpacity = 1;
            crewStatusButton.Layer.ShadowRadius = 1;
            crewStatusButton.Layer.ShadowOffset = new CGSize(-3, 3);

        }

        /// <summary>
        /// Drops tagging and user tables from ldb.
        /// </summary>
        /// <param name="sender"></param>
        partial void LogoutButton_TouchUpInside(UIButton sender)
        {
            var table = Application.getDB().Table<Tagging>();
            foreach (Tagging tag in table)
            {
                if (!tag.inCDB)
                {
                    UIAlertView al = new UIAlertView("Can't Logout", "There are still tags that need to be uploaded. Check your connection.", new UIAlertViewDelegate(), "OK", null);
                    al.Show();
                    return;
                }
            }

            Application.getDB().DropTable<Tagging>();
            Application.getDB().CreateTable<Tagging>();
            Application.getDB().DropTable<Users>();
            Application.getDB().CreateTable<Users>();
            Application.getDB().DropTable<RequestedByHistory>();
            Application.getDB().CreateTable<RequestedByHistory>();
            Application.getDB().DropTable<TruckHistory>();
            Application.getDB().CreateTable<TruckHistory>();
            Application.getDB().DropTable<RequestedForHistory>();
            Application.getDB().CreateTable<RequestedForHistory>();
            
            PerformSegue("logoutSegue", this);
        }

        /// <summary>
        /// Nada
        /// </summary>
        /// <param name="sender"></param>
        partial void CrewStatusButton_TouchUpInside(UIButton sender)
        {

        }

        /// <summary>
        /// Nada
        /// </summary>
        /// <param name="sender"></param>
        partial void TaggingButton_TouchUpInside(UIButton sender)
        {

        }
    }
}