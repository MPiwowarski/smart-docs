using Microsoft.AspNetCore.Mvc;
using SmartDocs.Services;
using System.Threading.Tasks;

namespace SmartDocs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlockchainController : ControllerBase
    {
        private readonly IMainService _mainService;

        public BlockchainController(IMainService mainService)
        {
            _mainService = mainService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransaction(string transactionAddress, string message)
        {
            return Ok(await _mainService.GetTransactionAsync(transactionAddress, message));
        }

        [HttpPost]
        public async Task<IActionResult> SendTransaction(int userId, string message)
        {
            return Ok(await _mainService.SendHashedTransaction(userId, message));
        }
    }
}
