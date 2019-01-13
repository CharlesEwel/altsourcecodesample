using System;
using System.Collections.Generic;

namespace AltSourceCodeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Account> accounts = new List<Account> { };
            bool sessionActive = true;
            Account loggedInAccount = null;
            Console.WriteLine("Welcome to Bank of Charlie. What would you like to do today? (Press ? for list of commands)");
            while (sessionActive)
            { 
                string command = Console.ReadLine().ToLower();
                if (loggedInAccount != null)
                {
                    if(command == "deposit")
                    {
                        Console.WriteLine("How much would you like to deposit?");
                        string depositedString = Console.ReadLine();
                        try
                        {
                            int deposit = Convert.ToInt32(depositedString);
                            if (deposit <= 0)
                            {
                                Console.WriteLine("Deposits must be a positive number, if you wish to take money out of the account, use 'withdraw' command");   
                            }
                            else
                            {
                                loggedInAccount.balance += deposit;
                                Console.WriteLine("Deposited $" + depositedString + " into account.");
                            }
                            
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine("There is a limit of $2,147,483,647 for any individual deposit");
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine(depositedString + " is not in a recognizable format.");
                        }
                    }
                    else if (command == "withdraw")
                    {
                        Console.WriteLine("How much would you like to withdraw?");
                        string withdrawnString = Console.ReadLine();
                        try
                        {
                            int withdrawal = Convert.ToInt32(withdrawnString);
                            if (withdrawal <= 0)
                            {
                                Console.WriteLine("Withdrawals must be a positive number, if you wish to add money to the account, use 'deposit' command");
                            }
                            else
                            {
                                loggedInAccount.balance += withdrawal;
                                Console.WriteLine("Withdrawn $" + withdrawnString + " into account.");
                            }

                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine("There is a limit of $2,147,483,647 for any individual withdrawal");
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine(withdrawnString + " is not in a recognizable format.");
                        }
                    }
                    else if (command == "checkbalance")
                    {
                        Console.WriteLine("Your balance is $" + loggedInAccount.balance);
                    }
                    else if(command == "logout")
                    {
                        loggedInAccount = null;
                    }
                    else if (command == "quit")
                    {
                        Quit();
                        sessionActive = false;
                    }
                    else if (command == "?")
                    {
                        ListLoggedInInputs();
                    }
                    else
                    {
                        Console.WriteLine("Command not recognized");
                        ListLoggedInInputs();

                    }
                } else
                {
                    if (command == "createaccount")
                    {
                        accounts.Add(CreateAccount(accounts));
                        Console.WriteLine("Account Created Successfully");
                    }
                    else if (command == "login")
                    {
                        loggedInAccount = LogIn(accounts);
                        if (loggedInAccount != null)
                        {
                            
                            Console.WriteLine("Log In Success");
                        }
                        else
                        {
                            Console.WriteLine("Log In Failed");
                        }
                    }
                    else if (command == "quit")
                    {
                        Quit();
                        sessionActive = false;
                    }
                    else if (command == "?")
                    {
                        ListLoggedOutInputs();
                    }
                    else
                    {
                        Console.WriteLine("Command not recognized");
                        ListLoggedOutInputs();

                    }
                }
                Console.WriteLine("What would you like to do next?");

            }
            
        }

        public static Account CreateAccount(List<Account> accounts)
        {
            //The while loop and initializing a blank username is done so that users cannot register usernames that are already taken
            string userName = "";
            while (userName == "")
            {
                bool validUserName = true;
                Console.WriteLine("Please enter your desired user name:");
                string desiredUserName = Console.ReadLine();
                //Here is where we check our list of existing account to see if the username is already in use
                foreach (Account account in accounts)
                {
                    if (account.username == desiredUserName) validUserName = false;
                }
                if (validUserName)
                {
                    userName = desiredUserName;
                }
                else Console.WriteLine("That username is already in use.");
            }
            //Similarly, we use the same pattern to force users to type their password identicaly twice
            string password = "";
            while(password == "")
            {
                Console.WriteLine("Please enter your desired password:");
                string desiredPassword = InputPassword();
                Console.WriteLine("Please re-enter your desired password:");
                string repeatedDesiredPassword = InputPassword();
                if (desiredPassword == repeatedDesiredPassword)
                {
                    password = desiredPassword;
                }
                else Console.WriteLine("Those passwords did not match, please try again.");
            }
            Account newAccount = new Account { username = userName, password = password, balance = 0 };
            return newAccount;
        }

        public static Account LogIn(List<Account> accounts)
        {
            Console.WriteLine("Please enter your user name:");
            string userName = Console.ReadLine();
            Console.WriteLine("Please enter your password:");
            string password = InputPassword();
            Account loggedInAccount = null;
            foreach(Account account in accounts)
            {
                if (account.username == userName && account.password == password) loggedInAccount = account;
            }
            return loggedInAccount;
        }
        
        public static void Quit()
        {
            Console.WriteLine("Ending Session");
        }
        public static void ListLoggedOutInputs()
        {
            Console.WriteLine("Recognized inputs are:");
            Console.WriteLine("CreateAccount ~ creates a new account");
            Console.WriteLine("LogIn ~ Log in to an existing account");
            Console.WriteLine("Quit ~ ends session");
        }

        public static void ListLoggedInInputs()
        {
            Console.WriteLine("Recognized inputs are:");
            Console.WriteLine("Deposit ~ add money to your account");
            Console.WriteLine("Withdraw ~ take money from your account");
            Console.WriteLine("Check Balance ~ check how much money you have in your account");
            Console.WriteLine("LogOut ~ Log out");
            Console.WriteLine("Quit ~ ends session");
        }

        public static string InputPassword()
        {
            string pass = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);
            Console.WriteLine("");
            return pass;
        }

        public class Account
        {
            public string username { get; set; }
            public string password { get; set; }
            public int balance { get; set; }
        }
    }
}
