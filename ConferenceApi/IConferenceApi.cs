using System.Collections.Generic;
using System.Threading.Tasks;
using ConferenceApi.Models;
using Refit;

namespace ConferenceApi
{
    [Headers(
        "Cache-Control: no-cache", 
        "Api-version: v1", 
        "Ocp-Apim-Subscription-Key: 67f50e1f75e84a78856eb15d8ec10a48")]
    public interface IConferenceApi
    {
        [Get("/sessions")]
        Task<ApiResponse<ResponseSession>> GetSessions();


        [Get("/session/{id}/topics")]
        Task<ApiResponse<ResponseTopic>> GetSessionTopics(int id);
    }
}