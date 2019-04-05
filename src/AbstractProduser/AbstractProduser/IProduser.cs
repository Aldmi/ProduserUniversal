using System;
using System.Threading.Tasks;
using AbstractProduser.Helpers;
using CSharpFunctionalExtensions;

namespace AbstractProduser.AbstractProduser
{
    public interface IProduser : IDisposable
    {
        Task<Result<string, ErrorWrapper>> Send(string message);
        Task<Result<string, ErrorWrapper>> Send(object message);
    }
}