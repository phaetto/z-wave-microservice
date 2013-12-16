namespace MicroServicesStarter.Deploy.Actions.Update
{
    using System;
    using Chains.Play;

    [Serializable]
    public sealed class PackageUploadData : SerializableSpecification
    {
        public string UpdateServerHostname;

        public int UpdateServerPort;

        public string YourApiKey;

        public string PackageFolder;

        public string PackageName;

        public string PackageVersionLabel;

        public override int DataStructureVersionNumber
        {
            get
            {
                return 1;
            }
        }
    }
}
