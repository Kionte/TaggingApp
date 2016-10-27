using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using UIKit;

namespace Tagging
{
    public partial class vLogin : UIViewController
    {
        /// <summary>
        /// ctor
        /// </summary>
        public static Users user = new Users();
        public vLogin(IntPtr handle) : base(handle) { }
        /// <summary>
        /// On load auto fill username and password with last logged in user.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            fillPassword.SecureTextEntry = true;
            var table = Application.getDB().Table<Users>();
            if (table.Count() != 0)
            {
                if (table.First().userStatus)
                {
                    this.fillUsername.Text = table.First().userName;
                    this.fillPassword.Text = table.First().password;
                }
            }
        }
        /// <summary>
        ///  on touch check to see if login is valid 
        /// </summary>
        /// <param name="sender"></param>
        partial void UIButton11159_TouchUpInside(UIButton sender)
        {
            if (fillUsername.Text != "" && fillPassword.Text != "")
            {
                var table = Application.getDB().Table<Users>();

                if (table.Count() != 0)
                {
                    if (table.First().userStatus)
                    {
                        if (fillUsername.Text == table.First().userName && fillPassword.Text == table.First().password)
                        {
                            PerformSegue("loginSegue", this);
                            return;
                        }
                    }

                }
                if (Reachability.IsHostReachable("http://google.com"))
                {
                    if (!validLogin())
                    {
                        UIAlertView invalid = new UIAlertView("Invalid", "Incorrect password or username ", new UIAlertViewDelegate(), "Ok", null);
                        invalid.Show();
                    }
                    else
                    {
                        Users user = new Users();
                        user.userName = fillUsername.Text;
                        user.password = fillPassword.Text;
                        user.userStatus = true;

                        Application.getDB().Insert(user);

                        PerformSegue("loginSegue", this);
                    }
                }
                else
                {
                    UIAlertView invalid = new UIAlertView("Invalid", "No internet connection. ", new UIAlertViewDelegate(), "Ok", null);
                    invalid.Show();
                }
            }
            else
            {
                UIAlertView invalid = new UIAlertView("Invalid", "Incorrect password or username", new UIAlertViewDelegate(), "Ok", null);
                invalid.Show();
            }
        }


        /// <summary>
        /// checks to see if login in credentials are valid by comparing getHash() to the set hash value
        /// </summary>
        /// <returns></returns>
        bool validLogin()
        {
            Parse p = new Parse();

            string[] users = getResponse();

            // if hash values match (username and password work)
            if (p.GetDataValue(users[0], "result") == fillUsername.Text) //~ for testing need to change if statement 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// sends login info to the php script and trys to find match if there is a match then this function will return the hash value
        /// </summary>
        /// <returns></returns>
        public string[] getResponse()
        {
            string[] loginList;
            Regex r = new Regex("(.)");

            WebClient wc = new WebClient();
            Uri insertStr = new Uri("http://tagging.inlandpower.com/users.php");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("username", fillUsername.Text);
            parameters.Add("password", fillPassword.Text);
            byte[] stringToBe = wc.UploadValues(insertStr, parameters);
            string loginToString = System.Text.Encoding.UTF8.GetString(stringToBe);

            Match mLogin = r.Match(loginToString);
            if (mLogin.Success)
            {
                loginList = loginToString.Split(new string[] { "~`^Y" }, StringSplitOptions.None);
                return loginList;
            }
            return null;
        }
        /// <summary>
        /// shows the pasword instead of dots or dots instead of password
        /// </summary>
        /// <param name="sender"></param>
        partial void ShowPasswordButton_TouchUpInside(UIButton sender)
        {
            if (fillPassword.SecureTextEntry == false)
            {
                showPasswordButton.SetTitle("show", UIControlState.Normal);
                fillPassword.SecureTextEntry = true;
            }
            else
            {
                showPasswordButton.SetTitle("hide", UIControlState.Normal);
                fillPassword.SecureTextEntry = false;
            }

        }
        /// <summary>
        /// prepares for the swguew to vhome
        /// </summary>
        /// <param name="segue"></param>
        /// <param name="sender"></param>
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            user.userName = fillUsername.Text;
        }
    }
}