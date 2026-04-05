

namespace Utility
{
    public class LogService
    {
       
        public void WriteErrorLog(Exception ex, string logPath)
        {
            WriteErrorLog(ex, 0, logPath);
        }

        public void WriteErrorLog(Exception ex, long logId, string logPath)
        {
            try
            {
                string logMsg = logId > 0 ? $" Error LogId: {logId} " : "";
                string logMessage = $"{DateTime.Now}: {ex.Source?.Trim()}; {ex.Message?.Trim()}{logMsg}";
                File.AppendAllText(Path.Combine(logPath, "logfileMobile.txt"), logMessage + Environment.NewLine);
            }
            catch
            {
                // Handle exception appropriately
            }
        }
        public static void WriteErrorLog1(Exception ex, long LogId, string Log_Path)
        {
            StreamWriter sw = null;
            try
            {
                string LogMsg = "";
                if (LogId > 0)
                {
                    LogMsg = " Erro LogId: " + LogId.ToString() + " ";
                }

                sw = new StreamWriter(Log_Path + "\\logfileMobile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
        public static void WriteErrorLog1(string MethedName, string Message, string Log_Path)
        {
            var PathWithFolderName = System.IO.Path.Combine(Log_Path);
            StreamWriter sw = null;
            try
            {
                

                bool exists = Directory.Exists(PathWithFolderName);
                if (!exists)
                {
                    Directory.CreateDirectory(PathWithFolderName);
                }
               

                string filePath = "path/to/your/file.txt";

                string fullPath = Log_Path + "\\logfileMobile.txt";
                if (File.Exists(fullPath))
                {
                    FileInfo fileInfo = new FileInfo(Log_Path + "\\logfileMobile.txt");
                    long fileSizeInBytes = fileInfo.Length;
                    //int fileSizeFormatted = GetFormattedFileSize(fileSizeInBytes);

                    double fileSizeFormatted = GetTextFileSizeInMB1(fullPath);
                    if (fileSizeFormatted>=5)
                    {
                       // DelFile.DeleteOldFiles(Log_Path);

                        string newPath = Log_Path + "\\logfileMobile_" + DateTime.Now.ToString("dd_MMM_yyyy_HH_mm_ss") + ".txt";
                        //File.Delete(fullPath);
                        File.Move(fullPath, newPath);
                    }
                  //  DelFile.DeleteOldFiles(Log_Path);

                }

                sw = new StreamWriter(Log_Path + "\\logfileMobile.txt", true);
                sw.WriteLine("Methad Name: " + MethedName.Trim());
                sw.WriteLine("\n");
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message.Trim());
                sw.WriteLine("--------------------------END------------------------------");
                sw.Flush();
                sw.Close();
            }
            catch { }
        }

        static double GetTextFileSizeInMB1(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long fileSizeInBytes = fileInfo.Length;
            double fileSizeInMB = (double)fileSizeInBytes / (1024 * 1024);
            return fileSizeInMB;
        }
        static int GetFormattedFileSize1(long fileSizeInBytes)
        {
            string[] sizeSuffixes = { "B", "KB", "MB", "GB", "TB" };
            int suffixIndex = 0;

            decimal fileSize = fileSizeInBytes;
            while (fileSize >= 1024 && suffixIndex < sizeSuffixes.Length - 1)
            {
                fileSize /= 1024;
                suffixIndex++;
            }

            return Convert.ToInt16(fileSize);
        }

        public static void WriteErrorLog(string methodName, string message, string logPath)
        {
            try
            {
                var fullPath = Path.Combine(logPath, "logfileEsolution.txt");

                if (File.Exists(fullPath))
                {
                    var fileInfo = new FileInfo(fullPath);
                    long fileSizeInBytes = fileInfo.Length;
                    double fileSizeInMB = GetTextFileSizeInMB(fileSizeInBytes);
                    if (fileSizeInMB >= 5)
                    {
                        string newPath = Path.Combine(logPath, $"logfileEsolution_{DateTime.Now:dd_MMM_yyyy_HH_mm_ss}.txt");
                        File.Move(fullPath, newPath);
                    }
                }

                string logMessage = $"Method Name: {methodName.Trim()}{Environment.NewLine}" +
                                    $"{Environment.NewLine}" +
                                    $"{DateTime.Now}: {message.Trim()}{Environment.NewLine}" +
                                    "--------------------------END------------------------------";
                File.AppendAllText(fullPath, logMessage + Environment.NewLine);
            }
            catch
            {
                // Handle exception appropriately
            }
        }

        private static double GetTextFileSizeInMB(long fileSizeInBytes)
        {
            return (double)fileSizeInBytes / (1024 * 1024);
        }
    }
}

