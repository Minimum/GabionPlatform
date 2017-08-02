using System;

namespace GabionPlatform.Models.Requests
{
    public class EditNewsStatusRequest
    {
        public String Title { get; set; }
        public String Content { get; set; }

        public EditNewsStatusRequest()
        {
            Title = "";
            Content = "";
        }
    }
}