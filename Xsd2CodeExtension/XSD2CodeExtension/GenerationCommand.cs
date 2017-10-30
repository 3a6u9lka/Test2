//------------------------------------------------------------------------------
// <copyright file="GenerationCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Xsd2Code.Library;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Constants = EnvDTE.Constants;

//public static readonly Guid CommandSet = new Guid("5a46f7be-ac6c-4b52-b8f3-c75ff2d8342e");
namespace XSD2CodeExtension
{
    /// <summary>
    /// Command handler
    /// </summary>

    internal sealed class GenerationCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("5a46f7be-ac6c-4b52-b8f3-c75ff2d8342e");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerationCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private GenerationCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            _package = package;

            var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService == null)
                return;
            //var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuCommandId = new CommandID(CommandSet, CommandId);
            //var menuItem = new MenuCommand(this.ExecGenerationCommand, menuCommandID);
            var menuItem = new OleMenuCommand(ExecGenerationCommand, menuCommandId);

            menuItem.BeforeQueryStatus += MenuItemBeforeQueryStatus;

            commandService.AddCommand(menuItem);
        }


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerationCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return _package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new GenerationCommand(package);
        }

        private void ExecGenerationCommand(object sender, EventArgs e)
        {
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            var uIh = dte.ToolWindows.SolutionExplorer;

            var proitem = uIh.DTE.SelectedItems.Item(1).ProjectItem;
            var proj = proitem.ContainingProject;

            var fileName = proitem.FileNames[0];
            try
            {
                proitem.Save(fileName);
            }
            catch (Exception)
            {
                // ignored
            }


            string defaultNamespace;
            uint? targetFramework = 262144;
            bool? isSilverlightApp = false;

            try
            {
                defaultNamespace = proj.Properties.Item("DefaultNamespace").Value as string;
                targetFramework = proj.Properties.Item("TargetFramework").Value as uint?;
                isSilverlightApp = proj.Properties.Item("SilverlightProject.IsSilverlightApplication").Value as bool?;
            }
            catch
            {
                // Try to get default nameSpace
                defaultNamespace = GetNamespaceByPath(fileName, proj);
            }

            var framework = TargetFramework.Net40;
            if (targetFramework.HasValue)
            {
                var target = targetFramework.Value;
                switch (target)
                {
                    case 196608:
                        framework = TargetFramework.Net30;
                        break;
                    case 196613:
                        framework = TargetFramework.Net35;
                        break;
                    case 262144:
                        framework = TargetFramework.Net40;
                        break;
                }
            }
            if (isSilverlightApp.HasValue)
            {
                if (isSilverlightApp.Value)
                {
                    framework = TargetFramework.Silverlight;
                }
            }

            var frm = new FormOption();
            frm.Init(fileName, proj.CodeModel.Language, defaultNamespace, framework);

            var result = frm.ShowDialog();

            var generatorParams = frm.GeneratorParams.Clone();
            generatorParams.InputFilePath = fileName;

            var gen = new GeneratorFacade(generatorParams);

            // Close file if open in IDE
            ProjectItem projElmts;
            var found = FindInProject(proj.ProjectItems, gen.GeneratorParams.OutputFilePath, out projElmts);
            if (found)
            {
                var window = projElmts.Open(Constants.vsViewKindCode);
                window.Close();
            }

            if (fileName.Length > 0)
            {
                if (result == DialogResult.OK)
                {
                    var generateResult = gen.Generate();
                    var outputFileName = generateResult.Entity;

                    if (!generateResult.Success)
                        MessageBox.Show(generateResult.Messages.ToString(), "XSD2Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        if (!found)
                        {
                            projElmts = proitem.Collection.AddFromFile(outputFileName);
                        }

                        ProjectItem srcFile;
                        FindInProject(proj.ProjectItems, gen.GeneratorParams.InputFilePath, out srcFile);

                        srcFile.ProjectItems.AddFromFile(gen.GeneratorParams.OutputFilePath);

                        if (frm.OpenAfterGeneration)
                        {
                            var window = projElmts.Open(Constants.vsViewKindCode);
                            window.Activate();
                            window.SetFocus();

                            try
                            {
                                //DEPRECATED: this.applicationObjectField.DTE.ExecuteCommand("Edit.RemoveAndSort", "");
                                dte.DTE.ExecuteCommand("Edit.FormatDocument", string.Empty);
                            }
                            catch (Exception)
                            {
                                // ignored
                            }
                        }
                    }
                }
            }
        }

        private string GetNamespaceByPath(string fileName, Project proj)
        {
            //get relative path to xsd file, replace separator with dot
            var fileNameUri = new Uri(fileName);
            var projectUri = new Uri(Path.GetDirectoryName(proj.FullName));
            var relativeUri = projectUri.MakeRelativeUri(fileNameUri);
            relativeUri = new Uri(Path.GetDirectoryName("c:/" + relativeUri));

            var fileNamespace = "";
            for (var i = 2; i < relativeUri.Segments.Length; i++)
            {
                var segment = relativeUri.Segments[i];

                if (i != relativeUri.Segments.Length - 1)
                {
                    fileNamespace += segment.Remove(segment.Length - 1);
                    fileNamespace += ".";
                }
                else fileNamespace += segment;
            }
            return fileNamespace;
        }

        private bool FindInProject(ProjectItems projectItems, string filename, out ProjectItem item)
        {
            item = null;
            if (projectItems == null)
                return false;

            foreach (ProjectItem projElmts in projectItems)
            {
                if (projElmts.FileNames[0] == filename)
                {
                    item = projElmts;
                    return true;
                }

                if (FindInProject(projElmts.ProjectItems, filename, out item))
                    return true;
            }

            return false;
        }

        private void MenuItemBeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;

            if (menuCommand != null)
            {
                menuCommand.Visible = false;
                menuCommand.Enabled = false;

                IVsHierarchy hierarchy;
                var itemId = VSConstants.VSITEMID_NIL;

                if (!IsSingleProjectItemSelected(out hierarchy, out itemId))
                    return;

                string fullItemPath;
                (hierarchy as IVsProject).GetMkDocument(itemId, out fullItemPath);
                var fileInfo = new FileInfo(fullItemPath);

                var isXsdFile = string.Compare(fileInfo.Extension.ToLower(), ".xsd", StringComparison.Ordinal) == 0;

                if (!isXsdFile)
                    return;

                menuCommand.Visible = true;
                menuCommand.Enabled = true;

            }
        }
        private bool IsSingleProjectItemSelected(out IVsHierarchy hierarchy, out uint itemid)
        {
            hierarchy = null;
            itemid = VSConstants.VSITEMID_NIL;

            var monitorSelection = Package.GetGlobalService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
            var solution = Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;
            if (monitorSelection == null || solution == null)
            {
                return false;
            }

            var hierarchyPtr = IntPtr.Zero;
            var selectionContainerPtr = IntPtr.Zero;

            try
            {
                IVsMultiItemSelect multiItemSelect;
                var hr = monitorSelection.GetCurrentSelection(out hierarchyPtr, out itemid, out multiItemSelect, out selectionContainerPtr);

                // there is no selection
                if (ErrorHandler.Failed(hr) || hierarchyPtr == IntPtr.Zero || itemid == VSConstants.VSITEMID_NIL)
                    return false;

                // multiple items are selected
                if (multiItemSelect != null)
                    return false;

                // there is a hierarchy root node selected, thus it is not a single item inside a project
                if (itemid == VSConstants.VSITEMID_ROOT)
                    return false;

                hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
                if (hierarchy == null)
                    return false;

                Guid guidProjectId;

                return !ErrorHandler.Failed(solution.GetGuidOfProject(hierarchy, out guidProjectId));

                // if we got this far then there is a single project item selected
            }
            finally
            {
                if (selectionContainerPtr != IntPtr.Zero)
                {
                    Marshal.Release(selectionContainerPtr);
                }

                if (hierarchyPtr != IntPtr.Zero)
                {
                    Marshal.Release(hierarchyPtr);
                }
            }
        }

    }
}