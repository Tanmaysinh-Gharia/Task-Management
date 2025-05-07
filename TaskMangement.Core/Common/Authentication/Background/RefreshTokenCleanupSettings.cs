namespace TaskManagement.Core.Common.Authentication.Background
{
    public class RefreshTokenCleanupSettings
    {

        public bool CleanupEnabled { get; set; } = true;
        public int CleanupIntervalMinutes { get; set; } = 60;
    }
}
