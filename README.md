# VKBotABC
небольшая библиотека для создания примитивных ботов для вк на паскалеабц (вообще на любых .net'овских языках, но я думаю там есть ***нормальные*** библиотеки)

делал по приколу на коленке, чтобы на парах по программированию было чем заняться. 

# Использование
Качаешь последний релиз VKBotABC, Newtonsoft.JSON (работало нормально с 12.0.3), подключаешь.

Использование в PascalABC.Net:
```pascal
program vk_bot;

{$reference 'VKBotABC.dll'}

uses VKBotABC;

const 
    TOKEN = 'ifakurbullshitshit';// токен группы    
    GROUP_ID = 210000;// айди группы

begin
    var bot := new BotAbc(TOKEN, GROUP_ID);

    bot.Longpoll.OnNewMessage += procedure(ev) -> begin
        bot.Reply(ev, $'{ev.Message.Text}?<br>Да я тебя трахну за такие слова!');
    end;


    bot.Run();
end.
```