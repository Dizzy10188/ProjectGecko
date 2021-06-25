using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectGecko.Models
{
    public class Account
    {
        public ObjectId _id;

        public long AccountID { get; set; }
        //Account display name
        public string DisplayName { get; set; }

        //Static username
        [Required(ErrorMessage = "Please enter a Permamnent UserName")]
        [StringLength(16, ErrorMessage = "Please enter a nmae between 5 and 16 Characters", MinimumLength = 5)]
        public string UserName { get; set; }

        //User password (one special, one number, one capitol, and at least 8 characters)
        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"^(?=.*[!@#$%^&*()\[\]{};:' <>,.\/?])(?=.*[A-Z])(?=.*[0-9]).{8,}$", ErrorMessage = "one special, one number, one capitol, and at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Query for getting img from database
        [DataType(DataType.Upload)]
        public string ProfPicPath { get; set; }

        [DataType(DataType.Upload)]
        public string CommissionPricesImage { get; set; }

        //Phone number for contact
        [RegularExpression(@"^\d?(\s|-)?(\(\d{3}\)|\d{3})(\s|-)?\d{3}(\s|-)?\d{4}?")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //Email for contact
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"((\w|\d)+)@((\w|\d){2,})\.((\w|\d){2,})")]
        [DataType(DataType.EmailAddress)]
        public string  Email { get; set; }

        //user's customized bio
        [DataType(DataType.MultilineText)]
        public string Biography { get; set; }

        //user's paypal (examplary for this project)
        [DataType(DataType.EmailAddress)]
        public string PayPal { get; set; }

        public byte CommissionLimit { get; set; }

        public string AllowCommissions { get; set; }

        public Account()
        {
            string manualCommand = "{find: 'AccountInfo'}";
            var cmd = new JsonCommand<BsonDocument>(manualCommand);
            var result = DatabaseConnection.RunCommand(cmd);
            var bsonList = result.Elements.ElementAt(0).Value.AsBsonDocument.Elements.ElementAt(0).Value.AsBsonArray.ToList();
            long biggest = 0;
            foreach (var item in bsonList)
            {
                long accountId = item.AsBsonDocument.GetElement(1).Value.ToInt64();
                if (accountId > biggest)
                {
                    biggest = accountId;
                }
            }
            //long idCount = mongoClient.GetCollection<Post>("Posts").Aggregate(def);

            AccountID = biggest + 1;
        }

        public static Account GetAccount(long Id)
        {
            return DatabaseConnection.GetAccount(Id);
        }

        public static Account GetAccount(string UserName)
        {
            return DatabaseConnection.GetAccount(UserName);
        }
    }
}
