using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebClientProduser
{
    /// <summary>
    /// Внедряется через HttpClientFactory.
    /// Регистрируется в DI через services.AddHttpClient.
    /// Обеспечивает Оптимальное использование Веб сокетов из ПУЛА.
    /// </summary>
    public class HttpClientSupport : IHttpClientSupport
    {
        private readonly HttpClient _httpClient;

        public HttpClientSupport(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken ct)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync(uri, ct);
                return responseMessage;
            }
            catch (TaskCanceledException) //Timeout ожидания ответа или Ручная отмена через ct
            {
                return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
            catch (HttpRequestException) // Сервер не доступен
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }




        public Task<HttpResponseMessage> PostAsync(Uri uri, string message, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}