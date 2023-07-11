using Microsoft.AspNetCore.Components;

namespace Ip.Client.Pages
{
    public partial class Index
    {
        private string ip;

        [Inject]
        public HttpClient Client { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ip = await Client.GetStringAsync("api/ip");
        }

    }
}
