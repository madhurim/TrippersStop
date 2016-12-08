using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DataLayer;
using System.Configuration;
using EmailService;

namespace DataLayer
{
    public class MongoDBConnector : INoSqlConnector<IMongoDatabase>
    {
        private MongoClient _mongoClient;
        public IMongoDatabase connect()
        {
            _mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoDBServer"]);

            try
            {
                foreach (MongoServerInstance instance in _mongoClient.GetServer().Instances)//_mongoClient.GetServer().Instances)
                {
                    //instance.Ping();
                    // or you can try
                    if (instance.State == MongoServerState.Disconnected)
                    {
                        connectionFailMail(instance.BuildInfo.ToString());
                        return null;
                    }
                }
                return _mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongodbName"]);

            }
            catch (MongoConnectionException mex)
            {
                connectionFailMail(mex.Message.ToString());
                return null;
            }
            catch (Exception ex)
            {
                connectionFailMail(ex.Message.ToString());
                return null;
            }
        }

        public IMongoDatabase connect(string connectionString, string databaseName)
        {
            _mongoClient = new MongoClient(connectionString);

            try
            {
                foreach (MongoServerInstance instance in _mongoClient.GetServer().Instances)//_mongoClient.GetServer().Instances)
                {
                    //instance.Ping();
                    // or you can try
                    if (instance.State == MongoServerState.Disconnected)
                    {
                        connectionFailMail(instance.BuildInfo.ToString());
                        return null;
                    }
                }
                return _mongoClient.GetDatabase(databaseName);
            }
            catch (MongoConnectionException mex)
            {
                connectionFailMail(mex.Message.ToString());
                return null;
            }
            catch (Exception ex)
            {
                connectionFailMail(ex.Message.ToString());
                return null;
            }
        }

        public void connectionFailMail(string ErrorMessage)
        {
            string email = ConfigurationManager.AppSettings["ConnectionFailNotification"].ToString();

            var toemail = email.Split(',');
            List<string> listToaddress = new List<string>();
            foreach (var toaddress in toemail)
                listToaddress.Add(toaddress);

            // changed to implement aws email
            // mail.SendComplexMessage("noreply@trippism.com", "MongoDB connection failed", listToaddress, "<html><body><div><p><strong>Title: </strong>MongoDB connection failed.</p><p><strong>Time: </strong>" + DateTime.Now.ToString() + "</p><p><strong style='color:#b90005;'>Error Message: </strong>" + ErrorMessage + "</p><p></p></div></body></html>");
            string fromEmail = ConfigurationManager.AppSettings["MailGunFromemail"];

            IEmailService iemail = new SESEmail();
            iemail.SendMessage(fromEmail, "MongoDB connection failed", listToaddress, "<html><body><div><p><strong>Title: </strong>MongoDB connection failed.</p><p><strong>Time: </strong>" + DateTime.Now.ToString() + "</p><p><strong style='color:#b90005;'>Error Message: </strong>" + ErrorMessage + "</p><p></p></div></body></html>");
        }
    }
}
