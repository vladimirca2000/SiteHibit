using System.Net;
using System.Net.Http.Json;
using Hibit.Web.Models;

namespace Hibit.Web.Services;

public interface IContactService
{
    Task<ContactResponse> SubmitAsync(ContactFormData data);
}

public class ContactService : IContactService
{
    private readonly HttpClient _http;

    public ContactService(HttpClient http)
    {
        _http = http;
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

        throw new ContactSubmissionException(response.StatusCode, error);
    }
}

public class ContactResponse
{
    public string Message { get; set; } = string.Empty;
}

public class ContactSubmissionException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public ContactSubmissionException(HttpStatusCode statusCode, string error)
        : base(BuildMessage(statusCode, error))
    {
        StatusCode = statusCode;
    }

    private static string BuildMessage(HttpStatusCode statusCode, string error)
    {
        return statusCode switch
        {
            HttpStatusCode.TooManyRequests =>
                "Voce enviou muitas mensagens em pouco tempo. Aguarde alguns minutos e tente novamente.",
            HttpStatusCode.BadRequest =>
                string.IsNullOrWhiteSpace(error)
                    ? "Dados invalidos. Verifique os campos e tente novamente."
                    : $"Dados invalidos: {error}",
            HttpStatusCode.ServiceUnavailable =>
                "O servico de mensageria esta indisponivel no momento. Tente novamente em instantes.",
            _ => $"Erro ao enviar mensagem ({(int)statusCode}). Tente novamente."
        };
    }
}
