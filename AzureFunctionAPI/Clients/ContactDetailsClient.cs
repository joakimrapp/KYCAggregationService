using AzureFunctionAPI.Models;
using Microsoft.Extensions.Logging;

namespace AzureFunctionAPI.Clients {
	public class ContactDetailsClient(ILogger<ContactDetailsClient> logger) : CachedClient<FlatContactDetails> {
		protected override int TimeToLiveMinutes { get; } = 30;
		protected override ILogger Logger { get { return logger; } }
		protected override async Task<FlatContactDetails> GetFromSource(string ssn) {
			using var httpClient = new HttpClient();
			var data = await new CustomerDataServiceClient(httpClient).GetContactDetailsAsync(ssn);
			return new FlatContactDetails( data );
		}
	}
}
