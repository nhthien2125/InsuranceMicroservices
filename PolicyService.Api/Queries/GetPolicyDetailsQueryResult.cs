using System;
using System.Collections.Generic;

namespace PolicyService.Api.Queries;

public class GetPolicyDetailsQueryResult
{
    public PolicyDetailsDto Policy { get; set; }
}

public class PolicyDetailsDto
{
    public string Number { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string PolicyHolder { get; set; }
    public decimal TotalPremium { get; set; }
    public string ProductCode { get; set; }
    public IEnumerable<string> Covers { get; set; } = new List<string>();
}

