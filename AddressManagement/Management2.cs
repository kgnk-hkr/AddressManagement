using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DialogflowWebhook.Requests;
using DialogflowWebhook.Responses;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Logging;

using ResponsePayload = DialogflowWebhook.Responses.Payload;
using static AddressManagement.Management;

namespace DialogflowSample.Functions
{
	public static class DialogflowWebhook
	{
		//TableStorage����Q�Ƃ��鍀�ڂ�\�����N���X
		public class Person : TableEntity
		{
			public string publishDate { get; set; }
			public string location { get; set; }

		}

		[FunctionName("Management2")]
		public static async Task<DialogflowResponse> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]DialogflowRequest req, [Table("AddressManagement", Connection = "")]IQueryable<Person> inTable,TraceWriter log)
		{
			log.Info(req.QueryResult.Intent.DisplayName);
			log.Info(req.QueryResult.Entity.DisplayName);

			if (req.QueryResult.Intent.DisplayName == "where")
			{
				var items = req.QueryResult.Entity.DisplayName=="items";
				string item = Convert.ToString(items);
				string itemskey = itemsKey[item];
				int idx = 0;

				//�t�B���^�[�̌`�Ō�������
				var word = inTable.ToList().
			Where(x => x.PartitionKey == itemskey).
			OrderByDescending(x => x.publishDate);
				var words = word.ElementAt(idx);

				string text = $"{words.PartitionKey}�̒u���ꏊ��{words.location}�ł��B";

				return CreateResponse(text);
			}
			return CreateResponse("����ɂ��́I�悭�킩��Ȃ�������I");

		}

		private static DialogflowResponse CreateResponse(string text)
		{
			return new DialogflowResponse
			{
				Payload = new ResponsePayload
				{
					Google = new Google
					{
						ExpectUserResponse = true,
						RichResponse = new RichResponse
						{
							Items = new[]
							{
								new Item
								{
									SimpleResponse = new SimpleResponse
									{
										TextToSpeech = text,
									}
								}
							}
						}
					}
				},
			};
		}

		static Dictionary<string, string> itemsKey = new Dictionary<string, string>
		{
			 { "�͂���", "�͂���" },
			 { "��񂲂̃s�A�X", "��񂲂̃s�A�X" },
			 { "���K�l", "���K�l" },
			 { "�����", "�����" },
			 { "�Ƃ̌�", "�Ƃ̌�" },
			 { "�ܐ؂�", "�ܐ؂�" },
			 { "������", "������" }

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
