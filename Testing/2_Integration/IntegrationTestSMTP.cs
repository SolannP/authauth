using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Testing._2_Integration;
public class IntegrationTestSMTP
{
    [SetUp]
    public void Setup()
    {

    }
    //[Test(Author = "S.PUYGRENIER")]
    public void SendEmail()
    {
        /*
        Outgoing Mail Server( for email sending )
        Hostname:  out.dnsexit.com
        Username:  youremail @yourdomain.com(remember to use your whole lower email address as username)  (welcome@spuygrenier.work.gd)
        Password:  xxxxx(the password you choose for the mail box)                                         (1241)
        Outbound SMTP Port: 25,  26,  80,  587,  940,  2525,  8001
        */
        /*
        var networkCredential = new NetworkCredential
        {
            UserName = mailArgs.MailFrom
            Password = mailArgs.Password,
        };*/

        MailMessage mm = new MailMessage();
        SmtpClient smtp = new SmtpClient();
        
        mm.From = new MailAddress("welcome@spuygrenier.work.gd", "Hi robot", System.Text.Encoding.UTF8);
        mm.To.Add(new MailAddress("puygrenier.solann@gmx.fr"));
        mm.Subject = "TestSubject";
        mm.Body = "<h1>Body</h1>";
        mm.IsBodyHtml = true;
        smtp.Host = "relay.dnsexit.com";// pop.dnsexit.com";
        /* relay.dnsexit.com
        if (ccAdd != "")
        {
            mm.CC.Add(ccAdd);
        }*/

        smtp.EnableSsl = false;
        //
        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
        NetworkCred.UserName = "welcome-spuygrenier.work.gd";//gmail user name
        NetworkCred.Password = "1241";// password
        smtp.UseDefaultCredentials = true;
        smtp.Credentials = NetworkCred;
        smtp.Port = 110; //Gmail port for e-mail 465 or 587
        smtp.Send(mm);
    }
}
