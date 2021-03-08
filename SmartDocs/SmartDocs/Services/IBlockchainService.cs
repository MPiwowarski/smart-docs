using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Services
{
    public interface IBlockchainService
    {
        Task<string> GetTransaction(string address);

        Task<string> SendTransaction(string message);
    }
}
