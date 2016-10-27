//using Esri.ArcGISRuntime.Mapping;
//using Esri.ArcGISRuntime.UI;
using Foundation;
using System;
using UIKit;

namespace Tagging
{    
    public partial class vEsriMap : UIViewController
    {
        private const int yPageOffset = 60;  // Constant holding offset where the MapView control should start

   //     private MapView _myMapView = new MapView();  // Create and hold reference to the used MapView

        public vEsriMap(IntPtr handle) : base(handle) { } // c tor

        public vEsriMap() { Title = "ArcGIS map image layer (URL)"; } // maybe dont need

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CreateLayout(); // Create the UI, setup the control references and execute initialization 
            Initialize();
        }

        private void Initialize()
        { 
      //      Map myMap = new Map();   // Create new Map 

            // Create uri to the map image layer
            var serviceUri = new Uri("http://services.arcgisonline.com/arcgis/rest/services/World_Imagery/MapServer");

            // Create new image layer from the url
     //       ArcGISTiledLayer imageLayer = new ArcGISTiledLayer(serviceUri);

            // Add created layer to the basemaps collection
     //       myMap.Basemap.BaseLayers.Add(imageLayer);
            serviceUri =  new Uri(
                "http://services.arcgisonline.com/arcgis/rest/services/ESRI_StreetMap_World_2D/MapServer");//street map
//
    //        ArcGISTiledLayer tileLayer = new ArcGISTiledLayer(serviceUri);

          //  myMap.Basemap.BaseLayers.Add(tileLayer);//add a layer to the map

 //           _myMapView.Map = myMap; // Assign the map to the MapView
        }

        private void CreateLayout()
        {
            // Setup the visual frame for the MapView
//            _myMapView.Frame = new CoreGraphics.CGRect(
       //         0, yPageOffset, View.Bounds.Width, View.Bounds.Height - yPageOffset);

            // Add MapView to the page
  //          View.AddSubviews(_myMapView);
        }
        
    }
}