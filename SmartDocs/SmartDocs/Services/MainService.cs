using SmartDocs.Entities;
using System;
using System.Threading.Tasks;

namespace SmartDocs.Services
{
    public class MainService : IMainService
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IEncryptService _encryptService;
        private readonly SmartDocsDbContext _dbContext;

        public MainService(IBlockchainService blockchainService, 
            IEncryptService encryptService, 
            SmartDocsDbContext dbContext)
        {
            _blockchainService = blockchainService;
            _encryptService = encryptService;
            _dbContext = dbContext;
        }

        public async Task<bool> GetTransactionAsync(string transactionAddress, string message)
        {
            var messageAfterHashing = _encryptService.Encrypt(message);
            var originalMessage = await _blockchainService.GetTransaction(transactionAddress);
            if (string.IsNullOrEmpty(originalMessage))
                return false;

            return ValidateMessage(messageAfterHashing, originalMessage);
        }

        public async Task<bool> SendHashedTransaction(int userId, string message)
        {
            var messageAfterHashing = _encryptService.Encrypt(message);
            var transactionAddress = await _blockchainService.SendTransaction(messageAfterHashing);
            if (string.IsNullOrEmpty(transactionAddress))
                return false;

            var tr = new BlockchainTransaction()
            {
                TransactionAddress = transactionAddress,
                UserId = userId
            };
            _dbContext.Add(tr);

            try
            {            
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                //todo: can add logging;
                return false;
            }

            return true;
        }


        private bool ValidateMessage(string messageAfterHashing, string originalMessage)
        {
            return messageAfterHashing.Equals(originalMessage);
        }
    }
}
