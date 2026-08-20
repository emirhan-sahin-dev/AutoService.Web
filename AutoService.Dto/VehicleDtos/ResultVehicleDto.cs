namespace AutoService.Dto.VehicleDtos;

public class ResultVehicleDto
{
    public int VehicleId { get; set; }

    public string Plate { get; set; } = null!;

    public string BrandName { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public string CustomerName { get; set; } = null!;
}