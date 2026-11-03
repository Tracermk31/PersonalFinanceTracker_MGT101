using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CSC160_ConsoleMenu;

namespace PersonalFianceTracker
{
    internal class MoneyManager
    {
        public decimal Balance { get; private set; }

        private readonly TransactionStorage transactionStorage;
        private readonly GoalStorage goalStorage;

        private readonly List<string> mainMenu = new List<string>
        {
            "View Balance",
            "Add Money (Income)",
            "Subtract Money (Expense)",
            "Set Goal",
            "View Goals"
        };

        public MoneyManager(decimal startingBalance = 0m)
        {
            if (startingBalance < 0m)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(startingBalance),
                    "Starting balance cannot be negative.");
            }

            transactionStorage = new TransactionStorage();
            goalStorage = new GoalStorage();

            Balance = startingBalance;

            // Rebuild the balance from saved transactions.
            foreach (Transaction transaction in
                transactionStorage.GetTransactions())
            {
                if (transaction.Type == TransactionType.Income)
                {
                    Balance += transaction.Amount;
                }
                else if (transaction.Type == TransactionType.Expense)
                {
                    Balance -= transaction.Amount;
                }
            }
        }

        public IReadOnlyList<Transaction> GetTransactions()
        {
            return transactionStorage.GetTransactions();
        }

        public bool AddMoney(decimal amount, string description = "Income")
        {
            if (amount <= 0m)
            {
                return false;
            }

            Transaction transaction = new Transaction
            {
                Type = TransactionType.Income,
                Amount = amount,
                Description = description
            };

            transactionStorage.AddTransaction(transaction);

            Balance += amount;

            return true;
        }

        public bool SubtractMoney(decimal amount, string description = "Expense")
        {
            if (amount <= 0m || amount > Balance)
            {
                return false;
            }

            Transaction transaction = new Transaction
            {
                Type = TransactionType.Expense,
                Amount = amount,
                Description = description
            };

            transactionStorage.AddTransaction(transaction);

            Balance -= amount;

            return true;
        }

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

            string description = CIO.PromptForInput(
                "Enter a reason for adding money: ", false);

            try
            {
                if (AddMoney(amount, description))
                {
                    Console.WriteLine(
                        $"Added {amount:C} to your balance.");

                    ViewBalance();
                }
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                Console.WriteLine(
                    $"Could not save transaction: {ex.Message}");
            }
        }

        private void SubtractMoneyMenu()
        {
            Console.WriteLine("--- Subtract Money ---");

            decimal amount = PromptForAmount();

            if (amount > Balance)
            {
                Console.WriteLine(
                    "Insufficient funds. Balance was not changed.");

                ViewBalance();
                return;
            }

            string description = CIO.PromptForInput(
                "Enter a reason for spending money: ", false);

            try
            {
                if (SubtractMoney(amount, description))
                {
                    Console.WriteLine(
                        $"Subtracted {amount:C} from your balance.");

                    ViewBalance();
                }
                else
                {
                    Console.WriteLine(
                        "Balance was not changed.");

                    ViewBalance();
                }
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                Console.WriteLine(
                    $"Could not save transaction: {ex.Message}");
            }
        }

        private void SetGoal()
        {
            string userGoal = CIO.PromptForInput(
                "What goal would you like to add? Enter 0 to go back: ",
                false);

            if (userGoal == "0")
            {
                Console.WriteLine("Returning to menu...");
                return;
            }

            int userTermInt = CIO.PromptForInt(
                "Enter 1 for long term and 2 for short term: ",
                1, 2);

            string userTerm = "";

            if (userTermInt == 1)
            {
                userTerm = "Long Term";
            }
            else if (userTermInt == 2)
            {
                userTerm = "Short Term";
            }

            Goal newGoal = new Goal
            {
                Name = userGoal,
                Term = userTerm
            };

            try
            {
                goalStorage.AddGoal(newGoal);
                Console.WriteLine("Goal saved successfully!");
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                Console.WriteLine(
                    $"Could not save goal: {ex.Message}");
            }
        }

        private void ViewGoals()
        {
            var goals = goalStorage.GetGoals();

            Console.WriteLine();
            Console.WriteLine("=== Your Goals ===");

            if (goals.Count == 0)
            {
                Console.WriteLine("You have no goals saved.");
                return;
            }

            for (int i = 0; i < goals.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"{i + 1}. {goals[i].Name}");
                Console.WriteLine($"   Term: {goals[i].Term}");
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

                    case 4:
                        SetGoal();
                        break;

                    case 5:
                        ViewGoals();
                        break;

                    default:
                        running = false;
                        break;
                }

            } while (running);
        }
    }
}