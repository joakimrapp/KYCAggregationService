using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionAPI.Models {
	public class FlatKYCData {
		public FlatKYCData(KYCForm data) {
			foreach (var item in data.Items)
				switch (item.Key) {
					case "tax_country":
						TaxCountry = item.Value;
						break;
					case "annual_income":
						AnualIncome = Convert.ToInt32(item.Value);
						break;
				}
		}
		public string? TaxCountry { get; private set; }
		public int? AnualIncome { get; private set; }
	}
}
