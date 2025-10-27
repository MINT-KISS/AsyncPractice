using WhitespaceCounter;

var answer = "";
while (answer != "Y" && answer != "N")
{
    Console.WriteLine("Read files and count the number of spaces in them? (Y/N)");
    answer = Console.ReadLine()?.Trim().ToUpper();
    Console.WriteLine();
}

if (answer == "N")
{
    Console.WriteLine("Operation canceled.");
    return;
}


var folderPath = Path.Combine(AppContext.BaseDirectory, "Data");
var filePaths = Directory.GetFiles(folderPath, "*.txt");
if (filePaths.Length == 0)
{
    Console.Error.WriteLine($".txt files not found in folder: \"{folderPath}\"");
    return;
}


Console.WriteLine("Processing files...\n");

var tasks = filePaths.Select(p => Task.Run(() => FileTextAnalyzer.GetFileStats(p)));
Task.WaitAll(tasks);


foreach (var task in tasks)
{
    var r = task.Result;
    if (r.Length > 0)
    {
        Console.WriteLine($"File name: \"{r[0]}\", \nFile size: {r[1]} bytes, \nWhitespace count: {r[2]}\n");
    }
}

Console.WriteLine("Processing completed.\n");