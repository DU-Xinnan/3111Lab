using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace SinExWebApp20256461
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
        //Instantiate a new MailMessage instance 
                MailMessage mailMessage = new MailMessage();
                //Add recipients 
                mailMessage.To.Add("gqi@connect.ust.hk");
                mailMessage.To.Add("xduac@connect.ust.hk");
                mailMessage.To.Add("zyuaf@connect.ust.hk");
                mailMessage.To.Add("swuai@connect.ust.hk");
                
                //Setting the displayed email address and display name
                //!!!Do not use this to prank others!!!
                mailMessage.From = new MailAddress("iLoveCKTang@cse.ust.hk","Wu Shangche");

                //Subject and content of the email
                mailMessage.Subject = "COMP3111: Activity 2 Graded";
                mailMessage.Body = "Dear COMP3111 Team 108 Members,\n \n Your Activity 2 has been graded, your group score is:\n 57 / 100\n Class Average: 89.7\nSD: 20.3\nClass Maximum: 100\n \n Prof.Leung\n Prof.Lochovsky";
                mailMessage.Priority = MailPriority.High;
                mailMessage.Attachments.Add(new Attachment("C:/Users/gqi/Downloads/graded-UseCaseDetailedSpecification.pdf"));

        //Instantiate a new SmtpClient instance
                SmtpClient smtpClient = new SmtpClient("smtp.cse.ust.hk");
                smtpClient.Credentials = new System.Net.NetworkCredential("comp3111_team108@cse.ust.hk","team108#");
                smtpClient.UseDefaultCredentials = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
        //Send
                smtpClient.Send(mailMessage);
                Response.Write("Email Sent!!! Yay!");
            }

            catch (Exception ex)
            {
                Response.Write("gg, Could not send the email -error" + ex.Message);
            }

        }
    }
}