using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

namespace Search_and_Zip
{
    public class ControlClass
    {
        public static async Task EssentialControlFunction()
        {
            #region
            var botClient = new TelegramBotClient("6967745296:AAFjCk8LIpgwN1F41gOniXckRaFGJVPs2zc");
            using CancellationTokenSource cts = new();
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
            #endregion
            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                try
                {
                    if (update.Message.Type != MessageType.Text)
                    {
                        return;
                    }
                    if (update.Message.Text == "/start")
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "Assalomu Alekum",
                            cancellationToken: cancellationToken);
                    }
                    else if (!update.Message.Text.StartsWith("https://www.instagram.com"))
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: update.Message.Chat.Id,
                            text: "Noto'g'ri link",
                            cancellationToken: cancellationToken);
                        return;
                    }

                    if (update.Message.Text.StartsWith("https://www.instagram.com/reel"))
                    {
                        string replymessage = update.Message.Text.Replace("www.", "dd");
                        await botClient.SendVideoAsync(
                            chatId: update.Message.Chat.Id,
                            video: $"{replymessage}",
                            supportsStreaming: true,
                            cancellationToken: cancellationToken);
                    }
                    else
                    {
                        string replymessage = update.Message.Text.Replace("www.", "dd");
                        await botClient.SendPhotoAsync(
                            chatId: update.Message.Chat.Id,
                            photo: $"{replymessage}",
                            cancellationToken: cancellationToken);
                    }
                    
                }
                catch (Exception ex) { }
                
            }
            #region
            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
            #endregion
        }
    }
}
