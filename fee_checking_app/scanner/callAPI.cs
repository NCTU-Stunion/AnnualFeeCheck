using RestSharp;

namespace scanner
{
	public class callAPI
	{
		const string url = "http://192.168.1.110/NCTU/api/";
		readonly string _accountID,_secretKey;
		public nctuAPI(string accountSid, string secretKey)
		{
			_accountID = accountSid;
			_secretKey = secretKey;
		}
		public T Execute<T>(RestRequest request) where T : new()
		{
			var client = new RestClient();
			client.BaseUrl = url;
			client.Authenticator = new Http
		}

	}
}

