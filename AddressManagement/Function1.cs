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

using ResponsePayload = DialogflowWebhook.Responses.Payload;

namespace DialogflowSample.Functions
{
	public static class Function1
	{
		[FunctionName("DialogflowWebhook")]
		public static async Task<DialogflowResponse> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]DialogflowRequest req,
			TraceWriter log)
		{
			log.Info(req.QueryResult.Intent.DisplayName);
			if (req.QueryResult.Intent.DisplayName == "now")
			{
				var tst = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
				var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tst);
				return CreateResponse($"こんにちは！今は{now.Hour}時だよ！");
			}

			return CreateResponse("こんにちは！よくわからなかったよ！");
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
	}
}