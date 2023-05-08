using CGBlockDA;
using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using CGBlockInfra.WebView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CGBlockAPIGate.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BlockExplorerController : ControllerBase
    {
        private readonly IApp app;

        public BlockExplorerController(IApp app)
        {
            this.app = app;
            GlobalConfig.StartupApp(app);
        }
      
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task< AccountVM> GetLedgerTrans(string id)
        {
            var account = new AccountVM();
            account.Address = id;
            account.Transactions =await CBlockExplorer.GetTransByAddressAsync(id);
            return account;
        }
    }
}
