using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ConferenceApi.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly IConferenceApi _client;
        private readonly IMemoryCache _cache;

        public TopicRepository(IConferenceApi client, IMemoryCache memoryCache)
        {
            _client = client;
            _cache = memoryCache;
        }

        public async Task<Item[]> FetchTopics(int sessionId)
        {
            if (_cache.TryGetValue(sessionId, out Item[] topics))
            {
                return topics;
            }

            var topicResponse = await _client.GetSessionTopics(sessionId);
            topics = topicResponse.Content.Collection.Items;
            _cache.Set(sessionId, topics, TimeSpan.FromMinutes(Constants.CacheMinute));

            return topics;
        }
    }

    public interface ITopicRepository
    {
        public Task<Item[]> FetchTopics(int sessionId);
    }
}
