using System;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApi.Models;
using ConferenceApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConferenceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionsTopicsController : ControllerBase
    {
        private readonly ILogger<SessionsTopicsController> _logger;
        private readonly ISessionRepository _sessionRepository;
        private readonly ITopicRepository _topicRepository;

        public SessionsTopicsController(ILogger<SessionsTopicsController> logger,
            ISessionRepository sessionRepository, ITopicRepository topicRepository)
        {
            _logger = logger;
            _sessionRepository = sessionRepository;
            _topicRepository = topicRepository;
        }

        [HttpGet]
        public async Task<Item[]> GetSessionsTopics(string speaker, DateTime? date, string timeSlot)
        {
            var sessions = await _sessionRepository.FetchSessions();
            sessions = DataFilter.FilterSessions(speaker, date, timeSlot, sessions);

            foreach (var session in sessions)
            {
                var sessionId = SessionRepository.GetSessionId(session.Href);
                var topics = await _topicRepository.FetchTopics(sessionId);

                session.Topics = topics;
            }

            return sessions;
        }
    }
}