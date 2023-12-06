using System.Text.Json;
using Coreplus.Sample.Api.Types;
using System.Globalization;

namespace Coreplus.Sample.Api.Services;

public record PractitionerDto(long id, string name);
public record AppointmentDto(long id, DateTime date, string client_name, string appointment_type, int duration, decimal revenue, decimal cost, int practitioner_id);

public class PractitionerService
{
	public async Task<IEnumerable<PractitionerDto>> GetPractitioners()
	{
		using var fileStream = File.OpenRead(@"./Data/practitioners.json");
		var data = await JsonSerializer.DeserializeAsync<Practitioner[]>(fileStream);
		if (data == null)
		{
			throw new Exception("Data read error");
		}

		return data.Select(prac => new PractitionerDto(prac.id, prac.name));
	}

	public async Task<IEnumerable<PractitionerDto>> GetSupervisorPractitioners(int pusertype)
	{
		using var fileStream = File.OpenRead(@"./Data/practitioners.json");
		var data = await JsonSerializer.DeserializeAsync<Practitioner[]>(fileStream);
		if (data == null)
		{
			throw new Exception("Data read error");
		}
		if(pusertype == (int)PUserType.OWNERorADMIN)
		{
			return data.Where(practitioner => (int)practitioner.level == 0 || (int)practitioner.level == 1).Select(prac => new PractitionerDto(prac.id, prac.name));
		}
		else
		{
			return data.Where(practitioner => (int)practitioner.level >= 2).Select(prac => new PractitionerDto(prac.id, prac.name));
		}
	}

	public async Task<IEnumerable<AnalysisReport>> GetAnalysisReport(long? Id, DateTime startdate, DateTime enddate)
	{
		List<PractitionerDto> PList = new();
		List<AppointmentDto> AList = new();
		List<AnalysisReport> FinalResult = new();
		AnalysisReport ard;

		using var PfileStream = File.OpenRead(@"./Data/practitioners.json");
		var Pdata = await JsonSerializer.DeserializeAsync<Practitioner[]>(PfileStream);

		PList = Pdata.Select(prac => new PractitionerDto(prac.id, prac.name)).OrderBy(a => a.id).ToList();

		using var AfileStream = File.OpenRead(@"./Data/appointments.json");
		var Adata = await JsonSerializer.DeserializeAsync<Appointment[]>(AfileStream);
		if (Adata == null)
		{
			throw new Exception("Data read error");
		}

		AList = Adata.Where(app => (int)app.practitioner_id == (Id != null ? Id : app.practitioner_id) && (DateTime.ParseExact(app.date, "M/d/yyyy", CultureInfo.InvariantCulture) >= startdate && DateTime.ParseExact(app.date, "M/d/yyyy", CultureInfo.InvariantCulture) <= enddate)).Select(prac => new AppointmentDto(prac.id, DateTime.ParseExact(prac.date, "M/d/yyyy", CultureInfo.InvariantCulture), prac.client_name, prac.appointment_type, prac.duration, prac.revenue, prac.cost, prac.practitioner_id)).ToList();

		foreach (var p in PList)
		{
			if (AList.Any(a => a.practitioner_id == p.id))
			{
				var mdata = AList
						.Where(a => a.practitioner_id == p.id)
						.Select(prac => new AppointmentDto(
								prac.id,
								prac.date,
								prac.client_name,
								prac.appointment_type,
								prac.duration,
								prac.revenue,
								prac.cost,
								prac.practitioner_id
						))
						.GroupBy(a => a.date.Month)  // Group by month, adjust if needed
						.Select(group => new
						{
							Month = group.Key,
							Appointments = group.ToList()
						})
						.ToList();

				foreach (var m in mdata)
				{
					ard = new AnalysisReport();
					ard.practitioner_id = p.id;
					ard.name = p.name;
					ard.month = m.Month;
					ard.TotalAppointments = m.Appointments.Count(); 
					ard.Totalcost = m.Appointments.Sum(a => a.cost);
					ard.Totalduration = m.Appointments.Sum(a => a.duration);
					ard.Totalrevenue = m.Appointments.Sum(a => a.revenue);

					FinalResult.Add(ard);
				}
			}
		}

		return FinalResult;
	}

	public async Task<IEnumerable<AppointmentDto>> GetAppointmentList(long? Id, DateTime startdate, DateTime enddate)
	{
		using var fileStream = File.OpenRead(@"./Data/appointments.json");
		var data = await JsonSerializer.DeserializeAsync<Appointment[]>(fileStream);
		if (data == null)
		{
			throw new Exception("Data read error");
		}

		return data.Where(app => (int)app.practitioner_id == Id && (DateTime.ParseExact(app.date, "M/d/yyyy", CultureInfo.InvariantCulture) >= startdate && DateTime.ParseExact(app.date, "M/d/yyyy", CultureInfo.InvariantCulture) <= enddate)).Select(prac => new AppointmentDto(prac.id, DateTime.ParseExact(prac.date, "M/d/yyyy", CultureInfo.InvariantCulture), prac.client_name, prac.appointment_type, prac.duration, prac.revenue, prac.cost, prac.practitioner_id));
	}
}