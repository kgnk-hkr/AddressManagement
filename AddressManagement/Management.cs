using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Logging;

namespace AddressManagement
{
    public static class Management
    {
		//TableStorageから参照する項目を表したクラス
		public class Person : TableEntity
		{
			public string publishDate { get; set; }
			public string location { get; set; }
			
		}

		[FunctionName("Management")]
		public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, [Table("AddressManagement", Connection = "")]IQueryable<Person> inTable, TraceWriter log)
		{
			log.Info("C# HTTP trigger function processed a request.");
			dynamic data1 = await req.Content.ReadAsStringAsync();
			log.Info($"{data1}");

			string data = await req.Content.ReadAsStringAsync();
			var jsondata = JsonConvert.DeserializeObject<ReqBody>(data);
			string items = jsondata.result.parameters["items"];
			string itemskey = itemsKey[items];
			int idx = 0;

			//フィルターの形で検索する
			var word = inTable.ToList().
			Where(x => x.PartitionKey == itemskey).
			OrderByDescending(x => x.publishDate);
			var words = word.ElementAt(idx);
			string text = $"{words.PartitionKey}の置き場所は{words.location}です。";

			var result = req.CreateResponse(HttpStatusCode.OK, new
			{
				speech = text,
				displayText = text
			});
			result.Headers.Add("ContentType", "application/json");
			return result;
		}

		//Dialogflowから送信されるリクエストとそれに対するレスポンスを表したクラス
		public class ReqBody
		{
			public Result result { get; set; }
		}

		public class Result
		{
			public string resolvedQuery { get; set; }
			public Dictionary<string, string> parameters { get; set; }
		}

		public class Response
		{
			public string speech { get; set; }
			public string displayText { get; set; }
			public string source { get; set; }
		}


		static Dictionary<string, string> itemsKey = new Dictionary<string, string>
		{
			 { "はさみ", "はさみ" },
			 { "りんごのピアス", "りんごのピアス" },
			 { "メガネ", "メガネ" },
			 { "定期券", "定期券" },
			 { "家の鍵", "家の鍵" },
			 { "爪切り", "爪切り" },
			 { "耳かき", "耳かき" }

		};


		static Dictionary<int, int> index = new Dictionary<int, int>
		{
			{ 0 , 0 },
			{ 1 , 1 },
			{ 2 , 2 },
			{ 3 , 3 },
			{ 4 , 4 },
			{ 5 , 5 },
			{ 6 , 6 }

		};

	}
}
