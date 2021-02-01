using System;
using System.Collections.Generic;
using System.Text;
using ConferenceApi.Models;
using Xunit;

namespace ConferenceApi.Test
{
    public class DataFilterTest
    {
        [Fact]
        public void Extract_Date_Given_Date_Time_Should_Return_Correct_Date()
        {
            Assert.Equal("4 December 2013", DataFilter.ExtractDate("4 December 2013 09:00 - 10:00"));
        }

        [Fact]
        public void Extract_Time_Given_Date_Time_Should_Return_Correct_Time()
        {
            Assert.Equal("09:00-10:00", DataFilter.ExtractTime("4 December 2013 09:00 - 10:00"));
        }

        [Fact]
        public void Extract_Time_Given_Time_Should_Return_Correct_Time()
        {
            Assert.Equal("09:00-10:00", DataFilter.ExtractTime("09:00   -   10:00"));
        }

        [Fact]
        public void Filter_Date_Given_Empty_Data_Should_Return_Empty()
        {
            var items = new List<Item>();

            var result = DataFilter.FilterDate(items.ToArray(), DateTime.Now);

            Assert.Equal(items.Count, result.Length);
        }

        [Fact]
        public void Filter_Date_Given_Data_Should_Return_Filtered_Date()
        {
            var items = new List<Item>
            {
                new Item {Timeslot = "4 December 2013 09:00 - 10:00"}, 
                new Item {Timeslot = "5 December 2013 09:00 - 10:00"}
            };

            var result = DataFilter.FilterDate(items.ToArray(), new DateTime(2013, 12, 4));

            Assert.Single(result);
        }

        [Fact]
        public void Filter_Time_Given_Data_Should_Return_Filtered_Time()
        {
            var items = new List<Item>
            {
                new Item {Timeslot = "4 December 2013 09:00 - 10:00"}, 
                new Item {Timeslot = "5 December 2013 09:30 - 10:00"}
            };

            var result = DataFilter.FilterTime(items.ToArray(), "09:00-10:00");

            Assert.Single(result);
        }

        [Fact]
        public void Filter_Speaker_Given_Data_Should_Return_Filtered_Speaker()
        {
            var items = new List<Item>
            {
                new Item {Speaker = "Jon Skeet"},
                new Item {Speaker = "Scott Hanselman"}
            };

            var result = DataFilter.FilterSpeaker(items.ToArray(), "Jon Skeet");

            Assert.Single(result);
        }

        [Fact]
        public void Flatten_Data_Given_Nested_Data_Should_Return_Flattened_Data()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Data = new[]
                    {
                        new Datum {Name = Name.Speaker, Value = "Jon Skeet"},
                        new Datum {Name = Name.Timeslot, Value = "4 December 2013 09:00 - 10:00"},
                        new Datum {Name = Name.Title, Value = "C#"}
                    }
                }
            };

            var result = DataFilter.FlattenData(items.ToArray());

            Assert.Equal("C#", result[0].Title);
            Assert.Equal("Jon Skeet", result[0].Speaker);
            Assert.Equal("4 December 2013 09:00 - 10:00", result[0].Timeslot);
        }
    }
}
