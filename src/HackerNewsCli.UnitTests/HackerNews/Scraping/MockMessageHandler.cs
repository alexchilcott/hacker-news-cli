using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HackerNewsCli.UnitTests.HackerNews.Scraping
{
    public class MockMessageHandler : HttpMessageHandler
    {
        private readonly IList<RequestHandler> _requestHandlers = new List<RequestHandler>();

        public void SetupRequest(Predicate<HttpRequestMessage> canHandleRequest, Action<HttpResponseMessage> configureResponse)
        {
            _requestHandlers.Add(new RequestHandler(canHandleRequest, configureResponse));
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var foundHandler = _requestHandlers.FirstOrDefault(x => x.CanHandleRequest(request));
            if (foundHandler == null)
            {
                throw new InvalidOperationException($"No configured handler for request {request.Method} {request.RequestUri}");
            }

            var response = request.CreateResponse();
            foundHandler.ConfigureResponse(response);
            return Task.FromResult(response);
        }

        private class RequestHandler
        {
            public RequestHandler(Predicate<HttpRequestMessage> canHandleRequest, Action<HttpResponseMessage> configureResponse)
            {
                CanHandleRequest = canHandleRequest;
                ConfigureResponse = configureResponse;
            }

            public Predicate<HttpRequestMessage> CanHandleRequest { get; }
            public Action<HttpResponseMessage> ConfigureResponse { get; }
        }
    }
}