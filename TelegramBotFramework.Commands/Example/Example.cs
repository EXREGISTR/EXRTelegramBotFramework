using Telegram.Bot;
using TelegramBotFramework.Commands.States;
using TelegramBotFramework.Extensions;

namespace TelegramBotFramework.Commands.Example {


    public class QuestData {
        public string Username { get; private set; }
    }

    public class StartingQuest : ICommandState<QuestData> {
        public Task Execute(CommandStateContext<QuestData> context, ICommandStateChanger<QuestData> stateChanger) {
            var text =
                $"Салам алейкум! С прибытием в Дагестан, {context.Sender.GetUsername()}!\n";

            stateChanger.Next<SendOtherName>();
            return context.Bot.SendTextMessageAsync(context.Chat, text);
        }
    }

    public class SendOtherName : ICommandState<QuestData>, IBeforeEnteringStateHandler<QuestData> {
        public Task BeforeEnter(CommandStateContext<QuestData> context) {
            var text =
                $"Но можешь представиться по другому!\n\n" +
                $"Представься...";

            return context.Bot.SendTextMessageAsync(context.Chat, text);
        }

        public Task Execute(CommandStateContext<QuestData> context, ICommandStateChanger<QuestData> stateChanger) {
            var message = context.Message;
            var bot = context.Bot;
            if (message.Text == null) {
                var text = "Ты давай хуней не майся, скажи как звать";
                return bot.SendTextMessageAsync(context.Chat, text);
            }

            var name = message.Text!;
            if (name.Length > 14) {
                return bot.SendTextMessageAsync(context.Chat,
                    "Ну такое длинное мы не запомним. " +
                    "Максимум 14 букв и мы в расчете и я приму тебя");
            }

            stateChanger.Next<Lore>();

            return bot.SendTextMessageAsync(context.Chat,
                $"Ну и ну! Не зря тебя назвали {context.Sender.GetUsername()}! Такое только ты мог придумать" +
                $"Так уж и быть, буду звать тебя {name}");
        }
    }

    public class Lore : ICommandState<QuestData> {
        public Task Execute(CommandStateContext<QuestData> context, ICommandStateChanger<QuestData> stateChanger) {
            throw new NotImplementedException();
        }
    }
}
