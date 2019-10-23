using System;
using System.Collections.Generic;
namespace Data.FacebookObjects
{
    public class Feed
    {
        public DateTime created_time { get; set; }
        public string feed_id { get; set; }
        public string message { get; set; }
        public string from_id { get; set; }
        public string from_name { get; set; }
        public int likes_count { get; set; }
        public List<Comment> comments { get; set; }
    }
}
