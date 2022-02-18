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



IDictionary<String, Func<IList<String>, int>> commands = new Dictionary<String, Func<IList<String>, int>>();
int exitapp(IList<String> com_args) {
    exit = true;
    return 1;
}



int list_dir(IList<String> com_args) {
    String[] directories = Directory.GetDirectories(directory);
    String[] files = Directory.GetFiles(directory);
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

int change_dir(IList<String> com_args) {
    if (com_args.Count == 1)
    {
        String[] directories = Directory.GetDirectories(directory);
        for (int i = 0; i < directories.Length; i++) {
          directories[i] = directories[i].Replace(directory, ""); ;
        }
        if (directories.Contains('\\' + com_args[0]))
        {
            directory += "/" + com_args[0];
            return 1;
        }
        else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("cd:This directory does not exists");
            Console.ResetColor();
            return 0;
        }
    }
    else {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("cd:Invalid usage");
        Console.ResetColor();
        return 0;
    }
}

int cat_cmd(IList<String> com_args) {
    if (com_args.Count == 1)
    {
        String[] files = Directory.GetFiles(directory);
        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace(directory, ""); ;
        }
        if (files.Contains('\\' + com_args[0]))
        {
            String content = File.ReadAllText(directory + com_args[0]);
            Console.WriteLine(content);
            return 1;
        }
        else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("cat:This file doesnt seems to exist");
            Console.ResetColor();
            return 0;
        }
    }
    else if (com_args.Count > 1)
    {
        IList<String> input = new List<String>();
        String output = "";
        bool change = false;
        for (int k = 0; k < com_args.Count; k++)
        {
            if (!change)
            {
                if (com_args[k] != "<")
                {
                    input.Add(com_args[k]);
                }
                else
                {
                    if (k == (com_args.Count - 2))
                    {
                        change = true;
                    }

                }
            }
            else
            {
                output = com_args[k];
            }
        }
        if (change)
        {
            String[] files = Directory.GetFiles(directory);
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(directory, ""); ;
            }
            int valid = 0;
            for (int l = 0; l < input.Count; l++)
            {
                if (files.Contains('\\' + input[l]))
                {
                    valid++;
                }
            }
            if (valid == input.Count)
            {
                String content = "";
                for (int j = 0; j < input.Count; j++)
                {
                    String current = File.ReadAllText(directory + input[j]);
                    content += current;
                }
                File.WriteAllText(directory + output, content);
                return 1;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("cat:Some file/files dont/doesnt exist");
                Console.ResetColor();
                return 0;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("cat:Invalid usage");
            Console.ResetColor();
            return 0;
        }

    }
    else {
        return 0;
    }
    
}


commands["exit"] = exitapp;
commands["ls"] = list_dir;
commands["cd"] = change_dir;
commands["cat"] = cat_cmd;

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
        for (int i = 1; i < c_args.Length; i++)
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