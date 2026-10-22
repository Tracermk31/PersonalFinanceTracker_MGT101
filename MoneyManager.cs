
using System;
using System.Collections.Generic;
using System.Globalization;
using CSC160_ConsoleMenu;

namespace PersonalFianceTracker
{
    internal class MoneyManager
    {
        public decimal Balance { get; private set; }

        private readonly List<string> mainMenu = new List<string>
        {
            "View Balance",
            "Add Money (Income)",
            "Subtract Money (Expense)"
        };
        public MoneyManager(decimal startingBalance = 0m)
        {
            if (startingBalance < 0m)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startingBalance),
                    "Starting balance cannot be negative.");
            }
            //Technically not needed, but is good practice to include anyways.

            Balance = startingBalance;
        }

        public bool AddMoney(decimal amount)
        {
            if (amount <= 0m)
            {
                return false;
            }

            Balance += amount;
            return true;
        }

        public bool SubtractMoney(decimal amount)
        {
            if (amount <= 0m || amount > Balance)
            {
                return false;
            }

            Balance -= amount;
            return true;
        }
        //These return a bool for checking, as opposed to not returning anything

        private void ViewBalance()
        {
            Console.WriteLine(
                $"Your current balance is {Balance:C}.");
        }
        private decimal PromptForAmount()
        {
            while (true)
            {
                string input = CIO.PromptForInput(
                    "Enter an amount: ", false);

                if (decimal.TryParse(
                    input,
                    NumberStyles.Currency,
                    CultureInfo.CurrentCulture,
                    out decimal amount) && amount > 0m)
                {
                    return amount;
                }

                Console.WriteLine(
                    "Please enter a valid amount greater than zero.");
            }
        }
        private void AddMoneyMenu()
        {
            Console.WriteLine("--- Add Money ---");

            decimal amount = PromptForAmount();

            if (AddMoney(amount))
            {
                Console.WriteLine(
                    $"Added {amount:C} to your balance.");

                ViewBalance();
            }
        }
        private void SubtractMoneyMenu()
        {
            Console.WriteLine("--- Subtract Money ---");

            decimal amount = PromptForAmount();

            if (SubtractMoney(amount))
            {
                Console.WriteLine(
                    $"Subtracted {amount:C} from your balance.");

                ViewBalance();
            }
            else
            {
                Console.WriteLine(
                    "Insufficient funds. Balance was not changed.");

                ViewBalance();
            }
        }
        internal void MainMenu()
        {
            bool running = true;

            do
            {
                Console.WriteLine();

                Console.WriteLine(
                    "=== Personal Finance Tracker ===");

                Console.WriteLine(
                    $"Current balance: {Balance:C}");

                int choice = CIO.PromptForMenuSelection(mainMenu);

                switch (choice)
                {
                    case 1:
                        ViewBalance();
                        break;

                    case 2:
                        AddMoneyMenu();
                        break;

                    case 3:
                        SubtractMoneyMenu();
                        break;

                    default:
                        running = false;
                        break;
                }

            } while (running);
        }
    }
}
