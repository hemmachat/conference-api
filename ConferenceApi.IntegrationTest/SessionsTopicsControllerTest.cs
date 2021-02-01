using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConferenceApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Xunit;

namespace ConferenceApi.Test
{
    public class SessionsTopicsControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public SessionsTopicsControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Home_Page_Should_Return_Default_Items()
        {
            var httpResponse = await _client.GetAsync($"/SessionsTopics");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<Item[]>(stringResponse);

            Assert.NotEmpty(items);
            Assert.Equal(Constants.TotalItems, items.Length);
        }

        [Fact]
        public async Task Home_Given_Speaker_Should_Return_Speaker_Items()
        {
            var httpResponse = await _client.GetAsync($"/SessionsTopics?speaker=Jon Skeet");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<Item[]>(stringResponse);

            Assert.NotEmpty(items);
            Assert.Equal("Jon Skeet", items[0].Speaker);
        }

        // this test is flaky and need to be revise
        //[Fact]
        public async Task Home_Given_Date_Should_Return_Date_Items()
        {
            var httpResponse = await _client.GetAsync($"/SessionsTopics?date=2013-12-04");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<Item[]>(stringResponse);

            Assert.NotEmpty(items);
            Assert.Contains("04 December 2013", items[0].Timeslot);
        }

        [Fact]
        public async Task Home_Given_Timeslot_Should_Return_Timeslot_Items()
        {
            var httpResponse = await _client.GetAsync($"/SessionsTopics?timeslot=09:00-10:00");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<Item[]>(stringResponse);

            Assert.NotEmpty(items);
            Assert.Contains("09:00 - 10:00", items[0].Timeslot);
        }
    }
}
