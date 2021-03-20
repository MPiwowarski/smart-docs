﻿using Nethereum.Geth;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace SmartDocs.Services
{
    public class BlockchainService : IBlockchainService
    {
        //this value comes from the compiled by the Solidity proj bin/DocumentManager.bin
        private readonly string byteCode = "608060405234801561001057600080fd5b506040516105323803806105328339818101604052602081101561003357600080fd5b810190808051604051939291908464010000000082111561005357600080fd5b90830190602082018581111561006857600080fd5b825164010000000081118282018810171561008257600080fd5b82525081516020918201929091019080838360005b838110156100af578181015183820152602001610097565b50505050905090810190601f1680156100dc5780820380516001836020036101000a031916815260200191505b50604052505081516100f6915060009060208401906100fd565b505061019e565b828054600181600116156101000203166002900490600052602060002090601f0160209004810192826101335760008555610179565b82601f1061014c57805160ff1916838001178555610179565b82800160010185558215610179579182015b8281111561017957825182559160200191906001019061015e565b50610185929150610189565b5090565b5b80821115610185576000815560010161018a565b610385806101ad6000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c80630e3618f1146100465780632812988f146100ee5780632b2e00d31461016b575b600080fd5b6100ec6004803603602081101561005c57600080fd5b81019060208101813564010000000081111561007757600080fd5b82018360208201111561008957600080fd5b803590602001918460018302840111640100000000831117156100ab57600080fd5b91908080601f016020809104026020016040519081016040528093929190818152602001838380828437600092019190915250929550610173945050505050565b005b6100f661018a565b6040805160208082528351818301528351919283929083019185019080838360005b83811015610130578181015183820152602001610118565b50505050905090810190601f16801561015d5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b6100f6610218565b80516101869060009060208401906102ae565b5050565b6000805460408051602060026001851615610100026000190190941693909304601f810184900484028201840190925281815292918301828280156102105780601f106101e557610100808354040283529160200191610210565b820191906000526020600020905b8154815290600101906020018083116101f357829003601f168201915b505050505081565b60008054604080516020601f60026000196101006001881615020190951694909404938401819004810282018101909252828152606093909290918301828280156102a45780601f10610279576101008083540402835291602001916102a4565b820191906000526020600020905b81548152906001019060200180831161028757829003601f168201915b5050505050905090565b828054600181600116156101000203166002900490600052602060002090601f0160209004810192826102e4576000855561032a565b82601f106102fd57805160ff191683800117855561032a565b8280016001018555821561032a579182015b8281111561032a57825182559160200191906001019061030f565b5061033692915061033a565b5090565b5b80821115610336576000815560010161033b56fea2646970667358221220e055a53bc18957bccae06d9d0735dc20a9fc1c18c7aa7fd174899f4267895ebe64736f6c63430007040033";
        //this value comes from the compiled by the Solidity proj bin/DocumentManager.abi
        private readonly string abi = "[{'inputs':[{'internalType':'string','name':'_object','type':'string'}],'stateMutability':'nonpayable','type':'constructor'},{'inputs':[],'name':'getObject','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[],'name':'object','outputs':[{'internalType':'string','name':'','type':'string'}],'stateMutability':'view','type':'function'},{'inputs':[{'internalType':'string','name':'_object','type':'string'}],'name':'setObject','outputs':[],'stateMutability':'nonpayable','type':'function'}]";
        
        // blockchain password. In case of the mocked Eth blockchain it should be copied from the pass.txt.
        private readonly string password = "password";
        // the preconfigured account used across every Testchain
        private readonly string senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";

        public async Task<string> GetTransaction(string address)
        {
            //todo:inject via DI
            var web3 = new Web3Geth();
            var result = string.Empty;

            try
            {
                var contract = web3.Eth.GetContract(abi, address);
                var contractBody = contract.GetFunction("getOriginalMessage");
                result = await contractBody.CallAsync<string>();
            }
            catch (Exception e)
            {
                //todo: log error
                throw;
            }
            return result;
        }

        public async Task<string> SendTransaction(string message)
        {
            //todo:inject via DI
            var web3 = new Web3Geth();
            string result;
            try
            {
                web3.TransactionManager.DefaultGas = BigInteger.Parse("900000");
                web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("1");

                var unlock = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 10);

                var hash = await web3.Eth.DeployContract.SendRequestAsync(abi, byteCode, senderAddress, message);
                var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(hash);
                while (receipt == null)
                {
                    Thread.Sleep(1000);
                    receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(hash);
                }
                result = receipt.ContractAddress;
            }
            catch (Exception e)
            {
                //todo: log error
                throw;
            }

            return result;
        }
    }
}
