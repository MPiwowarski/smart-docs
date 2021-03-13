using Microsoft.AspNetCore.Mvc;
using SmartDocs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Controllers
{
    public class BlockchainController : Controller
    {
        private readonly IMainService _mainService;

        public BlockchainController(IMainService mainService)
        {
            _mainService = mainService;
        }

        public async Task<IActionResult> GetTransaction(string transactionAddress, string message)
        {
            return Ok(await _mainService.GetTransactionAsync(transactionAddress,message));
        }

        public async Task<IActionResult> SendTransaction(int userId, string message)
        {
            return Ok(await _mainService.SendHashedTransaction(userId, message));
        }
    }
}
