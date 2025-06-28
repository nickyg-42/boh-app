using ui.Models;

namespace ui.Services;

public class VerbOfTheDayStateService
{
    public Verb? CurrentVerb { get; private set; }
    private readonly VerbService _verbService;

    public VerbOfTheDayStateService(VerbService verbService)
    {
        _verbService = verbService;
    }

    public async Task LoadVerbOfTheDay()
    {
        if (CurrentVerb == null)
        {
            CurrentVerb = await _verbService.GetRandomVerb();
        }
    }

    public async Task LoadNextVerb()
    {
        CurrentVerb = await _verbService.GetRandomVerb();
    }
}
