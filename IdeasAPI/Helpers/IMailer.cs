using System;
using System.Linq;
using System.Net.Mail;
using IdeasAPI.DataContexts;
using IdeasAPI.Models;
using NLog;
using System.Data.Entity;

namespace IdeasAPI.Helpers
{
    public interface IMailer
    {
        void InformAboutVote(Entry entry);
        void InformAboutComment(Comment comment);
    }

    public class Mailer : IMailer
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void InformAboutVote(Entry entry)
        {
            try
            {
                if (entry.User == null)
                {
                    return;
                }
                var destination = entry.User.Email;


                MailMessage message = new MailMessage();
                message.From = new MailAddress("ideas@nortal.com");

                message.To.Add(new MailAddress(destination));

                message.Subject = "You have a vote";
                message.Body = $"Someone voted for your idea '{entry.Title}'";

                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void InformAboutComment(Comment comment)
        {
            try
            {


                using (var db = new IdeasDb())
                {
                    var entry = db.Entries.Include(x=>x.User).First(x => x.Comments.Any(y => y.Id == comment.Id));

                    if (comment.UserId != entry.UserId)
                    {
                        NotifyAuthor(entry);
                    }
                    var interestedUsers = db.Users.Where(x => x.Comments.Any(y => y.Entry.Id == entry.Id) && x.Id != comment.UserId && x.Id != entry.UserId);

                    foreach (var ur in interestedUsers)
                    {
                        if (ur?.Email == null)
                        {
                            return;
                        }

                        var message = new MailMessage();
                        message.From = new MailAddress("ideas@nortal.com");

                        message.To.Add(new MailAddress(ur.Email));

                        message.Subject = "Comment on idea you have commented on too";
                        message.Body = $"There's a new comment on idea '{entry.Title}'";

                        var client = new SmtpClient();
                        client.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void NotifyAuthor(Entry entry)
        {
            User author = entry.User;
            if (author?.Email == null)
            {
                return;
            }
           
            MailMessage message = new MailMessage();
            message.From = new MailAddress("ideas@nortal.com");

            message.To.Add(new MailAddress(author.Email));

            message.Subject = "You have a comment";
            message.Body = $"You have a comment for your idea '{entry.Title}'";

            SmtpClient client = new SmtpClient();
            client.Send(message);

        }
    }
}