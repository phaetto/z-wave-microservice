namespace MicroServicesStarter.ServiceManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Chains;

    public sealed class AdminSetupContext : Chain<AdminSetupContext>
    {
        public Action<string> LogMethod;

        public readonly SetupType SetupType;

        public readonly string SolutionDirectory;

        public readonly List<ProjectRegistration> Projects = new List<ProjectRegistration>();

        internal Process AdminProcess;

        public AdminSetupContext(Action<string> logMethod = null, SetupType setupType = SetupType.Debug)
        {
            LogMethod = logMethod;
            SetupType = setupType;

            SolutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
        }

        public AdminSetupContext LogToUi(string text)
        {
            if (LogMethod != null)
            {
                LogMethod(text);
            }

            return this;
        }
    }
}
