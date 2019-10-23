using System;
using System.IO;

namespace Services.Token
{
    public class TokenInformation : ITextReader
    {

		/// <summary>
		/// A private token value to be used in http-requests to the Facebook API
		/// </summary>
		private string privateToken;


		/// <summary>
		/// Identify path to token file. Method to be used as submethod - therefore private scope.
		/// </summary>
		/// <returns>A path to the token-file</returns>
		public string[] GetFile()
		{
			string[] _path = null;
			try
			{
				var currentDir = AppDomain.CurrentDomain.BaseDirectory;
				var parentDir = Directory.GetParent(currentDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
				var tokenDir = Path.Combine(parentDir, "fb_py", "token");
				_path = Directory.GetFiles(tokenDir, "*.txt");
				return _path;
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine(e.Message);
			}

			return _path;
		}

		/// <summary>
		/// Reads a whatever is provided by a TextReader stream object and writes to the private privateToken
		/// </summary>
		/// <param name="reader">TextReader object - here a p.</param>
        public void ReadFromFile(TextReader reader)
		{
			string _token = reader.ReadToEnd();
			privateToken = _token;
		}

		/// <summary>
		/// Provides a TextReader stream to the ReadToken()-method. 
		/// </summary>
		public void SetValue()
		{
			var file = GetFile();
			using (var reader = File.OpenText(file[0]))
			{
				ReadFromFile(reader);
			}
		}


		/// <summary>
		/// Enables other classes/projects to retrieve the token
		/// </summary>
		/// <value>Facebook OAuth Token</value>
		public string GetOAuthToken
		{
			get => privateToken;
		}

	}
}
    