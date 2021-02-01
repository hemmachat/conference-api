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

        /// <summary>
        /// Get sessions with topics
        /// </summary>
        /// <param name="speaker">Speaker name to filter e.g. 'Jon Skeet'</param>
        /// <param name="date">Date to filter e.g. '2013-12-04'</param>
        /// <param name="timeSlot">Timeslot to filter e.g. '09:00-10:00'</param>
        /// <returns>Sessions with topics</returns>
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