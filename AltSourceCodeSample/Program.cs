using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AltSourceCodeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize a list of accounts
            List<Account> accounts = new List<Account> { };
            //boolean that tells the application to keep prompting the user for new commands until they quit
            bool sessionActive = true;
            //which account is logged in, if any
            Account loggedInAccount = null;
            Console.WriteLine("Welcome to Bank of Charlie. What would you like to do today? (Press ? for list of commands)");
            while (sessionActive)
            {
                //Some Regex so that commands are not case sensitive, nor do they care about whitespace
                Regex space = new Regex(" ");
                string command = space.Replace(Console.ReadLine().ToLower(), "");
                //Here begins the list of commands available when a user is logged in
                //Being logged in is necessary for commands such as depositing, withdrawing, or checking balance to make sense
                if (loggedInAccount != null)
                {
                    if(command == "deposit")
                    {
                        Deposit(loggedInAccount);
                    }
                    else if (command == "withdraw")
                    {
                        Withdraw(loggedInAccount);
                    }
                    else if (command == "checkbalance")
                    {
                        Console.WriteLine("Your balance is $" + loggedInAccount.balance);
                    }
                    else if (command == "transactionhistory")
                    {
                        ListTransactionHistory(loggedInAccount);
                    }
                    else if(command == "logout")
                    {
                        loggedInAccount = null;
                    }
                    else if (command == "quit")
                    {
                        if(Quit()) sessionActive = false;
                        else Console.WriteLine();
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
                        if (Quit()) sessionActive = false;
                        else Console.WriteLine();
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
                //Prompts the user to enter another command before the while loop repeats
                Console.WriteLine("What would you like to do next? (Press ? for list of commands)");

            }
            
        }

        //Functions corresponding to different commands
        //Factored out so that the command loop can be navigated easily
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
            List<string> blankTransactionHistory = new List<string> { };
            Account newAccount = new Account { username = userName, password = password, balance = 0, transactionHistory=blankTransactionHistory};
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

        public static void Deposit(Account account)
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
                    account.balance += deposit;
                    string transaction = "Deposited $" + depositedString + " into account. New balance: $" + account.balance; ;
                    account.transactionHistory.Add(transaction);
                    Console.WriteLine(transaction);
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

        public static void Withdraw(Account account)
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
                    account.balance -= withdrawal;
                    string transaction = "Withdrew $" + withdrawnString + " from account. New balance: $"+account.balance;
                    account.transactionHistory.Add(transaction);
                    Console.WriteLine(transaction);
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

        public static void ListTransactionHistory(Account account)
        {
            int historyLength = account.transactionHistory.Count;
            for (int i = 0; i<historyLength; i++)
            {
                Console.WriteLine(account.transactionHistory[historyLength-1-i]);
            }
        }
        
        public static bool Quit()
        {
            Console.WriteLine("Are you sure you wish to quit? Press 'Y' to confirm");
            char response = Char.ToLower(Console.ReadKey().KeyChar);
            return response == 'y';
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
            Console.WriteLine("Transaction History ~ view history of withdrawals and deposits");
            Console.WriteLine("LogOut ~ Log out");
            Console.WriteLine("Quit ~ ends session");
        }

        //This method allows password masking
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
            public List<string> transactionHistory { get; set; }
        }
    }
}
