using System;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace dTools
{
    /// <summary>
    /// 邮件帮助类
    /// </summary>
    public  class MailHelper
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailServer">邮件服务器地址 例如: smtp.qq.com </param>
        /// <param name="mailUserName">用户名 例如:397706388@qq.com</param>
        /// <param name="mailPassword">密码 一般是客户端授权码,各大邮箱后台设置开启smtp生成授权码</param>
        /// <param name="mailName">显示名称 例如:【zz科技】</param>
        /// <param name="to">收件人邮箱地址 例如:abc@qq.com</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="encoding">编码</param>
        /// <param name="isBodyHtml">是否Html</param>
        /// <param name="enableSsl">是否SSL加密连接</param>
        /// <returns>是否成功</returns>
        public static bool Send(string mailServer, string mailUserName, string mailPassword, string mailName, string to, string subject, string body, string encoding = "UTF-8", bool isBodyHtml = true, bool enableSsl = false)
        {
            try
            {

                MailMessage message = new MailMessage();
                // 接收人邮箱地址
                message.To.Add(new MailAddress(to));
                message.From = new MailAddress(mailUserName, mailName);
                message.BodyEncoding = Encoding.GetEncoding(encoding);
                message.Body = body;
                //GB2312
                message.SubjectEncoding = Encoding.GetEncoding(encoding);
                message.Subject = subject;
                message.IsBodyHtml = isBodyHtml;

                SmtpClient smtpclient = new SmtpClient(mailServer, 25)
                {
                    Credentials = new System.Net.NetworkCredential(mailUserName, mailPassword),
                    //SSL连接
                    EnableSsl = enableSsl
                };
                smtpclient.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailServer">邮件服务器地址 例如: smtp.qq.com </param>
        /// <param name="mailUserName">用户名 例如:397706388@qq.com</param>
        /// <param name="mailPassword">密码 一般是客户端授权码,各大邮箱后台设置开启smtp生成授权码</param>
        /// <param name="mailName">显示名称 例如:【zz科技】</param>
        /// <param name="to">收件人邮箱地址 例如:abc@qq.com</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="encoding">编码</param>
        /// <param name="isBodyHtml">是否Html</param>
        /// <param name="enableSsl">是否SSL加密连接</param>
        /// <returns>是否成功</returns>
        public static void SendAsync(string mailServer, string mailUserName, string mailPassword, string mailName, string to, string subject, string body, string encoding = "UTF-8", bool isBodyHtml = true, bool enableSsl = false)
        {
            string callMsg = string.Empty;
            new Thread(new ThreadStart(delegate ()
            {
                Send(mailServer, mailUserName, mailPassword, mailName, to, subject, body, encoding, isBodyHtml, enableSsl);
            })).Start();
        }
    }
}
