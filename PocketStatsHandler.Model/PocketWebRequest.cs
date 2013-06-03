using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PocketStatsHandler.Contracts;

namespace PocketStatsHandler.Model
{
    public class PocketWebRequest : IPocketWebRequest
    {
        public string Url { get; set; }
        public string ContentyType { get; set; }
        public string RequestMethod { get; set; }
        public string Request { get; set; }
    }
}
