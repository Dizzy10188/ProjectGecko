using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko.Models
{
    public class Commission
    {
        //MongoDB ObjectID (This is required by Mongo)
        public ObjectId Id;

        //The ID of the commission
        public long CommissionID { get; set; }

        //User commissioning the art
        public long CommissionerID { get; set; }
        
        //User being commissioned for art
        public long CommissioneeID { get; set; }

        //An array of all the image paths given by the user
        public string[] ImagePaths { get; set; }

        //This is the summary of what the commissioner wants from the artist
        public string Description { get; set; }

        public Commission()
        {
            string manualCommand = "{find: 'Commissions'}";
            var cmd = new JsonCommand<BsonDocument>(manualCommand);
            var result = DatabaseConnection.RunCommand(cmd);
            //gets the id values as a list of values
            var bsonList = result.Elements.ElementAt(0).Value.AsBsonDocument.Elements.ElementAt(0).Value.AsBsonArray.ToList();
            long biggest = 0;
            foreach (var item in bsonList)
            {
                long comId = item.AsBsonDocument.GetElement(1).Value.ToInt64();
                if (comId > biggest)
                {
                    biggest = comId;
                }
            }
            //long idCount = mongoClient.GetCollection<Post>("Posts").Aggregate(def);

            CommissionID = biggest + 1;
        }


    }
}
