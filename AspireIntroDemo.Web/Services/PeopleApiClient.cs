using System.Text.Json;

namespace AspireIntroDemo.Web;

public class PeopleApiClient(HttpClient httpClient)
{
    public async Task<Person[]> GetPeopleAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Person[]>("/people", cancellationToken)
            ?? throw new JsonException("The people API returned a null response.");
    }
}

public record Person(int PersonId, string FirstName, string LastName);
