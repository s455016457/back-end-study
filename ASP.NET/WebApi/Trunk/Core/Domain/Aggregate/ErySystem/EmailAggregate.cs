using Domain.Emtity.ErySystem;
using Domain.Repository.Interface.ErySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregate.ErySystem
{

    /**
     * 写邮件和收邮件应该是两个集合吗？
     * */
    public class EmailAggregate
    {
        private int _nId;
        private EmailNote _emailNote;
        private IList<EmailInBox> _emailInBox;

        internal EmailAggregate(int Nid,String SendBy, EmailNote emailNote, IList<EmailInBox> emailInBox)
        {
            _nId = Nid;
            _emailNote = emailNote;
            _emailInBox = emailInBox;
        }

        /// <summary>
        /// 更改收件人
        /// </summary>
        /// <param name="Recipient">以分号分隔的收件人字符串</param>
        /// <param name="CopyRecipient">以分号分隔的抄收人字符串</param>
        /// <param name="Subject">主题</param>
        /// <param name="Message">邮件内容</param>
        public void UpdateEmailNote(String Recipient, String CopyRecipient,String Subject,String Message)
        {
            if (_emailNote.WasSent) throw new Exception("邮件已发出，不能修改！");
            _emailNote.Recipient = FormatRecipient(Recipient);
            _emailNote.CopyRecipients = FormatRecipient(CopyRecipient);
            _emailNote.Subject = Subject;
            _emailNote.Message = Message;
            _emailNote.UpdatedStamp = DateTime.Now;
        }
        /// <summary>
        /// 重要级别高
        /// </summary>
        public void ImpriorityHighLarge()
        {
            if (_emailNote.WasSent) throw new Exception("邮件已发出，不能更改重要级别！");
            _emailNote.Priority = 99;
        }

        /// <summary>
        /// 重要级别低
        /// </summary>
        public void ImportanceLowLarge()
        {
            if (_emailNote.WasSent) throw new Exception("邮件已发出，不能更改重要级别！");
            _emailNote.Priority = 0;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        public void Send()
        {
            if (_emailNote.WasSent) throw new Exception("邮件已发送,不能重复发送！");
            _emailNote.SendStamp = DateTime.Now;
            _emailNote.WasSent = true;
            foreach (var item in _emailNote.RecipientUids)
            {
                //验证用户存在
                _emailInBox.Add(new EmailInBox()
                {
                    CreatedStamp = DateTime.Now,
                    Delete = false,
                    Folder = "InBox",
                    IsNewEmail = true,
                    Nid = _nId,
                    Opened = false,
                    RecDate = DateTime.Now,
                    Recipient = item,
                    Status = 0,
                    WasRead = false
                });
            }
        }



        /// <summary>
        /// 格式化收件人
        /// </summary>
        /// <param name="Recipient"></param>
        /// <returns></returns>
        public String FormatRecipient(String Recipient)
        {
            return RecipientUidsToRecipient(RecipientToRecipientUids(Recipient));
        }

        private String RecipientUidsToRecipient(IList<String> recipientUids)
        {
            StringBuilder sb = new StringBuilder();
            var isFirst = true;
            foreach (var item in recipientUids)
            {
                if (!isFirst)
                {
                    sb.Append(";");
                }
                //var Users=userRepository.FindUsers(item);
                //if(Users!=null)
                //sb.AppendFormat("{0}<{1}>",Users.Name,Users.Uid);
                //else
                sb.Append(item);
                isFirst = false;
            }
            return sb.ToString();
        }

        private IList<String> RecipientToRecipientUids(String Recipient)
        {
            IList<String> recipientUids = null;
            if (Recipient != null)
            {
                recipientUids = new List<String>();
                string strselectedAutoComplete = Recipient.TrimEnd(';');
                string[] arrayselectedAutoComplete = strselectedAutoComplete.Split(';');
                for (int i = 0; i < arrayselectedAutoComplete.Length; i++)
                {
                    if (arrayselectedAutoComplete[i].IndexOf('<', 0) > 0)
                    {
                        recipientUids.Add(arrayselectedAutoComplete[i].Substring(arrayselectedAutoComplete[i].IndexOf('<', 0) + 1, arrayselectedAutoComplete[i].IndexOf('>', 0) - arrayselectedAutoComplete[i].IndexOf('<', 0) - 1).Trim());
                    }
                }
            }
            return recipientUids;
        }
    }
}
