using System;
using System.Collections.Generic;
using System.Text;
using CoreLocation;

namespace Tagging
{
    class TaggingList
    {
        //list of tags to populate the table wit
        private static List<Tagging> tableItems = new List<Tagging>();

        /// <summary>
        /// default constructor
        /// </summary>
        public TaggingList() { }

        /// <summary>
        /// get items in the table from anywhere in the program
        /// </summary>
        /// <returns></returns>
        public static List<Tagging> getItems()
        {
            return tableItems;
        }

        /// <summary>
        /// set the items in the list
        /// </summary>
        /// <param name="t"></param>
        public static void setItems(List<Tagging> t)
        {
            tableItems = t;
        }

        /// <summary>
        /// add a tag to the table items list
        /// </summary>
        /// <param name="tag"></param>
        public static void Add(Tagging tag)
        {
            for (int i = 0; i < tableItems.Count; i++)// for the amount of items in the tagging list
            {
                if (tag.tagNum == getItems()[i].tagNum)//check if the tag we are trying to add to the list has the same tag number as another 
                {
                    //edit(tag);      //if they are equal, then call the edit function instead and return
                    return;
                }
            }
            tableItems.Add(tag);//if none of the tags have the same id, then add the new tag to the list of tags
        }

        /// <summary>
        /// add a tag by reference so we can make sure we have the right data throughout the program
        /// </summary>
        /// <param name="tag"></param>
        public static void Add(ref Tagging tag)
        {
            for (int i = 0; i < tableItems.Count; i++)// for the amount of items in the tagging list
            {
                if (tag.tagNum == getItems()[i].tagNum)//check if the tag we are trying to add to the list has the same tag number as another 
                {
                    edit(tag);      //if they are equal, then call the edit function instead and return
                    return;
                }

            }
            tableItems.Add(tag);//if none of the tags have the same id, then add the new tag to the list of tags
        }

        /// <summary>
        /// edit the tag with the same tag number as the paramater.tagNum;
        /// </summary>
        /// <param name="tag"></param>
        public static void edit(Tagging tag)
        {
            for (int i = 0; i < tableItems.Count; i++)//for all the tags in the list
            {
                if (tag.tagNum == tableItems[i].tagNum)//find the tag with the same number.
                {
                    tableItems[i] = tag;//replace that tag with the edited version.
                    return;
                }

            }
            Add(ref tag);   //if the same tag number does not exist in the list, then add the new one.
        }
    }
}