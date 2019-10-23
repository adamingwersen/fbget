using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Core.Connections;

using Services.Token;
using Services.Proxy;
using Data;
using Operations.ClientOperations;
using Client;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Define db connections
            DBClient dbcli = new DBClient("facebook", "adam", "sutdigsevl");
            IMongoCollection<object> coll = dbcli.ConnectToCollection("facebook", "posts_test2");
            IMongoCollection<object> commentColl = dbcli.ConnectToCollection("facebook", "comments_test2");
            IMongoCollection<object> feedColl = dbcli.ConnectToCollection("facebook", "feeds_test2");


            // Retrieve Token
            TokenInformation tokenmodule = new TokenInformation();
            tokenmodule.SetValue();
            var token = tokenmodule.GetOAuthToken;

            // Retrieve Proxy Credentials
            ProxyInformation proxymodule = new ProxyInformation();
            proxymodule.SetValue();
            var proxyCredentials = proxymodule.GetProxyCredentials;

            // Setup API arguments
            APIArgs _apiArgs = new APIArgs();
            var defaultPost = _apiArgs.GetArgs("DefaultPost");
            var defaultComment = _apiArgs.GetArgs("DefaultComment");
            var defaultFeed = _apiArgs.GetArgs("DefaultFeed");

            // Setup clients
            var facebookClient = new FacebookClient(proxyCredentials);
            var clientOperator = new ClientGetOperations(facebookClient);
            var dbOperator = new ClientStoreOperations(dbcli);


            // Run Retrieval of posts //

            //string next = "";
            //string arg = defaultPost;
            //do
            //{
            //    var getPostTask = clientOperator.GetPostsAsync(token, "youseedanmark", arg);
            //    Task.WaitAll(getPostTask);
            //    var posts = getPostTask.Result;
            //    next = clientOperator.GetNextPostId;
            //    arg = _apiArgs.AddNextKey("DefaultPost", next);
            //    var storeTask = dbOperator.WritePostsAsync(coll, posts);
            //    Task.WaitAll(storeTask);
            //    Console.WriteLine(next);

            //} while (next != "");
 

            // Run retrieval of feed //

            //string next = "";
            //string arg2 = defaultFeed;
            //do
            //{
            //    var getFeedTask = clientOperator.GetFeedAsync(token, "youseedanmark", arg2);
            //    Task.WaitAll(getFeedTask);
            //    var feed = getFeedTask.Result;
            //    next = clientOperator.GetNextFeedId;
            //    arg2 = _apiArgs.AddNextKey("DefaultFeed", next);
            //    var storeTask = dbOperator.WriteFeedAsync(feedColl, feed);
            //    Task.WaitAll(storeTask);
            //} while (next != "");

			
            // Run Retrieval of comments //

                // Setup postids
            var postIds = dbOperator.ReadDistinctAsync(coll, "post_id").Result;

            var next = "";
            foreach (var idd in postIds)
            {
                try
                {
					Console.WriteLine("primo");
					var id = idd.ToString();
					var getCommentTask = clientOperator.GetCommentsAsync(token, id, defaultComment);
					Task.WaitAll(getCommentTask);
					var comments = getCommentTask.Result;
					next = clientOperator.GetNextCommentId;
					var storeTask = dbOperator.WriteCommentsAsync(commentColl, comments);
					Task.WaitAll(storeTask);
                    List<string> checklist = new List<string>();
					while (next != "")
					{
						var subargs = _apiArgs.AddNextKey("DefaultComment", next);
						var getSubCommentTask = clientOperator.GetCommentsAsync(token, id, subargs);
						Task.WaitAll(getSubCommentTask);
                        next = clientOperator.GetNextCommentId;
                        checklist.Add(next);
                        if (checklist.Contains(next))
                        {
                            break;
                        }
                        else
                        {
                            var subcomments = getSubCommentTask.Result;
                            var storeSubTask = dbOperator.WriteCommentsAsync(commentColl, comments);
                            Task.WaitAll(storeSubTask);
                        }
					}
                }
                catch (System.AggregateException)
                {
                    Console.WriteLine("Bad response");
                    continue;
                }

            }

		}
    }
}
