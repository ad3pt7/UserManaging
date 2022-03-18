using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UserManaging.Model;

namespace UsersManagingTests
{
    [TestClass]
    public class UsersManagingTests
    {
        UsersDAL dAL = new UsersDAL();
        [TestMethod]
        public void DB_WithValidUsersCount()
        {
            int expected = 2;
            List<User> usersList = (List<User>)dAL.GetUsers();
            int actual = usersList.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DB_WithValidUserInsert()
        {
            List<User> usersList = (List<User>)dAL.GetUsers();
            int expected = usersList.Count +1;

            User newUser = new User
            {
                Id = Convert.ToInt32(usersList[usersList.Count - 1].Id + 1),
                FirstName = "Александр",
                LastName = "Петров",
                MiddleName = "Иванович",
                Phone = "89824952282",
                Email = "twer@gdew.et"
            };
            dAL.Insert(newUser);
            usersList = (List<User>)dAL.GetUsers();
            int actual = usersList.Count;
            Assert.AreEqual(expected, actual);
            dAL.Delete((int)newUser.Id);
        }

        [TestMethod]
        public void DB_WithValidUserDelete()
        {
            List<User> usersList = (List<User>)dAL.GetUsers();
            int expected = usersList.Count - 1;
            dAL.Delete((int)usersList[usersList.Count - 1].Id);
            usersList = (List<User>)dAL.GetUsers();
            int actual = usersList.Count;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DB_WithValidUserUpdate()
        {
            List<User> usersList = (List<User>)dAL.GetUsers();
            User editableUser = usersList[usersList.Count - 1];
            string expected = "Test";
            editableUser.FirstName = "Test";
            dAL.Update(editableUser);
            string actual = usersList[usersList.Count - 1].FirstName;
            Assert.AreEqual(expected, actual);
        }
    }
}
