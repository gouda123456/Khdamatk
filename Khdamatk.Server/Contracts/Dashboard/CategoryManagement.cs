namespace Khdamatk.Server.Contracts.Dashboard
{
    public class CategoryManagement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public bool IsActive { get; set; }

        public int ServiceCount { get; set; }
    }
}
