using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistration.Application.Wrapper
{
    public class Response : IResponse
    {
        public Response()
        {
        }

        public bool Succeeded { get; set; }
        public List<string> Messages { get; set; } = new();

        public static IResponse Success()
        {
            return new Response { Succeeded = true };
        }

        public static IResponse Fail()
        {
            return new Response { Succeeded = false };
        }

        public static IResponse Fail(string message)
        {
            return new Response { Succeeded = false, Messages = new List<string> { message } };
        }

        public static IResponse Fail(List<string> messages)
        {
            return new Response { Succeeded = false, Messages = messages };
        }


    }


    public class Response<T> : Response, IResponse<T>
    {
        public Response()
        {
        }

        public T Data { get; set; }



        public new static Response<T> Success()
        {
            return new() { Succeeded = true };
        }


        public static Response<T> Success(T data)
        {
            return new() { Succeeded = true, Data = data };
        }

        public new static Response<T> Fail()
        {
            return new() { Succeeded = false };
        }

        public new static Response<T> Fail(string message)
        {
            return new() { Succeeded = false, Messages = new List<string> { message } };
        }


        public new static Response<T> Fail(List<string> messages)
        {
            return new() { Succeeded = false, Messages = messages };
        }

    }


}
