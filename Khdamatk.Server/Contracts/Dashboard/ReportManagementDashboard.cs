namespace Khdamatk.Server.Contracts.Dashboard
{
    public class ReportManagementDashboard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public string Icon { get; set; }
        public string Attachments { get; set; }
    }
}
