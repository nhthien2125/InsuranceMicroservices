using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PricingService.Domain;

public class Tariff
{
    public string Code { get; set; }
    public BasePremiumRules BasePremiumRules { get; } = new();
    public DiscountMarkupRules DiscountMarkupRules { get; } = new();

    public Tariff(string code)
    {
        Code = code;
    }

    public CalculationResult CalculatePrice(Calculation calculation)
    {
        var result = new CalculationResult();
        var subject = calculation.Subject ?? new Dictionary<string, object>();

        decimal GetDecimal(string key, decimal def = 0M)
        {
            if (subject.TryGetValue(key, out var val))
            {
                if (val is decimal d) return d;
                if (decimal.TryParse(val?.ToString(), out var parsed)) return parsed;
            }
            return def;
        }

        string GetString(string key, string def = "")
        {
            if (subject.TryGetValue(key, out var val) && val != null)
                return val.ToString()!;
            return def;
        }

        if (Code == "TRI")
        {
            var adults = GetDecimal("NUM_OF_ADULTS", 1M);
            var children = GetDecimal("NUM_OF_CHILDREN", 0M);
            var dest = GetString("DESTINATION", "EUR");

            if (calculation.SelectedCovers.Contains("C1"))
                result.Covers["C1"] = new CoverPrice { Price = adults * (dest == "EUR" ? 26.00M : 34.00M) };
            if (calculation.SelectedCovers.Contains("C2"))
                result.Covers["C2"] = new CoverPrice { Price = (adults + children) * 26.00M };
            if (calculation.SelectedCovers.Contains("C3"))
                result.Covers["C3"] = new CoverPrice { Price = (adults + children) * 10.00M };

            decimal total = 0M;
            foreach (var cp in result.Covers.Values) total += cp.Price;
            if (dest == "WORLD") total *= 1.5M;
            result.TotalPremium = total;
        }
        else if (Code == "HSI")
        {
            var typ = GetString("TYP", "APT");
            var area = GetDecimal("AREA", 0M);

            if (calculation.SelectedCovers.Contains("C1"))
                result.Covers["C1"] = new CoverPrice { Price = area * (typ == "APT" ? 1.25M : 1.50M) };
            if (calculation.SelectedCovers.Contains("C2"))
                result.Covers["C2"] = new CoverPrice { Price = area * (typ == "APT" ? 0.25M : 0.45M) };
            if (calculation.SelectedCovers.Contains("C3"))
                result.Covers["C3"] = new CoverPrice { Price = 30M };
            if (calculation.SelectedCovers.Contains("C4"))
                result.Covers["C4"] = new CoverPrice { Price = 50M };

            decimal total = 0M;
            foreach (var cp in result.Covers.Values) total += cp.Price;
            if (GetString("FLOOD") == "YES") total *= 1.50M;
            if (GetDecimal("NUM_OF_CLAIM") > 1) total *= 1.25M;
            result.TotalPremium = total;
        }
        else if (Code == "FAI")
        {
            if (calculation.SelectedCovers.Contains("C1")) result.Covers["C1"] = new CoverPrice { Price = 10M };
            if (calculation.SelectedCovers.Contains("C2")) result.Covers["C2"] = new CoverPrice { Price = 20M };
            if (calculation.SelectedCovers.Contains("C3")) result.Covers["C3"] = new CoverPrice { Price = 30M };
            if (calculation.SelectedCovers.Contains("C4")) result.Covers["C4"] = new CoverPrice { Price = 40M };

            decimal total = 0M;
            foreach (var cp in result.Covers.Values) total += cp.Price;
            if (GetString("FLOOD") == "YES") total *= 1.50M;
            if (GetDecimal("NUM_OF_CLAIM") > 2) total *= 2.00M;
            result.TotalPremium = total;
        }
        else if (Code == "CAR")
        {
            if (calculation.SelectedCovers.Contains("C1")) result.Covers["C1"] = new CoverPrice { Price = 100M };

            decimal total = 0M;
            foreach (var cp in result.Covers.Values) total += cp.Price;
            if (GetDecimal("NUM_OF_CLAIM") > 2) total *= 1.50M;
            result.TotalPremium = total;
        }
        else
        {
            decimal total = 0M;
            foreach (var cover in calculation.SelectedCovers)
            {
                result.Covers[cover] = new CoverPrice { Price = 10M };
                total += 10M;
            }
            result.TotalPremium = total;
        }

        return result;
    }
}

public class BasePremiumRules
{
    public void AddBasePriceRule(string coverCode, string? condition, string formula) { }
}

public class DiscountMarkupRules
{
    public void AddPercentMarkup(string condition, decimal markup) { }
}

public class Calculation
{
    public string ProductCode { get; }
    public DateTimeOffset PolicyFrom { get; }
    public DateTimeOffset PolicyTo { get; }
    public List<string> SelectedCovers { get; }
    public Dictionary<string, object> Subject { get; }

    public Calculation(string productCode, DateTimeOffset policyFrom, DateTimeOffset policyTo, List<string> selectedCovers, Dictionary<string, object> subject)
    {
        ProductCode = productCode;
        PolicyFrom = policyFrom;
        PolicyTo = policyTo;
        SelectedCovers = selectedCovers;
        Subject = subject;
    }
}

public class CalculationResult
{
    public decimal TotalPremium { get; set; }
    public Dictionary<string, CoverPrice> Covers { get; set; } = new();
}

public class CoverPrice
{
    public decimal Price { get; set; }
}

public interface IDataStore
{
    ITariffRepository Tariffs { get; }
}

public interface ITariffRepository
{
    Task<Tariff> this[string code] { get; }
}

