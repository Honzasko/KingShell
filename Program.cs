Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("KingShell v0.0.1");
Console.ResetColor();
bool exit = false;
String directory = "";

if (Environment.OSVersion.Platform == PlatformID.Unix)
{
    directory = Environment.GetEnvironmentVariable("HOME");
}
else
{
    directory = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
}


String[] directories = Directory.GetDirectories(directory);
String[] files = Directory.GetFiles(directory);

IDictionary<String, Func<IList<String>, int>> commands = new Dictionary<String, Func<IList<String>, int>>();
int exitapp(IList<String> com_args) {
    exit = true;
    return 1;
}



int list_dir(IList<String> com_args) {
    Console.ForegroundColor = ConsoleColor.White;
    Console.BackgroundColor = ConsoleColor.Green;
    for (int i = 0; i < directories.Length; i++) {
        String r = directories[i].Replace(directory, "");
        Console.WriteLine(r);
    }
    Console.ResetColor();
    for (int k = 0; k < files.Length; k++) {
        String r = files[k].Replace(directory, "");
        Console.WriteLine(r);
      
    }

    return 1;
}

commands["exit"] = exitapp;
commands["ls"] = list_dir;

while (!exit) {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("[{0}]",directory);
    Console.ResetColor();
    Console.Write(":");
    String command = Console.ReadLine();
    String[] c_args = command.Split(" ");
    IList<String> arguments = new List<String>();
    if (c_args.Length > 1)
    {
        for (int i = 0; i < c_args.Length; i++)
        {
            arguments.Add(c_args[i]);
        }
        try
        {
            int result = commands[c_args[0]](arguments);
        }
        catch
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Command not found");
            Console.ResetColor();
        }
    }
    else if (c_args.Length == 1) {

        try
        {
            int result = commands[c_args[0]](arguments);
        }
        catch {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Command not found");
            Console.ResetColor();
        }
        
    }



}