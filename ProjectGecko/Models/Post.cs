using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko.Models
{
    public class Post
    {
        public ObjectId _id;

        public long PostID { get; set; }

        public string Title { get; set; }

        public long PosterID { get; set; }

        public string[] ImagePaths { get; set; }

        public List<Comment> Comments { get; set; }

        public int Likes { get; set; }

        public List<long> PeopleLiked { get; set; }

        public Post()
        {
            //var mongoClient = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority").GetDatabase("AccountDB");
            //return user by id
            string manualCommand = "{find: 'Posts'}";
            var cmd = new JsonCommand<BsonDocument>(manualCommand);
            var result = DatabaseConnection.RunCommand(cmd);
            var bsonList = result.Elements.ElementAt(0).Value.AsBsonDocument.Elements.ElementAt(0).Value.AsBsonArray.ToList();
            long biggest = 0;
            foreach (var item in bsonList)
            {
                long postId = item.AsBsonDocument.GetElement(1).Value.ToInt64();
                if (postId > biggest)
                {
                    biggest = postId;
                }
            }
            //long idCount = mongoClient.GetCollection<Post>("Posts").Aggregate(def);

            PostID = biggest + 1;
            Comments = new List<Comment>();
            PeopleLiked = new List<long>();
        }

        public static Post GetPost(long Id)
        {
            return DatabaseConnection.GetPost(Id);
        }

        public Account GetPostAccount()
        {
            return DatabaseConnection.GetAccount(PosterID);
        }

        public static List<Post> GetAllPosts()
        {
            return DatabaseConnection.GetAllPosts(true);
        }

        public bool UserLikedPost(long accountid)
        {
            return PeopleLiked.Contains(accountid);
        }
    }
}
