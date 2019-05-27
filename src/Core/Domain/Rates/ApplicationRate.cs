using System;
using Core.Debezium;
using Newtonsoft.Json;

namespace Core.Domain.Rates
{
    /*
      "candidate_id": "1a813941-74d8-4cb0-bb85-a76d00fd3fc6",
      "version": 1,
      "created_on": "2018-03-29T01:51:16.553187Z",
      "created_by_id": "022ec8ab-1239-43dc-9cc6-95254e486c0b",
      "created_by_account_type": "internal",
      "updated_on": "2018-03-29T01:51:16.553187Z",
      "updated_by_id": "022ec8ab-1239-43dc-9cc6-95254e486c0b",
      "updated_by_account_type": "internal",
      "currency_country": "USA",
      "currency_code": "USD",
      "pay_type": "PerHour",
      "minimum_rate_of_pay": "C7g=",
      "maximum_rate_of_pay": "DIA=",
      "is_rate_private": false,
      "ordinal": 71175
      
     */
    public class ApplicationRate
    {
        [JsonProperty("candidate_id")]
        public Guid CandidateId { get; set; }
         
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("created_on")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("created_by_id")]
        public Guid CreatedById { get; set; }
        
        [JsonProperty("created_by_account_type")]
        public string CreatedByAccountType { get; set; }
      
        [JsonProperty("updated_on")]
        public DateTimeOffset UpdatedOn { get; set; }

        [JsonProperty("updated_by_id")]
        public Guid UpdatedById { get; set; }
        
        [JsonProperty("updated_by_account_type")]
        public string UpdatedByAccountType { get; set; }
        
        [JsonProperty("currency_country")]
        public string CurrencyCountry { get; set; }
         
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("pay_type")]
        public string PayType { get; set; }
         
        [JsonProperty("minimum_rate_of_pay")]
        public string MinimumRateOfPay { get; set; }
        
        [JsonProperty("maximum_rate_of_pay")]
        public string MaximumRateOfPay { get; set; }
         
        [JsonProperty("ordinal")]
        public bool IsRatePrivate { get; set; }
        
        [JsonProperty("is_rate_private")]
        public long Ordinal { get; set; }
    }
}