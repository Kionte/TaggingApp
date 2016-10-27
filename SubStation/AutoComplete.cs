using System;
using System.Collections.Generic;
using System.Text;
using CoreLocation;
using SQLite;

namespace Tagging
{
    /// <summary>
    /// Table in the local database that holds the previous entrys of Requested By.
    /// </summary>
    public class RequestedByHistory
    {
        [PrimaryKey, AutoIncrement]
        // The id of each item in the table will auto increment 
        public int id { get; set; }
        // The string that the user enters 
        public string entry { get; set; }

        public int count { get; set; }
        /// <summary>
        /// Constructor when user submits tag it will create this object and take whatever they wrote for 'requested By' history and store that in the local database.
        /// </summary>
        /// <param name="entry">What ever the user enters in 'Requested By' will be set as their entry"string"</param>
        public RequestedByHistory(string entry)
        {
            this.entry = entry;
        }
        /// <summary>
        /// No args constructor. 
        /// </summary>
        public RequestedByHistory() { }
    }

    /// <summary>
    /// Table in the local database that holds the previous entrys of Truck Num.
    /// </summary>
    public class TruckHistory
    {
        [PrimaryKey, AutoIncrement]
        // The id of each item in the table will auto increment 
        public int id { get; set; }
        // The string that the user enters 
        public string entry { get; set; }

        /// <summary>
        /// Constructor when user submits tag it will create this object and take whatever they wrote for 'Truck Num' and store that in the local database.
        /// </summary>
        /// <param name="entry">What ever the user enters in 'Truck num' will be set as their entry string</param>
        public TruckHistory(string entry)
        {
            this.entry = entry;
        }
        /// <summary>
        /// No arg constructor.
        /// </summary>
        public TruckHistory() { }
    }

    /// <summary>
    /// Table in the local database that holds the previous entrys of Requested For.
    /// </summary>
    public class RequestedForHistory
    {
        [PrimaryKey, AutoIncrement]
        // The id of each item in the table will auto increment 
        public int id { get; set; }
        // The string that the user enters
        public string entry { get; set; }

        /// <summary>
        /// Constructor when user submits tag it will create this object and take whatever they wrote for 'Requested For' and store that in the local database.
        /// </summary>
        /// <param name="entry">What ever the user enters in 'Requested For' will be set as their entry string</param>
        public RequestedForHistory(string entry)
        {
            this.entry = entry;
        }
        /// <summary>
        /// No arg constructor.
        /// </summary>
        public RequestedForHistory() { }
    }
}
