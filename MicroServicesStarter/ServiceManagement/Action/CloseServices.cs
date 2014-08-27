namespace MicroServicesStarter.ServiceManagement.Action
{
    using System;
    using Chains;
    using Chains.Play.Web;
    using Services.Communication.Protocol;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Update;
    using Services.Management.Administration.Worker;

    public sealed class CloseServices : IChainableAction<AdminSetupContext, AdminSetupContext>
    {
        public AdminSetupContext Act(AdminSetupContext context)
        {
            using (
                var adminConnection =
                    new Client(StartAdmin.LocalAdminHost, StartAdmin.LocalAdminPort).Do(new WaitUntilClientConnects()))
            {
                var reports = adminConnection.Do(new Send<GetReportedDataReturnData>(new GetReportedData())).Reports;

                foreach (var report in reports)
                {
                    context.LogToUi(string.Format("Housekeeping on service {0}...", report.Key));

                    if (report.Value.WorkerState == WorkUnitState.Running)
                    {
                        adminConnection.Do(new Send(new StopWorkerProcess(report.Key)));
                    }
                }

                foreach (var report in reports)
                {
                    if (report.Value.WorkerState == WorkUnitState.Running)
                    {
                        context.LogToUi(
                            string.Format(
                                "Stopping running service {0} with id {1}",
                                report.Value.StartData.ServiceName,
                                report.Key));

                        adminConnection.Do(new WaitUntilServiceIsDown(report.Key));
                    }

                    context.LogToUi(string.Format("Deleting service repo {0} with id {1}", report.Value.StartData.ServiceName, report.Key));

                    try
                    {
                        adminConnection.Do(new Send(new DeleteWorkerProcessEntry(report.Key)));
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return context;
        }
    }
}
