using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using ReaLTaiizor.Forms;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Forms;

namespace Code_Editor
{
    public partial class Form1 : ReaLTaiizor.Forms.LostForm
    {
        public Form1()
        {
            InitializeComponent();
            InitializeWebView();
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            InitializeAutoSaveTimer();
        }

        private CustomStack openFiles = new CustomStack();
        private CustomQueue snapshotQueue = new CustomQueue();
        private Dictionary<string, Button> fileButtons = new Dictionary<string, Button>();
        private Dictionary<string, string> filePaths = new Dictionary<string, string>();
        private string currentFile = "";
        private Button activeButton = null;
        private System.Windows.Forms.Timer autoSaveTimer;
        private bool isViewingSnapshot = false;

        private void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileName(ofd.FileName);
                string content = File.ReadAllText(ofd.FileName);

                if (!openFiles.ContainsKey(fileName))
                {
                    openFiles.Set(fileName, content);
                    filePaths[fileName] = ofd.FileName;

                    Button fileBtn = new Button();
                    fileBtn.Text = fileName;
                    fileBtn.Dock = DockStyle.Top;
                    fileBtn.Padding = new Padding(10, 5, 10, 5);
                    fileBtn.Height = 35;
                    fileBtn.FlatStyle = FlatStyle.Flat;
                    fileBtn.FlatAppearance.BorderSize = 0;
                    fileBtn.BackColor = ColorTranslator.FromHtml("#2d2d30");
                    fileBtn.ForeColor = Color.White;
                    fileBtn.TextAlign = ContentAlignment.MiddleLeft;
                    fileBtn.Click += (s, e) => SwitchToFile(fileName);

                    fileButtons[fileName] = fileBtn;
                    splitContainer1.Panel1.Controls.Add(fileBtn);
                }

                SwitchToFile(fileName);
            }
        }

        private async void SwitchToFile(string fileName)
        {
            // Save current editor content before switching if not viewing snapshot
            if (!string.IsNullOrEmpty(currentFile) && !isViewingSnapshot)
            {
                try
                {
                    string code = await editor.ExecuteScriptAsync("getEditorCode();");
                    string editorCode = JsonSerializer.Deserialize<string>(code);
                    openFiles.Set(currentFile, editorCode);
                }
                catch { }
            }

            isViewingSnapshot = false;
            this.Text = "Code Editor";

            if (activeButton != null)
            {
                activeButton.BackColor = ColorTranslator.FromHtml("#2d2d30");
                activeButton.ForeColor = Color.White;
            }

            if (fileButtons.ContainsKey(fileName))
            {
                activeButton = fileButtons[fileName];
                activeButton.BackColor = ColorTranslator.FromHtml("#007ACC");
                activeButton.ForeColor = Color.White;
            }

            currentFile = fileName;

            string content = openFiles.Get(fileName);
            string language = GetLanguageFromExtension(fileName);
            string escapedCode = JsonSerializer.Serialize(content);
            await editor.ExecuteScriptAsync($"setEditorCode({escapedCode}, '{language}');");
        }

        private string GetLanguageFromExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".cpp" => "cpp",
                ".js" => "javascript",
                ".ts" => "typescript",
                ".py" => "python",
                _ => "plaintext"
            };
        }

        private async void InitializeWebView()
        {
            await editor.EnsureCoreWebView2Async();
            string htmlPath = Path.Combine(Application.StartupPath, "index.html");
            editor.Source = new Uri(htmlPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            splitContainer1.BackColor = ColorTranslator.FromHtml("#202020");
            splitContainer2.BackColor = ColorTranslator.FromHtml("#202020");
        }

        private async void run_code_Click(object sender, EventArgs e)
        {
            try
            {
                await SaveCurrentFile();

                string fileExtension = Path.GetExtension(currentFile).ToLower();

                if (fileExtension == ".cpp")
                {
                    string cppCode = openFiles.Get(currentFile);
                    string output = await CompileCppWithPowerShell(cppCode);
                    code_output.Text = output;
                }
                else
                {
                    string outputJson = await editor.ExecuteScriptAsync("executeCode();");
                    string output = JsonSerializer.Deserialize<string>(outputJson);
                    code_output.Text = output;
                }
            }
            catch (Exception ex)
            {
                code_output.Text = "Exception: " + ex.Message;
            }
        }

        private async Task<string> CompileCppWithPowerShell(string cppCode)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "CppCodeRunner");
            Directory.CreateDirectory(tempDir);

            string sourceFile = Path.Combine(tempDir, "temp.cpp");
            string exeFile = Path.Combine(tempDir, "temp.exe");

            try
            {
                File.WriteAllText(sourceFile, cppCode);

                var compileProcess = new ProcessStartInfo
                {
                    FileName = @"C:\msys64\msys2_shell.cmd",
                    Arguments = $"-mingw64 -defterm -no-start -here -c \"g++ '{sourceFile}' -o '{exeFile}' -static -static-libgcc -static-libstdc++ -O2 -Wall\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                string compileOutput = "";
                string compileErrors = "";
                using (Process p = Process.Start(compileProcess))
                {
                    compileOutput = await p.StandardOutput.ReadToEndAsync();
                    compileErrors = await p.StandardError.ReadToEndAsync();
                    await p.WaitForExitAsync();
                }

                await Task.Delay(500);

                if (!File.Exists(exeFile))
                {
                    return "❌ Compilation Failed:\n" + compileErrors + "\n" + compileOutput;
                }

                var runProcess = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/k \"{exeFile}\" && pause && exit",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                Process.Start(runProcess);

                return "✔ Compilation Successful!\n\nProgram is running in a new terminal window...";
            }
            catch (Exception ex)
            {
                return "❌ Error: " + ex.Message;
            }
            finally
            {
                try
                {
                    if (File.Exists(sourceFile)) File.Delete(sourceFile);
                }
                catch { }
            }
        }

        private async Task SaveCurrentFile()
        {
            if (string.IsNullOrEmpty(currentFile))
                return;

            try
            {
                string code = await editor.ExecuteScriptAsync("getEditorCode();");
                string editorCode = JsonSerializer.Deserialize<string>(code);
                openFiles.Set(currentFile, editorCode);
                isViewingSnapshot = false;
            }
            catch (Exception ex)
            {
                code_output.Text = "Save Error: " + ex.Message;
            }
        }

        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                await SaveCurrentFile();
                MessageBox.Show("File saved!");
            }

            if (e.Control && e.KeyCode == Keys.O)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                OpenFile();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await SaveCurrentFile();
            MessageBox.Show("File saved!");
        }

        private void InitializeAutoSaveTimer()
        {
            autoSaveTimer = new System.Windows.Forms.Timer();
            autoSaveTimer.Interval = 8000;
            autoSaveTimer.Tick += async (s, e) => await CaptureSnapshot();
            autoSaveTimer.Start();
        }

        private async Task CaptureSnapshot()
        {
            if (string.IsNullOrEmpty(currentFile) || isViewingSnapshot)
                return;

            try
            {
                string code = await editor.ExecuteScriptAsync("getEditorCode();");
                string editorCode = JsonSerializer.Deserialize<string>(code);

                snapshotQueue.Enqueue(new CustomQueue.QueueElement
                {
                    fileName = currentFile,
                    fileContent = editorCode,
                    timestamp = DateTime.Now,
                    filePath = filePaths.ContainsKey(currentFile) ? filePaths[currentFile] : ""
                });

                UpdateSnapshotListBox();
            }
            catch { }
        }

        private void UpdateSnapshotListBox()
        {
            code_queue.Items.Clear();
            var snapshots = snapshotQueue.GetAll();

            foreach (var snapshot in snapshots)
            {
                string displayText = $"{snapshot.fileName} - {snapshot.timestamp:HH:mm:ss}";
                code_queue.Items.Add(displayText);
            }
        }

        private async void code_queue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (code_queue.SelectedIndex < 0)
                return;


            // Save current editor content before viewing snapshot
            if (!string.IsNullOrEmpty(currentFile) && !isViewingSnapshot)
            {
                try
                {
                    string currentCode = await editor.ExecuteScriptAsync("getEditorCode();");
                    string currentEditorCode = JsonSerializer.Deserialize<string>(currentCode);
                    openFiles.Set(currentFile, currentEditorCode);
                }
                catch { }
            }

            var snapshots = snapshotQueue.GetAll();
            if (code_queue.SelectedIndex < snapshots.Count)
            {
                var snapshot = snapshots[code_queue.SelectedIndex];
                string language = GetLanguageFromExtension(snapshot.fileName);
                string escapedCode = JsonSerializer.Serialize(snapshot.fileContent);

                await editor.ExecuteScriptAsync($"setEditorCode({escapedCode}, '{language}');");

                isViewingSnapshot = true;
                this.Text = $"Code Editor - Snapshot: {snapshot.fileName} ({snapshot.timestamp:HH:mm:ss})";
            }
        }
    }
}