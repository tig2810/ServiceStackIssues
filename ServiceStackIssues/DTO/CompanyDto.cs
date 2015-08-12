using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStackIssues
{
    [JsonObject(IsReference = true)]
    public class CompanyDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }
}
