using System.Net;
using System.Collections.Generic;
using Test.Framework.Logging; 
using Task_6.Testing.Models;
namespace Task_6.Testing
{
    public static class ApplicationApiRequests
    {
        public static (HttpStatusCode ResponseStatusCode, string MediaType, Queue<T> ResponseData) GetQueueData<T>(string rout)
        {
            var responsDataTask = APIUtils.GetQueueData<T>(rout);
            responsDataTask.Wait();
            Queue<T> responseData = responsDataTask.Result;
            return (APIUtils.StatusCode,APIUtils.MediaType, responseData);
        }

        public static (HttpStatusCode ResponseStatusCode, string MediaType, T Object) GetObject<T>(string rout, string objectId)
            where T : class
        {
            var responseObjectTask = APIUtils.GetDataItem<T>(rout, objectId);
            responseObjectTask.Wait();
            T responseObject = responseObjectTask.Result;
            return (APIUtils.StatusCode, APIUtils.MediaType, responseObject);
        }
        public static (HttpStatusCode ResponseStatusCode, Post ExpectedPost, Post CreatedPost) CreatePost(string rout, int userId)
        {
            Post expectedPost = new Post();
            expectedPost.UserId = userId;
            expectedPost.Title = Utilities.GetRandomString(10);
            expectedPost.Body = Utilities.GetRandomString(100);
            Log.Info($"The expected object Post has been created {expectedPost}.");
            Log.Info($"Send POST request.");
            var createdPostTask = APIUtils.CreatePostDataItem<Post>(rout, expectedPost);
            createdPostTask.Wait();
            Post createdPost = createdPostTask.Result;
            return (APIUtils.StatusCode, expectedPost, createdPost);
        }
    }
}