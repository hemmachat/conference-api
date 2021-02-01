using System;
using System.Net.Http;
using System.Threading.Tasks;
using ConferenceApi.Models;
using Xunit;
using ConferenceApi.Repository;
using MemoryCache.Testing.Moq;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Refit;

namespace ConferenceApi.Test
{
    public class SessionRepositoryTest
    {
        private readonly IMemoryCache _cache;
        private readonly IConferenceApi _client;
        private readonly ISessionRepository _sessionRepository;

        public SessionRepositoryTest()
        {
            _cache = Create.MockedMemoryCache();
            _client = Substitute.For<IConferenceApi>();
            var apiResponse = new ApiResponse<ResponseSession>(new HttpResponseMessage(), 
                ResponseHelper.BuildResponseSession(), null);
            _client.GetSessions().Returns(apiResponse);
            _sessionRepository = new SessionRepository(_client, _cache);
        }

        [Fact]
        public void Get_Session_Id_Should_Return_Session_Id()
        {
            var sessionId = SessionRepository.GetSessionId(new Uri("https://conferenceapi.azurewebsites.net/session/100"));

            Assert.Equal(100, sessionId);
        }

        [Fact]
        public async Task Fetch_Sessions_Should_Return_Sessions()
        {
            var items = await _sessionRepository.FetchSessions();

            Assert.NotEmpty(items);
        }
    }
}
