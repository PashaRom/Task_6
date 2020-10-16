using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Task_6.Testing.Models;
using Test.Framework.Configuration;
using Test.Framework.Logging;
namespace Task_6.Testing
{
    public class Tests
    {
        private string postsRout;
        private string usersRout;
        private string mediaType;
        private User checkingUser = new User();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            postsRout = ConfigurationManager.TestData.GetStringParam("data:posts:rout");
            usersRout = ConfigurationManager.TestData.GetStringParam("data:users:rout");
            mediaType = ConfigurationManager.TestData.GetStringParam("mediaType");
            APIUtils.Initialization(
                ConfigurationManager.Configuration.GetStringParam("httpClient:uri"),
                ConfigurationManager.Configuration.GetStringParam("httpClient:mediaType"),
                ConfigurationManager.Configuration.GetStringParam("httpClient:userAgent"));
        }

        [Test]
        [Order(1)]
        public void GetAllPosts()
        {            
            Log.Info($"Get posts from \"{postsRout}\".");
            (HttpStatusCode StatusCode, string MediaType, Queue<Post> Posts) responsePosts = ApplicationApiRequests.GetQueueData<Post>(postsRout);            
            string postsCodeStatusMessage = $"The rout \"{postsRout}\" has returned the status code {Convert.ToInt32(responsePosts.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(postsCodeStatusMessage);
            Assert.AreEqual(HttpStatusCode.OK, responsePosts.StatusCode, postsCodeStatusMessage);            
            string postsMediaTypeMessage = $"The rout \"{postsRout}\" has returned the actual media type {responsePosts.MediaType}. The expected media type is {mediaType}.";
            Log.Info(postsMediaTypeMessage);
            Assert.IsTrue(responsePosts.MediaType.Equals(mediaType), postsMediaTypeMessage);            
            bool isAscSort = Utilities.IsAscSort<Post>(responsePosts.Posts);
            Log.Info($"The function IsAscSort has returned \"{isAscSort}\".");
            Assert.IsTrue(isAscSort,
                $"The rout \"{postsRout}\" has returned the post list not increase.");            
        }

        [Test]
        [Order(2)]
        public void GetPostPositiveTest()
        {            
            string postId = ConfigurationManager.TestData.GetStringParam("data:posts:getPositiveTest:postId");
            Log.Info($"Get the post whith has rout \"{postsRout}/{postId}\".");
            (HttpStatusCode StatusCode, string MediaType, Post Post) responsePost = ApplicationApiRequests.GetObject<Post>(postsRout, postId);            
            string postStatusCodeMessage = $"The rout \"{postsRout}/{postId}\" has returned the status code {Convert.ToInt32(responsePost.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(postStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.OK, responsePost.StatusCode, postStatusCodeMessage);            
            Log.Info($"Rout \"{postsRout}/{postId}\" has returned the object {responsePost.Post}.");
            int expectedUserId = ConfigurationManager.TestData.GetIntParam("data:posts:getPositiveTest:userId");
            Assert.AreEqual(expectedUserId, responsePost.Post.UserId,
                $"The expected userId={expectedUserId}. The post has actual userId={responsePost.Post.UserId}.");
            int expectedPostId = ConfigurationManager.TestData.GetIntParam("data:posts:getPositiveTest:postId");
            Assert.AreEqual(expectedPostId, responsePost.Post.PostId,
                $"The expected postId={expectedPostId}. The post has actual postId={responsePost.Post.PostId}");
            Assert.IsFalse(String.IsNullOrEmpty(responsePost.Post.Title),
                $"The post when postId={responsePost.Post.PostId} has null value title.");
            Assert.IsFalse(String.IsNullOrEmpty(responsePost.Post.Body),
                $"The post when postId={responsePost.Post.PostId} has null value body.");           
        }

        [Test]
        [Order(3)]
        public void GetPostNegativeTest()
        {            
            string postId = ConfigurationManager.TestData.GetStringParam("data:posts:getNegativeTest:postId");
            Log.Info($"Get the post whith has rout \"{postsRout}/{postId}\".");
            (HttpStatusCode StatusCode, string MediaType, Post Post) responsePost = ApplicationApiRequests.GetObject<Post>(postsRout, postId);            
            string postStatusCodeMessage = $"The rout \"{postsRout}/{postId}\" has returned the status code {Convert.ToInt32(responsePost.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.NotFound)}.";
            Log.Info(postStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.NotFound, responsePost.StatusCode, postStatusCodeMessage);            
            Log.Info($"The rout \"{postsRout}/{postId}\" has returned the object {responsePost.Post}.");
            long? contentLenght = APIUtils.ContentLenght;
            Log.Info($"The rout \"{postsRout}/{postId}\" has returned the contentLenght = {contentLenght}.");
            Assert.AreEqual(2, contentLenght,
                $"The rout \"{postsRout}/{postId}\" has returned the contentLenght = {contentLenght}. The expected value = 2.");            
        }

        [Test]
        [Order(4)]
        public void SendPostCreate()
        {            
            Log.Info($"Request POST to rout \"{postsRout}\".");            
            (HttpStatusCode StatusCode, Post ExpectedPost, Post ActualPost) responseCreatedPost = ApplicationApiRequests.CreatePost(postsRout, ConfigurationManager.TestData.GetIntParam("data:posts:postPositiveTest:userId"));
            string createPostStatusCodeMessage = $"The server on the rout \"{postsRout}\" has returned the status code {Convert.ToInt32(responseCreatedPost.StatusCode)} on POST request. The expected status code is {Convert.ToInt32(HttpStatusCode.Created)}.";
            Log.Info(createPostStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.Created, responseCreatedPost.StatusCode, createPostStatusCodeMessage);            
            Log.Info($"POST response on the rout \"{postsRout}\" has returned the object {responseCreatedPost.ExpectedPost}.");
            Assert.AreEqual(responseCreatedPost.ExpectedPost.UserId, responseCreatedPost.ActualPost.UserId,
                $"POST response has returned the actual userId = {responseCreatedPost.ActualPost.UserId}. The expected userId = {responseCreatedPost.ExpectedPost.UserId}.");
            Assert.AreEqual(responseCreatedPost.ExpectedPost.Title, responseCreatedPost.ActualPost.Title,
                $"POST response has returned the actual Title = {responseCreatedPost.ActualPost.Title}. The expected Title = {responseCreatedPost.ExpectedPost.Title}.");
            Assert.AreEqual(responseCreatedPost.ExpectedPost.Body, responseCreatedPost.ActualPost.Body,
                $"POST response has returned the actual Body = {responseCreatedPost.ActualPost.Body}. The expected Body = {responseCreatedPost.ExpectedPost.Body}.");
            Assert.NotNull(responseCreatedPost.ActualPost.PostId,
                $"POST response has returned the actual postId is empty.");            
        }

        [Test]
        [Order(5)]
        public void GetUserPositiveTest()
        {
            Log.Info($"Get users from \"{usersRout}\".");            
            (HttpStatusCode StatusCode, string MediaType, Queue<User> Users) responseUsers = ApplicationApiRequests.GetQueueData<User>(usersRout);            
            string usersStatusCodeMessage = $"The rout \"{usersRout}\" has returned the status code {Convert.ToInt32(responseUsers.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(usersStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.OK, responseUsers.StatusCode, usersStatusCodeMessage);
            string userMediaType = $"The rout \"{usersRout}\" has returned the actual media type {responseUsers.MediaType}. The expected media type is {mediaType}.";
            Log.Info(userMediaType);
            Assert.IsTrue(responseUsers.MediaType.Equals(mediaType), userMediaType);
            User expectedUser = ConfigurationManager.TestData.GetObjectParam<User>("data:users:getPositiveTest:user");            
            checkingUser = responseUsers.Users.Where<User>(chosenUser => chosenUser.UserId == expectedUser.UserId).FirstOrDefault();            
            string chosenUserMessage = $"The rout \"{usersRout}\" has returned object users list where was object user =\"{checkingUser}\". The expected user is \"{expectedUser}\".";
            Log.Info(chosenUserMessage);
            Assert.AreEqual(expectedUser, checkingUser, chosenUserMessage);
        }

        [Test]
        [Order(6)]
        public void CheckUserGetPreviousStep()
        {            
            string userId = ConfigurationManager.TestData.GetStringParam("data:users:getPositiveTest:user:userId");
            Log.Info($"Get user from the rout \"{usersRout}/{userId}\".");
            (HttpStatusCode StatusCode, string MediaType, User User) responseUser = ApplicationApiRequests.GetObject<User>(usersRout, userId);            
            string statusCodeMessage = $"The rout \"{usersRout}/{userId}\" has returned the status code {Convert.ToInt32(responseUser.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(statusCodeMessage);
            Assert.AreEqual(HttpStatusCode.OK, responseUser.StatusCode, statusCodeMessage);            
            string actualUserMessage = $"The rout \"{usersRout}/{userId}\" has returned object {responseUser.User}. The expected object {checkingUser}.";
            Log.Info(actualUserMessage);
            Assert.AreEqual(checkingUser, responseUser.User, actualUserMessage);
        }

        [TearDown]
        public void TearDown()
        {
            APIUtils.StatusCode = 0;
            APIUtils.MediaType = String.Empty;
            APIUtils.ContentLenght = 0;
        }
    }
}