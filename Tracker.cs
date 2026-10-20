using CSC160_ConsoleMenu;

namespace PersonalFianceTracker
{
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
