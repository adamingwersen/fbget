using System;
using System.IO;
using Services;

namespace Services.Proxy
{
    public class ProxyInformation : ITextReader
    {

		/// <summary>
		/// A private token value to be used in http-requests to the Facebook API
		/// </summary>
		private Tuple<string, string> _proxyCredentials;
		private string _usr;
		private string _pwd;

		public string[] GetFile()
		{
			string[] _path = null;
			try
			{
				var currentDir = AppDomain.CurrentDomain.BaseDirectory;
				var parentDir = Directory.GetParent(currentDir).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
				var credDir = Path.Combine(parentDir, "fb_py", "credentials");
				_path = Directory.GetFiles(credDir, ".credentials");
				return _path;
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine(e.Message);
			}

			return _path;
		}

		public void ReadFromFile(TextReader reader)
		{
			@_usr = reader.ReadLine();
			@_pwd = reader.ReadLine();
		}

		public void SetValue()
		{
            var file = GetFile();
			using (var reader = File.OpenText(file[0]))
			{
				ReadFromFile(reader);
			}
			_proxyCredentials = new Tuple<string, string>(_usr, _pwd);
		}

		/// <summary>
		/// Enables other classes/projects to retrieve the token
		/// </summary>
		/// <value>Facebook OAuth Token</value>
		public Tuple<string, string> GetProxyCredentials
		{
			get => _proxyCredentials;
		}
        
    }
}
