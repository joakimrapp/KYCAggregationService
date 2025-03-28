using System.ComponentModel.DataAnnotations;
using AzureFunctionAPI;
using AzureFunctionAPI.Models;

namespace AzureFunctionAPITests.Models {
	public class FlatContactDetailsTests {
		/*
		 * Should select the first address, and prefered email and phone number.
		 */
		[Fact]
		public void FlattenContactDetailsSelectFirstAndPrefered() {
			// Arrange
			var contactDetails = new ContactDetails {
				Addresses = [
					new() {
						Street = "First Street 1",
						Postal_code = "11111",
						City = "First City"
					},
					new() {
						Street = "Second Street 2",
						Postal_code = "22222",
						City = "Second City"
					}
				],
				Emails = [
					new() {
						Preferred = false,
						Email_address = "not.prefered@test.com"
					},
					new() {
						Preferred = true,
						Email_address = "prefered@test.com"
					},
					new() {
						Preferred = false,
						Email_address = "not.prefered@test.com"
					}
				],
				Phone_numbers = [
					new() {
						Preferred = false,
						Number = "00000000"
					},
					new() {
						Preferred = true,
						Number = "11111111"
					},
					new() {
						Preferred = false,
						Number = "00000000"
					}
				]
			};

			// Act
			var flatContactDetails = new FlatContactDetails(contactDetails);

			// Assert
			Assert.Equal("prefered@test.com", flatContactDetails.Email);
			Assert.Equal("11111111", flatContactDetails.Phone);
			Assert.Equal("First Street 1, 11111 First City", flatContactDetails.Address);
		}
		/*
		 * Should select the first address, email and phone number if email and/or phone has no preferred.
		 */
		[Fact]
		public void FlattenContactDetailsSelectFirst() {
			// Arrange
			var contactDetails = new ContactDetails {
				Addresses = [
					new() {
						Street = "First Street 1",
						Postal_code = "11111",
						City = "First City"
					}
				],
				Emails = [
					new() {
						Preferred = false,
						Email_address = "not.prefered1@test.com"
					},
					new() {
						Preferred = false,
						Email_address = "not.prefered2@test.com"
					}
				],
				Phone_numbers = [
					new() {
						Preferred = false,
						Number = "11111111"
					},
					new() {
						Preferred = false,
						Number = "22222222"
					}
				]
			};

			// Act
			var flatContactDetails = new FlatContactDetails(contactDetails);

			// Assert
			Assert.Equal("not.prefered1@test.com", flatContactDetails.Email);
			Assert.Equal("11111111", flatContactDetails.Phone);
			Assert.Equal("First Street 1, 11111 First City", flatContactDetails.Address);
		}
	}
}
