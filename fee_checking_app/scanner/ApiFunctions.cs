using System;
using Android.OS;
using Android.Net;
using System.Net;
using System.Net.Http;
using Android.Content;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace scanner
{
	public class ApiFunctions
	{
		/*-----------fetch all paid student info from server-----------*/

		public async Task<string> parseData(string url)
		{
			string TARGETURL = url;
			//Toast.MakeText(this, TARGETURL, ToastLength.Short).Show();
			var handler = new HttpClientHandler();
			//Toast.MakeText(this, "handler gen pass", ToastLength.Short).Show();
			// ... Use HttpClient.            
			var client = new HttpClient(handler);
			//Toast.MakeText(this, "client gen pass", ToastLength.Short).Show();
			string result = null;
			var byteArray = Encoding.ASCII.GetBytes(secret.username+":"+secret.password);
			//Toast.MakeText(this, "encode gen pass", ToastLength.Short).Show();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
			//Toast.MakeText(this, "auth gen pass", ToastLength.Short).Show();
			//Console.WriteLine("Successful!\n");
			try
			{
				HttpResponseMessage ServerResponse = await client.GetAsync(TARGETURL);
				HttpContent content = ServerResponse.Content;
				result = await Task.Run(() => content.ReadAsStringAsync());
				// ... Check Status Code                                
			}
			catch (HttpRequestException except)
			{
				Console.WriteLine("client get failed:" + except.Message);
			}
			return result;
		}

		public async Task<int> PostStudentAsync(JsonValue student)
		{
			int result = 404;

			return result;
		}

		/*-----------fetch student id info from server-----------*/

		public async Task<JsonValue> FetchStudentAsync(string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new System.Uri(url));
			request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("theblackcat102:AF51FAF9BE961002"));
			request.ContentType = "application/json";
			request.Method = "GET";
			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync())
			{
				//Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream())
				{
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
					//string reponse = "Response: " + jsonDoc.ToString();
					//debug purpose
					//Toast.MakeText(this, reponse, ToastLength.Long).Show();
					// Return the JSON document:
					return jsonDoc;
				}
			}
		}
		/*----------------parse pay json data------------------*/
		public bool GotPay(JsonValue json)
		{
			JsonValue Pay = json["task"];
			bool payment = Pay["pay"];
			return payment;
		}

	}
}

