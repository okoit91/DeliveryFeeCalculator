using App.Contracts.BLL;
using App.Contracts.BLL.Services;

namespace App.BLL.Services
{
    public class DeliveryFeeService : IDeliveryFeeService
    {
        private readonly IAppBLL _bll;

        public DeliveryFeeService(IAppBLL bll)
        {
            _bll = bll;
        }

        public async Task<(decimal ExtraFee, string Error)> CalculateExtraFee(Guid cityId, Guid vehicleTypeId)
        {
            var latestWeather = await _bll.Weathers.GetLatestWeatherByCityAsync(cityId);
            if (latestWeather == null)
            {
                return (0, "No weather data available for this city.");
            }

            decimal extraFee = 0;
            string? weatherCondition = latestWeather.WeatherCondition?.ToLower();

            if (!string.IsNullOrEmpty(weatherCondition) &&
                (weatherCondition.Contains("glaze") || weatherCondition.Contains("hail") || weatherCondition.Contains("thunder")))
            {
                if (await IsVehicleRestricted(vehicleTypeId))
                {
                    return (0, "Usage of selected vehicle type is forbidden due to extreme weather conditions.");
                }
            }

            var extraFeeRules = await _bll.ExtraFees.GetAllForVehicleAsync(vehicleTypeId);

            foreach (var rule in extraFeeRules)
            {
                if (MatchesCondition(rule.ConditionType, latestWeather, rule))
                {
                    extraFee += rule.FeeAmount;
                }

                if (rule.ConditionType.ToLower() == "windspeed" && latestWeather.WindSpeed > 20)
                {
                    if (await IsVehicleRestricted(vehicleTypeId))
                    {
                        return (0, "Usage of selected vehicle type is forbidden due to extreme wind conditions.");
                    }
                }
            }

            return (extraFee, null);
        }

        public async Task<bool> IsVehicleRestricted(Guid vehicleTypeId)
        {
            var vehicle = await _bll.VehicleTypes.FirstOrDefaultAsync(vehicleTypeId);
            if (vehicle == null) return false;

            var name = vehicle.Name.ToLower();
            return name == "bike" || name == "scooter";
        }

        private bool MatchesCondition(string conditionType, dynamic weather, dynamic rule)
        {
            var condition = conditionType.ToLower();
            var weatherCondition = weather.WeatherCondition?.ToLower();

            switch (condition)
            {
                case "temperature":
                    bool tempInRange = weather.AirTemperature >= rule.MinValue &&
                                       weather.AirTemperature <= rule.MaxValue;
                    return tempInRange;

                case "windspeed":
                    bool windInRange = weather.WindSpeed >= rule.MinValue &&
                                       weather.WindSpeed <= rule.MaxValue;
                    return windInRange;

                case "snow":
                case "sleet":
                    if (!string.IsNullOrEmpty(weatherCondition))
                    {
                        return weatherCondition?.Contains(condition);
                    }
                    return false;

                case "rain":
                    if (!string.IsNullOrEmpty(weatherCondition))
                    {
                        return weatherCondition?.Contains("rain");
                    }
                    return false;

                default:
                    return false;
            }
        }
    }
}
