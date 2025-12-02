using Microsoft.Web.WebView2.WinForms;
using ReaLTaiizor.Forms;
using System.Windows.Forms;
using System.Text.Json;
using System.Diagnostics;

namespace Code_Editor
{
    public partial class Form1 : ReaLTaiizor.Forms.LostForm

    {
        public Form1()
        {
            InitializeComponent();
            InitializeWebView();
        }
        private CustomStack openFiles = new CustomStack();
        private Dictionary<string, Button> fileButtons = new Dictionary<string, Button>();
        private string currentFile = "";
        private Button activeButton = null;
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
            // Reset previous active button
            if (activeButton != null)
            {
                activeButton.BackColor = ColorTranslator.FromHtml("#2d2d30");
                activeButton.ForeColor = Color.White;
            }

            // Set new active button
            if (fileButtons.ContainsKey(fileName))
            {
                activeButton = fileButtons[fileName];
                activeButton.BackColor = ColorTranslator.FromHtml("#007ACC"); // VS Code blue
                activeButton.ForeColor = Color.White;
            }

            currentFile = fileName;

            string code = openFiles.Get(fileName);
            string escapedCode = System.Text.Json.JsonSerializer.Serialize(code);
            await editor.ExecuteScriptAsync($"setEditorCode({escapedCode});");
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

                if (fileExtension == ".cpp" || fileExtension == ".cc" || fileExtension == ".cxx" || fileExtension == ".c")
                {
                    string cppCode = openFiles.Get(currentFile);
                    string output = await ExecuteCppCode(cppCode);
                    code_output.Text = output;
                }
                else
                {
                    string outputJson = await editor.ExecuteScriptAsync("executeCode();");

                    string output = System.Text.Json.JsonSerializer.Deserialize<string>(outputJson);
                    code_output.Text = output;
                }
            }
            catch (Exception ex)
            {
                code_output.Text = "Exception: " + ex.Message;
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private async Task SaveCurrentFile()
        {
            if (string.IsNullOrEmpty(currentFile))
                return;

            try
            {
                string code = await editor.ExecuteScriptAsync("getEditorCode();");
                string editorCode = System.Text.Json.JsonSerializer.Deserialize<string>(code);
                openFiles.Set(currentFile, editorCode);
            }
            catch (Exception ex)
            {
                code_output.Text = "Save Error: " + ex.Message;
            }
        }

        // C++ Execution Methods
        private async Task<string> ExecuteCppCode(string cppCode)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "CppCodeRunner");
            Directory.CreateDirectory(tempDir);

            string sourceFile = Path.Combine(tempDir, "temp.cpp");
            string exeFile = Path.Combine(tempDir, "temp.exe");

            try
            {
                // Write C++ code to temporary file
                File.WriteAllText(sourceFile, cppCode);

                // Compile with GCC
                string compileOutput = RunCommand("gcc", $"\"{sourceFile}\" -o \"{exeFile}\"");
                if (!File.Exists(exeFile))
                {
                    return "Compilation Error:\n" + compileOutput;
                }

                // Execute the compiled program
                string executionOutput = RunCommand(exeFile, "");
                return executionOutput;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            finally
            {
                // Cleanup
                try
                {
                    if (File.Exists(sourceFile)) File.Delete(sourceFile);
                    if (File.Exists(exeFile)) File.Delete(exeFile);
                }
                catch { }
            }
        }

        private string RunCommand(string command, string arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                    return string.IsNullOrEmpty("error") ? output : error;
                }
            }
            catch (Exception ex)
            {
                return "Failed to execute: " + ex.Message;
            }
        }
      

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Saving current file...");
            await SaveCurrentFile();

        }
    }
}