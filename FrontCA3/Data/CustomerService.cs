using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace FrontCA3.Data
{
    public class CustomerDto
    {
        public int CustomerId { get; set; } = -1;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public int AnnualSpend { get; set; } = 0;

        public CustomerDto() { }
    }

    public class EditCustomerFormDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; } = new DateOnly();
        public int AnnualSpend { get; set; } = 0;

        public EditCustomerFormDto() { }
    }
    public class CustomerService
    {

        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7279/api/Customer/")
        };
   
        public CustomerService() 
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        }

        public async Task<IEnumerable<CustomerDto>?> GetCustomersList<CustomerDto>()
        {
            HttpResponseMessage response = await _client.GetAsync("GetCustomerList");
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<CustomerDto>>(rawJson);
            }
            return null;
        }

        public async Task<CustomerDto?> GetCustomersById<CustomerDto>(int customerId)
        {
            HttpResponseMessage response = await _client.GetAsync("GetCustomerById?id=" + customerId);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                if(rawJson != null)
                    return JsonConvert.DeserializeObject<CustomerDto>(rawJson);
            }
            return default;
        }

        public async Task<string> CreateNewCustomer<EditCustomerFormDto>(EditCustomerFormDto editCustomerFormDto)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(editCustomerFormDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("CreateCustomer", content);
            if(response.IsSuccessStatusCode)
            {
                return "Customer created";
            } else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> UpdateCustomer<EditCustomerFormDto>(EditCustomerFormDto editCustomerFormDto, int customerId)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(editCustomerFormDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateCustomer?id=" + customerId, content);
            if (response.IsSuccessStatusCode)
            {
                return "Customer updated";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> DeleteCustomer<EditCustomerFormDto>(int customerId)
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteCustomer?id="+customerId);
            if (response.IsSuccessStatusCode)
            {
                return "Customer deleted";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }
    }
}
