using System;
using System.Text;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Newtonsoft.Json;

namespace BlazorWasmClient.Components.Auth;

public static class SessionStorageExtension
{
    public static async Task SaveItemAsEncrypted<T>
    (
        this ISessionStorageService sessionStorageService,
        string key,
        T item
    )
    {
        var itemJson = JsonConvert.SerializeObject(item);
        var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
        var base64Json = Convert.ToBase64String(itemJsonBytes);
        await sessionStorageService.SetItemAsync(key, base64Json);
    }

    public static async Task<T> ReadEncryptedItem<T>(this ISessionStorageService sessionStorageService, string key)
    {
        var base64Json = await sessionStorageService.GetItemAsync<string>(key);
        if (string.IsNullOrEmpty(base64Json))
            return default;
        var itemJsonBytes = Convert.FromBase64String(base64Json);
        var itemJson = Encoding.UTF8.GetString(itemJsonBytes);
        var item = JsonConvert.DeserializeObject<T>(itemJson);
        return item;
    }
}
