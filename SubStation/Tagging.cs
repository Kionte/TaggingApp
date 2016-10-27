using System;
using System.Collections.Generic;
using System.Text;
using CoreLocation;
using SQLite;
namespace Tagging

{
    public class Tagging
    {
        [PrimaryKey]
        // public int virtTagNum { get; set; }//a virtual tag number to act as a primary key in the local database.

        public long tagNum { get; set; }// = DateTime.Now.ToString("yyMMddHHmm"); // temp tag num year month day hour minunte 
        [MaxLength(20)]
        public string date { get; set; }// = DateTime.Today.ToString();
        [MaxLength(20)]
        public string time { get; set; }// = DateTime.Now.ToString();
        [MaxLength(20)]
        public string type { get; set; }// = "  "; // i think we need these three on the storyboard
        [MaxLength(20)]
        public string issuedBy { get; set; }// = "  ";
        [MaxLength(20)]
        public string requestedBy { get; set; }// = "  ";
        public string truckNum { get; set; }// = truckNumText.Text
        [MaxLength(20)]
        public string requestedFor { get; set; }// = requestedForText.Text;[MaxLength(20)]
        [MaxLength(20)]
        public string purpose { get; set; }// PurposeTextView.Text;
        [MaxLength(20)]
        public string equipment { get; set; }// = EquipmentTextView.Text;
        [MaxLength(20)]
        public string poleOne { get; set; }// = poleOneText.Text;
        [MaxLength(20)]
        public string poleTwo { get; set; }// = poleTwoText.Text;
        [MaxLength(20)]
        public string notifications { get; set; }// = RequiredNotifications.Text;
        [MaxLength(250)]
        public string comments { get; set; }// = commentsText.Text;
        public string username { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public CLLocationCoordinate2D loc;

        public bool isClosed { get; set; }  //if the tag is closed
        public bool hasChanged { get; set; }    //has the tag changed?
        public bool inCDB { get; set; }         //is the tag in the CDB(central database
        //this is so we can do the foreach loops
        /// <summary>
        /// ctor
        /// </summary>
        public Tagging() { tagNum = 0; isClosed = false; hasChanged = false; inCDB = false; }



    }
}