/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Text.RegularExpressions;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// this class parses the php info for all the opentags and mytags
/// </summary>

namespace Tagging
{
    public class Parse
    {
        // will store the cdb will add to the array for every row in cdb
        private string[] tagList;

        /// <summary>
        /// creates parsable array from php inforamtion
        /// </summary>
        /// <returns></returns>
        public List<Tagging> start()
        {
            //changed ip to scotts computer. also hacked yours and stole opentags.php
            string queryStr = string.Format("http://tagging.inlandpower.com/openTags.php");
            //creates an http webrequest for the url above
            var request = System.Net.HttpWebRequest.Create(queryStr);
            // sets the method to 'get' rather than 'post'.
            request.Method = System.Net.WebRequestMethods.Http.Get;
            //get the response from the page
            var response = request.GetResponse();
            // convert response into something we can work with
            System.IO.StreamReader str = new System.IO.StreamReader(response.GetResponseStream());

            // convert database to string
            string tagToString = str.ReadToEnd();
            // it will retrieve everything in the php script
            Regex r = new Regex("(.)");
            // uses the regex to parse the db( in this case it gets everything)
            Match mtag = r.Match(tagToString);
            if (mtag.Success) // if the parsing succeeds
            {
                // this puts the the string in the array seperating each part of the array by a sequence that should not be typed
                tagList = tagToString.Split(new string[] { "~`^Y" }, StringSplitOptions.None);
            }
            // moves data from tag list array to the to a actual tag object. 
            transferDataTag(false, false);
            // tried returning just to debug. right now this doesnt do much at all.
            return TaggingList.getItems();
        }

        /// <summary>
        /// parses through php script to set information in the tag
        /// </summary>
        /// <param name="myTags"></param>
        /// <param name="fromEdit"></param>
        /// <returns></returns>
        List<Tagging> transferDataTag(bool myTags, bool fromEdit)
        {
            TaggingList.setItems(new List<Tagging>());
            if (tagList != null) // this makes sure that the array containing the cdb actualy has information in it
            {
                for (int i = 0; i < tagList.Length - 1; i++) // goes through each row of the array 
                {
                    Tagging myTag = new Tagging(); // creates new tagging object
                    if (GetDataValue(tagList[i], "tagNum ") != null) // double check to make sure the array hasinformaoiton 
                    {
                        /**
                         * all this will do is get the correct daqta from the array and set that to the correct attribute in the tag object 
                         * */
                        myTag.tagNum = Convert.ToInt64(GetDataValue(tagList[i], "tagNum "));
                        myTag.comments = GetDataValue(tagList[i], "comment");
                        myTag.date = GetDataValue(tagList[i], "date");
                        myTag.equipment = GetDataValue(tagList[i], "equipment");
                        myTag.hasChanged = getBool(GetDataValue(tagList[i], "hasChanged"));
                        myTag.isClosed = getBool(GetDataValue(tagList[i], "isClosed"));
                        myTag.lat = getDouble(GetDataValue(tagList[i], "lat"));
                        myTag.lon = getDouble(GetDataValue(tagList[i], "lon"));
                        myTag.notifications = GetDataValue(tagList[i], "notification");
                        myTag.poleOne = GetDataValue(tagList[i], "pole1");
                        myTag.poleTwo = GetDataValue(tagList[i], "pole2");
                        myTag.purpose = GetDataValue(tagList[i], "purpose");
                        myTag.requestedBy = GetDataValue(tagList[i], "requestedBy");
                        myTag.requestedFor = GetDataValue(tagList[i], "requestedFor");
                        myTag.truckNum = GetDataValue(tagList[i], "truckNum");
                        myTag.type = GetDataValue(tagList[i], "type");
                        myTag.username = GetDataValue(tagList[i], "user");
                        myTag.inCDB = true;

                        //checks to see if dispatch made any changes to user tags 
                        updateLDB(myTag);
                        if (!myTags)//if we are in all tags update the list of tags to be displayed in the table
                            TaggingList.Add(myTag);
                        else
                        {
                            bool possibleToTransfer = true; // makes sure the tag is beign transefered and not just loaded from cdb
                            var table = Application.getDB().Table<Tagging>();
                            foreach (Tagging tag in table)
                            {
                                if (tag.tagNum == myTag.tagNum) 
                                {
                                    possibleToTransfer = false; // if tag num matches then it is no longer a possible transfer
                                    if (myTag.username == vLogin.user.userName && myTag.hasChanged) // if the username matches then it is the users tag and add it to ldb
                                    {
                                        Application.getDB().InsertOrReplace(myTag); 
                                    }
                                    else if(!(myTag.username == vLogin.user.userName)) // if not a match then delte the tag becasue it has been transfered.
                                    {
                                        Application.getDB().Delete<Tagging>(tag.tagNum);
                                    }
                                }
                  
                            }
                            if (myTag.username == vLogin.user.userName && possibleToTransfer) // final check for transfer if tag num does not match and user does then it is a transfer and add it to ldb
                            {
                                Application.getDB().InsertOrReplace(myTag);
                            }
                            if (table.Count() == 0) // if the table is empty and the username matches add to ldb
                            {
                                if (myTag.username == vLogin.user.userName)
                                {
                                    Application.getDB().InsertOrReplace(myTag);
                                }
                            }
                        }
                    }
                }
            }

            return TaggingList.getItems();//a debugging issue again unnecessary
        }


        /// <summary>
        /// convert to bool
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool getBool(string s)
        {
            if (s == "0")    //converting strings to booleans
                return false;

            return true;
        }


        /// <summary>
        /// convert to double
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private double getDouble(string s)
        {
            double num = 0f;
            num = Convert.ToDouble(s);//converting string to doubles
            return num;
        }


        /// <summary>
        /// convert to int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int getInt(string s)
        {
            int num = 0;
            num = Int32.Parse(s);//convert string to int
            return num;
        }


        /// <summary>
        /// parse through the array of information and look for keyword. once keyword is found get information related to the keyword and return only that 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetDataValue(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index) + index.Length);//split the different variables in tagging on a sequence of random characters
            if (value.Contains("|é| +"))
            {
                value = value.Remove(value.IndexOf("|é| +"));
            }
            return value;
        }


        /// <summary>
        /// this checks if dispatch has made any changes to the tag 
        /// </summary>
        /// <param name="cNewTag"></param>
        void updateLDB(Tagging cNewTag)
        {
            cNewTag.inCDB = true;
            var table = Application.getDB().Table<Tagging>(); // sets table to whatwever is in te db
            foreach (Tagging tag in table)
            {
                if (cNewTag.tagNum == tag.tagNum)   //if the central tag matches a local tag
                {
                    if (cNewTag.hasChanged && !tag.hasChanged)//if the central tag has changed and local hasnt
                    {
                        // cNewTag.virtTagNum = tag.virtTagNum;
                        Application.getDB().Update(cNewTag); //update the local db with the central tag.
                    }
                }
            }
        }


        /// <summary>
        /// creates parsable array from php inforamtion catered to myTag 
        /// </summary>
        /// <param name="fromEdit"></param>
        /// <returns></returns>
        public List<Tagging> myTagStart(bool fromEdit)
        {
            Regex r = new Regex("(.)"); // it will retrieve everything in the php script

            string queryStr = string.Format("http://tagging.inlandpower.com/openMyTags.php");    //changed ip to scotts computer. also hacked yours and stole opentags.php
            var request = System.Net.HttpWebRequest.Create(queryStr); //idk??

            request.Method = System.Net.WebRequestMethods.Http.Get; //idk??

            var response = request.GetResponse(); //idk??

            System.IO.StreamReader str = new System.IO.StreamReader(response.GetResponseStream()); //idk??

            string tagToString = str.ReadToEnd(); // convert database to string

            Match mtag = r.Match(tagToString); // uses the regex to parse the db( in this case it gets everything)
            if (mtag.Success) // if the parsing succeeds
            {
                tagList = tagToString.Split(new string[] { "~`^Y" }, StringSplitOptions.None); // this puts the the string in the array seperating each part of the array by a ';'
            }
            transferDataTag(true, fromEdit); // moves data from tag list array to the to a actual tag object. 
            return TaggingList.getItems();

        }
    }
}