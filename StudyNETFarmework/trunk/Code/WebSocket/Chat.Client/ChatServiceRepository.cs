using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Client
{
    public class ChatServiceRepository 
    {
        private static List<KeyValuePair<Guid,string>> Users;

        private static List<KeyValuePair<Guid, string>> chatGroups;

        static ChatServiceRepository()
        {
            var user1 = new KeyValuePair<Guid, string>(Guid.Parse("D0517885-55AF-489D-B703-D5FBDB504CB4"), "用户1");
            var user2 = new KeyValuePair<Guid, string>(Guid.Parse("9DA7F85F-8FE5-4DED-B677-29FDF73D627B"), "用户2");
            var user3 = new KeyValuePair<Guid, string>(Guid.Parse("58B51CC0-AB69-421B-B149-85BCB74F31C8"), "用户3");
            var user4 = new KeyValuePair<Guid, string>(Guid.Parse("0DB0544E-F576-4B51-A0CC-DAF2E3950965"), "用户4");
            var user5 = new KeyValuePair<Guid, string>(Guid.Parse("AB8450C8-5778-4D46-9750-11AD774AA9AE"), "用户5");
            var user6 = new KeyValuePair<Guid, string>(Guid.Parse("1778FFFC-4AC3-49D3-8138-4B3905F3E7C4"), "用户6");
            var user7 = new KeyValuePair<Guid, string>(Guid.Parse("F3B96936-033B-4A94-93AA-D571277E53C1"), "用户7");
            var user8 = new KeyValuePair<Guid, string>(Guid.Parse("44F11AD5-CCD1-492A-AECC-98EFF39CBA64"), "用户8");
            var user9 = new KeyValuePair<Guid, string>(Guid.Parse("E785A2EA-B27E-4B79-A7B8-3E06A0D019E7"), "用户9");
            var user10 = new KeyValuePair<Guid, string>(Guid.Parse("D943CF46-9B39-4A53-976D-F1696B2BE1BA"), "用户10");
            var user11 = new KeyValuePair<Guid, string>(Guid.Parse("5F030EDF-0157-43C4-873B-D5D7A724518C"), "用户11");
            var user12 = new KeyValuePair<Guid, string>(Guid.Parse("428EDC95-8B09-4F74-A098-0B41CC75FF85"), "用户12");
            var user13 = new KeyValuePair<Guid, string>(Guid.Parse("98AFEF68-CA74-4F68-AF43-13F21C300484"), "用户13");
            var user14 = new KeyValuePair<Guid, string>(Guid.Parse("A40A321B-B4D5-491B-B465-0A5F7BA3BD83"), "用户14");
            var user15 = new KeyValuePair<Guid, string>(Guid.Parse("71A7E2CC-02C6-4938-80A5-9967011EBE4B"), "用户15");
            var user16 = new KeyValuePair<Guid, string>(Guid.Parse("890C07DF-A346-496A-BAB9-C3EC621ED168"), "用户16");
            var user17 = new KeyValuePair<Guid, string>(Guid.Parse("1B136ABF-B5CF-40F0-A218-C26B7CD3466F"), "用户17");
            var user18 = new KeyValuePair<Guid, string>(Guid.Parse("0A181801-9A2F-4966-88A5-2F86FC926ABC"), "用户18");
            var user19 = new KeyValuePair<Guid, string>(Guid.Parse("0EBFD1D2-F17A-4AA7-AE04-2D7B530FF09C"), "用户19");
            var user20 = new KeyValuePair<Guid, string>(Guid.Parse("B2E5A63B-384F-488F-A4CE-11A0BD115990"), "用户20");
            var user21 = new KeyValuePair<Guid, string>(Guid.Parse("9E9ED6D0-F2DE-4860-B81D-07E758F16A16"), "用户21");

            Users = new List<KeyValuePair<Guid, string>> { user1, user2, user3, user4, user5, user6, user7, user8, user9, user10,
                user11, user12, user13, user14, user15, user16, user17, user18, user19, user20, user21, };

            var ChatGroup1 = new KeyValuePair<Guid, string>(Guid.Parse("E000CC86-6EB0-4A57-9749-EBAC366DE33A"), "聊天群1");
            var ChatGroup2 = new KeyValuePair<Guid, string>(Guid.Parse("254F140D-2702-4CC0-ACB7-364B7DFCE3B1"), "聊天群2");
            var ChatGroup3 = new KeyValuePair<Guid, string>(Guid.Parse("0802611E-5499-4623-A905-6F099A1B7A8D"), "聊天群3");

            chatGroups = new List<KeyValuePair<Guid, string>> { ChatGroup1, ChatGroup2, ChatGroup3 };
        }

        public ChatServiceRepository() { }

        public List<KeyValuePair<Guid, string>> GetUsers()
        {
            return Users;
        }

        public List<KeyValuePair<Guid, string>> GetChatGroup()
        {
            return chatGroups;
        }
    }
}
