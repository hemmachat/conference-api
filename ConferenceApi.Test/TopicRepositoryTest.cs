using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceApi.Models;
using ConferenceApi.Repository;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Refit;
using Xunit;

namespace ConferenceApi.Test
{
    public class TopicRepositoryTest
    {
        private readonly IMemoryCache _cache;
        private readonly IConferenceApi _client;
        private readonly ITopicRepository _topicRepository;
        private readonly int sessionId = 100;

        public TopicRepositoryTest()
        {
            _cache = Create.MockedMemoryCache();
            _client = Substitute.For<IConferenceApi>();

            var apiResponse = new ApiResponse<ResponseTopic>(new HttpResponseMessage(), 
                ResponseHelper.BuildResponseTopic(), null);
            _client.GetSessionTopics(sessionId).Returns(apiResponse);
            _topicRepository = new TopicRepository(_client, _cache);
        }

        [Fact]
        public async Task Fetch_Topics_Should_Return_Topics()
        {
            var items = await _topicRepository.FetchTopics(sessionId);

            Assert.NotEmpty(items);
        }
    }
}
