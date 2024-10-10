using System.Net;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace ApiGateWay;

public class ServiceRegistry
{
    public static Dictionary<string, List<RegisterServiceModel>> _services = [];

    public static void AddService(RegisterServiceModel model)
    {
        if (_services.TryGetValue(model.Name, out var service))
        {
            _services[model.Name].Add(model);
        }
        else
        {
            var list = new List<RegisterServiceModel> { model };
            _services.Add(model.Name, list);
        }
    }

    public static RegisterServiceModel GetHealthyServices(string serviceName)
    {
        _services.TryGetValue(serviceName, out var services);

        if (services == null) throw new Exception("service not found");
        foreach (var service in services)
        {
            var checkedHealthService = CheckHealthCheck(service.Url);
            if (checkedHealthService.Status == HealthStatus.Healthy)
            {
                service.IsHealthy = true;
                return service;
            }
        }

        throw new Exception("service not found");
    }


    public static HealthCheckResult CheckHealthCheck(string serviceUrl)
    {

        var client = new RestClient($"{serviceUrl}/hc");
        var request = new RestRequest();
        var response = client.Get(request);
        return response.StatusCode == HttpStatusCode.OK
            ? response.Content != null && response.Content.Contains("Healthy")
                ? new HealthCheckResult(
                    status: HealthStatus.Healthy,
                    description: "Healthy"
                )
                : new HealthCheckResult(
                    status: HealthStatus.Unhealthy,
                    description: "Unhealthy"
                )
            : new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description: "server is Unreachable statusCode is not 200"
            );
    }

}