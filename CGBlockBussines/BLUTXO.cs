using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGBlockBussines
{
    public class BLUTXO
    {
        private readonly IApp app;

        public BLUTXO(IApp app)
        {
            this.app = app;
        }

        public void GenerateOutput(LedgerTransModel trans)
        {
            foreach (var vin in trans.Inputs)
            {
                var output = vin.Amount - trans.Amount;
                if (output > 0)
                {
                    vin.Spent = 1;
                    var vout = new UTXOModel
                    {
                        Amount = output,
                        TransId = trans.TransId,
                        Address = trans.Sender,
                        OutputIndex = vin.Id
                    };
                    trans.Outputs.Add(vout);
                }
                if (output == 0)
                {
                    vin.Spent = 1;
                }
                else if (output < 0)
                {

                }
            }

        }
        public void GenerateReceiverOutput(LedgerTransModel trans)
        {
            var vout = new UTXOModel
            {
                Amount = trans.Amount-trans.Fee,
                TransId = trans.TransId,
                Address = trans.Reciver,
                OutputIndex = 0
            };
            trans.Outputs.Add(vout);
        }
        public void GenerateFeeOutput(LedgerTransModel trans)
        {
            var vout = new UTXOModel
            {
                Amount =  trans.Fee,
                TransId = trans.TransId,
                Address = app.Node.Address,
                OutputIndex = 0
            };
            trans.Outputs.Add(vout);
        }

    }
}
