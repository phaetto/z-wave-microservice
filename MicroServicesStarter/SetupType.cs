namespace MicroServicesStarter
{
    public enum SetupType
    {
        /// <summary>
        /// Install services to local admin and attach debugger
        /// </summary>
        Debug,

        /// <summary>
        /// Install services in local admin for testing
        /// </summary>
        Release,

        /// <summary>
        /// Install / update to centralized admin for integration tests and multi-service testing
        /// </summary>
        IntegrationTest,

        /// <summary>
        /// Deploy services to remote admin, update or nuget server (does not start services)
        /// </summary>
        Deploy
    }
}
