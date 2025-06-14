using System.Net.Http.Json;
using ui.Models;

namespace ui.Services;

public class VerbService
{
    private readonly HttpClient _http;

    public VerbService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Verb?> GetRandomVerb()
    {
        return await _http.GetFromJsonAsync<Verb>("api/verbs/random");
    }    public async Task<IEnumerable<Verb>> SearchVerbsByName(string name)
    {
        return await _http.GetFromJsonAsync<IEnumerable<Verb>>($"api/verbs/search/byname?name={name}") ?? Array.Empty<Verb>();
    }

    public async Task<IEnumerable<Verb>> SearchVerbsByDefinition(string definition)
    {
        return await _http.GetFromJsonAsync<IEnumerable<Verb>>($"api/verbs/search/bydef?definition={definition}") ?? Array.Empty<Verb>();
    }

    public async Task<IEnumerable<Verb>> SearchVerbsByTag(string tag)
    {
        return await _http.GetFromJsonAsync<IEnumerable<Verb>>($"api/verbs/search/bytag?tag={tag}") ?? Array.Empty<Verb>();
    }

    public async Task<IEnumerable<Tag>> GetTags()
    {
        return await _http.GetFromJsonAsync<IEnumerable<Tag>>("api/verbs/tags") ?? Array.Empty<Tag>();
    }    public async Task AddTag(int verbId, string tagName)
    {
        await _http.PatchAsync($"api/verbs/{verbId}/tags/add?tagName={tagName}", null);
    }

    public async Task RemoveTag(int verbId, string tagName)
    {
        await _http.PatchAsync($"api/verbs/{verbId}/tags/del?tagName={tagName}", null);
    }

    public async Task ClearTags(int verbId)
    {
        await _http.PostAsync($"api/verbs/{verbId}/tags/clear", null);
    }

    public async Task ClearAllTags()
    {
        await _http.PostAsync("api/verbs/tags/clear-all", null);
    }
}