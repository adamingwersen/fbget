using System;
using System.Collections.Generic;
namespace Data
{
    public class APIArgs
    {
        public Dictionary<string, string> argDict = new Dictionary<string, string>()
        {
            {"DefaultPost", "id,"+
                            "created_time," +
                            "status_type," +
                            "type," +
                            "promotion_status," +
                            "properties," +
                            "message," +
                            "likes.limit(0).summary(true)," +
                            "shares.limit(0).summary(true)," +
                            "comments.limit(0).summary(true)"},
            {"DefaultComment", "id," +
                "created_time," +
                "comments{created_time,application,message{tags,to}," +
                "message_tags{id,name}," +
                "from," +
                "likes.limit(0).summary(true)," +
                "tags}"},
            {"DefaultFeed", "id," +
                "created_time," +
                "message," +
                "from," +
                "likes.limit(0).summary(true)," +
                "comments{id,created_time,message,application," +
                "likes.limit(0).summary(true),from}"}
        };

        public string AddNextKey(string argType, string key)
        {
            return (argDict[argType] + "&after=" + key); 
        }

        public string GetArgs(string argType)
        {
            return (argDict[argType]);
        }

    }
}
