using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Hibit.Web.Models;

namespace Hibit.Web.Services;

public interface IContactService
{
    Task<ContactResponse> SubmitAsync(ContactFormData data);
}

public class ContactService : IContactService
{
    private readonly HttpClient _http;

    public ContactService(HttpClient http, IOptions<ApiSettings> settings)
    {
        _http = http;
        // BaseAddress já configurado no Program.cs via HttpClientFactory
        // Se precisar de ApiUrl aqui, use settings.Value.ApiUrl
    }

    public async Task<ContactResponse> SubmitAsync(ContactFormData data)
    {
        var response = await _http.PostAsJsonAsync("api/contact", data);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ContactResponse>();
            return result ?? new ContactResponse { Message = "Mensagem enviada com sucesso!" };
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Erro ao enviar mensagem: {response.StatusCode} - {error}");
    }
}

public class ContactResponse
{
    public string Message { get; set; } = string.Empty;
}