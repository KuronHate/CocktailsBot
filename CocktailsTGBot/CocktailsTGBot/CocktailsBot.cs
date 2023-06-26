using CocktailsTGBot.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace CocktailsTGBot
{
    public class CocktailsBot
    {
        private static string token = Constants.Token;
        TelegramBotClient botClient = new TelegramBotClient(token);
        CancellationToken cancellationToken = new CancellationToken(); 
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        Client client = new Client();
        Database db = new Database();
        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Бот {botMe.Username} почав працювати");
            Console.ReadKey();
        }

        
        private Task HandlerError(ITelegramBotClient botclient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMassage = exception switch
            {
                ApiRequestException apiRequestException => $"Помилка в телеграм бот API:\n{apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMassage);
            return Task.CompletedTask;
        }
        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text!=null)
            {
                await HandlerMessageAsync(botClient, update);
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Update update)
        {
            var message = update.Message;
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "The bot provides information about the necessary ingredients for making a cocktail:\n" +
                    "the ratio of ingredients, the preparation method and the optimal type of glass.");
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                    {
                        new KeyboardButton[] { "Find cocktail by name", "Find cocktail by ingrediente" },
                        new KeyboardButton[] { "Random cocktail"},
                        new KeyboardButton[] { "See all my data", "Delete all my data" }
                    }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Select a menu item:", replyMarkup: replyKeyboardMarkup);
                return;
            }
            if (message.Text == "Find cocktail by name")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter the drink:");
                var result = client.GetCocktailsByNameAsync(message.Text).Result;
                if (result.drinks == null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "No cocktails found. Try to write in English");
                }
                else
                {
                    foreach (var e in result.drinks)
                    {
                        PrintInfo(result);
                    }
                }
                return;
            }
            if (message.Text == "Find cocktail by ingrediente")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter the ingrediente:");
                var result = client.GetCocktailsByIngredienteAsync(message.Text).Result;
                if (result.drinks == null)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "No cocktails found. Try to write in English");
                }
                else
                {
                    foreach (var e in result.drinks)
                    {
                        PrintInfo(result);
                    }
                }
                return;
            }
            if (message.Text == "Random cocktail")
            {
                var result = client.GetCocktailRandomAsync().Result;
                PrintInfo(result);
                return;
            }
            if (message.Text == "See all my data")
            {
                var result = JsonConvert.SerializeObject(db.SelectStatisticsAsync().Result);
                if (result.Length == 2)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "No data");
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, result);
                    return;
                }
                return;
            }
            if (message.Text == "Delete all my data")
            {
                db.DeleteStatisticsAsync();
                await botClient.SendTextMessageAsync(message.Chat.Id, "All your data has been deleted");
                return;
            }
            async Task PrintInfo(Cocktail result)
            {
                for (int i = 0; i < result.drinks.Count; i++)
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                          result.drinks[i].StrDrink + "\n"
                        + "Glass type:" + result.drinks[i].StrGlass + "\n"
                        + "Ingredients:" + "\n"
                        + result.drinks[i].StrIngredient1 + ": " + result.drinks[i].StrMeasure1 + "\n"
                        + result.drinks[i].StrIngredient2 + ": " + result.drinks[i].StrMeasure2 + "\n"
                        + result.drinks[i].StrIngredient3 + ": " + result.drinks[i].StrMeasure3 + "\n"
                        + result.drinks[i].StrIngredient4 + ": " + result.drinks[i].StrMeasure4 + "\n"
                        + result.drinks[i].StrIngredient5 + ": " + result.drinks[i].StrMeasure5 + "\n"
                        + result.drinks[i].StrIngredient6 + ": " + result.drinks[i].StrMeasure6 + "\n"
                        + result.drinks[i].StrIngredient7 + ": " + result.drinks[i].StrMeasure7 + "\n"
                        + result.drinks[i].StrIngredient8 + ": " + result.drinks[i].StrMeasure8 + "\n"
                        + result.drinks[i].StrIngredient9 + ": " + result.drinks[i].StrMeasure9 + "\n"
                        + result.drinks[i].StrIngredient10 + ": " + result.drinks[i].StrMeasure10 + "\n"
                        + result.drinks[i].StrIngredient11 + ": " + result.drinks[i].StrMeasure11 + "\n"
                        + result.drinks[i].StrIngredient12 + ": " + result.drinks[i].StrMeasure12 + "\n"
                        + result.drinks[i].StrIngredient13 + ": " + result.drinks[i].StrMeasure13 + "\n"
                        + result.drinks[i].StrIngredient14 + ": " + result.drinks[i].StrMeasure14 + "\n"
                        + result.drinks[i].StrIngredient15 + ": " + result.drinks[i].StrMeasure15 + "\n"
                        + result.drinks[i].StrInstructions);
                }
            }

        }
    }
}
