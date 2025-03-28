using Microsoft.Extensions.Logging;

namespace AzureFunctionAPI.Clients {
	public class PersonalDetailsClient(ILogger<PersonalDetailsClient> logger) : CachedClient<PersonalDetails> {
		protected override ILogger Logger { get { return logger; } }
		protected override async Task<PersonalDetails> GetFromSource(string key) {
			using var httpClient = new HttpClient();
			var data = await new CustomerDataServiceClient(httpClient).GetPersonalDetailsAsync(key);
			return data;
		}
	}
}
