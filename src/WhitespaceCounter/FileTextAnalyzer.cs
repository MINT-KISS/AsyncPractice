namespace WhitespaceCounter
{
    public static class FileTextAnalyzer
    {
        public static string[] GetFileStats(string filePath)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileWeight = new FileInfo(filePath).Length.ToString();
                var spaceCount = 0;

                using (var sr = new StreamReader(filePath))
                {
                    var buf = new char[4096];
                    int read;
                    while ((read = sr.Read(buf, 0, buf.Length)) > 0)
                    {
                        for (int i = 0; i < read; i++)
                        {
                            if (char.IsWhiteSpace(buf[i]))
                            {
                                spaceCount++;
                            }
                        }
                    }
                }

                return new[]
                {
                    fileName,
                    fileWeight,
                    spaceCount.ToString()
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading \"{Path.GetFileName(filePath)}\": {ex.GetType().Name}: {ex.Message}");
                return Array.Empty<string>();
            }
        }
    }
}
