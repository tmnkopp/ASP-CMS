using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPXUtils; 
namespace WebSite.Core
{

    [ToolboxData("<{0}:GoogleMap ID=oGoogleMap runat=server></{0}:GoogleMap>")]
    public class GoogleMap : Control
    {
        #region Properties  
        public int Zoom
        {  get;   set;  }
 
        public string DestinationAddress
        { get; set; }
        #endregion   
        
  
        protected override void Render(HtmlTextWriter writer)
        { 
            writer.WriteLine(String.Format("{0}", GetScript())); 
            base.Render(writer);
        }

        private string GetScript()
        {

            StringBuilder sb = new StringBuilder();
            if (this.Zoom < 1)
                this.Zoom = 14;

 
            sb.AppendFormat(@"
            <script src='http://maps.googleapis.com/maps/api/js?key=AIzaSyADdUQVf3zYfWe5Kx3yG564bCqbN9kkWTE&sensor=false' type='text/javascript'></script>
            <script type='text/javascript'>
                var dest_address = '{1}';
                var izoom = {0};
                var map_canvas_id = 'map_canvas';
                var directions_panel_id = 'directions_panel';  
            
            </script>", this.Zoom, this.DestinationAddress);

         
            sb.Append(@"
            <script type='text/javascript'>
         
            var directionsDisplay;
            var directionsService = new google.maps.DirectionsService();
            var map;
            var oldDirections = [];
            var currentDirections = null; 
            var geocoder = new google.maps.Geocoder();  
            
          function initialize() {
            var myOptions = {
              zoom: izoom, 
              mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            
            map = new google.maps.Map(document.getElementById(  map_canvas_id  ), myOptions);
         
            geocoder.geocode( { 'address': dest_address}, function(results, status) {
              if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                    var marker = new google.maps.Marker({
                        map: map,
                        position: results[0].geometry.location
                    }); 
                }
            }); 
            directionsDisplay = new google.maps.DirectionsRenderer({
                'map': map,
                'preserveViewport': true 
            });
            directionsDisplay.setPanel(document.getElementById(  directions_panel_id  ));

            google.maps.event.addListener(map, 'click',
              function(e) { 
                  var request = {
                    origin:e.latLng ,
                    destination:dest_address,
                    travelMode: google.maps.DirectionsTravelMode.DRIVING
                  };
                  calcRoute( request ); 
             });    
          } 
          function calcRoute( request ) { 
            directionsService.route(request, function(response, status) {
              if (status == google.maps.DirectionsStatus.OK) {
                directionsDisplay.setDirections(response);
              }
            });
          } 
        </script>
             
            <div id='map_canvas' ></div>
            <a name='dir'></a>
            <div id='directions_panel' ></div>     
 
            <script>   initialize() ;   </script>");

            return sb.ToString();
            //Page.ClientScript.RegisterStartupScript(this.GetType(), this.ClientID, js);
        }
    }
}
