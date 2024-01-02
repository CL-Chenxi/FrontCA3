using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FrontCA3.Data
{
    public class OrderDto
    { 
        public int OrderId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public DateOnly OrderDate { get; set; } = new DateOnly();
    }
    public class EditOrderFormDto
    {
        public int CustomerId { get; set; } = 0;
        public DateOnly OrderDate { get; set; } = new DateOnly();
    }
    public class OrderService
    {
        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7279/api/Order/")
        };

        public OrderService() 
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        }

        public async Task<IEnumerable<OrderDto>?> GetAllOrderList<OrderDto>()
        {
            HttpResponseMessage response = await _client.GetAsync("GetAllOrders");
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(rawJson);
            }
            return null;
        }

        public async Task<OrderDto?> GetOrderById<OrderDto>(int orderId)
        {
            HttpResponseMessage response = await _client.GetAsync("GetOrderById?orderId=" + orderId);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                if (rawJson != null)
                    return JsonConvert.DeserializeObject<OrderDto>(rawJson);
            }
            return default;
        }

        public async Task<IEnumerable<OrderDto>?> GetAllOrdersForCustomer<OrderDto>(int customerId)
        {
            HttpResponseMessage response = await _client.GetAsync("GetAllOrdersForCustomer?customerId="+customerId);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(rawJson);
            }
            return null;
        }

        public async Task<string> CreateOrder<EditOrderFormDto>(EditOrderFormDto orderDto)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(orderDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("CreateOrder", content);
            if (response.IsSuccessStatusCode)
            {
                return "Order created";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> UpdateOrder<EditOrderFormDto>(EditOrderFormDto orderDto, int orderId)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(orderDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateOrder?orderId="+orderId, content);
            if (response.IsSuccessStatusCode)
            {
                return "Order updated";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> DeleteOrder<EditOrderFormDto>(int orderId)
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteOrder?orderId=" + orderId);
            if (response.IsSuccessStatusCode)
            {
                return "Order deleted";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

    }
}
