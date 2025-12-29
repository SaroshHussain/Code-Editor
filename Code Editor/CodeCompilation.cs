using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Code_Editor
{
    internal class CodeCompilation
    {
        public async Task<string> CompileCppWithPowerShell(string cppCode)
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

        public async Task<string> ExecutePythonScript(string pythonCode)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "PythonCodeRunner");
            Directory.CreateDirectory(tempDir);

            string sourceFile = Path.Combine(tempDir, "temp.py");

            try
            {
                File.WriteAllText(sourceFile, pythonCode);

                var runProcess = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/k python \"{sourceFile}\" && pause && exit",
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                var process = Process.Start(runProcess);

                return "✔ Python script running in a new terminal window...";
            }
            catch (Exception ex)
            {
                return "❌ Error: " + ex.Message;
            }
        }
    }
}
