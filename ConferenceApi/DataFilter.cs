using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConferenceApi.Models;

namespace ConferenceApi
{
    public class DataFilter
    {
        /// <summary>
        /// Get session based on speaker, date and timeslot
        /// </summary>
        /// <param name="speaker">Speaker name</param>
        /// <param name="date">Date</param>
        /// <param name="timeSlot">Timeslot</param>
        /// <param name="sessions">Session data</param>
        /// <returns>The filtered data</returns>
        public static Item[] FilterSessions(string speaker, DateTime? date, string timeSlot, Item[] sessions)
        {
            // limit the items if no input specify
            if (string.IsNullOrEmpty(speaker) && date == null && string.IsNullOrEmpty(timeSlot))
            {
                sessions = sessions.Take(Constants.TotalItems).ToArray();
            }

            if (!string.IsNullOrEmpty(speaker))
            {
                sessions = DataFilter.FilterSpeaker(sessions, speaker);
            }

            if (date != null)
            {
                sessions = DataFilter.FilterDate(sessions, date);
            }

            if (!string.IsNullOrEmpty(timeSlot))
            {
                sessions = DataFilter.FilterTime(sessions, timeSlot);
            }

            return sessions;
        }

        /// <summary>
        /// Get only data for a specific speaker
        /// </summary>
        /// <param name="sessions">Sessions data</param>
        /// <param name="speaker">Speaker name to filter</param>
        /// <returns>The filtered data</returns>
        public static Item[] FilterSpeaker(Item[] sessions, string speaker)
        {
            var speakerSessions = new List<Item>();

            foreach (var session in sessions)
            {
                if (session.Speaker == speaker)
                {
                    speakerSessions.Add(session);
                }
            }

            return speakerSessions.ToArray();
        }

        /// <summary>
        /// Get only data for a specific time
        /// </summary>
        /// <param name="sessions">Sessions data</param>
        /// <param name="timeSlot">Timeslot to filter</param>
        /// <returns>The filtered data</returns>
        public static Item[] FilterTime(Item[] sessions, string timeSlot)
        {
            var timeSessions = new List<Item>();

            foreach (var session in sessions)
            {
                var datumTime = ExtractTime(session.Timeslot);

                if (string.IsNullOrEmpty(datumTime))
                {
                    break;
                }

                if (datumTime == timeSlot)
                {
                    timeSessions.Add(session);
                }
            }

            return timeSessions.ToArray();
        }

        /// <summary>
        /// Flatten the data to be faster traversal and avoid nested loops finding
        /// </summary>
        /// <param name="sessions">Sessions data</param>
        /// <returns>Flatten sessions data</returns>
        public static Item[] FlattenData(Item[] sessions)
        {
            var items = new List<Item>();

            foreach (var session in sessions)
            {
                var newSession = session;

                foreach (var datum in session.Data)
                {
                    switch (datum.Name)
                    {
                        case Name.Title:
                            newSession.Title = datum.Value;
                            break;

                        case Name.Timeslot:
                            newSession.Timeslot = datum.Value;
                            break;

                        case Name.Speaker:
                            newSession.Speaker = datum.Value;
                            break;

                        default:
                            newSession.Speaker = datum.Value;
                            break;
                    }
                }

                items.Add(newSession);
            }

            return items.ToArray();
        }

        /// <summary>
        /// Get only specific date data from sessions
        /// </summary>
        /// <param name="sessions">Sessions data</param>
        /// <param name="date">Date to be filtered</param>
        /// <returns>Filtered sessions based of the date</returns>
        public static Item[] FilterDate(Item[] sessions, DateTime? date)
        {
            var dateSessions = new List<Item>();

            foreach (var session in sessions)
            {
                var dateString = ExtractDate(session.Timeslot);

                if (string.IsNullOrEmpty(dateString))
                {
                    break;
                }

                var datumDate = Convert.ToDateTime(dateString);

                if (datumDate == date)
                {
                    dateSessions.Add(session);
                }
            }

            return dateSessions.ToArray();
        }

        /// <summary>
        /// Extract time from timeslot text
        /// </summary>
        /// <param name="timeSlot">Timeslot text</param>
        /// <returns>Time text</returns>
        public static string ExtractTime(string timeSlot)
        {
            var pattern = new Regex(@"\d{1,2}:\d{1,2}\s*-\s*\d{1,2}:\d{1,2}");

            return !pattern.IsMatch(timeSlot) ? string.Empty : 
                pattern.Match(timeSlot).Value.Replace(" ", "");
        }

        /// <summary>
        /// Extract date text from timeslot text
        /// </summary>
        /// <param name="timeSlot">Timeslot text</param>
        /// <returns>Date text</returns>
        public static string ExtractDate(string timeSlot)
        {
            var pattern = new Regex(@"\d{1,2}\s*\w+\s*\d{4}");

            return !pattern.IsMatch(timeSlot) ? string.Empty : pattern.Match(timeSlot).Value;
        }
    }
}
