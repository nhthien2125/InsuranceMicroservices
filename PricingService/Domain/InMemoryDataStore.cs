using System.Collections.Generic;
using System.Threading.Tasks;

namespace PricingService.Domain;

public class InMemoryDataStore : IDataStore, ITariffRepository
{
    private readonly Dictionary<string, Tariff> tariffs = new();

    public InMemoryDataStore()
    {
        tariffs["TRI"] = new Tariff("TRI");
        tariffs["HSI"] = new Tariff("HSI");
        tariffs["FAI"] = new Tariff("FAI");
        tariffs["CAR"] = new Tariff("CAR");
    }

    public ITariffRepository Tariffs => this;

    public Task<Tariff> this[string code] =>
        Task.FromResult(tariffs.TryGetValue(code, out var t) ? t : new Tariff(code));
}

