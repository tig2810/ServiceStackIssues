using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackIssues
{
    public class TestService : Service
    {
        public object Any(OrderRequestDto request)
        {
            var response = new OrderResponseDto();

            List<OrderDto> orders = new List<OrderDto>();

            CompanyDto company = new CompanyDto { FullName = "Google", Code = "GOOG", Address1 = "1, Expensive Road", Address2 = "Expensive District", City = "More expensive city", Phone = "+800$$$$$" };

            for (int i = 0; i < 100000; i++)
                orders.Add(new OrderDto { Id = i + 1, Buyer = request.AttachBuyerToOrder ? company : null, OrderNumber = "ORD-{0}".Fmt(i + 1) });

            response.Orders = orders;

            if (!request.AttachBuyerToOrder)
                response.Companies = new List<CompanyDto> { company };

            return response;
        }
    }

    public class OrderRequestDto : IReturn<OrderResponseDto>
    {
        public bool Compressed { get; set; }
        public bool AttachBuyerToOrder { get; set; }
    }

    public class OrderResponseDto
    {
        public List<CompanyDto> Companies { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
