using Microsoft.JSInterop;

namespace WebApp.Repositories
{
    public class LocalStorageService
    {
        private JSRuntime JsRuntime { get; set; } 

        public LocalStorageService(JSRuntime _jSRuntime) 
        {
            JsRuntime = _jSRuntime;
        }

        public async Task SetItemAsync(string key, string value)
        {
            await JsRuntime.InvokeVoidAsync("localStorage.setItem",key,value);
        }

        public async Task<string> GetItemAsync(string key)
        {
            var result = await JsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return result;
        }

        public async Task RemoveItemAsync(string key)
        {
            await JsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
