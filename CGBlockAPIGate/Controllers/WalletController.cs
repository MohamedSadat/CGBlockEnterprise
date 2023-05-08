using CGBlockBussines;
using CGBlockDA;
using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CGBlockAPIGate.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IApp app;

        public WalletController(IApp app)
        {
            this.app = app;
        }
    
        [HttpPost("Send")]
        public  ActionResult<LedgerTransModel> Send([FromBody] LedgerTransModel trans)
        {
            try
            {
                WalletModel wallet = new WalletModel();
                wallet.PublicKey = "bf020af2dadec840cc6e979924be2c75e1a4c3567c93185ed5a0f7beff8df5a4";
                wallet.Address = "c7d67c3ad12097f3a86a8c0b23380c511ee5322558dc49553944559352e2f325";
                wallet.PrivateKey = "58AE8D527DF86DEB04F2C256ACF185E81DE86C9A286D608782F214CFC95D6FB8";
                var x = new BLWallet();
                x.Wallet = wallet;

                GlobalConfig.StartupWallet(wallet);



                var t = new LedgerTransModel
                {
                    Amount = 15,
                    Sender = wallet.Address,
                    Receiver = "9766d4ffaa2e599dddc66ff936948fc7fad3961227dfebda0e21f6e3ae012400",
                    Note = "send again",
                    Fee = 0,
                    PublicKey = wallet.PublicKey,
                    TransType = CGBlockInfra.CGTypes.TTransType.Normal
                };


                x.Sign(trans,"");
                //if (await x.CancelJournalAsync() != true)
                //{
                //    return BadRequest(trans);
                //}
                //else
                //{
                //    return Ok(trans);
                //}
                return trans;

            }
            catch (Exception ex)
            {
               
                return BadRequest();
            }
        }
    }
}
