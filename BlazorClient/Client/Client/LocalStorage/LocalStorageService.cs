using Microsoft.JSInterop;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Client.LocalStorage;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }


    public async Task SetAsync<TValue>(string key, TValue value) where TValue : class
    {
        var data = JsonSerializer.Serialize(value);
        await _jsRuntime.InvokeVoidAsync("set", key, data);
    }

    public async Task<TValue?> GetAsync<TValue>(string key) where TValue : class
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<string>("get", key);
            if (string.IsNullOrEmpty(result))
                return null;

            return JsonSerializer.Deserialize<TValue>(result)!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }

    public async Task RemoveAsync(string key)
    {
        await _jsRuntime.InvokeAsync<string>("remove", key);
    }
}