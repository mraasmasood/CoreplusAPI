namespace Coreplus.Sample.Api.Types;

public class Appointment
{
	public int id { get; set; }
	public string date { get; set; }
	public string client_name { get; set; }
	public string appointment_type { get; set; }
	public int duration { get; set; }
	public decimal revenue { get; set; }
	public decimal cost { get; set; }
	public int practitioner_id { get; set; }

}

//public record Appointment(long id, DateTime date, string client_name, string appointment_type, int duration, decimal revenue, decimal cost, int practitioner_id);