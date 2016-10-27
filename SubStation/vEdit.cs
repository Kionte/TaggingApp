using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace Tagging
{
    public partial class vEdit : UIViewController
    {

        private Tagging myTag;//a tag to pass between classes. much easier than passing into variables.
        /// <summary>
        /// sets the value of the tag
        /// </summary>
        /// <param name="m"></param>
        public void setMyTag(Tagging m)
        {
            myTag = m;
        }
        PickerModel pickerModelNot; //these picker variables set up the selection for the equipment, purpose, and required notifications.
        UIPickerView pickerNot;

        PickerModel pickerModelPur;//these picker variables set up the selection for the equipment, purpose, and required notifications.
        UIPickerView pickerPur;

        PickerModel pickerModelEq;//these picker variables set up the selection for the equipment, purpose, and required notifications.
        UIPickerView pickerEq;

        PickerModel pickerModelType;//these picker variables set up the selection for the equipment, purpose, and required notifications.
        UIPickerView pickerType;

        //bool tag = false;//true if it is passed in from another class so we dont recreate dates for the tag number. ensures consistency in tag number operations.

        /// <summary>
        /// ctor for segues
        /// </summary>
        /// <param name="handle"></param>
        public vEdit(IntPtr handle) : base(handle) { }
        //copied from view controller 3? im not sure if it is necessary in this class actually.
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
 
            if (myTag.tagNum == 0)//if this is a brand new tag (which it wont be in this class) create new tag number based on the date.
            {
                myTag.tagNum = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmss")); // temp tag num year month day hour minunte
                this.myTag.date = DateTime.Now.ToString("MM/dd/yyyy");
                // tag = true;//set tag to true so that this this is only done once. not sure that this line is needed.
            }
            fillText();
            setPickers();

            UIToolbar toolbar = new UIToolbar(); //create the toolbar (black bar with done button on it)
            toolbar.BarStyle = UIBarStyle.BlackOpaque;//set the color of the bar that goes along the top of the pickers.
            toolbar.Translucent = true;//set the bar to be translucent(might change)
            toolbar.SizeToFit();//fit the picker

            //creating a button for the edge of the picker
            //lambda to create the done button
            
            UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (s, e) =>
            {
                foreach (UIView view in this.View.Subviews)
                {
                    if (view.IsFirstResponder)
                    {
                        if (view == this.fillRequiredNotifications)//check which text view was pressed and save the correct value in it on Done click
                            setTextView(view, pickerModelNot, pickerNot);

                        else if (view == this.fillPurpose)
                            setTextView(view, pickerModelPur, pickerPur);

                        else if (view == this.fillEquipment)
                            setTextView(view, pickerModelEq, pickerEq);

                        else if (view == this.fillType)
                            setTextView(view, pickerModelType, pickerType);
                    }
                }
            });
            toolbar.SetItems(new UIBarButtonItem[] { doneButton }, true);

            //change inputs so that the picker is input and not keyboard.
            this.fillRequiredNotifications.InputView = pickerNot;
            this.fillRequiredNotifications.InputAccessoryView = toolbar;

            this.fillEquipment.InputView = pickerEq;
            this.fillEquipment.InputAccessoryView = toolbar;

            this.fillPurpose.InputAccessoryView = toolbar;
            this.fillPurpose.InputView = pickerPur;

            this.fillType.InputAccessoryView = toolbar;
            this.fillType.InputView = pickerType;
        }
        /// <summary>
        /// puts the correct string in to the text label
        /// </summary>
        /// <param name="view"></param>
        /// <param name="model"></param>
        /// <param name="picker"></param>
        void setTextView(UIView view, PickerModel model, UIPickerView picker)
        {

            UITextView textview = (UITextView)view;
            textview.Text = model.getValues()[(int)picker.SelectedRowInComponent(0)].ToString();//change text on click of done
            textview.ResignFirstResponder();//?
        }
        /// <summary>
        /// prepare for the segue to come
        /// send valuable information to the following mytags view controller
        /// </summary>
        /// <param name="segue"></param>
        /// <param name="sender"></param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.Identifier == "EditSubmitSegue")
            {
                vMyTags dest = (vMyTags)segue.DestinationViewController;
                dest.fromEdit = true;
                insertHelper();//update the tag with the new values in the textboxes before updating local db
                myTag.hasChanged = true;
                myTag.inCDB = Application.getDB().Find<Tagging>(myTag.tagNum).inCDB;

                Application.getDB().Update(myTag);
                var table = Application.getDB().Table<Tagging>();
                foreach(Tagging tag in table)
                {
                    if(tag.tagNum == myTag.tagNum)
                    {
                        tag.type = myTag.type;
                        tag.issuedBy = myTag.issuedBy;
                        tag.requestedBy = myTag.requestedBy;
                        tag.truckNum = myTag.truckNum;
                        tag.requestedFor = myTag.requestedFor;
                        tag.purpose = myTag.purpose;
                        tag.equipment = myTag.equipment;
                        tag.poleOne = myTag.poleOne;
                        tag.poleTwo = myTag.poleTwo;
                        tag.notifications = myTag.notifications;
                        tag.comments = myTag.comments;
                        tag.username = myTag.username;
                        tag.lat = myTag.lat;
                        tag.lon = myTag.lon;
                        tag.isClosed = myTag.isClosed;
                        tag.hasChanged = myTag.hasChanged;
                        tag.inCDB = myTag.inCDB;
                        break;
                    }
                }

            }
            else if (segue.Identifier == "CloseTagSegue")//on close button click
            {
                vMyTags dest = (vMyTags)segue.DestinationViewController;
                dest.fromEdit = true;
                myTag.isClosed = true;//set tag to be closed
                myTag.hasChanged = true;
                Application.getDB().Update(myTag);//update local db so that it is closed on the db


                //var table = Application.getDB().Table<Tagging>();//get the tagging table in the db       
            }
        }
        /// <summary>
        /// does nothing!!!!
        /// </summary>
        /// <param name="sender"></param>
        partial void SubmitButton_TouchUpInside(UIButton sender) { }
        /// <summary>
        /// also does 0!!!!!
        /// </summary>
        /// <param name="sender"></param>
        partial void CloseTagButton_TouchUpInside(UIButton sender) { }
        /// <summary>
        /// assigns values from the text boxes
        /// </summary>
        public void insertHelper()
        {
            myTag.type = this.fillType.Text;                                //change the editable stuff to match the text on screen in the text views
            myTag.requestedBy = this.fillRequest.Text;
            myTag.truckNum = this.fillTruck.Text;
            myTag.requestedFor = this.fillRequestFor.Text;
            myTag.purpose = this.fillPurpose.Text;
            myTag.equipment = this.fillEquipment.Text;
            myTag.poleOne = this.fillPole1.Text;
            myTag.poleTwo = this.fillPole2.Text;
            myTag.notifications = this.fillRequiredNotifications.Text;
            myTag.comments = this.fillComments.Text;
            myTag.hasChanged = true;
        }
        /// <summary>
        /// Fills the text boxes with the data from the tag clicked on in the previous view.
        /// </summary>
        private void fillText()
        {
            this.fillTagNumber.Text = myTag.tagNum.ToString();//set the tagnumber view to display the current tag's tagnumber
            this.fillDate.Text = myTag.date;//show the tag date of the current tag on the edit screen.
            this.fillType.Text = myTag.type;
            this.fillRequest.Text = myTag.requestedBy;
            this.fillTruck.Text = myTag.truckNum;
            this.fillRequestFor.Text = myTag.requestedFor;
            this.fillPurpose.Text = myTag.purpose;
            this.fillEquipment.Text = myTag.equipment;
            this.fillPole1.Text = myTag.poleOne;
            this.fillPole2.Text = myTag.poleTwo;
            this.fillRequiredNotifications.Text = myTag.notifications;
            this.fillComments.Text = myTag.comments;
            if (this.fillComments.Text.Length > 0)
            {
                NSRange range = new NSRange(0, this.fillComments.Text.Length);
                this.fillComments.AccessibilityScroll(UIAccessibilityScrollDirection.Down);
            }
        }
        /// <summary>
        /// Sets the pickers to contain the appropriate values.
        /// </summary>
        public void setPickers()
        {

            List<string> notifications = new List<string>();
            notifications.Add("None");//none first for ease of access
            notifications.Add("BPA (MCC Operator)");
            notifications.Add("Avista (Operator)");
            notifications.Add("POPUD Operator");
            notifications.Add("Grant Co. PUD (Dispatch)");

            //strings for the purpose selection
            List<string> purpose = new List<string>();
            purpose.Add("Maintenence");
            purpose.Add("Construction");
            purpose.Add("Trouble/Storm");
            purpose.Add("Assurance no Backfeed");
            purpose.Add("POPUD Mtr Exchange");

            //strings for the equipment selection
            List<string> equipment = new List<string>();
            equipment.Add("Recloser");
            equipment.Add("Regulator");
            equipment.Add("Switch/Fuse");
            equipment.Add("Elbow/Jumper");
            equipment.Add("Circuit Switcher");

            //strings for type selection
            List<string> type = new List<string>();
            type.Add("Clearance");
            type.Add("Caution");
            type.Add("Hold");

            //set up pickers for each of the selectors
            pickerModelNot = new PickerModel(notifications);
            pickerModelPur = new PickerModel(purpose);
            pickerModelEq = new PickerModel(equipment);
            pickerModelType = new PickerModel(type);

            //set up the picker view and model for each selector
            pickerNot = new UIPickerView();
            pickerNot.Model = pickerModelNot;
            pickerNot.ShowSelectionIndicator = true;

            pickerPur = new UIPickerView();
            pickerPur.Model = pickerModelPur;
            pickerPur.ShowSelectionIndicator = true;

            pickerEq = new UIPickerView();
            pickerEq.Model = pickerModelEq;
            pickerEq.ShowSelectionIndicator = true;

            pickerType = new UIPickerView();
            pickerType.Model = pickerModelType;
            pickerType.ShowSelectionIndicator = true;
        }
    }
}