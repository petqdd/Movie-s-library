namespace MovieLibrary.Services.Models
{
    using System;

    public class GoogleResponse
    {
        public bool Success { get; set; }

        //public double Score { get; set; }

        //public string Action { get; set; }

        public DateTime ChallangeTimeStamp { get; set; }

        public string HostName { get; set; }
    }
}
