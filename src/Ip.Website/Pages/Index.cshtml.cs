using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Sockets;
using System.Net;

namespace Ip.Website.Pages
{
    public class IndexModel : PageModel
    {
        public string IPv4Address { get; set; }
        public string IPv6Address { get; set; }

        public void OnGet()
        {
            if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                foreach (var ip in forwardedFor.ToString().Split(','))
                {
                    if (System.Net.IPAddress.TryParse(ip.Trim(), out var ipAddress))
                    {
                        if (ipAddress.AddressFamily == AddressFamily.InterNetwork && string.IsNullOrEmpty(IPv4Address))
                        {
                            IPv4Address = ip.Trim();
                        }
                        else if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6 && string.IsNullOrEmpty(IPv6Address))
                        {
                            IPv6Address = ip.Trim();
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(IPv4Address) && HttpContext.Connection.RemoteIpAddress?.AddressFamily == AddressFamily.InterNetwork)
            {
                IPv4Address = HttpContext.Connection.RemoteIpAddress.ToString();
            }

            if (string.IsNullOrEmpty(IPv6Address) && HttpContext.Connection.RemoteIpAddress?.AddressFamily == AddressFamily.InterNetworkV6)
            {
                IPv6Address = HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

    }
}