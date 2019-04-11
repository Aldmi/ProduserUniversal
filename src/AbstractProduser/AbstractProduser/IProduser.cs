using System;
using System.Threading.Tasks;
using AbstractProduser.Helpers;
using CSharpFunctionalExtensions;

namespace AbstractProduser.AbstractProduser
{
    public interface IProduser : IDisposable
    {
        Task<Result<string, ErrorWrapper>> Send(string message, string invokerName = null);
        Task<Result<string, ErrorWrapper>> Send(object message, string invokerName = null);
    }
}