﻿using AppsterBackendAdmin.Infrastructures.Exceptions;
using AppsterBackendAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AppsterBackendAdmin.Infrastructures.Implementations
{
    public partial class AppsterDataAccess
    {
        public void DeleteUser(Func<user, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var users = context.users.Where(predicate);
                try
                {
                    if (users.Count() > 0)
                    {
                        context.users.RemoveRange(users);
                    }
                }
                finally
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.SaveChanges();
                }

            }
        }

        public void DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }


        public void SuspendUser(Func<user, bool> predicate)
        {
            using (var context = new appsterEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var users = context.users.Where(predicate);
                try
                {
                    foreach (var usr in users)
                    {
                        usr.status = 0;
                        context.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                finally
                {
                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.SaveChanges();
                }
            }
        }

        public void SuspendUser(int userId)
        {
            using (var context = new appsterEntities())
            {
                var dbUser = context.users.SingleOrDefault(i => i.id == userId);
                dbUser.status = 0;
                context.SaveChanges();
            }
        }

        public async Task UpdateUser(user updatedUser)
        {
            using (var context = new appsterEntities())
            {
                var checkData = context.users.SingleOrDefault(i => i.id != updatedUser.id &&
                                (i.username == updatedUser.username || i.email == updatedUser.email));
                if (checkData == null)
                {
                    context.users.Add(updatedUser);
                    context.Entry(updatedUser).State = System.Data.Entity.EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                else
                {
                    if (checkData.username == updatedUser.username) throw new UsernameAlreadyExistException();
                    if (checkData.email == updatedUser.email) throw new EmailAlreadyExistException();
                }
            }
        }

        public async Task<int> CreateUser(user newUser)
        {
            using (var context = new appsterEntities())
            {
                var checkData = context.users.FirstOrDefault(i => i.username == newUser.username || i.email == newUser.email);
                if (checkData == null)
                {
                    context.users.Add(newUser);
                    await context.SaveChangesAsync();
                    return newUser.id;
                }
                else
                {
                    if (checkData.username == newUser.username) throw new UsernameAlreadyExistException();
                    if (checkData.email == newUser.email) throw new EmailAlreadyExistException();
                }
                throw new DatabaseExecutionException();
            }
        }
    }
}