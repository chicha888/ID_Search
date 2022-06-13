using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_BOT
{
    class Program
    {
        private static int state = 0;
        private static string language = "🇺🇦 Українська";
        private static string token = "5427825359:AAGhB-HqU9YoACLGYrlk1JxS3I_YM0m89zg";
        private static TelegramBotClient client;
        private static string pathToDatabase = "../../../PassportDB";
        private static string pathToSearchHistory = "../../../SearchHistory.txt";
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;
            if (!Passport.DirectoryValidation(pathToDatabase)) return;
            if (!Passport.FileValidation(pathToSearchHistory)) return;
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();
        }
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            string infoMenu;
            Passport passport;
            var msg = e.Message;
            switch (msg.Text)
            {
                case "/start":
                    if (language == "🇺🇦 Українська")
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id, "Оберіть опцію", replyMarkup: MainMenuButtons());
                    }
                    else
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id, "Choose the option", replyMarkup: MainMenuButtons());
                    }
                    break;
                case "🔍 Знайти паспорт":
                    await client.SendTextMessageAsync(msg.Chat.Id, "Введіть номер паспорту (наприклад, 798601):", replyMarkup: SearchMenuButtons());
                    state = 1;
                    break;
                case "🔍 Find a passport":
                    await client.SendTextMessageAsync(msg.Chat.Id, "Input passport number (for example, 798601):", replyMarkup: SearchMenuButtons());
                    state = 1;
                    break;
                case "📓 Інформаційне меню":
                    infoMenu = "🤖 Цього бота створено для пошуку вкраденого/втраченого/недійсного паспорту.\n\n" +
                        "🔍 Натисніть кнопку \"Знайти паспорт\" для здійснення пошуку\n" +
                        "📓 Натисніть кнопку \"Інформаційне меню\" для перегляду інформаційного меню та інструкції щодо користування\n" +
                        "🔃 Натисніть кнопку \"Змінити мову\" для зміни мови боту\n";
                    await client.SendTextMessageAsync(msg.Chat.Id, infoMenu);
                    break;
                case "📓 Information menu":
                    infoMenu = "🤖 This bot was created to search for a stolen/lost/invalid passport.\n\n" +
                        "🔍 Press the button \"Find a passport\" to search.\n" +
                        "📓 Press the button \"Information menu\" to output information menu and instructions for use. \n" +
                        "🔃 Press the button \"Change the language\" to change the bot language\n";
                    await client.SendTextMessageAsync(msg.Chat.Id, infoMenu);
                    break;
                case "🔃 Змінити мову":
                    await client.SendTextMessageAsync(msg.Chat.Id, "Оберіть мову", replyMarkup: LanguageMenuButtons());
                    state = 2;
                    break;
                case "🔃 Change the language":
                    await client.SendTextMessageAsync(msg.Chat.Id, "Choose the language", replyMarkup: LanguageMenuButtons());
                    state = 2;
                    break;
                case "↪️ Повернутися назад":
                    await client.SendTextMessageAsync(msg.Chat.Id, "Оберіть опцію", replyMarkup: MainMenuButtons());
                    state = 0;
                    break;
                case "↪️ Back":
                    await client.SendTextMessageAsync(msg.Chat.Id, "Choose the option", replyMarkup: MainMenuButtons());
                    state = 0;
                    break;
                case "🇺🇦 Українська":
                    language = "🇺🇦 Українська";
                    await client.SendTextMessageAsync(msg.Chat.Id, "Оберіть опцію", replyMarkup: MainMenuButtons());
                    state = 0;
                    break;
                case "🇬🇧 English":
                    language = "🇬🇧 English";
                    await client.SendTextMessageAsync(msg.Chat.Id, "Choose the option", replyMarkup: MainMenuButtons());
                    state = 0;
                    break;
                default:
                    if (state == 1)
                    {
                        string messageText = msg.Text;
                        passport = Passport.FindPassport(messageText, pathToDatabase);
                        if (passport != null)
                        {
                            if (language == "🇺🇦 Українська")
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "📜 Інформація щодо паспорта:\n" + passport.ToStringUkr());
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "📜 Passport information:\n" + passport.ToStringEng());
                            }
                            Console.WriteLine("Вдала спроба пошуку паспорту за номером: " + messageText);
                            Passport.WriteResultToFile(messageText, pathToSearchHistory, true);
                        }
                        else
                        {
                            if (language == "🇺🇦 Українська")
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "‼️ Паспорт з таким номером не знайдено у базі даних! Перевірте номер та " +
                                "спробуйте знову!");
                            }
                            else
                            {
                                await client.SendTextMessageAsync(msg.Chat.Id, "‼️ Passport by this number is not found in database! Check number and" +
                                    "try again");
                            }
                            Console.WriteLine("Невдала спроба пошуку паспорту за номером: " + messageText);
                            Passport.WriteResultToFile(messageText, pathToSearchHistory, false);
                        }
                        if (language == "🇺🇦 Українська")
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, "Введіть номер паспорту (наприклад, 798601):", replyMarkup: SearchMenuButtons());
                        }
                        else
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, "Input passport number (for example, 798601):", replyMarkup: SearchMenuButtons());
                        }
                    }
                    else
                    {
                        await client.SendTextMessageAsync(msg.Chat.Id, "❌ Некоректна команда! ❌");
                    }
                    break;
            }
        }
        private static IReplyMarkup MainMenuButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup;
            if (language == "🇺🇦 Українська")
            {
                replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { new KeyboardButton { Text= "🔍 Знайти паспорт" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "📓 Інформаційне меню" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "🔃 Змінити мову" } }
                })
                {
                    ResizeKeyboard = true
                };
            }
            else
            {
                replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { new KeyboardButton { Text= "🔍 Find a passport" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "📓 Information menu" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "🔃 Change the language" } }
                })
                {
                    ResizeKeyboard = true
                };
            }
            return replyKeyboardMarkup;
        }
        private static IReplyMarkup LanguageMenuButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup;
            if (language == "🇺🇦 Українська")
            {
                replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { new KeyboardButton { Text= "🇺🇦 Українська" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "🇬🇧 English" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "↪️ Повернутися назад" } }
                })
                {
                    ResizeKeyboard = true
                };
            }
            else
            {
                replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { new KeyboardButton { Text= "🇺🇦 Українська" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "🇬🇧 English" } },
                    new KeyboardButton[] { new KeyboardButton { Text= "↪️ Back" } }
                })
                {
                    ResizeKeyboard = true
                };
            }
            return replyKeyboardMarkup;

        }
        private static IReplyMarkup SearchMenuButtons()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup;
            if (language == "🇺🇦 Українська")
            {
                replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { new KeyboardButton { Text= "↪️ Повернутися назад" } }
                })
                {
                    ResizeKeyboard = true
                };
            }
            else
            {
                replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { new KeyboardButton { Text= "↪️ Back" } }
                })
                {
                    ResizeKeyboard = true
                };
            }
            return replyKeyboardMarkup;
        }
    }
}
