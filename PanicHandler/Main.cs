using Microsoft.Win32;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PanicHandler
{
    public partial class PanicHandler : Form
    {
        private Core _core;

        public PanicHandler()
        {
            InitializeComponent();

            notifyIcon
                .ContextMenuStrip = new ContextMenuStrip();

            notifyIcon
                .ContextMenuStrip
                .Items
                .Add("Show configuration", null,
                    new EventHandler((s, e) =>
                    {
                        var appSettingsPath = Path.Combine(
                            Path.GetDirectoryName(Assembly
                                .GetExecutingAssembly()
                                .Location),
                            "appsettings.Production.json");

                        Process.Start("notepad.exe", appSettingsPath);
                    }));

            notifyIcon
                .ContextMenuStrip
                .Items
                .Add(CheckIfWakeUpAtStartup() ? "Start at startup (Actived)"
                    : "Start at startup (Deactived)",
                    null,
                    new EventHandler((s, e)
                        => StartupStripItemBehavior()));

            notifyIcon
                .ContextMenuStrip
                .Items
                .Add("About", null,
                    new EventHandler((s, e) => MessageBox.Show("Icon by: to https://icons8.com", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)));

            notifyIcon
                .ContextMenuStrip
                .Items
                .Add("Close", null,
                    new EventHandler((s, e) => Application.Exit()));

            notifyIcon.Visible = true;

            ShowInTaskbar = false;
        }

        private bool CheckIfWakeUpAtStartup()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

            string value = key.GetValue("PanicHandler", null) as string;

            return value != null;
        }

        private void AddApplicationToStartup()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("PanicHandler", "\"" + Directory.GetCurrentDirectory() + "\\PanicHandler.exe\"");
        }

        private void RemoveApplicationFromStartup()
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.DeleteValue("PanicHandler", false);
        }

        private void StartupStripItemBehavior()
        {
            var item = notifyIcon.ContextMenuStrip.Items[1];

            try
            {
                if (!CheckIfWakeUpAtStartup())
                {
                    AddApplicationToStartup();
                    item.Text = "Start at startup (Actived)";
                }
                else
                {
                    RemoveApplicationFromStartup();
                    item.Text = "Start at startup (Deactived)";
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Error during the procedure: cannot set application as startup applications: ");
                Log.Logger.Error(ex.Message);

                MessageBox.Show("Error during the procedure. See log for more informations.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PanicHandler_Shown(object sender, EventArgs e)
        {
            Hide();

            _core = new();

            _ = _core.Run();
        }
    }
}