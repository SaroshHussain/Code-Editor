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
        private readonly CodeCompilation _compiler = new();
        public Form1()
        {
            InitializeComponent();
            InitializeWebView();
            //this.KeyPreview = true;
            //this.KeyDown += Form1_KeyDown;
        }

        private CustomLinkedList openFiles = new CustomLinkedList();
        private CustomQueue snapshotQueue = new CustomQueue();
        private Dictionary<string, Button> fileButtons = new Dictionary<string, Button>();
        private string currentFile = "";
        private Button activeButton = null;
        private bool isViewingSnapshot = false;

        private void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileName(ofd.FileName);
                string filePath = ofd.FileName;

                if (!openFiles.ContainsKey(fileName))
                {
                    try
                    {
                        string content = FileManager.ReadFile(filePath);
                        openFiles.Set(fileName, content, filePath);

                        Panel filePanel = FileTabManager.CreateFileTab(
                            fileName,
                            SwitchToFile,
                            CloseFile
                        );

                        Button fileBtn = FileTabManager.GetFileButtonFromTab(filePanel);
                        fileButtons[fileName] = fileBtn;
                        splitContainer1.Panel1.Controls.Add(filePanel);
                    }
                    catch (Exception ex)
                    {
                        code_output.Text = $"Error opening file: {ex.Message}";
                        return;
                    }
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
                FileTabManager.SetTabInactive(activeButton);
            }

            if (fileButtons.ContainsKey(fileName))
            {
                activeButton = fileButtons[fileName];
                FileTabManager.SetTabActive(activeButton);
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
            
            editor.CoreWebView2.WebMessageReceived += async (s, e) =>
            {
                string json = e.WebMessageAsJson;
                if (json.Contains("\"action\":\"save\""))
                {
                    await SaveCurrentFile();
                    MessageBox.Show("File saved!");
                }
                else if (json.Contains("\"action\":\"open\""))
                {
                    OpenFile();
                }
            };
            
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
                    string output = await _compiler.CompileCppWithPowerShell(cppCode);
                    code_output.Text = output;
                }
                else if (fileExtension == ".py")
                {
                    string pythonCode = openFiles.Get(currentFile);
                    string output = await _compiler.ExecutePythonScript(pythonCode);
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


        private async Task SaveCurrentFile()
        {
            if (string.IsNullOrEmpty(currentFile))
                return;

            try
            {
                string code = await editor.ExecuteScriptAsync("getEditorCode();");
                string editorCode = JsonSerializer.Deserialize<string>(code);

                // Update in-memory storage
                openFiles.Set(currentFile, editorCode);

                // Get file path and save to disk
                var allFiles = openFiles.GetAll();
                var currentFileNode = allFiles.FirstOrDefault(f => f.fileName == currentFile);

                if (!string.IsNullOrEmpty(currentFileNode.filePath))
                {
                    FileManager.SaveFile(currentFileNode.filePath, editorCode);
                }

                isViewingSnapshot = false;
            }
            catch (Exception ex)
            {
                code_output.Text = "Save Error: " + ex.Message;
            }
        }

        //private async void Form1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.S)
        //    {
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //        await SaveCurrentFile();
        //        MessageBox.Show("File saved!");
        //    }

        //    if (e.Control && e.KeyCode == Keys.O)
        //    {
        //        e.Handled = true;
        //        e.SuppressKeyPress = true;
        //        OpenFile();
        //    }
        //}

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await SaveCurrentFile();
            MessageBox.Show("File saved!");
        }

        private async void saveToMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFile))
                return;

            try
            {
                string code = await editor.ExecuteScriptAsync("getEditorCode();");
                string editorCode = JsonSerializer.Deserialize<string>(code);

                // Get file path from CustomLinkedList
                var allFiles = openFiles.GetAll();
                var currentFileNode = allFiles.FirstOrDefault(f => f.fileName == currentFile);

                snapshotQueue.Enqueue(new CustomQueue.QueueElement
                {
                    fileName = currentFile,
                    fileContent = editorCode,
                    timestamp = DateTime.Now,
                    filePath = currentFileNode.filePath
                });

                UpdateSnapshotListBox();
                MessageBox.Show("Snapshot saved to memory!");
            }
            catch (Exception ex)
            {
                code_output.Text = $"Snapshot Error: {ex.Message}";
            }
        }

        private void UpdateSnapshotListBox()
        {
            code_queue.Items.Clear();
            var snapshots = snapshotQueue.GetAll();

            foreach (var snapshot in snapshots)
            {
                string displayText = $"{snapshot.fileName}.. saved: {snapshot.timestamp:HH:mm:ss}";
                code_queue.Items.Add(displayText);
            }
        }

        private async Task CaptureSnapshot()
        {
            if (string.IsNullOrEmpty(currentFile) || isViewingSnapshot)
                return;

            try
            {
                string code = await editor.ExecuteScriptAsync("getEditorCode();");
                string editorCode = JsonSerializer.Deserialize<string>(code);

                // Get file path from CustomLinkedList
                var allFiles = openFiles.GetAll();
                var currentFileNode = allFiles.FirstOrDefault(f => f.fileName == currentFile);

                snapshotQueue.Enqueue(new CustomQueue.QueueElement
                {
                    fileName = currentFile,
                    fileContent = editorCode,
                    timestamp = DateTime.Now,
                    filePath = currentFileNode.filePath
                });

                UpdateSnapshotListBox();
            }
            catch { }
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

        private void CloseFile(string fileName, Panel filePanel)
        {
            if (fileButtons.ContainsKey(fileName))
            {
                // Remove from CustomLinkedList
                openFiles.Remove(fileName);

                // Remove file button and panel
                splitContainer1.Panel1.Controls.Remove(filePanel);
                fileButtons.Remove(fileName);

                // Switch to another file if available
                if (fileButtons.Count > 0 && currentFile == fileName)
                {
                    var firstFile = fileButtons.First();
                    SwitchToFile(firstFile.Key);
                }
                else if (fileButtons.Count == 0)
                {
                    currentFile = "";
                    activeButton = null;
                }
            }
        }

    }
}