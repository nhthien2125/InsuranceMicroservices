using System;
using System.Collections.Generic;
using PolicyService.Api.Commands.Dtos;
using PricingService.Api.Commands.Dto;

namespace PolicyService.Api.Commands;

public class CreateOfferCommand
{
    public string ProductCode { get; set; } = string.Empty;
    public DateTime PolicyFrom { get; set; }
    public DateTime PolicyTo { get; set; }
    public List<string> SelectedCovers { get; set; } = new();
    public List<QuestionAnswer> Answers { get; set; } = new();
}

public class CreateOfferResult
{
    public string OfferNumber { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public Dictionary<string, decimal> Covers { get; set; } = new();
}

public class CreatePolicyCommand
{
    public string OfferNumber { get; set; } = string.Empty;
    public PersonDto PolicyHolder { get; set; } = new();
    public AddressDto PolicyHolderAddress { get; set; } = new();
}

public class CreatePolicyResult
{
    public string PolicyNumber { get; set; } = string.Empty;
}

