using SignalRExercises.DDD.Comm;
using SignalRExercises.DDD.Entities;
using SignalRExercises.DDD.Repositories;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SignalRExercises.DDD.Services
{
    public class UserService: IService
    {
        ISysRepository sysRepository;

        public UserService(ISysRepository sysRepository)
        {
            this.sysRepository = sysRepository;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public User CrateUser(string userId,string userName,string password)
        {
            if (sysRepository.Users.Any(p => p.UserId.Equals(userId)))
                throw new Exception($"用户{userId}已存在！");

            var user = User.CreateNew(userId, userName, EncryptPassword(password));

            sysRepository.Users.Add(user);
            sysRepository.SaveChanges();
            return user;
        }

        public void Dispose()
        {
            if (sysRepository != null)
                sysRepository.Dispose();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string ResetPassword(string userId)
        {
            string password = Utility.GetRandomString(8);
            string encryPassword = EncryptPassword(password);
            User user = sysRepository.Users.FirstOrDefault(p => p.UserId.Equals(userId));

            if (user == null)
                throw new Exception($"用户{userId}不存在！");

            user.UpdatePassword(user.Password, encryPassword);
            sysRepository.SaveChanges();
            return password;
        }
        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool VerifyUserPassword(string userId,string password)
        {
            User user = sysRepository.Users.FirstOrDefault(p => p.UserId.Equals(userId));

            string encryPassword = EncryptPassword(password);
            if (user == null)
                throw new Exception($"用户{userId}不存在！");

            return user.Password.Equals(encryPassword);
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string EncryptPassword(string password)
        {
            password = $"aaa{password}bbb";
            password = GetMd5Hash(MD5.Create(), password);
            return password;
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            // 将输入字符串转换为字节数组并计算散列。
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 创建一个新的Stringbuilder来收集字节并创建一个字符串。
            StringBuilder sBuilder = new StringBuilder();

            // 循环遍历哈希数据的每个字节，并将每个字节格式化为十六进制字符串。
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // 返回十六进制字符串。
            return sBuilder.ToString();
        }
    }
}