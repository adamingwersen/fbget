using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

using Client;
using Data.FacebookObjects;
using Operations.DataOperations;

namespace Operations.ClientOperations
{
    public interface IClientGetOperations
    {
        Task<List<Post>> GetPostsAsync(string accesToken, string pageview, string args);
        Task<List<Comment>> GetCommentsAsync(string accessToken, string endpoint, string args);
        Task<List<Feed>> GetFeedAsync(string accessToken, string endpoint, string args);
    }

    public class ClientGetOperations : IClientGetOperations
    {
        /// <summary>
        /// A (string) cursor for next post id
        /// </summary>
        private static string NextPostId = null;

		/// <summary>
		/// A (string) cursor for next post id
		/// </summary>
		private static string NextCommentId = null;

		/// <summary>
		/// A (string) cursor for next post id
		/// </summary>
		private static string NextFeedId = null;

        /// <summary>
        /// An instance of the facebook-client interface
        /// </summary>
        private readonly IFacebookClient _facebookClient;

        /// <summary>
        /// An instance of the PopulateNETObject class
        /// </summary>
        private readonly PopulateNETObject _dataHandler = new PopulateNETObject();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Operations.ClientOperations.ClientGetOperations"/> class.
        /// </summary>
        /// <param name="facebookClient">Facebook client.</param>
        public ClientGetOperations(IFacebookClient facebookClient)
        {
            _facebookClient = facebookClient;
        }

        /// <summary>
        /// Getrequest for posts
        /// </summary>
        /// <returns>A list of dynamic post-responses</returns>
        /// <param name="accessToken">Access token.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="args">Arguments.</param>
        public async Task<List<Post>> GetPostsAsync(string accessToken, string endpoint, string args)
        {
            var response = await _facebookClient.GetAsync<dynamic>(
                accessToken, endpoint, "posts", args);
            
            var posts = _dataHandler.PopulatePosts(response);

            if (response.paging.next != null)
            {
                NextPostId = response.paging.cursors.after;   
            }
            else 
            {
                NextPostId = "";
            }

            return posts;
        }

		/// <summary>
		/// Getrequest for comments
		/// </summary>
		/// <returns>A list of comment-objects/returns>
		/// <param name="accessToken">Access token.</param>
		/// <param name="endpoint">Endpoint.</param>
		/// <param name="args">Arguments.</param>
		public async Task<List<Comment>> GetCommentsAsync(string accessToken, string endpoint, string args)
        {
            var response = await _facebookClient.GetAsync<dynamic>(
                accessToken, endpoint, "", args);

            if(response.comments.paging.next != null)
            {
                NextCommentId = response.comments.paging.cursors.after;
            }
            else
            {
                NextCommentId = "";
            }

            var cmnts = _dataHandler.PopulateComments(response, "Post");

            return cmnts;
        }

        /// <summary>
        /// Get-request for page-feed
        /// </summary>
        /// <returns>A list of feed-objects> </returns>
        /// <param name="accessToken">Access token.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="args">Arguments.</param>
        public async Task<List<Feed>> GetFeedAsync(string accessToken, string endpoint, string args)
        {
            var response = await _facebookClient.GetAsync<dynamic>(
                accessToken, endpoint, "feed", args);

            if(response.paging.next != null)
            {
                NextFeedId = response.paging.cursors.after;
            }
            else
            {
                NextFeedId = "";
            }

            Console.WriteLine(NextFeedId);

            var feed = _dataHandler.PopulateFeed(response);
            return feed;
        }

        /// <summary>
        /// Gets the get next post identifier.
        /// </summary>
        /// <value>The get next post identifier.</value>
		public string GetNextPostId
		{
			get => NextPostId;
		}

        /// <summary>
        /// Gets the next comment identifier.
        /// </summary>
        /// <value>The next comment identifier.</value>
        public string GetNextCommentId
        {
            get => NextCommentId;
        }

		/// <summary>
		/// Gets the next feed identifier.
		/// </summary>
		/// <value>The next feed identifier.</value>
		public string GetNextFeedId
        {
            get => NextFeedId;
        }
    }
}
