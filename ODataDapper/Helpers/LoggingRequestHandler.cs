using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Serilog;

namespace ODataDapper.Helpers
{
    /// <summary>
    /// The logging request handler helper.
    /// </summary>
    /// <seealso cref="DelegatingHandler" />
    public class LoggingRequestHandler : DelegatingHandler
    {
        // Get application base directory
        private static string basedir = AppDomain.CurrentDomain.BaseDirectory;
        ILogger log = new LoggerConfiguration()
            .WriteTo.RollingFile(basedir + "/Logs/log-{Date}.txt")
            .CreateLogger();

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // log request body if it has one
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                var reqUri = request.RequestUri.ToString();
                log.Information("Request URI: " + reqUri);
                log.Information(requestBody);
            }

            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);

            // if response body exists, once it is ready, log it
            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                log.Information("Response data: " + responseBody);
            }

            return result;
        }
    }
}