using Foundation;
using System;
using UIKit;
using System.Threading;

namespace Tagging
{
    public partial class vThreeButtonView : UIViewController
    {
        taggingThread tThread;
        public vThreeButtonView(IntPtr handle) : base(handle) { }

        /// <summary>
        /// loads the view with the three buttons
        /// </summary>
        public override void ViewDidLoad()
        {
            // My tags customization 
            base.ViewDidLoad();
            this.MyTagsButton.Layer.ShadowColor = new CoreGraphics.CGColor(0f, 134f / 255f, 1f);
            this.MyTagsButton.Layer.ShadowOpacity = 1;
            this.MyTagsButton.Layer.ShadowRadius = 1;
            this.MyTagsButton.Layer.ShadowOffset = new CoreGraphics.CGSize(2, 2);
            this.MyTagsButton.Layer.CornerRadius = (nfloat)this.MyTagsButton.Bounds.Height / 2;
            this.MyTagsButton.ClipsToBounds = true;

            this.NewTagButton.Layer.CornerRadius = 1;
            this.MyTagsButton.Layer.CornerRadius = 1;
            this.AllTagButton.Layer.CornerRadius = 1;

            if (Application.getThread() == null)
            {// this is where we start the thread.  
                tThread = new taggingThread();
                Application.setThread(new Thread(tThread.checkList));// checking if the list is empty
                Application.getThread().Start();
            }
        }

        /// <summary>
        /// does nothing yet
        /// </summary>
        /// <param name="sender"></param>
        partial void AllTagButton_TouchUpInside(UIButton sender) { }


    }
}