using CSC160_ConsoleMenu;

namespace PersonalFianceTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Tracker().MainMenu();
            new MoneyManager().MainMenu();
            Tracker trackerRemote = new Tracker();
        }
    }
}
