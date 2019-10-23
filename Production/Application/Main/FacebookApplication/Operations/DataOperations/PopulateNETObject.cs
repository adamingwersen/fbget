using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

using Data.FacebookObjects;


namespace Operations.DataOperations
{
    public interface IPopulateNETObject
    {
        List<Post> PopulatePosts(dynamic response);
        List<Comment> PopulateComments(dynamic response, string origin_type);
        List<Feed> PopulateFeed(dynamic response);
    }

    /// <summary>
    /// Class with methods for populating custom object-types
    /// </summary>
    public class PopulateNETObject : IPopulateNETObject
    {
        /// <summary>
        /// Populates a list of Post-objects
        /// </summary>
        /// <returns>A list of Post-objects</returns>
        /// <param name="response">A list of dynamic objects from GetAsync.</param>
        public List<Post> PopulatePosts(dynamic response)
        {
            if(response == null)
            {
                return new List<Post>();
            }

			List<Post> posts = new List<Post>();
			Post post = new Post();

			foreach (var result in response.data)
			{
				try
				{
					post = new Post
					{
						post_id = result.id,
						created_time = Convert.ToDateTime(result.created_time),
						status_type = result.status_type,
						promotion_status = result.promotion_status,
						type = result.type,
						message = result.message,
						likes_count = result.likes.summary.total_count,
						shares_count = result.shares.count,
						comments_count = result.comments.summary.total_count
					};
				}
                catch
				{
					post = new Post
					{
						post_id = result.id,
						created_time = Convert.ToDateTime(result.created_time),
						status_type = result.status_type,
						promotion_status = result.promotion_status,
						type = result.type,
						message = result.message,
						likes_count = result.likes.summary.total_count,
						shares_count = 0,
						comments_count = result.comments.summary.total_count
					};
				}

				posts.Add(post);
			}
            return posts;
        }

        /// <summary>
        /// Populates a list of Comment-objects
        /// </summary>
        /// <returns>A list of Comment-objects</returns>
        /// <param name="response">A list of dynamic objects from GetAsync.</param>
        public List<Comment> PopulateComments(dynamic response, string origin_type)
        {
            List<Comment> comments = new List<Comment>();
            Comment comment = new Comment();

            try
            {
                foreach (var result in response.comments.data)
                {
                    try
                    {
                        comment = new Comment
                        {
                            origin_type = origin_type,
                            origin_id = response.id,
                            origin_created_time = Convert.ToDateTime(response.created_time),
                            comment_id = result.id,
                            created_time = Convert.ToDateTime(result.created_time),
                            message = result.message,
                            from_name = result.from.name,
                            from_id = result.from.id,
                            likes_count = result.likes.summary.total_count,
                            application = new Application
                            {
                                category = result.application.category,
                                link = result.application.link,
                                name = result.application.name,
                                application_id = result.application.id,
                            }
                        };
                    }
                    catch
                    {
                        comment = new Comment
                        {
                            origin_type = origin_type,
                            origin_id = response.id,
                            origin_created_time = Convert.ToDateTime(response.created_time),
                            comment_id = result.id,
                            created_time = Convert.ToDateTime(result.created_time),
                            message = result.message,
                            from_name = result.from.name,
                            from_id = result.from.id,
                            likes_count = result.likes.summary.total_count,
                            application = new Application
                            {
                                category = "NA",
                                link = "NA",
                                name = "NA",
                                application_id = "NA"
                            }
                        };
                    };
                    comments.Add(comment);
                }
            }
            catch
            {
                comment = new Comment
                {
                    origin_type = origin_type,
                    origin_id = response.id,
                    origin_created_time = response.created_time,
                    comment_id = "NA",
                    created_time = Convert.ToDateTime(response.created_time),
                    message = "NA",
                    from_name = "NA",
                    from_id = "NA",
                    likes_count = 0,
                    application = new Application
                    {
                        category = "NA",
                        link = "NA",
                        name = "NA",
                        application_id = "NA"
                    }
                };
                comments.Add(comment);
            };
            return comments;
        }


		/// <summary>
		/// Populates a list of Feed objects
		/// </summary>
		/// <returns>A list of Feed objects</returns>
		/// <param name="response">A list of dynamic objects from GetAsync.</param>
		public List<Feed> PopulateFeed(dynamic response)
        {
			if (response == null)
			{
				return new List<Feed>();
			}

			List<Feed> feeds = new List<Feed>();
			Feed feed = new Feed();

			foreach (var result in response.data)
			{
				try
				{
                    feed = new Feed
                    {
                        created_time = Convert.ToDateTime(result.created_time),
                        feed_id = result.id,
                        message = result.message,
                        from_id = result.from.id,
                        from_name = result.from.name,
                        likes_count = result.likes.summary.total_count,
                        comments = PopulateComments(result, "Feed")
					};
				}
				catch
				{
                    feed = new Feed
					{
						created_time = Convert.ToDateTime(result.created_time),
						feed_id = result.id,
						message = result.message,
						from_id = "NA",
						from_name = "NA",
                        likes_count = result.likes.summary.total_count,
                        comments = PopulateComments(result, "Feed")
					};
				}
                feeds.Add(feed);
			}
            return feeds;
		}
    }
}
