namespace WanderlustMemoir.Application.DTOs.TravelStats;

public class TravelStatsDto
{
    public int DreamDestinations { get; set; } // Destinos planejados (não visitados)
    public int VisitedPlaces { get; set; } // Lugares visitados
    public int SharedPhotos { get; set; } // Total de fotos compartilhadas
    public int ExploredCountries { get; set; } // Países explorados
}