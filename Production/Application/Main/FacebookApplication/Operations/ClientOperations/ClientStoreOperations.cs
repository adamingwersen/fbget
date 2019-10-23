using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


using Client;
using Data.FacebookObjects;
using Operations.DataOperations;


namespace Operations.ClientOperations
{
    public interface IClientStoreOperations
	{
        Task WritePostsAsync(IMongoCollection<object> collection, List<Post> data);
        Task WriteCommentsAsync(IMongoCollection<object> collection, List<Comment> data);
        Task WriteFeedAsync(IMongoCollection<object> collection, List<Feed> data);
        Task<List<object>> ReadDistinctAsync(IMongoCollection<object> collection, string identifier);
	}

    public class ClientStoreOperations : IClientStoreOperations
    {
        private readonly IDBClient _dbClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Operations.ClientOperations.ClientStoreOperations"/> class.
        /// </summary>
        /// <param name="dbClient">Db client.</param>
        public ClientStoreOperations(IDBClient dbClient)
        {
            _dbClient = dbClient;
        }

        /// <summary>
        /// Writes chunks of posts to a mongoDB collection
        /// </summary>
        /// <returns>Nothing.</returns>
        /// <param name="collection">Collection.</param>
        /// <param name="data">Data.</param>
        public async Task WritePostsAsync(IMongoCollection<object> collection, List<Post> data)
        {
            await _dbClient.WriteToCollectionAsync(collection, data);
        }

		/// <summary>
		/// Writes chunks of comments to a mongoDB collection
		/// </summary>
		/// <returns>Nothing.</returns>
		/// <param name="collection">Collection.</param>
		/// <param name="data">Data.</param>
		public async Task WriteCommentsAsync(IMongoCollection<object> collection, List<Comment> data)
        {
            await _dbClient.WriteToCollectionAsync(collection, data);
        }

		/// <summary>
		/// Writes chunks of feeds to a mongoDB collection
		/// </summary>
		/// <returns>Nothing.</returns>
		/// <param name="collection">Collection.</param>
		/// <param name="data">Data.</param>
		public async Task WriteFeedAsync(IMongoCollection<object> collection, List<Feed> data)
        {
            await _dbClient.WriteToCollectionAsync(collection, data);
        }

        /// <summary>
        /// Reads distinct observations from a mongoDB collection
        /// </summary>
        /// <returns>A list of values</returns>
        /// <param name="collection">Collection.</param>
        /// <param name="identifier">Any value to search by distinct on.</param>
        public async Task<List<object>> ReadDistinctAsync(IMongoCollection<object> collection, string identifier)
        {
            List<object> returnList = new List<object>();

            var results = await collection.Aggregate()
                                          .Group(new BsonDocument
            { { $"_id", new BsonDocument
                    {{$"{identifier}", $"${identifier}"}}}})
                                          .ToListAsync();


            foreach(BsonDocument item in results)
            {
                returnList.Add(item.ToBsonDocument()["_id"][$"{identifier}"]);
            }

            return (returnList);
            
        }
    }

}
