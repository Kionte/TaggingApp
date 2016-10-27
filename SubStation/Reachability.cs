using CoreFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SystemConfiguration;


namespace Tagging
{
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Reachability
    {
        public static string HostName = "www.google.com";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0
                || (flags & NetworkReachabilityFlags.IsWWAN) != 0;

            return isReachable && noConnectionRequired;
        }

        /// <summary>
        /// Is the host reachable with the current network configuration
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool IsHostReachable(string host)
        {
            if (string.IsNullOrEmpty(host))
                return false;

            using (var r = new NetworkReachability(host))
            {
                NetworkReachabilityFlags flags;

                if (r.TryGetFlags(out flags))
                    return IsReachableWithoutRequiringConnection(flags);
            }
            return false;
        }

        /// <summary>
        ///  Raised every time there is an interesting reachable event, we do not even pass the info as to what changed, and we lump all three status we probe into one
        /// </summary>
        public static event EventHandler ReachabilityChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        static void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if (h != null)
                h(null, EventArgs.Empty);
        }

        /// <summary>
        /// Returns true if it is possible to reach the AdHoc WiFi network and optionally provides extra network reachability flags as the out parameter
        /// </summary>
        static NetworkReachability adHocWiFiNetworkReachability;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (adHocWiFiNetworkReachability == null)
            {
                adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] { 169, 254, 0, 0 }));
                adHocWiFiNetworkReachability.SetNotification(OnChange);
                adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            return adHocWiFiNetworkReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        /// <summary>
        /// 
        /// </summary>
        static NetworkReachability defaultRouteReachability;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                defaultRouteReachability.SetNotification(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            return defaultRouteReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        /// <summary>
        /// 
        /// </summary>
        static NetworkReachability remoteHostReachability;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;

            if (remoteHostReachability == null)
            {
                remoteHostReachability = new NetworkReachability(HostName);

                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = remoteHostReachability.TryGetFlags(out flags);

                remoteHostReachability.SetNotification(OnChange);
                remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
            {
                reachable = remoteHostReachability.TryGetFlags(out flags);
            }

            if (!reachable)
                return NetworkStatus.NotReachable;

            if (!IsReachableWithoutRequiringConnection(flags))
                return NetworkStatus.NotReachable;

            return (flags & NetworkReachabilityFlags.IsWWAN) != 0 ?
                NetworkStatus.ReachableViaCarrierDataNetwork : NetworkStatus.ReachableViaWiFiNetwork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);
            if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
                return NetworkStatus.NotReachable;
            else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return NetworkStatus.ReachableViaCarrierDataNetwork;
            else if (flags == 0)
                return NetworkStatus.NotReachable;
            return NetworkStatus.ReachableViaWiFiNetwork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static NetworkStatus LocalWifiConnectionStatus() //i believe this checks for wifi specifically
        {
            NetworkReachabilityFlags flags;
            if (IsAdHocWiFiNetworkAvailable(out flags))
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return NetworkStatus.ReachableViaWiFiNetwork;

            return NetworkStatus.NotReachable;
        }
    }
}