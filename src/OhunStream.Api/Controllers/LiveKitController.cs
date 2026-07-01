using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OhunStream.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiveKitController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LiveKitController(IConfiguration config)
        {
            _config = config;
        }

        // POST /api/livekit/token
        // Authenticated users joining interactive sessions (can publish + subscribe)
        [HttpPost("token")]
        public IActionResult CreateToken([FromBody] TokenRequest request)
        {
            var (apiKey, apiSecret, err) = GetCredentials();
            if (err != null) return BadRequest(err);

            var token = GenerateToken(apiKey, apiSecret, request.Room, request.Identity,
                canPublish: true, canSubscribe: true);

            return Ok(new { token });
        }

        // POST /api/livekit/guest-token
        // Anyone with a meeting link can join — no auth required
        [HttpPost("guest-token")]
        public IActionResult CreateGuestToken([FromBody] GuestTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.MeetingId))
                return BadRequest("Meeting ID is required.");

            var displayName = string.IsNullOrWhiteSpace(request.DisplayName) ? "Guest" : request.DisplayName.Trim();
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var identity = $"{displayName}_{suffix}";

            var (apiKey, apiSecret, err) = GetCredentials();
            if (err != null) return BadRequest(err);

            var token = GenerateToken(apiKey, apiSecret, request.MeetingId, identity,
                canPublish: true, canSubscribe: true);

            return Ok(new { token, identity, displayName });
        }

        // POST /api/livekit/viewer-token
        // Viewers watching a live broadcast — subscribe only, no publish
        [HttpPost("viewer-token")]
        public IActionResult CreateViewerToken([FromBody] ViewerTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SessionId))
                return BadRequest("Session ID is required.");

            var displayName = string.IsNullOrWhiteSpace(request.DisplayName) ? "Viewer" : request.DisplayName.Trim();
            var identity = $"viewer_{displayName}_{Guid.NewGuid().ToString("N")[..6]}";

            var (apiKey, apiSecret, err) = GetCredentials();
            if (err != null) return BadRequest(err);

            var token = GenerateToken(apiKey, apiSecret, request.SessionId, identity,
                canPublish: false, canSubscribe: true);

            return Ok(new { token, identity });
        }

        private (string apiKey, string apiSecret, string? error) GetCredentials()
        {
            var apiKey = _config["LIVEKIT_API_KEY"];
            var apiSecret = _config["LIVEKIT_API_SECRET"];
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                return ("", "", "LIVEKIT_API_KEY and LIVEKIT_API_SECRET must be configured.");
            return (apiKey, apiSecret, null);
        }

        private static string GenerateToken(
            string apiKey, string apiSecret,
            string room, string identity,
            bool canPublish, bool canSubscribe)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // LiveKit JWT spec:
            //   iss  = API key
            //   sub  = participant identity
            //   video grant = { roomJoin, room, canPublish, canSubscribe, canPublishData }
            var payload = new
            {
                iss = apiKey,
                sub = identity,
                iat = now,
                nbf = now,
                exp = now + 3600,
                jti = Guid.NewGuid().ToString("N"),
                video = new
                {
                    roomJoin = true,
                    room = room ?? "",
                    canPublish,
                    canSubscribe,
                    canPublishData = true
                }
            };

            var header = new { alg = "HS256", typ = "JWT" };

            string B64Url(object obj)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(obj);
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(json))
                    .TrimEnd('=').Replace('+', '-').Replace('/', '_');
            }

            var headerEnc = B64Url(header);
            var payloadEnc = B64Url(payload);
            var unsigned = $"{headerEnc}.{payloadEnc}";

            using var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(apiSecret));
            var sig = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsigned));
            var sigEnc = Convert.ToBase64String(sig).TrimEnd('=').Replace('+', '-').Replace('/', '_');

            return $"{unsigned}.{sigEnc}";
        }

        public class TokenRequest
        {
            public string Room { get; set; } = "";
            public string Identity { get; set; } = "";
        }

        public class GuestTokenRequest
        {
            public string MeetingId { get; set; } = "";
            public string DisplayName { get; set; } = "";
        }

        public class ViewerTokenRequest
        {
            public string SessionId { get; set; } = "";
            public string DisplayName { get; set; } = "";
        }
    }
}
