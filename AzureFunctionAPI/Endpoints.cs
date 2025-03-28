using System.ComponentModel.DataAnnotations;
using System.Net;
using AzureFunctionAPI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace AzureFunctionAPI {
	public class Endpoints(ILogger<Endpoints> logger, PersonalDetailsClient personalDetailsClient, ContactDetailsClient contactDetailsClient, KYCFormDataClient kycFormDataClient) {
		[Function("AggregatedKYCDataFunction")]
		[OpenApiOperation(operationId: "GetAggregatedKycData", tags: ["KYC Aggregation"])]
		[OpenApiParameter(name: "ssn", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The name parameter")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(AggregatedKycData), Description = "The OK response")]
		public async Task<IActionResult> GetAggregatedKycData([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "kyc-data/{ssn}")] HttpRequest req, string ssn) {
			logger.LogInformation($"Handling request for GetAggregatedKycData: {ssn}");
			try {
				var respone = new AggregatedKycData {
					Ssn = ssn,
				};
				Validator.ValidateProperty(respone.Ssn, new ValidationContext(respone) { MemberName = "Ssn" });

				var personalDetailsTask = personalDetailsClient.GetAsync(ssn);
				var contactDetailsTask = contactDetailsClient.GetAsync(ssn);
				var kycFormDataTask = kycFormDataClient.GetAsync(ssn);

				await Task.WhenAll(personalDetailsTask, contactDetailsTask, kycFormDataTask);

				var personalDetails = await personalDetailsTask;
				var contactDetails = await contactDetailsTask;
				var kycFormData = await kycFormDataTask;

				if (personalDetails?.First_name != null)
					respone.First_name = personalDetails.First_name;
				if (personalDetails?.Sur_name != null)
					respone.Last_name = personalDetails.Sur_name;
				if (contactDetails?.Email != null)
					respone.Email = contactDetails.Email;
				if (contactDetails?.Phone != null)
					respone.Phone_number = contactDetails.Phone;
				if (contactDetails?.Address != null)
					respone.Address = contactDetails.Address;
				if (kycFormData?.TaxCountry != null)
					respone.Tax_country = kycFormData.TaxCountry;
				if (kycFormData?.AnualIncome != null)
					respone.Income = kycFormData.AnualIncome;
				return new OkObjectResult(respone);
			}
			catch (Exception) {
				return new BadRequestObjectResult($"Customer not found: {ssn}") {
					StatusCode = (int)HttpStatusCode.NotFound
				};
			}
		}
	}
}

