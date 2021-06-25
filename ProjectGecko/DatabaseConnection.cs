using MongoDB.Bson;
using MongoDB.Driver;
using ProjectGecko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectGecko
{
    public class DatabaseConnection
    {
        private static IMongoDatabase accountDatabase;

        public static void DatabaseConnect()
        {
            accountDatabase = new MongoClient("mongodb+srv://admin:password1234@test-un7p6.azure.mongodb.net/test?retryWrites=true&w=majority&connect=replicaSet").GetDatabase("AccountDB");
        }

        public static Account GetAccount(long Id)
        {
            //return user by id
            return accountDatabase.GetCollection<Account>("AccountInfo").Find(a => a.AccountID == Id).FirstOrDefault();
        }

        public static Account GetAccount(string UserName)
        {
            //return user by id
            return accountDatabase.GetCollection<Account>("AccountInfo").Find(a => a.UserName == UserName).FirstOrDefault();
        }

        public static List<Account> GetAccounts(FilterDefinition<Account> FindFunc)
        {
            return accountDatabase.GetCollection<Account>("AccountInfo").Find(FindFunc).ToList();
        }

        public static void UpdateAccount(FilterDefinition<Account> fil, UpdateDefinition<Account> upd)
        {
            accountDatabase.GetCollection<Account>("AccountInfo").UpdateOne(fil, upd);
        }

        public static Commission GetCommission(long Id)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").Find(c => c.CommissionID == Id).FirstOrDefault();
        }

        public static List<Commission> GetCommissions(long commissioneeId)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").Find(c => c.CommissioneeID == commissioneeId).ToList();
        }

        public static List<Commission> GetCommissionsFromUser(long userID)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").Find(x => true).ToList().Where(x => x.CommissionerID == userID).ToList(); ;
        }

        public static List<Commission> GetCommissionsForUser(long userID)
        {
            return accountDatabase.GetCollection<Commission>("Commissions").Find(x => true).ToList().Where(x => x.CommissioneeID == userID).ToList(); ;
        }

        public static Post GetPost(long Id)
        {
            return accountDatabase.GetCollection<Post>("Posts").Find(a => a.PostID == Id).FirstOrDefault();
        }

        public static List<Post> FindPosts(string Title)
        {
            return accountDatabase.GetCollection<Post>("Posts").Find(p => p.Title.Contains(Title)).ToList();
        }
        
        public static List<Account> FindAccounts(string Name)
        {
            return accountDatabase.GetCollection<Account>("AccountInfo").Find(m => m.UserName.Contains(Name) || m.DisplayName.Contains(Name)).ToList();
        }

        public static List<Post> GetAllPosts(bool reversed)
        {
            if (reversed)
            {
                var ret = accountDatabase.GetCollection<Post>("Posts").Find(x => true).ToList();
                //return ret.SortByDescending(x => x.PostID).ToList();
                return ret;
            }
            else
                return accountDatabase.GetCollection<Post>("Posts").Find(x => true).ToList();
        }

        public static List<Post> GetPosts(FilterDefinition<Post> FindFunc)
        {
            return accountDatabase.GetCollection<Post>("Posts").Find(FindFunc).ToList();
        }

        public static List<Post> GetUserPosts(long userID)
        {
            return accountDatabase.GetCollection<Post>("Posts").Find(x => true).ToList().Where(x => x.PosterID == userID).ToList();
        }

        public static BsonDocument RunCommand(JsonCommand<BsonDocument> command)
        {
            return accountDatabase.RunCommand(command);
        }

        public static void InsertAccount(Account a)
        {
            accountDatabase.GetCollection<Account>("AccountInfo").InsertOne(a);
        }

        public static void InsertCommision(Commission c)
        {
            accountDatabase.GetCollection<Commission>("Commissions").InsertOne(c);
        }
        public static void InsertPost(Post p)
        {
            accountDatabase.GetCollection<Post>("Posts").InsertOne(p);
        }

        public static void UpdatePost(Post p)
        {
            accountDatabase.GetCollection<Post>("Posts").ReplaceOne(X => X.PostID == p.PostID, p);
        }
        
        public static void UpdateAccount(Account a)
        {
            accountDatabase.GetCollection<Account>("AccountInfo").ReplaceOne(X => X.AccountID == a.AccountID, a);
        }
        
        public static void UpdateCommission(Commission c)
        {
            accountDatabase.GetCollection<Commission>("Commissions").ReplaceOne(X => X.CommissionID == c.CommissionID, c);
        }
    }
}
