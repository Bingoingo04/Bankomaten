using System;

namespace Bankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Arrays for users {userID, password} and the name of the accounts and their value
            int[,] users = {
                    { 1, 1111 },
                    { 2, 2222 }, { 3, 3333 }, { 4, 4444 }, { 5, 5555 } };

            string[][] accountsName =
            {
                    new string[] { "Lönekonto", "Sparkonto" },
                    new string[] { "Lönekonto" },
                    new string[] { "Lönekonto", "Sparkonto", "Buffert" },
                    new string[] { "Lönekonto", "Buffert" },
                    new string[] { "Sparkonto" }
            };

            double[][] accountsValue =
            {
                    new double[] { 10000.00, 20000.00 },
                    new double[] { 10000.00 },
                    new double[] { 10000.00, 20000.00, 30000.00 },
                    new double[] { 10000.00, 30000.00 },
                    new double[] { 10000.00 }
            };

            bool programRunning = true;

            while (programRunning)
            {
                Console.WriteLine("Välkommen till Bank AB!");

                Console.WriteLine("Vänligen skriv in användar ID:");
                int userID = Convert.ToInt32(Console.ReadLine());

                // Check if the user exists and gets their index
                int userIndex = GetUserIndex(users, userID);
                if (userIndex != -1)
                {
                    Console.WriteLine("Vänligen skriv in lösenordet (xxxx):");

                    // Attempt to log in with password (max 3 attempts)
                    bool loginSuccessful = AuthenticateUser(users, userIndex);

                    if (loginSuccessful)
                    {
                        Console.WriteLine("Inloggning Lyckades!");

                        // Loop for the main menu after login until the user logs out
                        bool loggedIn = true;
                        while (loggedIn)
                        {
                            // Menu
                            Console.WriteLine("Vad vill du göra?");
                            Console.WriteLine("1. Se dina konton och saldo");
                            Console.WriteLine("2. Överföring mellan konton");
                            Console.WriteLine("3. Ta ut pengar");
                            Console.WriteLine("4. Logga ut");

                            string menuChoice = Console.ReadLine();

                            // Switch case to handle user choice
                            switch (menuChoice)
                            {
                                case "1":
                                    ShowAccounts(userIndex, accountsName, accountsValue);
                                    break;
                                case "2":
                                    TransferBetweenAccounts(userIndex, accountsValue);
                                    break;
                                case "3":
                                    WithdrawMoney();
                                    break;
                                case "4":
                                    loggedIn = false;
                                    Console.WriteLine("Du har loggats ut.");
                                    break;
                                default:
                                    Console.WriteLine("Ogiltigt val, försök igen.");
                                    break;
                            }

                            if (loggedIn)
                            {
                                Console.WriteLine("Klicka enter för att komma till huvudmenyn.");
                                Console.ReadLine();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Du skrev in fel lösenord för många gånger! Starta om programmet.");
                        programRunning = false;
                    }
                }
                else
                {
                    Console.WriteLine("Användaren finns ej!");
                }
            }
        }

        // Function to find the user's index based on user ID
        static int GetUserIndex(int[,] users, int userID)
        {
            for (int i = 0; i < users.GetLength(0); i++)
            {
                if (users[i, 0] == userID)
                {
                    return i;
                }
            }
            return -1;
        }

        // Function to authenticate the user with password
        static bool AuthenticateUser(int[,] users, int userIndex)
        {
            for (int i = 0; i < 3; i++) // Max 3 attempts
            {
                int userPSWD = Convert.ToInt32(Console.ReadLine());

                if (users[userIndex, 1] == userPSWD)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Fel lösenord! Försök igen.");
                }
            }
            return false;
        }

        // Show accounts and their value
        static void ShowAccounts(int userIndex, string[][] accountsName, double[][] accountsValue)
        {
            for(int i = 0; i < accountsName[userIndex].Length; i++)
            {
                string accountName = accountsName[userIndex][i];
                double accountValue = accountsValue[userIndex][i];

                Console.WriteLine($"{i + 1}. {accountName}: {accountValue:C}");
            }
        }

        // Dummy function to simulate transfer between accounts
        static void TransferBetweenAccounts(int userIndex, double[][] accountsValue)
        {
            
        }

        // Dummy function to simulate withdrawing money
        static void WithdrawMoney()
        {

        }
    }
}