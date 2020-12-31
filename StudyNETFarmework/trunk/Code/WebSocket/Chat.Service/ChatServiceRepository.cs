using Chat.Service.Core.Entities;
using Chat.Service.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service
{
    public class ChatServiceRepository : IChatServiceRepository
    {
        private static List<User> Users;

        private static List<ChatGroup> chatGroups;

        private static List<UserChatMessage> userChatMessages;
        private static List<ChatGroupMessage> chatGroupMessages;
        private static List<ChatGroupMessageReceiverUser> ChatGroupMessageReceiverUsers;

        static ChatServiceRepository()
        {
            var user1 = new User(Guid.Parse("D0517885-55AF-489D-B703-D5FBDB504CB4"), "用户1");
            var user2 = new User(Guid.Parse("9DA7F85F-8FE5-4DED-B677-29FDF73D627B"), "用户2");
            var user3 = new User(Guid.Parse("58B51CC0-AB69-421B-B149-85BCB74F31C8"), "用户3");
            var user4 = new User(Guid.Parse("0DB0544E-F576-4B51-A0CC-DAF2E3950965"), "用户4");
            var user5 = new User(Guid.Parse("AB8450C8-5778-4D46-9750-11AD774AA9AE"), "用户5");
            var user6 = new User(Guid.Parse("1778FFFC-4AC3-49D3-8138-4B3905F3E7C4"), "用户6");
            var user7 = new User(Guid.Parse("F3B96936-033B-4A94-93AA-D571277E53C1"), "用户7");
            var user8 = new User(Guid.Parse("44F11AD5-CCD1-492A-AECC-98EFF39CBA64"), "用户8");
            var user9 = new User(Guid.Parse("E785A2EA-B27E-4B79-A7B8-3E06A0D019E7"), "用户9");
            var user10 = new User(Guid.Parse("D943CF46-9B39-4A53-976D-F1696B2BE1BA"), "用户10");
            var user11 = new User(Guid.Parse("5F030EDF-0157-43C4-873B-D5D7A724518C"), "用户11");
            var user12 = new User(Guid.Parse("428EDC95-8B09-4F74-A098-0B41CC75FF85"), "用户12");
            var user13 = new User(Guid.Parse("98AFEF68-CA74-4F68-AF43-13F21C300484"), "用户13");
            var user14 = new User(Guid.Parse("A40A321B-B4D5-491B-B465-0A5F7BA3BD83"), "用户14");
            var user15 = new User(Guid.Parse("71A7E2CC-02C6-4938-80A5-9967011EBE4B"), "用户15");
            var user16 = new User(Guid.Parse("890C07DF-A346-496A-BAB9-C3EC621ED168"), "用户16");
            var user17 = new User(Guid.Parse("1B136ABF-B5CF-40F0-A218-C26B7CD3466F"), "用户17");
            var user18 = new User(Guid.Parse("0A181801-9A2F-4966-88A5-2F86FC926ABC"), "用户18");
            var user19 = new User(Guid.Parse("0EBFD1D2-F17A-4AA7-AE04-2D7B530FF09C"), "用户19");
            var user20 = new User(Guid.Parse("B2E5A63B-384F-488F-A4CE-11A0BD115990"), "用户20");
            var user21 = new User(Guid.Parse("9E9ED6D0-F2DE-4860-B81D-07E758F16A16"), "用户21");

            Users = new List<User> { user1, user2, user3, user4, user5, user6, user7, user8, user9, user10,
                user11, user12, user13, user14, user15, user16, user17, user18, user19, user20, user21, };

            var ChatGroup1 = new ChatGroup(Guid.Parse("E000CC86-6EB0-4A57-9749-EBAC366DE33A"), "群组1", new List<ChatGroupUser>());
            ChatGroup1.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user1));
            ChatGroup1.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user2));
            ChatGroup1.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user3));
            ChatGroup1.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user7));
            ChatGroup1.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user9));

            var ChatGroup2 = new ChatGroup(Guid.Parse("254F140D-2702-4CC0-ACB7-364B7DFCE3B1"), "群组2", new List<ChatGroupUser>());
            ChatGroup2.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user1));
            ChatGroup2.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user2));
            ChatGroup2.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user6));
            ChatGroup2.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user10));
            ChatGroup2.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user15));

            var ChatGroup3 = new ChatGroup(Guid.Parse("0802611E-5499-4623-A905-6F099A1B7A8D"), "群组3", new List<ChatGroupUser>());
            ChatGroup3.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user3));
            ChatGroup3.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user2));
            ChatGroup3.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user6));
            ChatGroup3.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user20));
            ChatGroup3.Users.Add(new ChatGroupUser(Guid.NewGuid(), ChatGroup1, user8));

            chatGroups = new List<ChatGroup> { ChatGroup1, ChatGroup2, ChatGroup3 };
            userChatMessages = new List<UserChatMessage>();
            chatGroupMessages = new List<ChatGroupMessage>();
            ChatGroupMessageReceiverUsers = new List<ChatGroupMessageReceiverUser>();
        }

        public ChatServiceRepository() { }
        public void AddChatGroupMessage(ChatGroupMessage chatGroupMessage)
        {
            chatGroupMessages.Add(chatGroupMessage);
        }

        public void AddChatGroupMessageReceiverUser(ChatGroupMessageReceiverUser chatGroupMessageReceiverUser)
        {
            ChatGroupMessageReceiverUsers.Add(chatGroupMessageReceiverUser);
        }

        public void AddUserChatMessage(UserChatMessage userChatMessage)
        {
            userChatMessages.Add(userChatMessage);
        }

        public void Dispose()
        {
            //userChatMessages.Clear();
            //chatGroupMessages.Clear();
            //ChatGroupMessageReceiverUsers.Clear();

            //userChatMessages = null;
            //chatGroupMessages = null;
            //ChatGroupMessageReceiverUsers = null;
        }

        public ChatGroupMessage GetChatGroupMessage(Guid id)
        {
            return chatGroupMessages.FirstOrDefault(p => p.ChatGroupMessageId.Equals(id));
        }

        public ChatGroupMessageReceiverUser GetChatGroupMessageReceiverUser(Guid id)
        {
            return ChatGroupMessageReceiverUsers.FirstOrDefault(p => p.ID.Equals(id));
        }

        public ChatGroupMessageReceiverUser GetChatGroupMessageReceiverUser(Guid chatGroupMessageid, Guid userId)
        {
            ChatGroupMessage chatGroupMessage = GetChatGroupMessage(chatGroupMessageid);
            return chatGroupMessage.Users.FirstOrDefault(p => p.ChatGroupUser.UserId.Equals(userId));
        }

        public IQueryable<ChatGroup> GetChatGroups()
        {
            return chatGroups.AsQueryable();
        }

        public IQueryable<ReceiveMessage> GetUnReadChatMessage(Guid UserId)
        {
            var unReadMessages1 = userChatMessages.Where(p => p.ReceiverId.Equals(UserId) && !p.IsRead)
                .Select(p => new ReceiveMessage(p.MessageId, p.SenderId, p.ReceiverId, MessageType.message, ReceiverType.individual, p.Message)).ToList();
            var unReadMessages2 = ChatGroupMessageReceiverUsers.Where(p => p.ChatGroupUser.UserId.Equals(UserId) && !p.IsRead)
                .Select(p => new ReceiveMessage(p.ChatGroupMessage.ChatGroupMessageId, p.ChatGroupMessage.SenderId, p.ChatGroupUser.UserId, MessageType.message, ReceiverType.group, p.ChatGroupMessage.Message)).ToList();

            unReadMessages1.AddRange(unReadMessages2);
            return unReadMessages1.AsQueryable();
        }

        public UserChatMessage GetUserChatMessage(Guid id)
        {
            return userChatMessages.FirstOrDefault(p => p.MessageId.Equals(id));
        }

        public IQueryable<User> GetUsers()
        {
            return Users.AsQueryable();
        }

        public void SaveChange()
        {
            /***
             * 
             * 将数据写入磁盘
             * 
             * */
        }

        public void UpdateChatGroupMessageReceiverUser(ChatGroupMessageReceiverUser chatGroupMessageReceiverUser)
        {
            var oldModel = ChatGroupMessageReceiverUsers.FirstOrDefault(p => p.ID.Equals(chatGroupMessageReceiverUser.ID));
            ChatGroupMessageReceiverUsers.Remove(oldModel);
            ChatGroupMessageReceiverUsers.Add(chatGroupMessageReceiverUser);
        }

        public void UpdateUserChatMessage(UserChatMessage userChatMessage)
        {
            var oldModel = userChatMessages.FirstOrDefault(p => p.MessageId.Equals(userChatMessage.MessageId));
            userChatMessages.Remove(oldModel);
            userChatMessages.Add(userChatMessage);
        }
    }
}
