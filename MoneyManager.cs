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

            transactionStorage = new TransactionStorage();

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