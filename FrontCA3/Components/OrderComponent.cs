using FrontCA3.Data;
using Microsoft.AspNetCore.Components;

namespace FrontCA3.Components
{
    public class OrderComponent : ComponentBase
    {
        public OrderDto OrderDTO { get; set; } = new OrderDto();
        public EditOrderFormDto EditOrderFormDTO { get; set; } = new EditOrderFormDto();
        public EditOrderFormDto CreateOrderFormDTO { get; set; } = new EditOrderFormDto();
        public IEnumerable<OrderDto> OrderList { get; set; }
        public string creationFeedback = string.Empty;
        public string updateFeedback = string.Empty;
        public Boolean displayable = false;

        [Parameter]
        public int OrderId { get; set; }
        [Parameter]
        public int CustomerId { get; set; }
        [Inject]
        private OrderService ApiService { get; set; }

        protected async Task Fetch()
        {
            if (CustomerId == null || CustomerId < 1) 
            {
                OrderList = await ApiService.GetAllOrderList<OrderDto>();
            } 
            else
            {
                OrderList = await ApiService.GetAllOrdersForCustomer<OrderDto>(CustomerId);
            }
        }

        protected async Task Search()
        {
            OrderDTO = await ApiService.GetOrderById<OrderDto>(OrderId);
            if (OrderDTO == null)
            {
                displayable = false;
                OrderDTO = new OrderDto();
                EditOrderFormDTO = new EditOrderFormDto();
            } 
            else
            {
                displayable = true;
                EditOrderFormDTO.CustomerId = OrderDTO.CustomerId;
                EditOrderFormDTO.OrderDate = OrderDTO.OrderDate;
            }
        }

        protected async Task Create()
        {
            creationFeedback = await ApiService.CreateOrder<EditOrderFormDto>(CreateOrderFormDTO);
        }

        protected async Task Update()
        {
            updateFeedback = await ApiService.UpdateOrder<EditOrderFormDto>(EditOrderFormDTO, OrderId);
        }

        protected async Task Delete()
        {
            updateFeedback = await ApiService.DeleteOrder<EditOrderFormDto>(OrderId);
        }


    }

}
