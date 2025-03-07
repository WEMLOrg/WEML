﻿using System.Net.Mail;
using System.Net;
using WEML.Models;
using Microsoft.CodeAnalysis.CSharp;
using WEML.Areas.Identity.Data;
using Microsoft.Identity.Client;
using System.Net.Http;
// using Azure.Communication;
// using Azure.Communication.Sms;
using System.Text.Json;
using WEML.Repos;

namespace WEML.Service
{
    public class SendStatusService
    {
        private class SmsResult
        {
            public string status { get; set; }
        }
        private List<Symptom> symptoms;
        private String subject;
        private String diagnosys;
        private String userName;
        private String recipientEmail;
        private String recipientNumber;
        private readonly SymptomsRepo _symptomsRepo;
        private readonly DiagnosisEngine _diagnosisEngine;


        public SendStatusService(User user, List<Symptom> symptoms)
        {
            this.symptoms = symptoms;
            this.diagnosys = " the client something";       
            this.userName = user.FirstName + " " + user.LastName;

            this.recipientEmail = user.ContactDoctorEmail;  
            this.subject = "Health Update On Pacient " + userName;
            SendToDoctor();

            this.recipientNumber = user.ContactPersonEmail;
            SendToFamily();

        }

        //public async Task SendSmsAsync(string to, string message)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl);

        //    request.Content = JsonContent.Create(new
        //    {
        //        apikey = AccountSid,
        //        senderid = "WEML",
        //        text = message,
        //        numbers = to
        //    });

        //    try
        //    {
        //        var response = await _httpClient.SendAsync(request);
        //        response.EnsureSuccessStatusCode();

        //        var responseBody = await response.Content.ReadAsStringAsync();
        //        var result = JsonSerializer.Deserialize<SmsResult>(responseBody);

        //        Console.WriteLine($"Message sent successfully. Status: {result.status}");
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        Console.WriteLine($"Error sending SMS: {ex.Message}");
        //    }
        //}

        public async void SendToFamily()
        {
            
            //string connectionString = "endpoint=https://wemlresource.europe.communication.azure.com/;accesskey=8IeJ2KlkFhevlQbAStwBYjbQFnYvTE4JpUjL8V5NzNSTFMVAh9QyJQQJ99ALACULyCpH2AOLAAAAAZCS9Mld";
            // SmsClient smsClient = new SmsClient(connectionString);
            // smsClient.Send(
            //    from: new PhoneNumber("0723210410"),
            //    to: new PhoneNumber(recipientNumber),
            //    message: body
            // );
            
            String body = "Hello! \n We are contacting you on behalf of the WEML app, regarding the physical and mental well-being of our user " + this.userName + " They have reported \n ";
            foreach (Symptom s in symptoms)
            {
                body += "on " + s.DateTime + ": " + s.SymptomName + " with severity " + s.Severity + "\n";
            }
            body = body + "\n Based on the new info and our previously added data, our ai system suggested " + this.diagnosys + " as a possible diagnosys. \n You might consider giving them a call. \n \n Have a great day!";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("appweml@gmail.com", "euea vhqx nwtf wuhm"),
                Host = "smtp.gmail.com",
                EnableSsl = true,
            };
            Console.WriteLine(this.recipientEmail);
            smtpClient.Send("appweml@gmail.com", this.recipientEmail, this.subject, body);
        }

        public void SendToDoctor()
        {
            String body = "Hello! \n We are contacting you on behalf of the WEML app, regarding the physical and mental well-being of our user " + this.userName + " They have reported \n ";
            foreach (Symptom s in symptoms)
            {
                body += "on " + s.DateTime + ": " + s.SymptomName + " with severity " + s.Severity + "\n";
            }
            body += "\n Based on this new info, our ai system suggested " + this.diagnosys + " as a possible diagnosys. \n We hope this information is usefull if they come for a check up. \n \n Have a great day!";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("appweml@gmail.com", "euea vhqx nwtf wuhm"),
                Host = "smtp.gmail.com",
                EnableSsl = true,
            };
            Console.WriteLine(this.recipientEmail);
            smtpClient.Send("appweml@gmail.com", this.recipientEmail, this.subject, body);
        }
    }
}
