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

                            string? menuChoice = Console.ReadLine();

                            // Switch case to handle user choice
                            switch (menuChoice)
                            {
                                case "1":
                                    ShowAccounts(userIndex, accountsName, accountsValue);
                                    break;
                                case "2":
                                    accountsValue = TransferBetweenAccounts(userIndex, accountsName, accountsValue);
                                    break;
                                case "3":
                                    accountsValue = WithdrawMoney(userIndex, accountsName, accountsValue);
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

        // Function to transfer money between accounts
        static double[][] TransferBetweenAccounts(int userIndex, string[][] accountsName, double[][] accountsValue)
        {
            int numberOfAccounts = accountsValue[userIndex].Length;

            if(numberOfAccounts < 2)
            {
                Console.WriteLine("Du kan inte flytta pengar eftersom du inte har mer än ett konto");
                return accountsValue;
            }

            Console.WriteLine("Vilket konto vill du flytta pengar från?");
            string? inputAccountOne = Console.ReadLine();
            if(!int.TryParse(inputAccountOne, out int accountOneIndex))
            {
                Console.WriteLine("Du inmata inte ett nummer");
                return accountsValue;
            }

            Console.WriteLine("Vilket konto vill du flytta pengar till?");
            string? inputAccountTwo = Console.ReadLine();
            if (!int.TryParse(inputAccountTwo, out int accountTwoIndex))
            {
                Console.WriteLine("Du inmata inte ett nummer");
                return accountsValue;
            }

            if (accountOneIndex <= numberOfAccounts && accountTwoIndex <= numberOfAccounts && accountOneIndex > 0 && accountTwoIndex > 0) // Checks if user entered an account index that exists
            {
                // Asks user how much money to transfer and get the amount of money that is in the accounts
                Console.WriteLine("Hur mycket pengar vill du flytta?");
                string? inputValueMove = Console.ReadLine();
                if (!double.TryParse(inputValueMove, out double valueMove))
                {
                    Console.WriteLine("Du inmata inte ett nummer");
                    return accountsValue;
                }
                valueMove = Math.Round(valueMove, 2);

                double accountOneMoney = accountsValue[userIndex][accountOneIndex - 1];
                double accountTwoMoney = accountsValue[userIndex][accountTwoIndex - 1];

                if (valueMove <= accountOneMoney && valueMove > 0) // Checks if transfer is possible
                {
                    //Transfers money and updates the account values
                    accountOneMoney -= valueMove;
                    accountTwoMoney += valueMove;

                    accountsValue[userIndex][accountOneIndex - 1] = accountOneMoney;
                    accountsValue[userIndex][accountTwoIndex - 1] = accountTwoMoney;

                    Console.WriteLine($"Överföring lyckades! {valueMove:C} flyttades från {accountsName[userIndex][accountOneIndex - 1]} till {accountsName[userIndex][accountTwoIndex - 1]}");
                    return accountsValue;
                }
                else if(valueMove > accountOneMoney) // if-statements if user entered a invalid amount of money
                {
                    Console.WriteLine("Du kan inte flytta mer pengar än vad som finns");
                    return accountsValue;
                }
                else if(valueMove <= 0)
                {
                    Console.WriteLine("Du kan inte flytta 0 eller mindre pengar");
                    return accountsValue;
                }
                else
                {
                    Console.WriteLine("Det blev fel vid val av pengar att flytta");
                    return accountsValue;
                }
            }
            else if (accountOneIndex == accountTwoIndex) // if-statements if user entered wrong account
            {
                Console.WriteLine("Du kan inte flytta pengar till och från samma konto!");
                return accountsValue;
            }
            else if (accountOneIndex > numberOfAccounts || accountOneIndex <= 0)
            {
                Console.WriteLine("Du kan inte flytta pengar från ett konto som inte finns");
                return accountsValue;
            }
            else if (accountTwoIndex > numberOfAccounts || accountTwoIndex <= 0)
            {
                Console.WriteLine("Du kan inte flytta pengar till ett konto som inte finns");
                return accountsValue;
            }
            else
            {
                Console.WriteLine("Det blev fel vid val av konton");
                return accountsValue;
            }
        }

        // Function to withdrawing money
        static double[][] WithdrawMoney(int userIndex, string[][] accountsName, double[][] accountsValue)
        {
            int numberOfAccounts = accountsValue[userIndex].Length;

            Console.WriteLine("Vilket konto vill du ta ut pengar ifrån?");
            string? inputAccountIndex = Console.ReadLine();
            if (!int.TryParse(inputAccountIndex, out int accountIndex))
            {
                Console.WriteLine("Du inmata inte ett nummer");
                return accountsValue;
            }

            if (accountIndex <= numberOfAccounts && accountIndex > 0)
            {
                Console.WriteLine("Hur mycket pengar vill du ta ut?");
                string? inputValueWithdraw = Console.ReadLine();
                if (!double.TryParse(inputValueWithdraw, out double valueWithdraw))
                {
                    Console.WriteLine("Du inmata inte ett nummer");
                    return accountsValue;
                }
                valueWithdraw = Math.Round(valueWithdraw, 2);

                double accountMoney = accountsValue[userIndex][accountIndex - 1];

                if (valueWithdraw <= accountMoney && valueWithdraw > 0) // Checks if withdrawl is possible
                {
                    accountMoney -= valueWithdraw;
                    accountsValue[userIndex][accountIndex - 1] = accountMoney;

                    Console.WriteLine($"Uttag lyckades! {valueWithdraw:C} togs ut från {accountsName[userIndex][accountIndex - 1]}");
                    return accountsValue;
                }
                else if (valueWithdraw > accountMoney) // if-statements if user entered a invalid amount of money
                {
                    Console.WriteLine("Du kan inte ta ut mer pengar än vad som finns");
                    return accountsValue;
                }
                else if (valueWithdraw <= 0)
                {
                    Console.WriteLine("Du kan inte ta ut 0 eller mindre pengar");
                    return accountsValue;
                }
                else
                {
                    Console.WriteLine("Det blev fel vid val av pengar att flytta");
                    return accountsValue;
                }
            }
            else
            {
                Console.WriteLine("Du kan inte ta ut pengar från ett konto som inte finns");
                return accountsValue;
            }
        }
    }
}