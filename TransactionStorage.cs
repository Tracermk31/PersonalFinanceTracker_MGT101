using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PersonalFianceTracker
{
    internal class TransactionStorage
    {
        private readonly string filePath;

        private readonly List<Transaction> transactions;

        private readonly JsonSerializerOptions jsonOptions =
            new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };

        public TransactionStorage()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "PersonalFianceTracker");

            Directory.CreateDirectory(folder);

            filePath = Path.Combine(folder, "transactions.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                transactions =
                    JsonSerializer.Deserialize<List<Transaction>>(
                        json, jsonOptions) ?? new List<Transaction>();
            }
            else
            {
                transactions = new List<Transaction>();
            }
        }

        public IReadOnlyList<Transaction> GetTransactions()
        {
            return transactions.AsReadOnly();
        }

        public void AddTransaction(Transaction transaction)
        {
            transactions.Add(transaction);

            try
            {
                string json = JsonSerializer.Serialize(
                    transactions, jsonOptions);

                string tempPath = filePath + ".tmp";

                File.WriteAllText(tempPath, json);

                File.Move(tempPath, filePath, true);
            }
            catch
            {
                transactions.Remove(transaction);
                throw;
            }
        }
    }
}