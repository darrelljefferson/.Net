namespace PHEAAScheduleService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PHEAAScheduleService = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // PHEAAScheduleService
            // 
            this.PHEAAScheduleService.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PHEAAScheduleService.Password = null;
            this.PHEAAScheduleService.Username = null;
            this.PHEAAScheduleService.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.PHEAAScheduleService_AfterInstall);
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "PHEAAServices";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PHEAAScheduleService,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PHEAAScheduleService;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}