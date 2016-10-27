using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
//using SubStation;

namespace Tagging
{
    public partial class vMyTags : UITableViewController
    {
        UITableView table;//table to represent our tableview
        public bool fromEdit;
        public bool newTag;



        public vMyTags(IntPtr handle) : base(handle) { }   //constructor
        /// <summary>
        /// view did load my tags
        /// </summary>
        public override void ViewDidLoad() // loaing the the tags into the table 
        {

            if (TableSource.myTags)
                fromEdit = TableSource.myTags;
            if (fromEdit || newTag)
            {
                this.NavigationItem.HidesBackButton = true;
            }
            try
            {
                base.ViewDidLoad();
                //update();
                Parse p = new Parse();
                if (Reachability.IsHostReachable("http://google.com"))
                    p.myTagStart(fromEdit);
                else
                {
                    UIAlertView al = new UIAlertView("Connection Error", "Cannot connect to\ntagging.inlandpower.com.\nCheck your internet connection.", new UIAlertViewDelegate(), "Ok", null);
                    al.Show();

                }
                table = new UITableView(View.Bounds); // defaults to Plain style
                var tables = Application.getDB().Table<Tagging>(); // sets table to whatwever is in te db
                                                                   //get items from db
                TaggingList.setItems(new List<Tagging>());

                foreach (Tagging s in tables)
                {
                    if (!s.isClosed)   // if tag is not closed 
                        TaggingList.Add(s);    //add all the db items to the list of tags ( if the tag is open if it is closed it will be skipped 
                }
                //create a table with the items in the list tableItems // source is used to populate table 
                table.Source = new TableSource(TaggingList.getItems(), this);
                Add(table);//add the table to TableViewMyTags screens as a subview
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "~");
                UIAlertView noConnection = new UIAlertView("connection Error", "cannot connect to tagging.inlandpower.com", new UIAlertViewDelegate(), "ok", null);
                noConnection.Show();
            }

        }

        /// <summary>
        /// send ingo to the details page 
        /// </summary>
        /// <param name="segue"></param>
        /// <param name="sender"></param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.Identifier == "MyTagsSegue")//save the values in the row and send them to the details screen
            {
                NSIndexPath index = this.table.IndexPathForSelectedRow;
                vDetails dest = (vDetails)segue.DestinationViewController;
                //dest.getMyTag().virtTagNum = TaggingList.getItems()[index.Row].virtTagNum;
                dest.getMyTag().tagNum = TaggingList.getItems()[index.Row].tagNum;         //save all the values from the row that was clicked onto
                dest.getMyTag().poleOne = TaggingList.getItems()[(int)index.Row].poleOne;
                dest.getMyTag().username = TaggingList.getItems()[(int)index.Row].username;
                dest.getMyTag().poleTwo = TaggingList.getItems()[(int)index.Row].poleTwo;
                dest.getMyTag().type = TaggingList.getItems()[(int)index.Row].type;
                dest.getMyTag().date = TaggingList.getItems()[(int)index.Row].date;
                dest.getMyTag().requestedBy = TaggingList.getItems()[(int)index.Row].requestedBy;
                dest.getMyTag().truckNum = TaggingList.getItems()[(int)index.Row].truckNum;
                dest.getMyTag().purpose = TaggingList.getItems()[(int)index.Row].purpose;
                dest.getMyTag().equipment = TaggingList.getItems()[(int)index.Row].equipment;
                dest.getMyTag().notifications = TaggingList.getItems()[(int)index.Row].notifications;
                dest.getMyTag().requestedFor = TaggingList.getItems()[(int)index.Row].requestedFor;
                dest.getMyTag().comments = TaggingList.getItems()[(int)index.Row].comments;
                dest.getMyTag().loc = TaggingList.getItems()[(int)index.Row].loc;
                dest.setIsAllTag(false);
                dest.getMyTag().hasChanged = TaggingList.getItems()[(int)index.Row].hasChanged;
                dest.getMyTag().inCDB = TaggingList.getItems()[index.Row].inCDB;
                dest.getMyTag().inCDB = Application.getDB().Find<Tagging>(TaggingList.getItems()[index.Row].tagNum).inCDB;
                dest.getMyTag().isClosed = Application.getDB().Find<Tagging>(TaggingList.getItems()[index.Row].tagNum).isClosed;
            }
            else if (segue.Identifier == "myTagToMyTagSegue")
            {
                this.ViewDidLoad();
            }
        }
    }
}