using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FrontCA3.Data
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; } = 0;
        public int OrderId { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal UnitPrice { get; set; } = decimal.Zero;
    }
    public class EditOrderItemFormDto
    {
        public int OrderId { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal UnitPrice { get; set; } = decimal.Zero;
    }
    public class OrderItemService
    {
        private static HttpClient _client = new()
        {
            BaseAddress = new Uri("https://localhost:7279/api/OrderItem/")
        };

        public OrderItemService()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");
        }

        public async Task<OrderItemDto?> GetOrderItemById<OrderItemDto>(int itemId)
        {
            HttpResponseMessage response = await _client.GetAsync("GetItemById?itemId=" + itemId);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                if (rawJson != null)
                    return JsonConvert.DeserializeObject<OrderItemDto>(rawJson);
            }
            return default;
        }

        public async Task<IEnumerable<OrderItemDto>?> GetAllItemsForOrderId<OrderItemDto>(int orderId)
        {
            HttpResponseMessage response = await _client.GetAsync("GetAllItemsForOrderId?orderId=" + orderId);
            if (response.IsSuccessStatusCode)
            {
                var rawJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<OrderItemDto>>(rawJson);
            }
            return null;

        }

        public async Task<string> CreateOrderItem<EditOrderItemFormDto>(EditOrderItemFormDto itemDto)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(itemDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("CreateOrderItem", content);
            if (response.IsSuccessStatusCode)
            {
                return "Item created";
            }
            else
            {
                return "Error " + response.StatusCode;
            }

        }

        public async Task<string> UpdateOrderItem<EditOrderItemFormDto>(EditOrderItemFormDto itemDto, int itemId)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(itemDto), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync("UpdateOrderItem?itemId=" + itemId, content);
            if (response.IsSuccessStatusCode)
            {
                return "Item updated";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }

        public async Task<string> DeleteOrderItem<EditOrderItemFormDto>(int itemId)
        {
            HttpResponseMessage response = await _client.DeleteAsync("DeleteOrderItem?itemId=" + itemId);
            if (response.IsSuccessStatusCode)
            {
                return "Item deleted";
            }
            else
            {
                return "Error " + response.StatusCode;
            }
        }
    }
}
