using System;
using System.Collections.Generic;

namespace PolicySearchService.Api.Queries.Dtos
{
    public class FindPolicyResult
    {
        public List<PolicyDto> Policies { get; set; } = new();
    }

    public class PolicyDto
    {
        public string PolicyNumber { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public string PolicyHolder { get; set; } = string.Empty;
        public decimal PremiumAmount { get; set; }
    }
}

namespace PolicySearchService.Api.Queries
{
    public class FindPolicyResult : Dtos.FindPolicyResult { }
}

