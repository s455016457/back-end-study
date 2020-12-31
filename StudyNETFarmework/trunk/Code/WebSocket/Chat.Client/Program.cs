using System;

namespace Chat.Client
{
    class Program
    {
        static ChatServiceRepository chatServiceRepository = new ChatServiceRepository();
        static Core.Entities.ReceiverType ReceiverType;
        static Guid ReceiveId=default(Guid);
        static Core.ChatClient chatClient;
        static void Main(string[] args)
        {
            Console.WriteLine("请选择登陆用户");

            PrintUsers();

            int count = 0;
            string value = Console.ReadLine();

            while (!int.TryParse(value, out count))
            {
                Console.WriteLine("请选择正确的序号");
                value = Console.ReadLine();
            }

            var userKeyValue = chatServiceRepository.GetUsers()[count - 1];

            Console.Title = $"聊天【{userKeyValue.Value}】";

            try
            {
                chatClient = new Core.ChatClient(userKeyValue.Key, "123456", "127.0.0.1", 8081, "chat");
                chatClient.OnReceiveMessage += ChatClient_OnReceiveMessage;
                chatClient.OpenAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("登陆成功！");
            Console.WriteLine("请选择聊天对象，或输入help获取帮助，输入exit退出客户端");
            string message = Console.ReadLine();

            while (!message.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                switch (message)
                {
                    case "help":
                        Console.WriteLine("help                  获取帮助");
                        Console.WriteLine("exit                  退出客户端");
                        Console.WriteLine("getUsers              获取用户列表");
                        Console.WriteLine("getChatGroups         获取聊天群列表");
                        Console.WriteLine("selectUser count      选择某个用户为聊天对象 ");
                        Console.WriteLine("selectChatGroup count 选择某个聊天群为聊天对象 ");
                        break;
                    case "getUsers":
                        PrintUsers();
                        break;
                    case "getChatGroups":
                        PrintChatGroups();
                        break;
                    default:
                        if (message.StartsWith("selectUser "))
                            SelectUser(message);
                        else if (message.StartsWith("selectChatGroup "))
                            SelectChatGroup(message);
                        else
                        {
                            if (ReceiveId == default(Guid))
                                Console.WriteLine("请选择聊天对象");
                            else
                                chatClient.SendMessageAsync(ReceiveId, ReceiverType, message).Wait();
                        }
                        break;
                }
                message = Console.ReadLine();
            }
        }
        private static int GetCount(string command)
        {
            var strs = command.Split(' ');
            if (strs.Length < 2)
            {
                Console.WriteLine("命令不正确，请重新输入");
                return -1;
            }
            int count = -1;
            if (int.TryParse(strs[1], out count))
                return count;
            Console.WriteLine("命令不正确，请重新输入");
            return -1;
        }

        private static void SelectUser(string command)
        {
            int count = GetCount(command);
            if (count < 1) return;
            var userKeyValue = chatServiceRepository.GetUsers()[count - 1];
            ReceiverType = Core.Entities.ReceiverType.individual;
            ReceiveId = userKeyValue.Key;
            Console.WriteLine($"选择了聊天对象【{userKeyValue.Value}】");
        }
        private static void SelectChatGroup(string command)
        {
            int count = GetCount(command);
            if (count < 1) return;
            var chatGroupKeyValue = chatServiceRepository.GetChatGroup()[count - 1];
            ReceiverType = Core.Entities.ReceiverType.group;
            ReceiveId = chatGroupKeyValue.Key;
            Console.WriteLine($"选择了聊天对象【{chatGroupKeyValue.Value}】");
        }

        private static void PrintUsers()
        {
            int count = 1;
            foreach (var item in chatServiceRepository.GetUsers())
            {
                Console.WriteLine($"[{count++}]      {item.Key}      {item.Value}");
            }
        }

        private static void PrintChatGroups()
        {
            int count = 1;
            foreach (var item in chatServiceRepository.GetChatGroup())
            {
                Console.WriteLine($"[{count++}]      {item.Key}      {item.Value}");
            }
        }

        private static void ChatClient_OnReceiveMessage(Core.ChatClient client, Core.Entities.ReceiveMessage receiveMessage)
        {
            switch (receiveMessage.MessageType)
            {
                case Core.Entities.MessageType.erroe:
                    Console.WriteLine($"服务端返回错误：{receiveMessage.Message}");
                    break;
                case Core.Entities.MessageType.message:
                    Console.WriteLine($"{(receiveMessage.ReceiverType==Core.Entities.ReceiverType.group?"聊天群消息=>":"用户聊天消息=>")}" +
                        $"{receiveMessage.SenderId} 说：{receiveMessage.Message}");
                    client.SendMessageReadAsync(receiveMessage).Wait();
                    break;
                case Core.Entities.MessageType.readStatusChange:
                    Console.WriteLine($"服务端返回消息{receiveMessage.MessageId}标记为已读{receiveMessage.Message}");
                    break;
            }
        }
    }
}
