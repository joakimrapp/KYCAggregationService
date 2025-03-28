using AzureFunctionAPI.Util;

namespace AzureFunctionAPI.Models {
	public class FlatContactDetails {
		public FlatContactDetails(ContactDetails contactDetails) {
			Phone = EnumerableExtensions.Find<PhoneNumber>(contactDetails.Phone_numbers, phoneNumber => phoneNumber.Preferred)?.Number;
			Email = EnumerableExtensions.Find<Email>(contactDetails.Emails, email => email.Preferred)?.Email_address;
			var address = contactDetails.Addresses?.First();
			if (address != null)
				Address = $"{address.Street}, {address.Postal_code} {address.City}";
		}
		public string? Address { get; private set; }
		public string? Email { get; private set; }
		public string? Phone { get; private set; }
	}
}
