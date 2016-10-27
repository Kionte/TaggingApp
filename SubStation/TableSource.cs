using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace Tagging
{
    public class TableSource : UITableViewSource
    {
        /// <summary>
        /// set up for cell selection on ALLTAGS page
        /// </summary>
        vAllTags owner;
        /// <summary>
        /// cell selection on MY TAGS page
        /// </summary>
        vMyTags owner2;
        /// <summary>
        /// // new tag object
        /// </summary>
        vNewTag owner3;
        /// <summary>
        ///  array of tags, may want to change to the list so that it can grow dynamically
        /// </summary>
        List<Tagging> TableItems;
        /// <summary>
        /// autofill tables
        /// </summary>
        RequestedByHistory[] RBTableItems;
        /// <summary>
        /// autofill tables
        /// </summary>
        RequestedForHistory[] RFTableItems;
        /// <summary>
        /// autofill tables
        /// </summary>
        TruckHistory[] TTableItems;
        /// <summary>
        /// identifies the type of cell we want the table to use
        /// </summary>
        string CellIdentifier = "MyTagsCell";
        /// <summary>
        /// 
        /// </summary>      
        string[] suggested;
        /// <summary>
        /// 
        /// </summary>
        public static bool myTags = false;


        // do we need this? ~`
        public TableSource(List<Tagging> items)
        {
            TableItems = items;//set table items
        }

        public TableSource(RequestedByHistory[] items)
        {
            RBTableItems = items;//set table items
        }
        public TableSource(RequestedForHistory[] items)
        {
            RFTableItems = items;//set table items
        }
        public TableSource(TruckHistory[] items)
        {
            this.TTableItems = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="owner"></param>
        public TableSource(List<Tagging> items, vMyTags owner)
        {
            TableItems = items;
            this.owner2 = owner;//set my tags as the owner of the source
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="owner"></param>
        public TableSource(string[] items, vNewTag owner)
        {
            suggested = items;
            this.owner3 = owner;//set my tags as the owner of the source
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suggestions"></param>
        public TableSource(string[] suggestions)
        {
            // TableItems = new List<Tagging>();
            this.suggested = suggestions;
        }

        /// <summary>
        /// constructor for the all tags view source
        /// </summary>
        /// <param name="items"></param>
        /// <param name="owner"></param>
        public TableSource(List<Tagging> items, vAllTags owner)
        {
            TableItems = items;//set table items
            this.owner = owner;//all tags as the owner
        }

        /// <summary>
        /// constructor for the myTags view     difference is in the parameters ~`
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override string TitleForDeleteConfirmation(UITableView tableView, NSIndexPath indexPath)
        {
            return "Close Tag";//return what the slide button should say
        }

        /// <summary>
        /// slide to delete functionality
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="editingStyle"></param>
        /// <param name="indexPath"></param>
        public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
        {
            // base.CommitEditingStyle(tableView, editingStyle, indexPath);
            if (editingStyle == UITableViewCellEditingStyle.Delete)
            {
                if (owner2 != null)  //if we are on mytags screen and we click delete
                {
                    TaggingList.getItems()[(int)indexPath.Item].isClosed = true;        //set the tag that is clicked to closed. 
                    TaggingList.getItems()[(int)indexPath.Item].hasChanged = true;
                    Tagging t = TaggingList.getItems()[(int)indexPath.Item];
                    Application.getDB().Update(TaggingList.getItems()[(int)indexPath.Item]); //update the database with the closed tag
                    myTags = true;
                    owner2.PerformSegue("myTagToMyTagSegue", owner2);   //perform the segue to the mytag navigation controller, which segues back into my tags.
                }
                else//if we are clicking delete on the alltags view
                {
                    UIAlertView al = new UIAlertView("Error!", "You can only close tags from My Tags screen", new UIAlertViewDelegate(), "OK", null); //create an alert that says error
                    al.Show();//show the alert to the users.
                }
            }
        }

        /// <summary>
        /// returns the number of items in the array since it was initialized manually
        /// </summary>
        /// <param name="tableview">hi scott</param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (TableItems != null)
            {
                return TableItems.Count;
            }
            else if (suggested != null)
            {
                return suggested.Length;
            }
            return 0;
        }
        /// <summary>
        /// this sets up the table to display the pole numbers because i believe that the pole numbers are more worth viewing than the tag numbers
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            if (TableItems == null)
            {
                string item = suggested[indexPath.Row];
                if (cell == null)
                { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

                cell.TextLabel.Text = item; //set display of each cell as a set of pole numbers
                return cell;
            }
            else
            {
                string item = TableItems[indexPath.Row].poleOne + " - " + TableItems[indexPath.Row].poleTwo; //display each cell as a set of pole numbers

                //---- if there are no cells to reuse, create a new one
                if (cell == null)
                { cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier); }

                cell.TextLabel.Text = item; //set display of each cell as a set of pole numbers

                return cell;
            }
        }

        /// <summary>
        /// perform a function when a row is selected in either table mytags or Alltags
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (owner != null)//if we are in all tags
            {

                owner.PerformSegue("detailssegue", this); //move into the details page from all tags
                tableView.DeselectRow(indexPath, true);
            }
            else if (owner2 != null)//if we are in myTags,
            {
                owner2.PerformSegue("MyTagsSegue", this);//go to details screen
                tableView.DeselectRow(indexPath, true);
            }
            else
            {
                if (owner3.getReq() == tableView)
                {
                    owner3.setRequest(GetCell(tableView, indexPath).TextLabel.Text);
                }
                else if (owner3.getReqFor() == tableView)
                {
                    owner3.setRequestFor(GetCell(tableView, indexPath).TextLabel.Text);
                }

                else if (owner3.getTruck() == tableView)
                {
                    owner3.setTruck(GetCell(tableView, indexPath).TextLabel.Text);
                }
                // set fill text
                // hide auto fill
            }
        }
    }
}