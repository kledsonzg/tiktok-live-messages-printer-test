using System.Diagnostics;

namespace KledsonZG.Browser
{
    internal class ProcessInitializer
    {
        private static Process? CurrentProcess = null;
        internal static void Start()
        {
            var process = new Process();
            
            process.StartInfo.Arguments = "--remote-debugging-port=40020 --user-data-dir=\\temp";

            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "msedge";

            if(process.Start() == false)
                throw new Exception("Houve um erro ao iniciar o web driver.");

            process.WaitForInputIdle();

            CurrentProcess = process;
        }

        internal static void Stop()
        {
            if(CurrentProcess == null)
                return;
            
            CurrentProcess.Kill();
        }
    }
}