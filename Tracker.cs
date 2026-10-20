using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using CSC160_ConsoleMenu;

namespace PersonalFianceTracker
{
    internal class Tracker
    {
        private Dictionary<string, string> accounts = new Dictionary<string, string>()
        {
            { "Veronica", "12Hi" }
        };

        private void CreateAccount()
        {
            string promptForAccount = "What would you like your account name to be? ";
            string promptForPassword = "What would you like your password to be? ";

            bool isInvalid = true;

            string userAccount;
            string userPassword;

            do
            {
                userAccount = CIO.PromptForInput(promptForAccount, false);
                if (string.IsNullOrEmpty(userAccount))
                {
                    Console.WriteLine("userAccount cannot be null or empty, please try again");
                }
                else
                {
                    isInvalid = false;
                }
            } while (isInvalid);

            isInvalid = true;

            do
            {
                userPassword = CIO.PromptForInput(promptForPassword, false);
                if (string.IsNullOrEmpty(userPassword))
                {
                    Console.WriteLine("userPassword cannot be null or empty, please try again");
                }
                else
                {
                    isInvalid = false;
                }
            } while (isInvalid);

            accounts.Add(userAccount, userPassword);

            // For testing
            // Console.WriteLine($"Your password is: {accounts[userAccount]}");
        }

        private bool LoginAccount() 
        {
            string userAccount;
            string userPassword;

            bool tryAgain = true;
            do
            {
                userAccount = CIO.PromptForInput("What is your username?", false);
                if (!accounts.ContainsKey(userAccount)) {
                    Console.WriteLine("This account doesn't exist!");
                    tryAgain = CIO.PromptForBool("Would you like to try again?", "Yes", "No");
                } 
            } while (tryAgain);

          
            return false;
        }

        internal void MainMenu(IEnumerable<string> options)
        {
            byte choice = (byte)CIO.PromptForMenuSelection(options);
            switch (choice)
            {
                case 1:
                    CreateAccount();
                    break;
                case 2:
                    if (LoginAccount())
                    {
                        //Internal Menu
                    }
                    break;
                default:
                    return;
            }
        }
    }
}
