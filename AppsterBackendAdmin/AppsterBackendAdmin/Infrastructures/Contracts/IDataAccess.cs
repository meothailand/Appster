using AppsterBackendAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsterBackendAdmin.Infrastructures.Contracts
{
    public interface IDataAccess
    {
        int GetRole(string roleName);
        string SignIn(string name, string password);
        user GetUser(Func<user, bool> predicate);
        IEnumerable<user> GetUsers(Func<user, bool> predicate);
        IEnumerable<user> GetNewAddedUser(int? take);
        post GetPost(Func<post, bool> predicate);
        IEnumerable<post> GetPosts(Func<post, bool> predicate);
        @event GetEvent(Func<@event, bool> predicate);
        IEnumerable<@event> GetEvents(Func<@event, bool> predicate);
        gift GetGift(Func<gift, bool> predicate);
        IEnumerable<gift> GetGifts(Func<gift, bool> predicate);
    }
}
