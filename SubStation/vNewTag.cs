using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Data.SqlClient;
using System.Threading;
using SQLite;
using System.Linq;
using CoreGraphics;
using System.Drawing;

namespace Tagging
{
    public partial class vNewTag : UIViewController
    {
        UITableView reqautoCompleteTable;//autofill table for requested by
        UITableView truckautoCompleteTable;//autofill for truck #
        UITableView reqforautoCompleteTable;//autofill for requested for
        Tagging myTag = new Tagging();//tag we pass to my tags??

        List<string> wordCollection = new List<string>(); // saves the requested by suggestions 
        List<string> RFwordCollection = new List<string>(); // saves the reuested for suggestions 
        List<string> TwordCollection = new List<string>(); // saves teh truck num suggestyions 

        PickerModel pickerModelNot; //these picker variables set up the selection for the equipment, purpose, and required notifications.
        UIPickerView pickerNot;
        PickerModel pickerModelPur;
        UIPickerView pickerPur;
        PickerModel pickerModelEq;
        UIPickerView pickerEq;
        PickerModel pickerModelType;
        UIPickerView pickerType;

        public vNewTag(IntPtr handle) : base(handle) { } //Ctor for the view controller        
        public override void ViewDidUnload()
        { // does this stuff // used it for gets rid of zombies??
            base.ViewDidUnload();
            ReleaseDesignerOutlets();
        }

        /// <summary>
        /// i think this is when a row is selected
        /// </summary>
        /// <param name="finalString"></param>
        public void SetAutoCompleteText(string finalString)
        {
            fillRequest.Text = finalString;     //set the text in the 
            fillRequest.ResignFirstResponder();
            reqautoCompleteTable.Hidden = true;

            fillTruck.Text = finalString;
            fillTruck.ResignFirstResponder();
            truckautoCompleteTable.Hidden = true;

            fillRequestFor.Text = finalString;
            fillRequestFor.ResignFirstResponder();
            reqforautoCompleteTable.Hidden = true;
        }

        /// <summary>
        /// load the tag num and date and the rest of the page
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            autoFillLoadDatabase(); // this loads the three databases to requestedby for and trucknum. this will load whatever is in the lite  db to the lsit of strings
            initDropdown();//set up the dropdown lists
            pickerSetup(); // sets up the pickers

            UIToolbar toolbar = new UIToolbar(); // create new tool bar  //create the toolbar (black bar with done button on it)
            toolbar.BarStyle = UIBarStyle.BlackOpaque; // set the styl
            toolbar.Translucent = true; // changes the style a little bit 
            toolbar.SizeToFit(); // makes sure the tool abr fits

            //creating a button for the edge of the picker this is refering to the done button that goes ont the tool bar lambda to create the done button
            UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, (s, e) => //"Done" is what is seen, createes a generic done button, (s,e) => parameters for the function that follow it will run when btton is clicked  
            {
                foreach (UIView view in this.View.Subviews)
                {
                    if (view.IsFirstResponder) // read description ??
                    {
                        if (view == this.fillRequiredNotifications)//check which text view was pressed and save the correct value in it on Done click
                            setTextboxFromPickerEntryOnSubmit(view, pickerModelNot, pickerNot);
                        else if (view == this.fillPurpose)
                            setTextboxFromPickerEntryOnSubmit(view, pickerModelPur, pickerPur);
                        else if (view == this.fillEquipment)
                            setTextboxFromPickerEntryOnSubmit(view, pickerModelEq, pickerEq);
                        else if (view == this.fillType)
                            setTextboxFromPickerEntryOnSubmit(view, pickerModelType, pickerType);
                    }
                }
            });
            toolbar.SetItems(new UIBarButtonItem[] { doneButton }, true); // putting the button onthe bar so it is visibale 

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
        /// set up the pickers
        /// </summary>
        public void pickerSetup()
        {

            myTag.tagNum = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssff")); // temp tag num year month day hour minunte
            this.myTag.date = DateTime.Now.ToString("MM/dd/yyyy");
            this.textTagNumber.Text = myTag.tagNum.ToString();
            this.fillDate.Text = myTag.date;
            this.fillRequest.Text = vLogin.user.userName; // doing this to make sure the names always match when pushing up tags

            //strings for the notification requirements
            List<string> notifications = new List<string>();
            notifications.Add("BPA (MCC Operator)");
            notifications.Add("Avista (Operator)");
            notifications.Add("POPUD Operator");
            notifications.Add("Grant Co. PUD (Dispatch)");
            notifications.Add("None");

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
            pickerModelType = new global::Tagging.PickerModel(type);

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
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender) //  when we click submit dothis 
        {
            vMyTags dest = (vMyTags)segue.DestinationViewController; // set destination 
            insertHelper(); // update tag
            dest.newTag = true;
            if (Application.getDB().Find<Tagging>(myTag.tagNum) == null)
            {// so if tag is NOT in the db
                myTag.hasChanged = false;
                myTag.inCDB = false;
                myTag.username = vLogin.user.userName;
            }
            Application.getDB().Insert(myTag);
        }
        partial void SubmitButton_TouchUpInside(UIButton sender) { }// if you want to use a different Application Delegate class from "AppDelegate" // you can specify it here.
        //change autofill suggestions when letters are typed.
        public void updateSuggestions(List<string> wordCollection, UITextField fill, UITableView autoCompleteTable)
        {
            string[] suggestions = null;
            try
            {
                bool hidden = false;
                //set the suggestions to the words that contain the substring in the text box
                InvokeOnMainThread(() => {
                    if (wordCollection.Count > 0)
                    {
                        suggestions = wordCollection.ToArray().Where(x => x.ToLowerInvariant().Contains(fill.Text.ToLowerInvariant())) // is this only sorting the text? if it is the line below does the same thing i think but for lists
                            .OrderByDescending(x => x.ToLowerInvariant().StartsWith(fill.Text.ToLowerInvariant()))
                            .Select(x => x).ToArray();
                        if (fill.Text == "")
                        {
                            autoCompleteTable.Hidden = true;
                            hidden = true;
                        }
                    }
                });
                if (suggestions.Length != 0)
                {
                    InvokeOnMainThread(() => {
                        if (!hidden)
                        {
                            autoCompleteTable.Hidden = false;
                        }
                        autoCompleteTable.Source = new TableSource(suggestions, this); //create a table with the suggested words in it.
                        autoCompleteTable.ReloadData();
                    });
                }
                else
                {
                    InvokeOnMainThread(() => {
                        autoCompleteTable.Hidden = true;

                    });
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Can't retrieve suggestions");
            }
        }
        /// <summary>
        /// helps insert the values from text boxes into the mytag object
        /// </summary>
        public void insertHelper()
        {// taking the stuff from the text field and putting it into the tag 
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
            addToList();
        }
        /// <summary>
        /// loads all the auto fill inforation 
        /// </summary>
        void autoFillLoadDatabase()
        {
            var tableRB = Application.getDB().Table<RequestedByHistory>(); // each of these three lines get the table form the db and set it to the current object so it can be used 
            var tableT = Application.getDB().Table<TruckHistory>();
            var tableRF = Application.getDB().Table<RequestedForHistory>();

            foreach (RequestedByHistory item in tableRB)
            {
                wordCollection.Add(item.entry);
            }
            foreach (TruckHistory item in tableT)
            {
                TwordCollection.Add(item.entry);
            }
            foreach (RequestedForHistory item in tableRF)
            {
                RFwordCollection.Add(item.entry);
            }
        }
        public UITableView getReq()
        {
            return reqautoCompleteTable;
        }
        public UITableView getTruck()
        {
            return truckautoCompleteTable;
        }
        public UITableView getReqFor()
        {
            return reqforautoCompleteTable;
        }
        public void setRequest(string s)
        {
            this.fillRequest.Text = s;
            this.reqautoCompleteTable.Hidden = true;
        }
        public void setRequestFor(string s)
        {
            this.fillRequestFor.Text = s;
            this.reqforautoCompleteTable.Hidden = true;
        }
        public void setTruck(string s)
        {
            this.fillTruck.Text = s;
            this.truckautoCompleteTable.Hidden = true;
        }
        /// <summary>
        /// adds user input to history ldb
        /// </summary>
        void addToList()
        {
            bool duplicate = false;
            // these three if statments will mkae sure no default entry's are added to the database and the list 
            if (myTag.requestedBy != "Text" && myTag.requestedBy != "")
            {  // if the entry is blank or default 'Text' then dont add that entry to the db
                var table = Application.getDB().Table<RequestedByHistory>();
                foreach (RequestedByHistory item in table)
                {
                    if (item.entry == myTag.requestedBy)
                    { // checks for duplicates 
                        duplicate = true;
                    }
                }
                if (!duplicate)
                    Application.getDB().Insert(new RequestedByHistory(myTag.requestedBy));
            }// add a new RequestedByHistory object to the db set the entry value inside requestedBy by passing through myTag.requestedby
            duplicate = false;
            if (myTag.requestedFor != "Text" && myTag.requestedFor != "")
            { // if the entry is blank or default 'Text' then dont add that entry to the db
                var table = Application.getDB().Table<RequestedForHistory>();
                foreach (RequestedForHistory item in table)
                {
                    if (item.entry == myTag.requestedFor)
                    { // checks for duplicates 
                        duplicate = true;
                    }
                }
                if (!duplicate)
                    Application.getDB().Insert(new RequestedForHistory(myTag.requestedFor));
            }// add requestedFor object to db passs through whatever the useer inputed 
            if (myTag.truckNum != "Text" && myTag.truckNum != "")
            { // if the entry is blank or default 'Text' then dont add that entry to the db
                var table = Application.getDB().Table<TruckHistory>();
                foreach (TruckHistory item in table)
                {
                    if (item.entry == myTag.truckNum)
                    { // checks for duplicates 
                        duplicate = true;
                    }
                }
                if (!duplicate)
                    Application.getDB().Insert(new TruckHistory(myTag.truckNum));
            }//  add truckNum object to database and pass through the user input 
        }

        /// <summary>
        /// create the table for the autofill 
        /// </summary>
        /// <param name="autoCompleteTable"></param>
        /// <param name="newRect"></param>
        void createAutoTable(ref UITableView autoCompleteTable, UITableView newRect)
        {
            autoCompleteTable = newRect;// );//create the table view.
            autoCompleteTable.ScrollEnabled = true;
            // autoCompleteTable.BackgroundColor = UIColor.Gray;
            autoCompleteTable.SeparatorColor = UIColor.Black;
            autoCompleteTable.Hidden = true;//dont show it
            this.View.AddSubview(autoCompleteTable); //  this adds this tot the current view
        }

        /// <summary>
        /// create dropdown tables for the autofill and set their controllers
        /// </summary>
        public void initDropdown()
        {
            createAutoTable(ref reqautoCompleteTable, new UITableView(new RectangleF(130, 200, 180, 120))); // actual dropdown menu (visual)
            createAutoTable(ref truckautoCompleteTable, new UITableView(new RectangleF(130, 235, 180, 120)));
            createAutoTable(ref reqforautoCompleteTable, new UITableView(new RectangleF(130, 270, 180, 120)));

            fillRequest.ShouldChangeCharacters += (sender, something, e) => {
                Thread autoCompleteThread = new Thread(() => {
                    updateSuggestions(wordCollection, fillRequest, reqautoCompleteTable);
                });
                autoCompleteThread.Start();
                return true;
            };
            fillTruck.ShouldChangeCharacters += (sender, something, e) => {
                Thread autoCompleteThread = new Thread(() => {
                    updateSuggestions(TwordCollection, fillTruck, truckautoCompleteTable);
                });
                autoCompleteThread.Start();
                return true;
            };
            fillRequestFor.ShouldChangeCharacters += (sender, something, e) => {
                Thread autoCompleteThread = new Thread(() => {
                    updateSuggestions(RFwordCollection, fillRequestFor, reqforautoCompleteTable);
                });
                autoCompleteThread.Start();
                return true;
            };
        }
        /// <summary>
        /// set the string form the picker selected
        /// </summary>
        /// <param name="view"></param>
        /// <param name="pickerModel"></param>
        /// <param name="pickerView"></param>
        void setTextboxFromPickerEntryOnSubmit(UIView view, PickerModel pickerModel, UIPickerView pickerView)
        {
            UITextView textview = (UITextView)view; // view is already a text view so in this line we are etting a new view = to the current view 
            textview.Text = pickerModel.getValues()[(int)pickerView.SelectedRowInComponent(0)].ToString();//change text on click of done
            textview.ResignFirstResponder(); // read desription ??
        }
    }
}