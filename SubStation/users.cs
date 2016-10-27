using System;
using System.Collections.Generic;
using System.Text;
using CoreLocation;
using SQLite;
namespace Tagging

{
    public class Users
    {
        [PrimaryKey]
        public int uId { get; set; }
        public bool userStatus { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
}