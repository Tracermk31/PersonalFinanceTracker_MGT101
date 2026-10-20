using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFianceTracker
{
    internal class Tracker
    {
        private Dictionary<string, string> accounts = new Dictionary<string, string>();

        internal void LoginAccount(string username, string password)
        {
            if (accounts[username] == password)
            {
                Console.WriteLine("You're in!");
            }
        }
    }
}
