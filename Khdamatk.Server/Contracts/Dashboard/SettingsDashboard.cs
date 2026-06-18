using System.Numerics;

namespace Khdamatk.Server.Contracts.Dashboard

{
    public class Settings                         
    {
        public int Id { get; set; }

        public bool SiteName { get; set; }

        public bool ContactEmail { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool TwoFactorAuthentication { get; set; }

        public bool PasswordPolicyEnabled { get; set; }

        public bool MaintenanceMode { get; set; }

        public bool EmailNotifications { get; set; }

        public bool SmsNotifications { get; set; }
    }
}
