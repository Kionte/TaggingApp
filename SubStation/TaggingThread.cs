using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data.SqlClient;
using System.Threading;

using System.Collections.Specialized;
//using SubStation;

namespace Tagging
{


    class taggingThread
    {
        /// <summary>
        /// checks to see if there is any to upload to the cdb
        /// </summary>
        public void checkList() //~~we might need to change this if we are going to be pulling down from the cdb becasue it will only run if ldb is empty
        {
            var table = Application.getDB().Table<Tagging>(); Console.WriteLine("~THREAD STARTING" + Thread.CurrentThread.Name);//print something to track the thread
            Console.WriteLine(table.Count());

            while (1 == 1) // keeping it in a constant loop so that it will always be checking for connection
            {

                try
                {
                    if (table.Count() > 0)

                        checkConnection(); //if there are items in the table then check for wifi 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Table is empty: " + ex.ToString());// if the table is empty print
                }
                Thread.Sleep(1000); // sleep for 3 minutes then go through thread again. 
            }
        }
        /// <summary>
        /// checks for interweb connectivity 
        /// </summary>
        void checkConnection()
        {
            try
            {
                if (Reachability.IsHostReachable("http://google.com"))
                {
                    Console.WriteLine("reachability  connect to google.com");
                    connectToDataBase();    //connect to the database

                    // Put alternative content/message here
                }
                else
                {
                    Thread.Sleep(3000);
                    // Put Internet Required Code here
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure to connect: " + ex.ToString()); // send message here 

            }
        }

        /// <summary>
        /// connects to dataase kinda useless
        /// </summary>
        void connectToDataBase() //gain a connection to our main database
        {
            try
            {
                insertTaggingValues();//insert the values in the db into the central db
            }
            catch (Exception ex)
            {
                Console.WriteLine("~Failed to insertTaggingValues: " + ex.ToString());//if any of that fails print failed to connect to the database.
            }
        }

        /// <summary>
        /// calls upadte or insert depending on the status of the tag
        /// </summary>
        /// <param name="conn">uselless paramater</param>
        public static void insertTaggingValues()
        {
            var table = Application.getDB().Table<Tagging>(); //get the table from the local db
            foreach (Tagging tag in table)
            {
                try
                {
                    if (tag.hasChanged && tag.inCDB) //i think that this says if it has changed or it is already in the cdb?? then update but kinda backwards
                        updateLDB(tag);//if it has changed and it is here then its either from the website, or it was on the local db already which means it should already be on cdb.
                    else if (!tag.inCDB)
                        uploadLDB(tag);


                }
                catch (Exception e)
                {
                    if (tag.isClosed)
                    {
                        Application.getDB().Delete<Tagging>(tag.tagNum);
                        Console.WriteLine("Deleting tag: " + tag.tagNum + ": " + e);
                    }
                }
            }
        }
        static void updateLDB(Tagging tag)
        {

            WebClient client = new WebClient();
            Uri insertStr = new Uri("http://tagging.inlandpower.com/updateTag.php");
            //changed ip to scotts computer. also hacked yours and stole opentags.php
            {
                // NameValueCollection parameters = new NameValueCollection();//set the parameters that are being sent through the php script
                NameValueCollection parameters = setParams(tag);
                if (tag.isClosed)
                    parameters.Add("isClosed", "1");
                else
                    parameters.Add("isClosed", "0");
                //client.UploadValuesCompleted += client_UploadValuesCompleted;
                try
                {

                    byte[] by = client.UploadValues(insertStr, "POST", parameters);    //upload the values to the given php script
                    string byt = Encoding.UTF8.GetString(by);
                    if (tag.isClosed)
                    {
                        Application.getDB().Delete<Tagging>(tag.tagNum); //if its closed remove it from the local database
                    }
                    else
                    {
                        tag.hasChanged = false; //otherwise just update the local database
                        Application.getDB().Update(tag);
                    }


                }
                catch (Exception)
                {

                }
            }
        }

        static void uploadLDB(Tagging tag)
        {
            WebClient client = new WebClient();

            Uri insertStr = new Uri("http://tagging.inlandpower.com/insertTag.php");
            UriBuilder build = new UriBuilder("http", "tagging.inlandpower.com/insertTag.php");

            //changed ip to scotts computer. also hacked yours and stole opentags.php
            {
                NameValueCollection parameters = setParams(tag);
                parameters.Add("isClosed", "0");


                tag.inCDB = true;
                Application.getDB().Update(tag);
                //client.UploadValuesCompleted += client_UploadValuesCompleted;
                try
                {

                    client.UploadValuesAsync(insertStr, "POST", parameters);    //upload the values to the php script with post so thearent in url



                }
                catch (Exception)
                {

                }
            }
        }


        public static NameValueCollection setParams(Tagging tag)
        {
            NameValueCollection parameters = new NameValueCollection();//give values to paramaters that we will pass to php
            parameters.Add("TagNum", tag.tagNum.ToString());
            parameters.Add("Date", tag.date);
            parameters.Add("Type", tag.type);
            parameters.Add("RequestedBy", tag.requestedBy);
            parameters.Add("TruckNum", tag.truckNum);
            parameters.Add("RequestedFor", tag.requestedFor);
            parameters.Add("Purpose", tag.purpose);
            parameters.Add("Equipment", tag.equipment);
            parameters.Add("Pole1", tag.poleOne);
            parameters.Add("Pole2", tag.poleTwo);
            parameters.Add("Comments", tag.comments);
            parameters.Add("Notifications", tag.notifications);
            Console.WriteLine("~" + tag.username);
            parameters.Add("User", vLogin.user.userName);
            // parameters.Add("VirtTagNum", tag.virtTagNum.ToString());
            parameters.Add("hasChanged", tag.hasChanged.ToString());
            parameters.Add("Lat", tag.lat.ToString());
            parameters.Add("Lon", tag.lon.ToString());
            return parameters;
        }

    }
}