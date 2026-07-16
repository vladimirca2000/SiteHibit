using Microsoft.JSInterop;
using Hibit.Web.Models;

namespace Hibit.Web.Services;

public interface IThemeService
{
    Task InitializeAsync();
    Task ToggleThemeAsync();
    bool IsDarkMode { get; }
    event Action? OnThemeChanged;
}

public class ThemeService : IThemeService
{
    private readonly IJSRuntime _js;
    private bool _isDarkMode;
    private bool _initialized;

    public bool IsDarkMode => _isDarkMode;
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            var saved = await _js.InvokeAsync<string?>("localStorage.getItem", "theme");
            _isDarkMode = saved == "dark" || (saved == null && await _js.InvokeAsync<bool>("hibitTheme.prefersDark"));
        }
        catch
        {
            _isDarkMode = false;
        }

        await ApplyThemeAsync();
        _initialized = true;
    }

    public async Task ToggleThemeAsync()
    {
        _isDarkMode = !_isDarkMode;
        await ApplyThemeAsync();
        OnThemeChanged?.Invoke();
    }

    private async Task ApplyThemeAsync()
    {
        var theme = _isDarkMode ? "dark" : "light";
        await _js.InvokeVoidAsync("document.documentElement.setAttribute", "data-theme", theme);
        await _js.InvokeVoidAsync("localStorage.setItem", "theme", theme);
    }
}