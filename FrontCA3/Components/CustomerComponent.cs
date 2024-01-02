using FrontCA3.Data;
using Microsoft.AspNetCore.Components;

namespace FrontCA3.Components
{
    public class CustomerComponent : ComponentBase
    {
        public CustomerDto CustomerSearchDTO { get; set; } = new CustomerDto();
        public EditCustomerFormDto CreateCustomerDTO { get; set; } = new EditCustomerFormDto();
        public EditCustomerFormDto EditCustomerDTO { get; set; } = new EditCustomerFormDto();

        public Boolean displayable = false;
        public string feedbackCreation = string.Empty;
        public string feedbackUpdate = string.Empty;

        [Parameter]
        public int CustomerSearchId { get; set; }

        [Inject]
        private CustomerService ApiService { get; set; }

        protected async Task Search()
        {
            CustomerSearchDTO = await ApiService.GetCustomersById<CustomerDto>(CustomerSearchId);
            if (CustomerSearchDTO == null)
            {
                displayable = false;
                CustomerSearchDTO = new CustomerDto();
                EditCustomerDTO = new EditCustomerFormDto();
            }
            else
            {
                EditCustomerDTO.FirstName = CustomerSearchDTO.FirstName;
                EditCustomerDTO.LastName = CustomerSearchDTO.LastName;
                EditCustomerDTO.DateOfBirth = CustomerSearchDTO.DateOfBirth;
                EditCustomerDTO.AnnualSpend = CustomerSearchDTO.AnnualSpend;
                displayable = true;
            }
        }

        protected async Task Create()
        {
            feedbackCreation = await ApiService.CreateNewCustomer<EditCustomerFormDto>(EditCustomerDTO);
        }

        protected async Task Update() 
        {
            feedbackUpdate = await ApiService.UpdateCustomer<EditCustomerFormDto>(EditCustomerDTO, CustomerSearchId);
        }

        protected async Task Delete()
        {
            feedbackUpdate = await ApiService.DeleteCustomer<EditCustomerFormDto>(CustomerSearchId);
        }
    }
}
