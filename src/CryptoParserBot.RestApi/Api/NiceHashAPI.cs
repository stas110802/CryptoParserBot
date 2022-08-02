using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CryptoParserBot.AdditionalToolLibrary;
using CryptoParserBot.RestApi.Enums.NiceHash;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CryptoParserBot.RestApi.Api
{
    public sealed class NiceHashAPI
    {
        private readonly string _uri;
        private readonly string _organizationId;
        private readonly string _key;
        private readonly string _secretKey;

        public NiceHashAPI(string uri, string id, string key, string secretKey)
        {
            _uri = uri;
            _organizationId = id;
            _key = key;
            _secretKey = secretKey;
        }

        public string GetResponseContent(Method method, NHEndpoint endpoint, bool auth = false, string? query = null,
            bool requestId = false, string? extraEndpoint = null, string? payload = null)
        {
            var strEndpoint = endpoint.ToDescription();

            var full = strEndpoint + query;

            if (extraEndpoint != null)
                strEndpoint += extraEndpoint;

            var client = new RestClient(_uri);
            var request = new RestRequest(full);

            // user authentication
            if (auth)
            {
                var time = GetServerTimestamp();
                var nonce = Guid.NewGuid().ToString();
                var digest = HashBySegments(time, nonce, _organizationId, method.ToString().ToUpper(), strEndpoint, GetQuery(full), payload);

                if (requestId)
                {
                    request.AddHeader("X-Request-Id", nonce);
                }

                request.AddHeader("X-Time", time);
                request.AddHeader("X-Nonce", nonce);
                request.AddHeader("X-Auth", _key + ":" + digest);
                request.AddHeader("X-Organization-Id", _organizationId);
            }

            // if need 'application/json' data
            if (payload != null)
            {
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-type", "application/json");
                request.AddJsonBody(payload);
            }

            var response = client.Execute(request, method);
            var content = response.Content;

            if (string.IsNullOrEmpty(content))
            {
                throw new HttpRequestException("[API ERROR] : Server not responded");
            }

            return content;
        }

        private string GetServerTimestamp()
        {
            var timeResponse = GetResponseContent(Method.Get, NHEndpoint.ServerTime);

            if (string.IsNullOrEmpty(timeResponse))
            {
                throw new Exception("[API ERROR] : The server is not responding");
            }

            var serverTimeObject = JsonConvert.DeserializeObject<JToken>(timeResponse);
            var time = serverTimeObject?["serverTime"]?.ToString();
            
            return time;
        }

        private string? HashBySegments(string time, string nonce, string orgId, string method, string encodedPath, string? query, string? bodyStr)
        {
            var segments = new List<string?>
            {
                _key,
                time,
                nonce,
                null,
                orgId,
                null,
                method,
                encodedPath,
                query
            };

            if (string.IsNullOrEmpty(bodyStr) == false)
            {
                segments.Add(bodyStr);
            }

            return CalcHMACSHA256Hash(JoinSegments(segments), _secretKey);
        }
        private static string GetBasePath(string url)
        {
            var arrSplit = url.Split('?');

            return arrSplit[0];
        }
        private static string? GetQuery(string url)
        {
            var arrSplit = url.Split('?');

            return arrSplit.Length == 1 ? null : arrSplit[1];
        }

        private static string JoinSegments(List<string?> segments)
        {
            var sb = new StringBuilder();
            var first = true;

            foreach (var segment in segments)
            {
                if (!first)
                {
                    sb.Append("\x00");
                }
                else
                {
                    first = false;
                }

                if (segment != null)
                {
                    sb.Append(segment);
                }
            }

            return sb.ToString();
        }

        private static string? CalcHMACSHA256Hash(string plaintext, string salt)
        {
            var encoding = Encoding.Default;

            byte[] baText2BeHashed = encoding.GetBytes(plaintext);
            byte[] baSalt = encoding.GetBytes(salt);

            var hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);

            var result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());

            return result;
        }
    }
}
