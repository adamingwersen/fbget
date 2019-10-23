using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.FacebookObjects
{
    public class Comment
    {
        public string origin_type { get; set; }
        public string origin_id { get; set; }
        public DateTime origin_created_time { get; set; }
        public string comment_id { get; set; }
        public DateTime created_time { get; set; }
        public string message { get; set; }
        public string from_name { get; set; }
        public string from_id { get; set; }
        public int likes_count { get; set; }
        public Application application { get; set; }
    }

    public class Application
    {
        public string category { get; set; }
        public string link { get; set; }
        public string name { get; set; }
        public string application_id { get; set; }
    }
}
