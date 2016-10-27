using System;
using System.Collections.Generic;
using UIKit;

namespace Tagging
{
    /// <summary>
    /// 
    /// </summary>
    public class PickerModel : UIPickerViewModel
    {
        private IList<string> values;//list of values to put in our picker

        private event EventHandler<PickerChangedEventArgs> PickerChanged;//for when the picker moves

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> getValues()
        {
            return values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public PickerModel(IList<string> values)
        {
            this.values = values;//set the values in the picker
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return values.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="row"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return values[(int)row].ToString(); //return the string selected
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 40f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="row"></param>
        /// <param name="component"></param>
        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            if (this.PickerChanged != null)
            {
                this.PickerChanged(this, new PickerChangedEventArgs { SelectedValue = values[(int)row] });
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class PickerChangedEventArgs : EventArgs
    {
        public object SelectedValue { get; set; }
    }

}