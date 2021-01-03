using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using RestSharp.Serialization;
using RestSharp.Serializers.NewtonsoftJson;

namespace KunaV2
{
    public class RestClientDecorator : IRestClient
    {
        private readonly IRestClient _restClient;
        private readonly ILogger<RestClientDecorator> _logger;

        public RestClientDecorator(ILogger<RestClientDecorator> logger)
        {
            _logger = logger;

            _restClient = new RestClient(Urls.Base + Urls.Version);
            _restClient.UseNewtonsoftJson(new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public IRestClient UseSerializer(Func<IRestSerializer> serializerFactory)
        {
            return _restClient.UseSerializer(serializerFactory);
        }

        public IRestClient UseSerializer<T>() where T : IRestSerializer, new()
        {
            return _restClient.UseSerializer<T>();
        }

        public IRestResponse<T> Deserialize<T>(IRestResponse response)
        {
            return _restClient.Deserialize<T>(response);
        }

        public IRestClient UseUrlEncoder(Func<string, string> encoder)
        {
            return _restClient.UseUrlEncoder(encoder);
        }

        public IRestClient UseQueryEncoder(Func<string, Encoding, string> queryEncoder)
        {
            return _restClient.UseQueryEncoder(queryEncoder);
        }

        public IRestResponse Execute(IRestRequest request)
        {
            var response = _restClient.Execute(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public IRestResponse Execute(IRestRequest request, Method httpMethod)
        {
            var response = _restClient.Execute(request, httpMethod);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var response = _restClient.Execute<T>(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public IRestResponse<T> Execute<T>(IRestRequest request, Method httpMethod)
        {
            var response = _restClient.Execute<T>(request, httpMethod);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public byte[] DownloadData(IRestRequest request)
        {
            return _restClient.DownloadData(request);
        }

        public byte[] DownloadData(IRestRequest request, bool throwOnError)
        {
            return _restClient.DownloadData(request, throwOnError);
        }

        public Uri BuildUri(IRestRequest request)
        {
            return _restClient.BuildUri(request);
        }

        public string BuildUriWithoutQueryParameters(IRestRequest request)
        {
            return _restClient.BuildUriWithoutQueryParameters(request);
        }

        public void ConfigureWebRequest(Action<HttpWebRequest> configurator)
        {
            _restClient.ConfigureWebRequest(configurator);
        }

        public void AddHandler(string contentType, Func<IDeserializer> deserializerFactory)
        {
            _restClient.AddHandler(contentType, deserializerFactory);
        }

        public void RemoveHandler(string contentType)
        {
            _restClient.RemoveHandler(contentType);
        }

        public void ClearHandlers()
        {
            _restClient.ClearHandlers();
        }

        public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
        {
            var response = _restClient.ExecuteAsGet(request, httpMethod);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
        {
            var response = _restClient.ExecuteAsPost(request, httpMethod);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod)
        {
            return _restClient.ExecuteAsGet<T>(request, httpMethod);
        }

        public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod)
        {
            var response = _restClient.ExecuteAsPost<T>(request, httpMethod);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecuteAsync<T>(request, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request, Method httpMethod,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecuteAsync<T>(request, httpMethod, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest request, Method httpMethod,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecuteAsync(request, httpMethod, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecuteAsync(request, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteGetAsync<T>(IRestRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecuteGetAsync<T>(request, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecutePostAsync<T>(IRestRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecutePostAsync<T>(request, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                           $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteGetAsync(IRestRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecuteGetAsync(request, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecutePostAsync(IRestRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var response = await _restClient.ExecutePostAsync(request, cancellationToken);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public IRestClient UseSerializer(IRestSerializer serializer)
        {
            return _restClient.UseSerializer(serializer);
        }

        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            return _restClient.ExecuteAsync(request, callback);
        }

        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
        {
            return _restClient.ExecuteAsync(request, callback);
        }

        public RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, Method httpMethod)
        {
            return _restClient.ExecuteAsync(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, Method httpMethod)
        {
            return _restClient.ExecuteAsync(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _restClient.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _restClient.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _restClient.ExecuteAsyncGet(request, callback, httpMethod);
        }

        public RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
        {
            return _restClient.ExecuteAsyncPost(request, callback, httpMethod);
        }

        public async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request)
        {
            var response = await _restClient.ExecuteTaskAsync<T>(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            var response = await _restClient.ExecuteTaskAsync<T>(request, token);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteTaskAsync<T>(IRestRequest request, Method httpMethod)
        {
            var response = await _restClient.ExecuteTaskAsync<T>(request, httpMethod);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request)
        {
            var response = await _restClient.ExecuteGetTaskAsync<T>(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecuteGetTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            var response = await _restClient.ExecuteGetTaskAsync<T>(request, token);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request)
        {
            var response = await _restClient.ExecutePostTaskAsync<T>(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse<T>> ExecutePostTaskAsync<T>(IRestRequest request, CancellationToken token)
        {
            var response = await _restClient.ExecutePostTaskAsync<T>(request, token);
            _logger.LogTrace($"Request: {request.Method} {request.Resource}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token)
        {
            var response = await _restClient.ExecuteTaskAsync(request, token);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteTaskAsync(IRestRequest request, CancellationToken token, Method httpMethod)
        {
            var response = await _restClient.ExecuteTaskAsync(request, token, httpMethod);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteTaskAsync(IRestRequest request)
        {
            var response = await _restClient.ExecuteTaskAsync(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request)
        {
            var response = await _restClient.ExecuteGetTaskAsync(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecuteGetTaskAsync(IRestRequest request, CancellationToken token)
        {
            var response = await _restClient.ExecuteGetTaskAsync(request, token);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request)
        {
            var response = await _restClient.ExecutePostTaskAsync(request);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public async Task<IRestResponse> ExecutePostTaskAsync(IRestRequest request, CancellationToken token)
        {
            var response = await _restClient.ExecutePostTaskAsync(request, token);
            _logger.LogTrace($"Request: {request.Method} {request.Resource} {request.Body?.Value}. " +
                             $"Response: {response.StatusCode} {response.Content}.".Truncate(500));
            return response;
        }

        public void AddHandler(string contentType, IDeserializer deserializer)
        {
            _restClient.AddHandler(contentType, deserializer);
        }

        public CookieContainer CookieContainer
        {
            get => _restClient.CookieContainer;
            set => _restClient.CookieContainer = value;
        }

        public bool AutomaticDecompression
        {
            get => _restClient.AutomaticDecompression;
            set => _restClient.AutomaticDecompression = value;
        }

        public int? MaxRedirects
        {
            get => _restClient.MaxRedirects;
            set => _restClient.MaxRedirects = value;
        }

        public string UserAgent
        {
            get => _restClient.UserAgent;
            set => _restClient.UserAgent = value;
        }

        public int Timeout
        {
            get => _restClient.Timeout;
            set => _restClient.Timeout = value;
        }

        public int ReadWriteTimeout
        {
            get => _restClient.ReadWriteTimeout;
            set => _restClient.ReadWriteTimeout = value;
        }

        public bool UseSynchronizationContext
        {
            get => _restClient.UseSynchronizationContext;
            set => _restClient.UseSynchronizationContext = value;
        }

        public IAuthenticator Authenticator
        {
            get => _restClient.Authenticator;
            set => _restClient.Authenticator = value;
        }

        public Uri BaseUrl
        {
            get => _restClient.BaseUrl;
            set => _restClient.BaseUrl = value;
        }

        public Encoding Encoding
        {
            get => _restClient.Encoding;
            set => _restClient.Encoding = value;
        }

        public bool ThrowOnDeserializationError
        {
            get => _restClient.ThrowOnDeserializationError;
            set => _restClient.ThrowOnDeserializationError = value;
        }

        public bool FailOnDeserializationError
        {
            get => _restClient.FailOnDeserializationError;
            set => _restClient.FailOnDeserializationError = value;
        }

        public bool ThrowOnAnyError
        {
            get => _restClient.ThrowOnAnyError;
            set => _restClient.ThrowOnAnyError = value;
        }

        public string ConnectionGroupName
        {
            get => _restClient.ConnectionGroupName;
            set => _restClient.ConnectionGroupName = value;
        }

        public bool PreAuthenticate
        {
            get => _restClient.PreAuthenticate;
            set => _restClient.PreAuthenticate = value;
        }

        public bool UnsafeAuthenticatedConnectionSharing
        {
            get => _restClient.UnsafeAuthenticatedConnectionSharing;
            set => _restClient.UnsafeAuthenticatedConnectionSharing = value;
        }

        public IList<Parameter> DefaultParameters => _restClient.DefaultParameters;

        public string BaseHost
        {
            get => _restClient.BaseHost;
            set => _restClient.BaseHost = value;
        }

        public bool AllowMultipleDefaultParametersWithSameName
        {
            get => _restClient.AllowMultipleDefaultParametersWithSameName;
            set => _restClient.AllowMultipleDefaultParametersWithSameName = value;
        }

        public X509CertificateCollection ClientCertificates
        {
            get => _restClient.ClientCertificates;
            set => _restClient.ClientCertificates = value;
        }

        public IWebProxy Proxy
        {
            get => _restClient.Proxy;
            set => _restClient.Proxy = value;
        }

        public RequestCachePolicy CachePolicy
        {
            get => _restClient.CachePolicy;
            set => _restClient.CachePolicy = value;
        }

        public bool Pipelined
        {
            get => _restClient.Pipelined;
            set => _restClient.Pipelined = value;
        }

        public bool FollowRedirects
        {
            get => _restClient.FollowRedirects;
            set => _restClient.FollowRedirects = value;
        }

        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback
        {
            get => _restClient.RemoteCertificateValidationCallback;
            set => _restClient.RemoteCertificateValidationCallback = value;
        }
    }
}
