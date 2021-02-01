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

        public static string ExtractTime(string timeSlot)
        {
            var pattern = new Regex(@"\d{1,2}:\d{1,2}\s*-\s*\d{1,2}:\d{1,2}");

            return !pattern.IsMatch(timeSlot) ? string.Empty : 
                pattern.Match(timeSlot).Value.Replace(" ", "");
        }

        public static string ExtractDate(string timeSlot)
        {
            var pattern = new Regex(@"\d{1,2}\s*\w+\s*\d{4}");

            return !pattern.IsMatch(timeSlot) ? string.Empty : pattern.Match(timeSlot).Value;
        }
    }
}
