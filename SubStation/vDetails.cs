using Foundation;
using System;
using UIKit;
using CoreLocation;

namespace Tagging
{
    /// <summary>
    /// i believe this is the details page view controller
    /// </summary>
    public partial class vDetails : UIViewController
    {
        private Tagging myTag = new Tagging();
        private bool isAllTag;//which screen came before? mytags or all tags? if all tags then no edit available

        public Tagging getMyTag()
        {
            return myTag;
        }
        public void setIsAllTag(bool a)
        {
            isAllTag = a;
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="handle"></param>
        public vDetails(IntPtr handle) : base(handle)
        {

        }

        /// <summary>
        /// load the view
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //fill the labels with the important information from the tag
            this.fillTag.Text = myTag.tagNum.ToString();
            this.fillType.Text = myTag.type;
            this.fillDate.Text = myTag.date.ToString();
            this.fillRequest.Text = myTag.requestedBy;
            this.fillTruck.Text = myTag.truckNum;
            this.fillRequestedFor.Text = myTag.requestedFor;
            this.fillPurpose.Text = myTag.purpose;
            this.fillEquipment.Text = myTag.equipment;
            this.fillPoles.Text = myTag.poleOne + " - " + myTag.poleTwo;
            this.fillRequiredNotifications.Text = myTag.notifications;
            this.fillComments.Text = myTag.comments;

            if (isAllTag)//if we came from all tags then editing is not allowed.
            {
                this.EditButton.Enabled = false;
                this.fillUsername.Text = myTag.username;
            }
            else
            {
                this.EditButton.Enabled = true;//make sure its enabled when we go through all tags and then back to my tags.
                fillUsername.Text = vLogin.user.userName;

            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="segue"></param>
        /// <param name="sender"></param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.Identifier == "MapSegue")
            {
                // TODO eventually pass location through this segue so that we can drop pins
            }
            else if (segue.Identifier == "EditSegue")
            {
                vEdit dest = (vEdit)segue.DestinationViewController;
                dest.setMyTag(myTag);// = myTag;//pass the tag to the edit view controller
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        partial void EditButton_TouchUpInside(UIButton sender)
        {

        }
    }
}