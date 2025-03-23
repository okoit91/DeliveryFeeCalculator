namespace App.Contracts.BLL.Services;

public interface IDeliveryFeeService
{
    Task<(decimal ExtraFee, string Error)> CalculateExtraFee(Guid cityId, Guid vehicleTypeId);
    Task<bool> IsVehicleRestricted(Guid vehicleTypeId);
}