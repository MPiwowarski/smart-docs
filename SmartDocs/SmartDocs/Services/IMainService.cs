using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Services
{
    public interface IMainService
    {
        Task<bool> SendHashedTransaction(int userId, string message);

        Task<bool> GetTransactionAsync(string transactionAddress, string message);
    }
}
