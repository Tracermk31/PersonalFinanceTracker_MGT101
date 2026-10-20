using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using CSC160_ConsoleMenu;

namespace PersonalFianceTracker
{
    internal class Tracker
    {
        public static Dictionary<string, string> accounts = new Dictionary<string, string>()
        {
            { "Veronica", "12Hi" }
        };
        public static void CreateAccount()
        {
            string promptForAccount = "What would you like your account name to be? ";
            string promptForPassword = "What would you like your password to be? ";
            bool isInvalid = true;


            string userAccount = CIO.PromptForInput(promptForAccount, false);
            do
            {
                if (string.IsNullOrEmpty(userAccount))
                {
                    Console.WriteLine("userAccount cannot be null or empty, please try again");
                    userAccount = CIO.PromptForInput(promptForAccount, false);
                }
                else
                {
                    isInvalid = false;
                }
            } while (isInvalid);

            string userPassword = CIO.PromptForInput(promptForPassword, false);

            isInvalid = true;
            do
            {
                if (string.IsNullOrEmpty(userPassword))
                {
                    Console.WriteLine("userPassword cannot be null or empty, please try again");
                    userAccount = CIO.PromptForInput(promptForPassword, false);
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
    }
}
