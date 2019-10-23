using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;

using Data.FacebookObjects;


namespace Client
{
    public interface IDBClient
    {
        IMongoDatabase ConnectToDatabase(string dbname);

        IMongoCollection<object> ConnectToCollection(string dbname, string collection);

        Task WriteToCollectionAsync(IMongoCollection<object> collection, List<Post> data);

        Task WriteToCollectionAsync(IMongoCollection<object> collection, List<Comment> data);

        Task WriteToCollectionAsync(IMongoCollection<object> collection, List<Feed> data);

        Task ReadFromCollectionAsync(IMongoCollection<object> collection, FilterDefinition<object> filter = null);
	}

    public class DBClient : IDBClient
    {
        private readonly MongoClient _mongoClient;

        /// <summary>
        /// Creates the credentials for a user given a db
        /// </summary>
        /// <returns>The credentials.</returns>
        /// <param name="db">Database name</param>
        /// <param name="usr">Username</param>
        /// <param name="pwd">Password</param>
		private MongoCredential CreateCredentials(string db, string usr, string pwd)
		{
			MongoCredential dbCredential = MongoCredential
				.CreateCredential(db, usr, pwd);
			return (dbCredential);
		}

		/// <summary>
		/// Establishes trivial connection with localhost and returns an instance of a mongodb client
		/// </summary>
		/// <returns>A connected instance of a MongoClient</returns>
		public DBClient()
		{
			var connSettings = new MongoClientSettings
			{
				Server = new MongoServerAddress("localhost", 27017),
				UseSsl = false
			};
			_mongoClient = new MongoClient(connSettings);
		}

		/// <summary>
		/// Establishes trivial connection with localhost and returns an instance of a mongodb client, with user-specified connection parameters.
		/// </summary>
		/// <returns>A connected instance of a MongoClient</returns>
		/// <param name="host">Host to connect to</param>
		/// <param name="port">Port number to connect to</param>
		public DBClient (string host, int port)
		{
			var connSettings = new MongoClientSettings
			{
				Server = new MongoServerAddress(host, port),
				UseSsl = false
			};
			_mongoClient = new MongoClient(connSettings);
		}


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Client.DBClient"/> class.
        /// </summary>
        /// <param name="db">Database</param>
        /// <param name="usr">Username</param>
        /// <param name="pwd">Password</param>
		public DBClient(string db, string usr, string pwd)
		{
			var connSettings = new MongoClientSettings
			{
				Server = new MongoServerAddress("localhost", 27017),
				UseSsl = false,
                Credentials = new[] { CreateCredentials(db, usr, pwd)}
			};
			_mongoClient = new MongoClient(connSettings);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Client.DBClient"/> class.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <param name="port">Port.</param>
		/// <param name="db">Database</param>
		/// <param name="usr">Username</param>
		/// <param name="pwd">Password</param>
		public DBClient(string host, int port, string db, string usr, string pwd)
		{
			var connSettings = new MongoClientSettings
			{
				Server = new MongoServerAddress(host, port),
				UseSsl = false,
				Credentials = new[] { CreateCredentials(db, usr, pwd) }
			};
			_mongoClient = new MongoClient(connSettings);
		}

        /// <summary>
        /// Connects to database.
        /// </summary>
        /// <returns> A connected instance of a MongoClient</returns>
        /// <param name="dbname">Dbname.</param>
        public IMongoDatabase ConnectToDatabase(string dbname)
        {
            return (_mongoClient.GetDatabase(dbname));
        }

		/// <summary>
		/// Connects to collection.
		/// </summary>
		/// <returns>A connected instance of a MongoClient.</returns>
		/// <param name="dbname">Dbname.</param>
		/// <param name="collection">Collection.</param>
		public IMongoCollection<object> ConnectToCollection(string dbname, string collection)
        {
            return (_mongoClient.GetDatabase(dbname).GetCollection<object>(collection));
        }

		
        /// <summary>
        /// Asynchronously writes a List of Post-objects to a MongoDB collection
        /// </summary>
        /// <returns>The to collection async.</returns>
        /// <param name="collection">Collection.</param>
        /// <param name="data">Data.</param>
        public async Task WriteToCollectionAsync(IMongoCollection<object> collection, List<Post> data)
        {
            try
            {
                await collection.InsertManyAsync(data);
            }
            catch (MongoWriteException)
			{
				Console.WriteLine("Could not write to Database - either data already exists" +
								  "Otherwise, connection could not be established");
			}
        }

		/// <summary>
		/// Asynchronously writes a List of Comment-objects to a MongoDB collection
		/// </summary>
		/// <returns>The to collection async.</returns>
		/// <param name="collection">Collection.</param>
		/// <param name="data">Data.</param>
		public async Task WriteToCollectionAsync(IMongoCollection<object> collection, List<Comment> data)
		{
			try
			{
				await collection.InsertManyAsync(data);
			}
			catch (MongoWriteException)
			{
				Console.WriteLine("Could not write to Database - either data already exists" +
								  "Otherwise, connection could not be established");
			}
		}

		/// <summary>
		/// Asynchronously writes a List of Feed-objects to a MongoDB collection
		/// </summary>
		/// <returns>The to collection async.</returns>
		/// <param name="collection">Collection.</param>
		/// <param name="data">Data.</param>
		public async Task WriteToCollectionAsync(IMongoCollection<object> collection, List<Feed> data)
		{
			try
			{
				await collection.InsertManyAsync(data);
			}
			catch (MongoWriteException)
			{
				Console.WriteLine("Could not write to Database - either data already exists" +
								  "Otherwise, connection could not be established");
			}
		}

        /// <summary>
        /// Reads from collection async.
        /// </summary>
        /// <returns>The from collection async.</returns>
        /// <param name="collection">Collection.</param>
        /// <param name="filter">Filter.</param>
        public async Task ReadFromCollectionAsync(IMongoCollection<object> collection, FilterDefinition<object> filter = null)
        {
            try
            {
                await Task.Run(() => collection.FindAsync(filter));
            }
            catch (MongoQueryException)
            {
                Console.WriteLine("Could not read from Database - either the provided filter is invalid" +
                                  "Otherwise, connection could not be established");
            }

        }


        /// <summary>
        /// Asynchronously retrieves a count of distinct documents in a MongoDB collection
        /// </summary>
        /// <returns>A numerical value: Number of documents</returns>
        /// <param name="collection">Collection.</param>
		private static async Task<long> AsyncCountDocumentsInCollection(IMongoCollection<BsonDocument> collection)
		{
			return await Task.Run(() =>
								  collection.CountAsync(new BsonDocument()).Result);
		}

		/// <summary>
		/// Call method to AsyncCountDocumentsInCollection
		/// </summary>
		/// <returns>A numerical value: Number of documents</returns>
		/// <param name="collection">Collection.</param>
		public long CallAsyncCountDocumentsInCollection(IMongoCollection<BsonDocument> collection)
		{
			Task<long> task = Task.Run(async () => await AsyncCountDocumentsInCollection(collection));
			task.Wait();
			return (task.Result);
		}
    }
}
