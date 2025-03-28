using AzureFunctionAPI.Models;
using Microsoft.Extensions.Logging;

namespace AzureFunctionAPI.Clients {
	public class KYCFormDataClient(ILogger<KYCFormDataClient> logger) : CachedClient<FlatKYCData> {
		protected override ILogger Logger { get { return logger; } }
		protected override async Task<FlatKYCData> GetFromSource(string ssn) {
			using var httpClient = new HttpClient();
			var data = await new CustomerDataServiceClient(httpClient).GetKYCFormDataAsync(ssn, DateTime.Now);
			return new FlatKYCData(data);
		}
	}
}
