using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{        

    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        ShoppingCart basket = new();

        if(!string.IsNullOrWhiteSpace(cachedBasket))        
            basket = JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        else
        {
            basket = await repository.GetBasket(userName, cancellationToken);
            if(basket is not null)
                await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        }
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }
}
