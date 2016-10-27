using Foundation;
using System;
using System.Collections.Generic;
using UIKit;


/// <summary>
/// this class sets up the table in the view all tags page. table items is the list of the tags that are open
/// </summary>
namespace Tagging
{


    public partial class vAllTags : UITableViewController
    {

        UITableView table;//just  a table to represent our tableview


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="handle"></param>
        public vAllTags(IntPtr handle) : base(handle)
        {
        }




        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                TaggingList.setItems(new List<Tagging>());
                Parse p = new Parse();
                if (Reachability.IsHostReachable("http://google.com"))
                    p.start();
                else
                {
                    UIAlertView al = new UIAlertView("Connection Error", "Cannot connect to tagging.inlandpower.com\nCheck your internet connection", new UIAlertViewDelegate(), "Ok", null);
                    al.Show();

                }




                table = new UITableView(View.Bounds); // defaults to Plain style

                table.Source = new TableSource(TaggingList.getItems(), this);//set a new source of the tagging list, which is static and accessable through the program
                Add(table);//add the table as a subview
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n" + e.ToString());
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
            if (segue.Identifier == "detailssegue")//save the data and pass it to the details screen
            {

                NSIndexPath index = this.table.IndexPathForSelectedRow;
                vDetails dest = (vDetails)segue.DestinationViewController;//send all the info to the tag in view controller
                //dest.getMyTag().virtTagNum = TaggingList.getItems()[index.Row].virtTagNum;
                dest.getMyTag().tagNum = TaggingList.getItems()[index.Row].tagNum;         //save all the values from the row that was clicked onto
                dest.getMyTag().poleOne = TaggingList.getItems()[(int)index.Row].poleOne;
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
                dest.getMyTag().username = TaggingList.getItems()[index.Row].username;

                dest.setIsAllTag(true);
            }
            else if (segue.Identifier == "allTagtoallTag")
            {
                //never used
            }
        }


    }
}