using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProduserUniversal.Helpers;

namespace ProduserUniversal.AbstractProduser
{
    public interface IProduser : IDisposable
    {
        Task<Result<string, ErrorWrapper>> Send(string message);
        Task<Result<string, ErrorWrapper>> Send(object message);

    }
}