﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DS.EFCore.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using DS.EFCore.Helper.Tests.Fakes;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DS.EFCore.Helper.Tests
{
    [TestClass()]
    public class DbContextExtensionsTests
    {
        [TestMethod()]
        public void RemoveUntrackedEntities_AddedEntities_EntitiesDetached()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User[] users = new User[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "User1",
                    Active = true,
                    CreationDate = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "User2",
                    Active = true,
                    CreationDate = DateTime.UtcNow
                }
            };
            fakeDbContext.AddRange(users);

            //Act
            fakeDbContext.RemoveUntrackedEntities(users);

            //Assert
            EntityState entityState1 = fakeDbContext.Entry(users[0]).State;
            EntityState entityState2 = fakeDbContext.Entry(users[1]).State;

            Assert.IsTrue(entityState1 == EntityState.Detached && entityState2 == EntityState.Detached);
        }

        [TestMethod()]
        public void RemoveUntrackedEntities_UntrackedEntities_EntitiesDeleted()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User[] usersToRemove = fakeDbContext
                .Users
                .Take(2)
                .AsNoTracking()
                .ToArray();

            //Act
            fakeDbContext.RemoveUntrackedEntities(usersToRemove);

            //Assert
            EntityState entityState1 = fakeDbContext.Entry(usersToRemove[0]).State;
            EntityState entityState2 = fakeDbContext.Entry(usersToRemove[1]).State;

            Assert.IsTrue(entityState1 == EntityState.Deleted && entityState2 == EntityState.Deleted);
        }

        [TestMethod()]
        public void RemoveUntrackedEntities_NullEntities_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User[] usersToRemove = null;

            //Act
            Action removeUntrackedEntities = () => fakeDbContext.RemoveUntrackedEntities(usersToRemove);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(removeUntrackedEntities);
        }

        [TestMethod()]
        public void RemoveUntrackedEntities_NullEntityInArray_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            List<User> usersToRemove = fakeDbContext
                .Users
                .Take(2)
                .AsNoTracking()
                .ToList();

            usersToRemove.Add(null);

            //Act
            Action removeUntrackedEntities = () => fakeDbContext.RemoveUntrackedEntities(usersToRemove);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(removeUntrackedEntities);
        }

        [TestMethod()]
        public void UpdateUntrackedEntities_DetatchedEntities_EntitiesModified()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User[] users = fakeDbContext
                .Users
                .Take(2)
                .AsNoTracking()
                .ToArray();

            //Act
            fakeDbContext.UpdateUntrackedEntities(users);

            //Assert
            EntityState entityState1 = fakeDbContext.Entry(users[0]).State;
            EntityState entityState2 = fakeDbContext.Entry(users[1]).State;

            Assert.IsTrue(entityState1 == EntityState.Modified && entityState2 == EntityState.Modified);
        }

        [TestMethod()]
        public void UpdateUntrackedEntities_NullEntities_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User[] usersToRemove = null;

            //Act
            Action updateUntrackedEntities = () => fakeDbContext.UpdateUntrackedEntities(usersToRemove);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(updateUntrackedEntities);
        }

        [TestMethod()]
        public void UpdateUntrackedEntities_NullEntityInArray_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            List<User> usersToRemove = fakeDbContext
                .Users
                .Take(2)
                .AsNoTracking()
                .ToList();

            usersToRemove.Add(null);

            //Act
            Action updateUntrackedEntities = () => fakeDbContext.UpdateUntrackedEntities(usersToRemove);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(updateUntrackedEntities);
        }

        [TestMethod()]
        public void RemoveUntrackedEntity_AddedEntity_EntityDetached()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = "User1",
                Active = true,
                CreationDate = DateTime.UtcNow
            };
            fakeDbContext.Add(user);

            //Act
            fakeDbContext.RemoveUntrackedEntity(user);

            //Assert
            EntityState entityState1 = fakeDbContext.Entry(user).State;

            Assert.IsTrue(entityState1 == EntityState.Detached);
        }

        [TestMethod()]
        public void RemoveUntrackedEntity_UntrackedEntity_EntityDeleted()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User user = fakeDbContext
                .Users
                .Take(1)
                .AsNoTracking()
                .Single();

            //Act
            fakeDbContext.RemoveUntrackedEntity(user);

            //Assert
            EntityState entityState1 = fakeDbContext.Entry(user).State;

            Assert.IsTrue(entityState1 == EntityState.Deleted);
        }

        [TestMethod()]
        public void RemoveUntrackedEntity_NullEntity_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User user = null;

            //Act
            Action removeUntrackedEntities = () => fakeDbContext.RemoveUntrackedEntity(user);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(removeUntrackedEntities);
        }

        [TestMethod()]
        public void UpdateUntrackedEntity_DetatchedEntity_EntityModified()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User user = fakeDbContext
                .Users
                .Take(1)
                .AsNoTracking()
                .Single();

            //Act
            fakeDbContext.UpdateUntrackedEntity(user);

            //Assert
            EntityState entityState1 = fakeDbContext.Entry(user).State;

            Assert.IsTrue(entityState1 == EntityState.Modified);
        }

        [TestMethod()]
        public void UpdateUntrackedEntity_NullEntity_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            User user = null;

            //Act
            Action updateUntrackedEntity = () => fakeDbContext.UpdateUntrackedEntity(user);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(updateUntrackedEntity);
        }

        [TestMethod()]
        public void RemoveBy_UntrackedEntity_EntityDeleted()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            Guid userId = fakeDbContext
                .Users
                .Take(1)
                .AsNoTracking()
                .Single()
                .Id;

            //Act
            fakeDbContext.RemoveBy<User>(userDb => userDb.Id == userId);

            //Assert
            User user = fakeDbContext
                .Users
                .Single(userDb => userDb.Id == userId);

            EntityState entityState1 = fakeDbContext.Entry(user).State;

            Assert.IsTrue(entityState1 == EntityState.Deleted);
        }

        [TestMethod()]
        public void RemoveBy_NullFilter_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();

            //Act
            Action removeBy = () => fakeDbContext.RemoveBy<User>(null);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(removeBy);
        }

        [TestMethod()]
        public void RemoveBy_NoEntityFoundInCondition_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            Guid userId = Guid.NewGuid();

            //Act
            Action removeBy = () => fakeDbContext.RemoveBy<User>(userDb => userDb.Id == userId);

            //Assert
            Assert.ThrowsException<InvalidOperationException>(removeBy);
        }

        [TestMethod()]
        public async Task RemoveAsyncBy_UntrackedEntity_EntityDeleted()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            Guid userId = fakeDbContext
                .Users
                .Take(1)
                .AsNoTracking()
                .Single()
                .Id;

            //Act
            await fakeDbContext.RemoveAsyncBy<User>(userDb => userDb.Id == userId);

            //Assert
            User user = fakeDbContext
                .Users
                .Single(userDb => userDb.Id == userId);

            EntityState entityState1 = fakeDbContext.Entry(user).State;

            Assert.IsTrue(entityState1 == EntityState.Deleted);
        }

        [TestMethod()]
        public async Task RemoveAsyncBy_NullFilter_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();

            //Act
            Func<Task> removeBy = async () => await fakeDbContext.RemoveAsyncBy<User>(null);

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(removeBy);
        }

        [TestMethod()]
        public async Task RemoveAsyncBy_NoEntityFoundInCondition_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            Guid userId = Guid.NewGuid();

            //Act
            Func<Task> removeBy = async () => await fakeDbContext.RemoveAsyncBy<User>(userDb => userDb.Id == userId);

            //Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(removeBy);
        }

        [TestMethod()]
        public void RemoveAll_UntrackedEntities_EntitiesDeleted()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            Guid[] usersId = fakeDbContext
                .Users
                .AsNoTracking()
                .Where(user => user.Active)
                .Select(user => user.Id)
                .Take(3)
                .ToArray();

            //Act
            fakeDbContext.RemoveAll<User>(userDb => userDb.Active);

            //Assert
            User[] users = fakeDbContext
                .Users
                .Where(userDb => usersId.Contains(userDb.Id))
                .ToArray();

            EntityState entityState1 = fakeDbContext.Entry(users[0]).State;
            EntityState entityState2 = fakeDbContext.Entry(users[1]).State;
            EntityState entityState3 = fakeDbContext.Entry(users[2]).State;

            Assert.IsTrue(entityState1 == EntityState.Deleted && entityState2 == EntityState.Deleted && entityState3 == EntityState.Deleted);
        }

        [TestMethod()]
        public void RemoveAll_NullFilter_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();

            //Act
            Action removeAll = () => fakeDbContext.RemoveAll<User>(null);

            //Assert
            Assert.ThrowsException<ArgumentNullException>(removeAll);
        }

        [TestMethod()]
        public async Task RemoveAllAsync_UntrackedEntities_EntitiesDeleted()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();
            Guid[] usersId = fakeDbContext
                .Users
                .AsNoTracking()
                .Where(user => user.Active)
                .Select(user => user.Id)
                .Take(3)
                .ToArray();

            //Act
            await fakeDbContext.RemoveAllAsync<User>(userDb => userDb.Active);

            //Assert
            User[] users = fakeDbContext
                .Users
                .Where(userDb => usersId.Contains(userDb.Id))
                .ToArray();

            EntityState entityState1 = fakeDbContext.Entry(users[0]).State;
            EntityState entityState2 = fakeDbContext.Entry(users[1]).State;
            EntityState entityState3 = fakeDbContext.Entry(users[2]).State;

            Assert.IsTrue(entityState1 == EntityState.Deleted && entityState2 == EntityState.Deleted && entityState3 == EntityState.Deleted);
        }

        [TestMethod()]
        public async Task RemoveAllAsync_NullFilter_ExceptionIsThrown()
        {
            //Arrange
            FakeDbContext fakeDbContext = FakeDbContext.GetFakeDbContext();

            //Act
            Func<Task> removeAllAsync = async () => await fakeDbContext.RemoveAllAsync<User>(null);

            //Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(removeAllAsync);
        }
    }
}