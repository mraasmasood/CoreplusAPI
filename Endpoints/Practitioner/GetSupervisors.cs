using Coreplus.Sample.Api.Services;
using Microsoft.AspNetCore.Http;

namespace Coreplus.Sample.Api.Endpoints.Practitioner;

public static class GetSupervisors
{
	public static RouteGroupBuilder MapGetSupervisorPractitioners(this RouteGroupBuilder group)
	{
		group.MapGet("/supervisors/{pusertype}/", async (HttpContext context, PractitionerService practitionerService) =>
		{
			int pusertype = int.Parse(context.Request.RouteValues["pusertype"].ToString());
			var practitioners = await practitionerService.GetSupervisorPractitioners(pusertype);
			return Results.Ok(practitioners);
		});

		return group;
	}
}