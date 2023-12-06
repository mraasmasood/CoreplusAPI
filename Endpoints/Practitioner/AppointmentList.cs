using Coreplus.Sample.Api.Services;
using Coreplus.Sample.Api.Types;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text;
using System.Globalization;

namespace Coreplus.Sample.Api.Endpoints.Practitioner;

public static class GetAppointmentList
{
	public static RouteGroupBuilder MapGetGetAppointmentList(this RouteGroupBuilder group)
	{
		group.MapPost("/getappointmentlist", async (HttpContext context, PractitionerService practitionerService) =>
		{
			using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
			{
				var requestBody = await reader.ReadToEndAsync();
				var parameters = JsonSerializer.Deserialize<AnalysisReportObject>(requestBody);
				var report = await practitionerService.GetAppointmentList(parameters.id, DateTime.ParseExact(parameters.startdate, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(parameters.enddate, "M/d/yyyy", CultureInfo.InvariantCulture));
				context.Response.StatusCode = 200;
				return Results.Ok(report);

			}
		});

		return group;
	}
}