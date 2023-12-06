namespace Coreplus.Sample.Api.Types;

public class AnalysisReport
{
	public long practitioner_id { get; set; }
	public string name { get; set; }
	public int month { get; set; }
	public int TotalAppointments { get; set; }
	public decimal Totalduration { get; set; }
	public decimal Totalrevenue { get; set; }
	public decimal Totalcost { get; set; }
}

//public record Appointment(long id, DateTime date, string client_name, string appointment_type, int duration, decimal revenue, decimal cost, int practitioner_id);