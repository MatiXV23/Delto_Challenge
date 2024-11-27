
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Library.BankSim;
using TelegramBot.Library.Bot;
using TelegramBot.Library.ChatGPT;

namespace Program
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var bot = new TelBot();
            await bot.Iniciate();


            //////////////////////////////////////////////////////////////////////////////
            // CODIGO EJEMOPLO PARA AGREGAR UN NUEVO USUARIO A LA PSEUDO "BASE DE DATOS"//
            //////////////////////////////////////////////////////////////////////////////

            /*var mati = new BankUser() 
            {
                Name = "Matias Perez",
                Username = "matias.perez",
                Password = "clave1234",

                Accounts = new List<Account>()
                {
                    new Account()
                    {
                        AccountID = "matiasp2218340",
                        Balance = 7800,
                        Currency = ECurrency.Pesos,
                        Cards = new List<Card>()
                        {
                            new Card()
                            {
                                AccountID = "matiasp2218340",
                                Number = 4219200398571113,
                                Moves = new List<Transaction>()
                                {
                                    new Transaction()
                                    {
                                        Import = 300,
                                        Currency = ECurrency.Pesos,
                                        PayeeAccountID = "juanp221304",
                                        PayerAccountID = "matiasp2218340",
                                        Date = "16/1/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 150,
                                        Currency = ECurrency.Pesos,
                                        PayeeAccountID = "matiasp2218340",
                                        PayerAccountID = "pedror32018",
                                        Date = "22/1/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 300,
                                        Currency = ECurrency.Pesos,
                                        PayeeAccountID = "H&M5554422",
                                        PayerAccountID = "matiasp2218340",
                                        Date = "9/2/24"
                                    }

                                }
                            },

                            new Card()
                            {
                                AccountID = "matiasp2218340",
                                Number = 3003233298871116,
                                Moves = new List<Transaction>()
                                {
                                    new Transaction()
                                    {
                                        Import = 3000,
                                        Currency = ECurrency.Pesos,
                                        PayeeAccountID = "clarap2245452",
                                        PayerAccountID = "matiasp2218340",
                                        Date = "9/2/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 1500,
                                        Currency = ECurrency.Pesos,
                                        PayeeAccountID = "matiasp2218340",
                                        PayerAccountID = "jorger43423",
                                        Date = "9/2/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 600,
                                        Currency = ECurrency.Pesos,
                                        PayeeAccountID = "H&M5554422",
                                        PayerAccountID = "matiasp2218340",
                                        Date = "12/2/24"
                                    }

                                }
                            }
                        },

                        Moves = new List<Transaction>()
                        {
                                new Transaction()
                                {
                                    Import = 1000,
                                    Currency = ECurrency.Pesos,
                                    PayeeAccountID = "clarap2245452",
                                    PayerAccountID = "matiasp2218340",
                                    Date = "9/1/24"
                                },
                                new Transaction()
                                {
                                    Import = 1200,
                                    Currency = ECurrency.Pesos,
                                    PayeeAccountID = "matiasp2218340",
                                    PayerAccountID = "jorger43423",
                                    Date = "9/2/24"
                                },


                        }
                    },
                    new Account()
                    {
                        AccountID = "matip24343132",
                        Balance = 78,
                        Currency = ECurrency.Dollars,
                        Cards = new List<Card>()
                        {
                            new Card()
                            {
                                AccountID = "matip24343132",
                                Number = 4219200398571113,
                                Moves = new List<Transaction>()
                                {
                                    new Transaction()
                                    {
                                        Import = 30,
                                        Currency = ECurrency.Dollars,
                                        PayeeAccountID = "juanp221304",
                                        PayerAccountID = "matip24343132",
                                        Date = "16/1/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 15,
                                        Currency = ECurrency.Dollars,
                                        PayeeAccountID = "matip24343132",
                                        PayerAccountID = "pedror32018",
                                        Date = "22/1/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 30,
                                        Currency = ECurrency.Dollars,
                                        PayeeAccountID = "H&M5554422",
                                        PayerAccountID = "matip24343132",
                                        Date = "9/2/24"
                                    }

                                }
                            },

                            new Card()
                            {
                                AccountID = "matip24343132",
                                Number = 3003233298871116,
                                Moves = new List<Transaction>()
                                {
                                    new Transaction()
                                    {
                                        Import = 30,
                                        Currency = ECurrency.Dollars,
                                        PayeeAccountID = "clarap2245452",
                                        PayerAccountID = "matip24343132",
                                        Date = "9/2/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 15,
                                        Currency = ECurrency.Dollars,
                                        PayeeAccountID = "matip24343132",
                                        PayerAccountID = "jorger43423",
                                        Date = "9/2/24"
                                    },
                                    new Transaction()
                                    {
                                        Import = 600,
                                        Currency = ECurrency.Dollars,
                                        PayeeAccountID = "H&M5554422",
                                        PayerAccountID = "matip24343132",
                                        Date = "12/2/24"
                                    }

                                }
                            }
                        },

                        Moves = new List<Transaction>()
                        {
                                new Transaction()
                                {
                                    Import = 10,
                                    Currency = ECurrency.Dollars,
                                    PayeeAccountID = "clarap2245452",
                                    PayerAccountID = "matip24343132",
                                    Date = "9/1/24"
                                },
                                new Transaction()
                                {
                                    Import = 12,
                                    Currency = ECurrency.Dollars,
                                    PayeeAccountID = "matip24343132",
                                    PayerAccountID = "jorger43423",
                                    Date = "9/2/24"
                                },


                        }
                    }
                }
            };
            var julio = new BankUser()
            {
                Name = "Julio Rodriguez",
                Username = "julio.rodriguez",
                Password = "clav232231",
            };

            var users = new List<BankUser>();
            users.Add(mati);
            users.Add(julio);
            JsonAdapter.RewriteUsersOfDB(users);*/

            
            //////////////////////////////////////////////////////////////////////////////////
            // FIN CODIGO EJEMOPLO PARA AGREGAR UN NUEVO USUARIO A LA PSEUDO "BASE DE DATOS"//
            //////////////////////////////////////////////////////////////////////////////////
        }
    }
}

