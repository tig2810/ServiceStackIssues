using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServiceStackIssues
{
    [JsonObject(IsReference = true)]
    public class OrderDto
    {
        public int Id { get; set; }
        public CompanyDto Buyer { get; set; }
        public string OrderNumber { get; set; }
    }
}
