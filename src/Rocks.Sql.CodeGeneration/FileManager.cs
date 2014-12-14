using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.TextTemplating;

namespace Rocks.Sql.CodeGeneration
{
	public class FileManager
	{
		#region Private fields

		private readonly ITextTemplatingEngineHost host;
		private readonly StringBuilder currentOutput;
		private readonly DTE dte;
		private readonly string templateFileName;
		private readonly IList<GeneratedFile> files;
		private readonly Action<string> checkOutAction;
		private readonly Action<ICollection<String>> projectSyncAction;

		private GeneratedFile generatedFile;

		#endregion

		#region Construct

		public FileManager (ITextTemplatingEngineHost host, StringBuilder generationEnvironment)
		{
			if (host == null)
				throw new ArgumentNullException ("host");
			if (generationEnvironment == null)
				throw new ArgumentNullException ("generationEnvironment");

			this.host = host;
			this.currentOutput = generationEnvironment;
			this.dte = (DTE) ((IServiceProvider) host).GetService (typeof (DTE));

			this.files = new List<GeneratedFile> ();
			this.checkOutAction = fileName => this.dte.SourceControl.CheckOutItem (fileName);

			this.templateFileName = host.TemplateFile;
			this.projectSyncAction = this.ProjectSync;
		}

		#endregion

		#region Public methods

		public void StartNewFile (string fileName)
		{
			if (string.IsNullOrEmpty (fileName))
				throw new ArgumentNullException ("fileName");

			this.EndFile ();

			this.generatedFile = new GeneratedFile
			                     {
				                     FileName = fileName
			                     };
		}


		public void EndFile ()
		{
			if (this.generatedFile == null)
				return;

			this.generatedFile.Content = this.currentOutput.ToString ();
			this.currentOutput.Length = 0;

			this.files.Add (this.generatedFile);

			this.generatedFile = null;
		}


		public void WriteAllFiles ()
		{
			this.EndFile ();

			var generated_file_paths = new List<string> ();
			var output_directory = Path.GetDirectoryName (this.host.TemplateFile) ?? string.Empty;

			foreach (var file in this.files)
			{
				if (file.IsEmpty)
					continue;

				var path = Path.Combine (output_directory, file.FileName);

				this.CheckoutFileIfRequired (path);

				// ReSharper disable once AssignNullToNotNullAttribute
				Directory.CreateDirectory (Path.GetDirectoryName (path));

				File.WriteAllText (path, file.Content.Trim ());

				generated_file_paths.Add (path);
			}

			this.projectSyncAction.EndInvoke (this.projectSyncAction.BeginInvoke (generated_file_paths, null, null));
		}

		#endregion

		#region Private methods

		private void CheckoutFileIfRequired (string fileName)
		{
			if (this.dte.SourceControl == null ||
			    !this.dte.SourceControl.IsItemUnderSCC (fileName) ||
			    this.dte.SourceControl.IsItemCheckedOut (fileName))
				return;

			// run on worker thread to prevent T4 calling back into VS
			this.checkOutAction.EndInvoke (this.checkOutAction.BeginInvoke (fileName, null, null));
		}


		private void ProjectSync (ICollection<string> keepFileNames)
		{
			var templateProjectItem = this.dte.Solution.FindProjectItem (this.templateFileName);

			var keep_file_names = new HashSet<string> (keepFileNames, StringComparer.OrdinalIgnoreCase);
			var project_files = new Dictionary<string, ProjectItem> ();
			var file_name_without_extension = Path.GetFileNameWithoutExtension (templateProjectItem.FileNames[0]);

			var project_items = templateProjectItem.ProjectItems.Cast<ProjectItem> ().ToList ();

			foreach (var project_item in project_items)
				project_files.Add (project_item.FileNames[0], project_item);

			// Remove unused items from the project
			foreach (var pair in project_files)
			{
				if (!keepFileNames.Contains (pair.Key) &&
				    !(Path.GetFileNameWithoutExtension (pair.Key) + ".").StartsWith (file_name_without_extension + "."))
					pair.Value.Delete ();
			}

			// Add missing files to the project
			foreach (var fileName in keep_file_names)
			{
				if (!project_files.ContainsKey (fileName))
					templateProjectItem.ProjectItems.AddFromFile (fileName);
			}
		}

		#endregion

		#region Nested type: GeneratedFile

		public class GeneratedFile
		{
			#region Public properties

			public string FileName { get; set; }
			public string Content { get; set; }

			public bool IsEmpty { get { return string.IsNullOrEmpty (this.Content); } }

			#endregion

			#region Public methods

			public override string ToString ()
			{
				return this.FileName + (this.IsEmpty ? " (empty)" : string.Empty);
			}

			#endregion
		}

		#endregion
	}
}