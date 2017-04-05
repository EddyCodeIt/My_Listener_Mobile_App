using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using System.Diagnostics;
using Windows.Services.Maps;
using Windows.UI.Popups;

namespace My_Listener.Services.GeoLocationServices
{
    class GeoLocationService
    {
        // Geo Variables
        Geolocator geolocator;
        Geoposition current_pos;
        private string geo_status; // status to feed UI with
        #region GET/SET for geo_status
        public string Geo_status
        {
            get
            {
                return geo_status;
            }

            set
            {
                geo_status = value;
            }
        }
        #endregion

        public GeoLocationService()
        {   // create Geolocator
            geolocator = new Geolocator();

        }
       // initialize Geo Locator 
        public async void initialiseGeoLocation()
        {
            // Ask for user permissions to use GPS
            var userPermission = await Geolocator.RequestAccessAsync();

            // check permission status 
            switch (userPermission)
            {
                case GeolocationAccessStatus.Unspecified:
                    geo_status = "Can't Access Location Services. Maybe your device don't have Geolocator?";
                    break;
                case GeolocationAccessStatus.Allowed:
                    geo_status = "Obtaining Geo Locations";

                    geolocator.StatusChanged += geolocatorStatusChanged; // status handler
                    geolocator.DesiredAccuracy = PositionAccuracy.Default; // run default, as we no need very precise coords
                                                                          // to get city and country data
                    break;
                case GeolocationAccessStatus.Denied:
                    // Will also return DENIED if GPS Location Service is not active on user device.
                    // User notification 
                    await new MessageDialog("Please, allow me to store your location! It's REST SERVICE. No privacy exposed! ;)").ShowAsync();
                    geo_status = "DENIED";
                    break;
                default:
                    break;
            }
        }

        private void geolocatorStatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Ready:
                    geo_status = "Ready";
                    break;
                case PositionStatus.Initializing:
                    geo_status = "Initializing";
                    break;
                case PositionStatus.NoData:
                    geo_status = "No Data";
                    break;
                case PositionStatus.Disabled:
                    geo_status = "Disabled";
                    break;
                case PositionStatus.NotInitialized:
                    geo_status = "Not Initialized";
                    break;
                case PositionStatus.NotAvailable:
                    geo_status = "Not Available";
                    break;
                default:
                    break;
            } // end of switch
        } // end of geolocatorStatusChanged()

        public async Task<string> GetCurrentPossition()
        {
            try
            {
                current_pos = await geolocator.GetGeopositionAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }

            BasicGeoposition cur_location = new BasicGeoposition {
                Longitude = current_pos.Coordinate.Point.Position.Longitude,
                Latitude = current_pos.Coordinate.Point.Position.Latitude
            };

            Geopoint reverseGeocode = new Geopoint(cur_location);
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(reverseGeocode);
        
            return result.Locations[0].Address.Town.ToString() + ", " 
                   + result.Locations[0].Address.Country.ToString();

        } // end of GetCurrentPossition()
    } // end of class
} // end of namespace
