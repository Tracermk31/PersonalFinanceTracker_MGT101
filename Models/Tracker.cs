using CSC160_ConsoleMenu;
using System.Text.Json;

namespace PersonalFianceTracker
{
    internal class AccountInfo
    {
        public string Username {  get; set; }
        public double Balance { get; set; }

        public AccountInfo(string username, double balance)
        {
            Username = username;
            Balance = balance;
        }
    }
    internal class Tracker
    {

        private Dictionary<string, string> accounts = new Dictionary<string, string>()
        {
            { "Veronica", "12Hi" }
        };

        private List<string> mainMenu = 
        [
            "Create Account",
            "Login"
        ];

        private void CreateAccount()
        {
            string promptForAccount = "What would you like your account name to be? ";
            string promptForPassword = "What would you like your password to be? ";
            string promptForBalance = "How much is your starting balance?";

            string userAccount;
            string userPassword;
            double balance = 0.0;

            userAccount = CIO.PromptForInput(promptForAccount, false);
            userPassword = CIO.PromptForInput(promptForPassword, false);
            balance = (double)CIO.PromptForInt(promptForBalance, 0, 1000000);

            AccountInfo accountInfo = new(userAccount, balance);

            string fileName = "AccountInfo.json";
            string jsonString = JsonSerializer.Serialize(accountInfo);
            Console.WriteLine(jsonString);

            File.WriteAllText(fileName, jsonString);

            accounts.Add(userAccount, userPassword);

            // For testing
            // Console.WriteLine($"Your password is: {accounts[userAccount]}");
        }

        private bool LoginAccount() 
        {
            string userAccount;
            string userPassword;

            byte attemptsRemaining = 3;

            bool tryAgain = false;
            do
            {
                userAccount = CIO.PromptForInput("What is your username?", false);
                if (!accounts.ContainsKey(userAccount)) {
                    Console.WriteLine("This account doesn't exist!");
                    tryAgain = CIO.PromptForBool("Would you like to try again?", "Yes", "No");
                    if (!tryAgain)
                    {
                        return false;
                    }
                } 
            } while (!accounts.ContainsKey(userAccount));

            do
            {
                userPassword = CIO.PromptForInput("What is your password?", false);
                if (accounts[userAccount] != userPassword)
                {
                    Console.WriteLine($"Wrong password you have {--attemptsRemaining} attempts left");
                    tryAgain = CIO.PromptForBool("Would you like to try again?", "Yes", "No");
                    if (!tryAgain || attemptsRemaining == 0)
                    {
                        return false;
                    }
                }
            } while (accounts[userAccount] != userPassword);

            return true;
        }

        internal void MainMenu()
        {
            bool running = true;
            do
            {
                byte choice = (byte)CIO.PromptForMenuSelection(mainMenu);
                switch (choice)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        if (LoginAccount())
                        {
                            Console.WriteLine("Succesful Login!");
                            //Internal Menu
                        }
                        break;
                    default:
                        running = false;
                        break;
                }
            } while (running);
        }
    }
}
