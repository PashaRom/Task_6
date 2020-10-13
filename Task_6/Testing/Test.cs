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
        string postsRout;
        string usersRout;
        string mediaType;
        User checkingUser = new User();

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
            Log.Info(1, $"Get posts from \"{postsRout}\".");
            var postsTask = APIUtils.GetQueueData<Post>(postsRout);
            postsTask.Wait();
            string postsCodeStatusMessage = $"The rout \"{postsRout}\" has returned the status code {Convert.ToInt32(APIUtils.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(postsCodeStatusMessage);
            Assert.AreEqual(HttpStatusCode.OK, APIUtils.StatusCode, postsCodeStatusMessage);
            string postsMediaTypeMessage = $"The rout \"{postsRout}\" has returned the actual media type {APIUtils.MediaType}. The expected media type is {mediaType}.";
            Log.Info(postsMediaTypeMessage);
            Assert.IsTrue(APIUtils.MediaType.Equals(mediaType), postsMediaTypeMessage);
            Queue<Post> posts = postsTask.Result;
            bool isAscSort = Utilities.IsAscSort<Post>(posts);
            Log.Info($"The function IsAscSort has returned \"{isAscSort}\".");
            Assert.IsTrue(isAscSort,
                $"The rout \"{postsRout}\" has returned the post list not increase.");            
        }

        [Test]
        [Order(2)]
        public void GetPostPositiveTest()
        {            
            string postId = ConfigurationManager.TestData.GetStringParam("data:posts:getPositiveTest:postId");
            Log.Info(2, $"Get the post whith has rout \"{postsRout}/{postId}\".");
            var postTask = APIUtils.GetDataItem<Post>(postsRout, postId);
            postTask.Wait();
            string postStatusCodeMessage = $"The rout \"{postsRout}/{postId}\" has returned the status code {Convert.ToInt32(APIUtils.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(postStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.OK, APIUtils.StatusCode, postStatusCodeMessage);
            Post post = postTask.Result;
            Log.Info($"Rout \"{postsRout}/{postId}\" has returned the object {post}.");
            int expectedUserId = ConfigurationManager.TestData.GetIntParam("data:posts:getPositiveTest:userId");
            Assert.AreEqual(expectedUserId, post.UserId,
                $"The expected userId={expectedUserId}. The post has actual userId={post.UserId}.");
            int expectedPostId = ConfigurationManager.TestData.GetIntParam("data:posts:getPositiveTest:postId");
            Assert.AreEqual(expectedPostId, post.PostId,
                $"The expected postId={expectedPostId}. The post has actual postId={post.PostId}");
            Assert.IsFalse(String.IsNullOrEmpty(post.Title),
                $"The post when postId={post.PostId} has null value title.");
            Assert.IsFalse(String.IsNullOrEmpty(post.Body),
                $"The post when postId={post.PostId} has null value body.");           
        }

        [Test]
        [Order(3)]
        public void GetPostNegativeTest()
        {            
            string postId = ConfigurationManager.TestData.GetStringParam("data:posts:getNegativeTest:postId");
            Log.Info(3, $"Get the post whith has rout \"{postsRout}/{postId}\".");
            var postTask = APIUtils.GetDataItem<Post>(postsRout, postId);
            postTask.Wait();
            string postStatusCodeMessage = $"The rout \"{postsRout}/{postId}\" has returned the status code {Convert.ToInt32(APIUtils.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.NotFound)}.";
            Log.Info(postStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.NotFound, APIUtils.StatusCode, postStatusCodeMessage);
            Post post = postTask.Result;
            Log.Info($"The rout \"{postsRout}/{postId}\" has returned the object {post}.");
            long? contentLenght = APIUtils.ContentLenght;
            Log.Info($"The rout \"{postsRout}/{postId}\" has returned the contentLenght = {contentLenght}.");
            Assert.AreEqual(2, contentLenght,
                $"The rout \"{postsRout}/{postId}\" has returned the contentLenght = {contentLenght}. The expected value = 2.");            
        }

        [Test]
        [Order(4)]
        public void SendPostCreate()
        {            
            Log.Info(4, $"Request POST to rout \"{postsRout}\".");
            Post expectedPost = new Post();
            expectedPost.UserId = ConfigurationManager.TestData.GetIntParam("data:posts:postPositiveTest:userId");
            expectedPost.Title = Utilities.GetRandomString(10);
            expectedPost.Body = Utilities.GetRandomString(100);
            Log.Info($"The expected object Post has been created {expectedPost}.");
            Log.Info($"Send POST request.");
            var createdPostTask = APIUtils.CreatePostDataItem<Post>(postsRout, expectedPost);
            createdPostTask.Wait();
            string createPostStatusCodeMessage = $"The server on the rout \"{postsRout}\" has returned the status code {Convert.ToInt32(APIUtils.StatusCode)} on POST request. The expected status code is {Convert.ToInt32(HttpStatusCode.Created)}.";
            Log.Info(createPostStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.Created, APIUtils.StatusCode, createPostStatusCodeMessage);
            Post createdPost = createdPostTask.Result;
            Log.Info($"POST response on the rout \"{postsRout}\" has returned the object {expectedPost}.");
            Assert.AreEqual(expectedPost.UserId, createdPost.UserId,
                $"POST response has returned the actual userId = {createdPost.UserId}. The expected userId = {expectedPost.UserId}.");
            Assert.AreEqual(expectedPost.Title,createdPost.Title,
                $"POST response has returned the actual Title = {createdPost.Title}. The expected Title = {expectedPost.Title}.");
            Assert.AreEqual(expectedPost.Body, createdPost.Body,
                $"POST response has returned the actual Body = {createdPost.Body}. The expected Body = {expectedPost.Body}.");
            Assert.NotNull(createdPost.PostId,
                $"POST response has returned the actual postId is empty.");            
        }

        [Test]
        [Order(5)]
        public void GetUserPositiveTest()
        {
            Log.Info(5, $"Get users from \"{usersRout}\".");
            var usersTask = APIUtils.GetQueueData<User>(usersRout);
            usersTask.Wait();
            string usersStatusCodeMessage = $"The rout \"{usersRout}\" has returned the status code {Convert.ToInt32(APIUtils.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(usersStatusCodeMessage);
            Assert.AreEqual(HttpStatusCode.OK, APIUtils.StatusCode, usersStatusCodeMessage);
            string userMediaType = $"The rout \"{usersRout}\" has returned the actual media type {APIUtils.MediaType}. The expected media type is {mediaType}.";
            Log.Info(userMediaType);
            Assert.IsTrue(APIUtils.MediaType.Equals(mediaType), userMediaType);
            User expectedUser = ConfigurationManager.TestData.GetObjectParam<User>("data:users:getPositiveTest:user");
            Queue<User> users = usersTask.Result;
            checkingUser = users.Where<User>(chosenUser => chosenUser.UserId == expectedUser.UserId).FirstOrDefault();            
            string chosenUserMessage = $"The rout \"{usersRout}\" has returned object users list where was object user =\"{checkingUser}\". The expected user is \"{expectedUser}\".";
            Log.Info(chosenUserMessage);
            Assert.AreEqual(expectedUser, checkingUser, chosenUserMessage);
        }

        [Test]
        [Order(6)]
        public void CheckUserGetPreviousStep()
        {            
            string userId = ConfigurationManager.TestData.GetStringParam("data:users:getPositiveTest:user:userId");
            Log.Info(6, $"Get user from the rout \"{usersRout}/{userId}\".");
            var userTask = APIUtils.GetDataItem<User>(usersRout,userId);
            userTask.Wait();
            string statusCodeMessage = $"The rout \"{usersRout}/{userId}\" has returned the status code {Convert.ToInt32(APIUtils.StatusCode)}. The expected status code: {Convert.ToInt32(HttpStatusCode.OK)}.";
            Log.Info(statusCodeMessage);
            Assert.AreEqual(HttpStatusCode.OK, APIUtils.StatusCode,
               statusCodeMessage);
            User actualUser = userTask.Result;
            string actualUserMessage = $"The rout \"{usersRout}/{userId}\" has returned object {actualUser}. The expected object {checkingUser}.";
            Log.Info(actualUserMessage);
            Assert.AreEqual(checkingUser, actualUser, actualUserMessage);
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