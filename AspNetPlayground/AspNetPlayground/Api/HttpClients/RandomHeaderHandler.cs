namespace Api.HttpClients;

public class RandomHeaderHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Api-Key", "12345");
        return await base.SendAsync(request, cancellationToken);
    }
}
