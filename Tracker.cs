using CSC160_ConsoleMenu;

namespace PersonalFianceTracker
{
    internal class Tracker
    {
        private Dictionary<string, string> accounts = new Dictionary<string, string>()
        {
            { "Veronica", "12Hi" }
        };

        private List<(string goal, string longOrShort)> goalList = new List<(string goal, string longOrShort)>();
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

        internal void SetGoal()
        {
            string userTerm = "";
            string userGoal = "";
            string prompt;
            prompt = "What goal for yourself would you like to add? Enter 0 if you'd like to go back: ";

            userGoal = CIO.PromptForInput(prompt, false);

            if (!string.Equals(userGoal, "0"))
            {
                prompt = "Is your goal a long or short term goal? Please enter 1 for long term and 2 for short term: ";

                int userTermInt = CIO.PromptForInt(prompt, 1, 2);

                if (userTermInt == 1)
                {
                    userTerm = "Long Term";
                }
                else if (userTermInt == 2)
                {
                    userTerm = "Short Term";
                }

                goalList.Add((userGoal, userTerm));

                //Code for manual testing
                //foreach(var (goal, term) in goalList)
                //{
                //    Console.WriteLine($"Goal: {goal}\nTerm: {term}");
                //}
            }
            else
            {
                Console.WriteLine("Returning to menu...");
            }

        }
    }

}
