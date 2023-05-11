using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EstateView.Core.Model;
using EstateView.Utilities;
using EstateView.View.InstallmentSale;
using EstateView.ViewModel.InstallmentSale;
using Microsoft.Win32;

namespace EstateView.ViewModel
{
    public class MainWindowViewModel : ViewModel
    {
        private readonly ClientReportGenerator clientReportGenerator;

        public MainWindowViewModel()
        {
            this.GenerateClientLetterCommand = new RelayCommand(this.GenerateClientLetter, () => this.Workspace != null);
            this.NewCommand = new RelayCommand(() => this.NewWorkspace());
            this.SaveCommand = new RelayCommand(this.Save, () => this.Workspace != null);
            this.SaveAsCommand = new RelayCommand(this.SaveAs, () => this.Workspace != null);
            this.OpenCommand = new RelayCommand(this.Load);
            this.CloseCommand = new RelayCommand(this.CloseWorkspace, () => this.Workspace != null);
            this.ExitCommand = new RelayCommand(this.ExitApplication);
            this.SaveScreenshotCommand = new RelayCommand(this.SaveScreenshot, () => this.Workspace != null);
            this.InstallmentSaleCommand = new RelayCommand(this.ShowInstallmentSaleWindow); 
            this.HelpDocCommand = new RelayCommand(this.OpenHelpDoc);
            this.YouTubeCommand = new RelayCommand(this.GoToYouTube);
            this.AfrLookupCommand = new RelayCommand(this.LookupAFR);
            this.clientReportGenerator = new ClientReportGenerator();
            this.NewWorkspace();
        }
        private void LookupAFR()
        {
            UrlHelper.LookupAFR();
        }

        private void GoToYouTube()
        {
            UrlHelper.GoToYouTubePlayList();
        }

        private void OpenHelpDoc()
        {
            HelpDocHelper.OpenHelpDoc();
        }

        public WorkspaceViewModel Workspace
        {
            get { return this.GetValue(() => this.Workspace); }
            private set { this.SetValue(() => this.Workspace, value); }
        }

        public string WindowTitle
        {
            get { return (Path.GetFileNameWithoutExtension(this.CurrentFileName) ?? "New File") + " - EstateView Planning Software v" + VersionHelper.Version; }
        }

        private string CurrentFileName
        {
            get
            {
                return this.GetValue(() => this.CurrentFileName);
            }
            set
            {
                this.SetValue(() => this.CurrentFileName, value);
                this.NotifyPropertyChanged(() => this.WindowTitle);
            }
        }

        public ICommand NewCommand { get; private set; }

        public ICommand OpenCommand { get; private set; }

        public ICommand CloseCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand SaveAsCommand { get; private set; }

        public ICommand ExitCommand { get; private set; }

        public ICommand GenerateClientLetterCommand { get; private set; }

        public ICommand SaveScreenshotCommand { get; private set; }

        public ICommand InstallmentSaleCommand { get; private set; }

        public ICommand HelpDocCommand { get; private set; }

        public ICommand YouTubeCommand { get; private set; }

        public ICommand AfrLookupCommand { get; private set; }

        private void NewWorkspace(EstateProjectionOptions options = null)
        {
            this.CurrentFileName = null;
            options = options ?? EstateProjectionOptions.CreateEmptyOptions();
            this.Workspace = new WorkspaceViewModel(options);
        }

        private void GenerateClientLetter()
        {
            this.clientReportGenerator.GenerateClientLetter(this.Workspace.Scenarios.Last().Scenario);
        }

        private void ExitApplication()
        {
            Application.Current.MainWindow.Close();
        }

        private void Save()
        {
            if (this.CurrentFileName != null || this.PromptForFileName())
            {
                this.SaveWorkspace();
            }
        }

        private void CloseWorkspace()
        {
            this.Workspace = null;
        }

        private void SaveAs()
        {
            if (this.PromptForFileName())
            {
                this.SaveWorkspace();
            }
        }

        private void Load()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".EstateView";
            dialog.Filter = "EstateView Files (.EstateView)|*.EstateView";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                this.LoadFile(dialog.FileName);
            }
        }

        private void LoadFile(string fileName)
        {
            try
            {
                EstateProjectionOptions options = SaveLoadHelper.Load(fileName);
                this.CurrentFileName = fileName;
                this.Workspace = new WorkspaceViewModel(options);
            }
            catch (IOException e)
            {
                MessageBox.Show("Error loading file: " + e.Message, "Error");
            }
        }

        private bool PromptForFileName()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = this.Workspace.CurrentScenario.Options.Spouse1.LastName;
            dialog.DefaultExt = "EstateView";
            dialog.Filter = "EstateView Files (.EstateView)|*.EstateView";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                this.CurrentFileName = dialog.FileName;
                return true;
            }

            return false;
        }

        private void SaveWorkspace()
        {
            try
            {
                SaveLoadHelper.Save(this.Workspace.CurrentScenario.Options.Options, this.CurrentFileName);
            }
            catch (IOException e)
            {
                MessageBox.Show("Error saving file: " + e.Message, "Error");
            }
        }

        private void SaveScreenshot()
        {
            this.Workspace.SaveScreenshot();
        }

        private void ShowInstallmentSaleWindow()
        {
            Window window = new Window();
            window.Title = "Installment Sale Calculator";
            InstallmentSaleView view = new InstallmentSaleView();
            view.DataContext = new InstallmentSaleViewModel();
            window.Content = view;
            window.Show();
        }
    }
}