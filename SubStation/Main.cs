using UIKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SQLite;
using System.IO;
using SQLitePCL;

namespace Tagging
{
    public class Application
    {
        private static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "testDB");//create a public static connection to the database
        private static SQLiteConnection db;//create a connection to database that is accessable throughout program


        private static Thread thread;//thread in the background pushing and pulling from the cdb
                                     // This is the main entry point of the application.
        public static Thread getThread()
        {
            return thread;
        }
        public static void setThread(Thread t)
        {
            thread = t;
        }
        public static SQLiteConnection getDB()
        {
            return db;
        }
        static void Main(string[] args)
        {
            db = new SQLiteConnection(dbPath);//open connection
            //db.DropTable<Users>();
            //db.DropTable<Tagging>();
            //db.DropTable<RequestedByHistory>();
            //db.DropTable<TruckHistory>();
            //db.DropTable<RequestedForHistory>();
            
            db.CreateTable<Users>();// create a User table ifndef
            db.CreateTable<Tagging>();//create tagging table ifndef
            db.CreateTable<RequestedByHistory>();// create a autocomplete table  ifndef
            db.CreateTable<RequestedForHistory>();// create a autocomplete table  ifndef
            db.CreateTable<TruckHistory>();// create a autocomplete table  ifndef
          
            UIApplication.Main(args, null, "AppDelegate");//start the app
            db.Close();
        }
    }
}