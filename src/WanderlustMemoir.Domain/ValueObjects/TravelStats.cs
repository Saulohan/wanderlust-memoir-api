namespace WanderlustMemoir.Domain.ValueObjects;

public class TravelStats
{
    public int TotalDestinations { get; private set; }
    public int VisitedPlaces { get; private set; }
    public int TotalPhotos { get; private set; }
    public int CountriesExplored { get; private set; }
    public int PlannedDestinations => TotalDestinations - VisitedPlaces;

    public TravelStats(int totalDestinations, int visitedPlaces, int totalPhotos, int countriesExplored)
    {
        TotalDestinations = totalDestinations;
        VisitedPlaces = visitedPlaces;
        TotalPhotos = totalPhotos;
        CountriesExplored = countriesExplored;
    }
}