namespace OperacaoCuriosidadeAPI.Services
{
    public class LogService
    {
        public void LogParaArquivo(string titulo, string mensagemLog)
        {
            string nomeArquivo = DateTime.Now.ToString("ddMMyyyy") + ".txt";

            StreamWriter swLog;

            if (File.Exists(nomeArquivo))
            {
                swLog = File.AppendText(nomeArquivo);
            }
            else
            {
                swLog = new StreamWriter(nomeArquivo);
            }

            swLog.WriteLine("Log:");
            swLog.WriteLine($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}");

            swLog.WriteLine($"Título da Mensagem: {titulo}");
            swLog.WriteLine($"Mensagem: {mensagemLog}");

            swLog.WriteLine("-------------------------------");
            swLog.WriteLine("");
            swLog.Close();
        }
    }
}