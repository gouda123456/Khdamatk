using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Khdamatk.Server.ResultPattern;

public static class ResultPattern
{
    public class resultBase
    {

        public resultBase(int status, string title, string message)
        {
            Status = status;
            Title = title;
            Message = message;

        }

        public bool IsSuccess => Status >= 200 && Status < 300;
        public int Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }



    }

    public record Error(string Title, string Message);

    //Advanced Result classes

    public class SuccessResult<T> : resultBase where T : class
    {
        public SuccessResult(int status, string title, string message, T Data) : base(status, title, message)
        {
            this.Data = Data;
        }

        [JsonPropertyOrder(100)]
        public T Data { get; set; }
    }

    public class ErrorResult(int Satus, string Title, string Message, params Error[] errors) : resultBase(Satus, Title, Message)
    {
        [JsonPropertyOrder(100)]
        public Error[] Errors { get; set; } = errors;
    }




    //Using Results as Method helpers
    public static resultBase Success(int Status, string Title, string Message) => new resultBase(Status, Title, Message);
    public static resultBase Success(int Status) => new resultBase(Status, SuccessMessages.General.Title, SuccessMessages.General.Message);
    public static resultBase Success<T>(int Satus, string Title, string Message, T Data) where T : class => new SuccessResult<T>(Satus, Title, Message, Data);
    public static SuccessResult<T> Success<T>(int Satus, T Data) where T : class => new SuccessResult<T>(Satus, "Success", "The operation has been completed successfully.", Data);
    public static ErrorResult Failure(int Status, string Title, string Message, params Error[] errors) => new ErrorResult(Status, Title, Message, errors);
    public static ErrorResult Failure(int Status, params Error[] errors) => new ErrorResult(Status, "Failure", "The operation has failed. Please check the error details.", errors);
    //public static ErrorResult Failure(int Status, string Title, string Message , string errorTitle, string errorMessage) => new ErrorResult(Status, Title, Message, new Error(errorTitle, errorMessage));


    //Respond of Result Helper
    public static ObjectResult Respond(this resultBase result)
    {


        return new ObjectResult(result) { StatusCode = result.Status };
    }

    public static ObjectResult Respond<T>(this SuccessResult<T> result) where T : class
    {

        if (result.IsSuccess)
        {
            var successResult = result as SuccessResult<object>;




            return new ObjectResult(new SuccessResult<object>(result.Status, result.Title, result.Message, result.Data)) { StatusCode = successResult!.Status! };
        }
        return new ObjectResult(result) { StatusCode = result.Status };
    }
}




public static class SuccessMessages
{
    public static class Create
    {
        public const string Title = "Created";
        public const string Message = "The record has been created successfully.";
    }

    public static class Update
    {
        public const string Title = "Updated";
        public const string Message = "The record has been updated successfully.";
    }

    public static class Delete
    {
        public const string Title = "Deleted";
        public const string Message = "The record has been deleted successfully.";
    }

    public static class Fetch
    {
        public const string Title = "Fetched";
        public const string Message = "The data has been retrieved successfully.";
    }

    public static class General
    {
        public const string Title = "Success";
        public const string Message = "The operation has been completed successfully.";
    }
}



public static class FailureMessages
{
    public static class Create
    {
        public const string Title = "Create Failed";
        public const string Message = "The record could not be created. Please try again.";
    }

    public static class Update
    {
        public const string Title = "Update Failed";
        public const string Message = "The record could not be updated. Please try again.";
    }

    public static class Delete
    {
        public const string Title = "Delete Failed";
        public const string Message = "The record could not be deleted. Please try again.";
    }

    public static class Fetch
    {
        public const string Title = "Fetch Failed";
        public const string Message = "The data could not be retrieved. Please try again.";
    }

    public static class NotImplemented
    {
        public const string Title = "Not Implemented";
        public const string Message = "This feature is not implemented yet.";
    }

    // ✅ تنفع لأي حالة فشل عامة
    public static class General
    {
        public const string Title = "Failure";
        public const string Message = "The operation has failed. Please check the error details.";
    }
}
