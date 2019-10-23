using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.FacebookObjects
{
    public class Post
    {
        public string post_id { get; set; }
        public DateTime created_time { get; set; }
        public string status_type { get; set; }
        public string promotion_status { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
        public string message { get; set; }
        public int likes_count { get; set; }
        public int shares_count { get; set; }
        public int comments_count { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public string text { get; set; }
    }
}
