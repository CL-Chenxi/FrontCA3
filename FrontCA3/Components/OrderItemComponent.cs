using FrontCA3.Data;
using Microsoft.AspNetCore.Components;

namespace FrontCA3.Components
{
    public class OrderItemComponent : ComponentBase
    {
        public OrderItemDto OrderItemDTO { get; set; } = new OrderItemDto();
        public EditOrderItemFormDto CreateOrderItemFormDTO { get; set; } = new EditOrderItemFormDto();
        public EditOrderItemFormDto EditOrderItemFormDTO { get; set; } = new EditOrderItemFormDto();

        public IEnumerable<OrderItemDto> OrderItemList { get; set; }
        public string creationFeedback = string.Empty;
        public string updateFeedback = string.Empty;
        public Boolean displayable = false;

        [Parameter]
        public int OrderItemId { get; set; }
        [Parameter]
        public int OrderId { get; set; }
        [Inject]
        private OrderItemService ApiService { get; set; }


        protected async Task Fetch()
        {
            if(OrderId > 0)
            {
                OrderItemList = await ApiService.GetAllItemsForOrderId<OrderItemDto>(OrderId);
            }
        }

        protected async Task Search()
        {
            OrderItemDTO = await ApiService.GetOrderItemById<OrderItemDto>(OrderItemId);
            if (OrderItemDTO == null)
            {
                displayable = false;
                OrderItemDTO = new OrderItemDto();
                EditOrderItemFormDTO = new EditOrderItemFormDto();

            } else
            {
                displayable = true;
                EditOrderItemFormDTO.OrderId = OrderItemDTO.OrderId;
                EditOrderItemFormDTO.ProductName = OrderItemDTO.ProductName;
                EditOrderItemFormDTO.Quantity = OrderItemDTO.Quantity;
                EditOrderItemFormDTO.UnitPrice = OrderItemDTO.UnitPrice;
            }

        }

        protected async Task Create()
        {
            creationFeedback = await ApiService.CreateOrderItem<EditOrderItemFormDto>(CreateOrderItemFormDTO);
        }

        protected async Task Update()
        {
            updateFeedback = await ApiService.UpdateOrderItem<EditOrderItemFormDto>(EditOrderItemFormDTO, OrderItemId);
        }

        protected async Task Delete()
        {
            updateFeedback = await ApiService.DeleteOrderItem<EditOrderItemFormDto>(OrderItemId);
        }
    }
}
