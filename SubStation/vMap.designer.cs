// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using MapKit;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Tagging
{
    [Register ("vEsriMap")]
    partial class vEsriMap
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MapViewController { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView myMap { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MapViewController != null) {
                MapViewController.Dispose ();
                MapViewController = null;
            }

            if (myMap != null) {
                myMap.Dispose ();
                myMap = null;
            }
        }
    }
}