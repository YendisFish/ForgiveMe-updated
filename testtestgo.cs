using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testtestgo
{
    class Program
    {
        public static void creatDir(String dirName, String userName) 
        {
            Console.WriteLine($@"Creating directory {dirName} [Path {userName}\{dirName}]");
            Directory.CreateDirectory($@"{userName}\{dirName}");
            Console.WriteLine("Directory Created!");
        }
        
        public static void createFile(String fileName, String userName) 
        {
            Console.WriteLine("Creating " + fileName);
            var fs = File.Create($@"{userName}\{fileName}");
            fs.Close();
        }

        public static String readFile(String fileName, String userName)
        {
            Console.WriteLine("Reading " + $@"{userName}\{fileName}");
            String fileContents = File.ReadAllText($@"{userName}\{fileName}");
            return fileContents;
        }

        public static void newEntryFile(String fileName, String userName)
        {
            var fs = File.Create($@"{userName}\{fileName}");
            fs.Close();

            Console.WriteLine("vvv |Enter Your Entry Below| vvv");
            String fileContents = File.ReadAllText($@"{userName}\{fileName}");
            String Entry = Console.ReadLine();

            File.WriteAllText($@"{userName}\{fileName}", fileContents + "\n" + Entry);
        }

        public static void newEntry(String fileName, String userName)
        {
            Console.WriteLine("vvv |Enter Your Entry Below| vvv");
            String fileContents = File.ReadAllText($@"{userName}\{fileName}");
            String Entry = Console.ReadLine();

            File.WriteAllText($@"{userName}\{fileName}", fileContents + "\n" + Entry);
        }

        private static void delFile(String fileName, String userName)
        {
            Console.WriteLine("Deleting " + fileName);
            File.Delete($@"{userName}\{fileName}");
        }

        public static String storeUname(String usrName, String storePoint)
        {
            File.WriteAllText(storePoint + ".txt", usrName);
            return $"{usrName} {storePoint}";
        }

        public static String storeUsrDat(String userName, String passWord, DateTime date)
        {
            File.WriteAllText("userdat." + userName + ".txt", $"{userName}, {passWord}, {date}");
            return $"Dat: {userName} {passWord} {date}";
        }
        
        public static void Main(String[]args)
        {
            Console.WriteLine("Welcome to ForgiveMe");
            Console.WriteLine();
            
            DateTime current = DateTime.Now;
            bool init = true;
            int passAttempt;
            int passAttemptNumber = 3;

            while (init == true)
            {
                Console.Write("Enter a username > ");
                String uName = Console.ReadLine();
                if (File.Exists("userdat." + uName + ".txt"))
                {
                    Console.Write("This user already exists, would you like to use it? [y/n] > ");
                    String ynUsr = Console.ReadLine();
                    if (ynUsr == "y")
                    {
                        Console.WriteLine("Using " + uName);
                        bool passWordMode = true;
                        while (passWordMode == true)
                        {
                            for (passAttempt = 0; passAttempt < passAttemptNumber; passAttempt = passAttempt + 1)
                            {
                                Console.Write($"Please enter your password [Attempt {passAttempt + 1} of {passAttemptNumber}] > ");
                                String passwdTry = Console.ReadLine();
                                String passwdFile = File.ReadAllText("userdat." + uName + ".txt");
                                if (passwdFile.Contains(passwdTry) == true && passwdTry.Length >= 6)
                                {
                                    Console.WriteLine("Successful!");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Password failed, please try again > ");
                                    continue;
                                }
                            }
                            break;
                        }
                    } else
                    {
                        Console.WriteLine("Going back to start.");
                        continue;
                    }

                } else
                {
                    Console.WriteLine("Setting username to " + uName);
                }

                if (Directory.Exists($@"{uName}"))
                {
                    Console.WriteLine($"Existing directory for {uName}!");
                    Console.WriteLine("Skipping rest of user creation process!");
                } else
                {
                    Directory.CreateDirectory(uName);
                    
                    Console.Write("Write a file name for your save [Type 'skip' to skip this process] > ");
                    String saveSpot = Console.ReadLine();

                    if (saveSpot == "skip")
                    {
                        Console.WriteLine("Skipping process");
                    }
                    else
                    {
                        String set = storeUname(uName, $@"{uName}\{saveSpot}");
                        Console.WriteLine("Save Details: " + set);

                        bool passwdSetup = true;
                        while (passwdSetup == true)
                        {
                            Console.Write("Enter a password > ");
                            String passwd = Console.ReadLine();
                            if (passwd.Length < 6)
                            {
                                Console.WriteLine("Failed to set password.");
                                continue;
                            }

                            Console.WriteLine("User set up finished, saving files...");

                            String datFile = storeUsrDat(uName, passwd, current);

                            Console.WriteLine("User set up completed.");

                            break;
                        }
                    }
                }
                

                bool takeIn = true;
                
                while (takeIn == true)
                {
                    Console.Write("/> ");
                    String userInput = Console.ReadLine();

                    String Commands = File.ReadAllText("cmds.txt");

                    if (Commands.Contains(userInput)) 
                    {
                        if (userInput == "new")
                        {
                            Console.Write("Are you creating a new entry in an existing file, or a new file? [existing/new-file]> ");
                            String existOrNot = Console.ReadLine();
                            
                            if (existOrNot == "existing")
                            {
                                Console.Write("What is the name of the file? > ");
                                String fileName = Console.ReadLine();
                                
                                if (File.Exists($@"{uName}\{fileName}"))
                                {
                                    Console.WriteLine("File Exists, proceeding!");
                                    newEntry(fileName, uName);

                                } else
                                {
                                    Console.WriteLine($"{fileName} either does not exist or you lack the proper permissions! Please try again!");
                                    continue;
                                }
                            } else
                            {
                                Console.Write("What would you like to name this file? > ");
                                String fileName = Console.ReadLine();

                                newEntryFile(fileName, uName);
                            }
                        }
                        if (userInput == "del")
                        {
                            Console.Write("What is the name of the file you are deleting? > ");
                            String fileName = Console.ReadLine();

                            if (File.Exists($@"{uName}\{fileName}"))
                            {
                                Console.WriteLine("File exists! Deleting...");
                                delFile(fileName, uName);
                            } else
                            {
                                Console.WriteLine($"{fileName} either does not exist or you lack the proper permissions! Please try again!");
                                continue;
                            }
                        }
                        if (userInput == "read")
                        {
                            Console.Write("Please enter the file name. > ");
                            String fileName = Console.ReadLine();

                            if (File.Exists($@"{uName}\{fileName}"))
                            {
                                Console.WriteLine("File Exists! Reading!");
                                String fileContents = readFile(fileName, uName);

                                Console.WriteLine(fileContents);
                            }
                            else 
                            {
                                Console.WriteLine("Could not find " + fileName);
                                Console.WriteLine("File does not exist or you do not have the required permissions! Please try again!");
                                continue;
                            }
                        }
                        if (userInput == "query")
                        {
                            Console.Write("Please enter the filename. > ");
                            String fileName = Console.ReadLine();

                            if (File.Exists(fileName))
                            {
                                if (File.Exists($@"{uName}\{fileName}"))
                                {
                                    Console.Write("File exists! Would you like to read it? [y/n] > ");
                                    String ynQuery = Console.ReadLine();
                                    if (ynQuery == "y")
                                    {
                                        Console.WriteLine("Reading File!");
                                        String fileContents = readFile(fileName, uName);
                                        Console.WriteLine(fileContents);
                                    } else
                                    {
                                        continue;
                                    }
                                } else
                                {
                                    Console.WriteLine("File does not exist within an accessable directory");
                                    continue;
                                }
                            } else
                            {
                                Console.WriteLine("File does not exist!");
                            }
                        }
                        if (userInput == "exit")
                        {
                            System.Environment.Exit(1);
                        }
                        if (userInput == "create") 
                        {
                            Console.Write("What would you like to name this file? > ");
                            String fileName = Console.ReadLine();

                            if (File.Exists($@"{uName}\{fileName}")) 
                            {
                                Console.WriteLine("This file already exists! Please choose a different name!");
                                continue;
                            } else 
                            {
                                createFile(fileName, uName);
                            }
                        }
                        if (userInput == "create-dir")
                        {
                            Console.Write("What is the name of the directory you are trying to create? > ");
                            String dirName = Console.ReadLine();

                            if (File.Exists($@"{uName}\{dirName}"))
                            {
                                Console.WriteLine("Directory already exists! Please try again!");
                                continue;
                            } else 
                            {
                                creatDir(dirName, uName);
                            }
                        }
                    } else
                    {
                        Console.WriteLine("This command does not exist, here is a list of commands:");
                        Console.WriteLine(Commands);
                    }
                }

                break;
            }
        }
    }
}