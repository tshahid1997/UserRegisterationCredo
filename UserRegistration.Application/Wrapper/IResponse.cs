using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRegistration.Application.Wrapper
{
    public interface IResponse
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T Data { get; }
    }
}
