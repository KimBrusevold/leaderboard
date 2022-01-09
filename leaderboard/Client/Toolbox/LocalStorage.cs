using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace leaderboard.Client.Toolbox;

public class LocalStorage
{
    private readonly IJSRuntime _js;

    public LocalStorage(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<string> GetItem(string key)
    {
        return await _js.InvokeAsync<string>("localStorage.getItem", key);
    }

    public async Task SetItem(string key, string value)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", key, value);
    }
    public async Task RemoveItem(string key)
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
