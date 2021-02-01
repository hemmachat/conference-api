using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConferenceApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ConferenceApi.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IConferenceApi _client;

        public SessionRepository(IConferenceApi client, IMemoryCache memoryCache)
        {
            _client = client;
            _cache = memoryCache;
        }

        public async Task<Item[]> FetchSessions()
        {
            if (_cache.TryGetValue("sessions", out Item[] sessions))
            {
                return sessions;
            }

            var sessionResponse = await _client.GetSessions();
            sessions = sessionResponse.Content.Collection.Items;
            _cache.Set("sessions", sessions, TimeSpan.FromMinutes(Constants.CacheMinute));

            return DataFilter.FlattenData(sessions);
        }

        public static int GetSessionId(Uri sessionHref)
        {
            var path = sessionHref.AbsolutePath;
            var pattern = new Regex(@"\d+");

            if (!pattern.IsMatch(path))
            {
                return 0;
            }

            int.TryParse(pattern.Match(path).Value, out var result);

            return result;
        }
    }

    public interface ISessionRepository
    {
        public Task<Item[]> FetchSessions();
    }
}
