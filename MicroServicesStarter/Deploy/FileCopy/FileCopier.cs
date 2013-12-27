namespace MicroServicesStarter.Deploy.FileCopy
{
    using System;
    using System.IO;
    using System.Xml;

    public sealed class FileCopier
    {
        private readonly string fromFolder;

        private readonly string toFolder;

        private readonly XmlDocument xmlDocument;

        public FileCopier(string fromFolder, string toFolder, string xmlFile)
        {
            this.fromFolder = fromFolder;
            this.toFolder = toFolder;
            xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFile);
        }

        public void Execute()
        {
            var fileNodes = xmlDocument.SelectNodes(string.Format("/*/*"));

            foreach (XmlNode fileNode in fileNodes)
            {
                Execute(fileNode);
            }
        }

        private void Execute(XmlNode directiveNode)
        {
            if (directiveNode.Name.ToLowerInvariant() == "file")
            {
                if (directiveNode.Attributes["name"] != null)
                {
                    var targetFile = directiveNode.Attributes["to"] == null
                        ? directiveNode.Attributes["name"].Value
                        : directiveNode.Attributes["to"].Value;

                    var target = toFolder + targetFile;

                    Directory.CreateDirectory(Path.GetDirectoryName(target));

                    File.Copy(fromFolder + directiveNode.Attributes["name"].Value, target, true);
                }
                else if (directiveNode.Attributes["search"] != null)
                {
                    var searchOption = directiveNode.Attributes["recursive"] == null
                        || directiveNode.Attributes["recursive"].Value != "true"
                        ? SearchOption.TopDirectoryOnly
                        : SearchOption.AllDirectories;

                    var searchPath = Path.GetFullPath(fromFolder + Path.GetDirectoryName(directiveNode.Attributes["search"].Value));
                    var results = Directory.GetFiles(searchPath, Path.GetFileName(directiveNode.Attributes["search"].Value), searchOption);

                    if (searchPath[searchPath.Length - 1] == '\\')
                    {
                        searchPath = searchPath.Substring(0, searchPath.Length - 1);
                    }

                    var changedFolderToCopy = directiveNode.Attributes["to"] != null
                        ? Path.GetFullPath(toFolder + directiveNode.Attributes["to"].Value)
                        : toFolder;

                    foreach (var file in results)
                    {
                        var filename = Path.GetFileName(file);

                        var target = changedFolderToCopy + Path.GetDirectoryName(file).Substring(searchPath.Length) + @"\" + filename;

                        Directory.CreateDirectory(Path.GetDirectoryName(target));

                        File.Copy(file, target, true);
                    }
                }
            }
            else if (directiveNode.Name.ToLowerInvariant() == "folder")
            {
                if (directiveNode.Attributes["remove"] != null)
                {
                    var dirPath = Path.GetFullPath(toFolder + directiveNode.Attributes["remove"].Value);
                    Directory.Delete(dirPath, true);
                }
            }
            else
            {
                throw new NotSupportedException(string.Format("The directive '{0}' is not supported", directiveNode.Name));
            }
        }
    }
}
