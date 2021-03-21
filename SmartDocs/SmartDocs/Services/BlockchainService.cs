using Nethereum.Geth;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace SmartDocs.Services
{
    public class BlockchainService : IBlockchainService
    {
        //this value comes from the compiled by the Solidity proj bin/DocumentManager.bin
        private readonly string byteCode = "608060405234801561001057600080fd5b506040516104473803806104478339810180604052602081101561003357600080fd5b81019080805164010000000081111561004b57600080fd5b8201602081018481111561005e57600080fd5b815164010000000081118282018710171561007857600080fd5b505080519093506100929250600091506020840190610099565b5050610134565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106100da57805160ff1916838001178555610107565b82800160010185558215610107579182015b828111156101075782518255916020019190600101906100ec565b50610113929150610117565b5090565b61013191905b80821115610113576000815560010161011d565b90565b610304806101436000396000f3fe60806040526004361061004b5763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663aea3116e8114610050578063e756e944146100da575b600080fd5b34801561005c57600080fd5b5061006561018d565b6040805160208082528351818301528351919283929083019185019080838360005b8381101561009f578181015183820152602001610087565b50505050905090810190601f1680156100cc5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b3480156100e657600080fd5b50610065600480360360208110156100fd57600080fd5b81019060208101813564010000000081111561011857600080fd5b82018360208201111561012a57600080fd5b8035906020019184600183028401116401000000008311171561014c57600080fd5b91908080601f016020809104026020016040519081016040528093929190818152602001838380828437600092019190915250929550610224945050505050565b60008054604080516020601f60026000196101006001881615020190951694909404938401819004810282018101909252828152606093909290918301828280156102195780601f106101ee57610100808354040283529160200191610219565b820191906000526020600020905b8154815290600101906020018083116101fc57829003601f168201915b505050505090505b90565b805160609061023a906000906020850190610240565b50919050565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061028157805160ff19168380011785556102ae565b828001600101855582156102ae579182015b828111156102ae578251825591602001919060010190610293565b506102ba9291506102be565b5090565b61022191905b808211156102ba57600081556001016102c456fea165627a7a723058205d0e43a30a9600f78fc6539fab63b8759795fdf4a646158c67fb333d083851760029";
        //this value comes from the compiled by the Solidity proj bin/DocumentManager.abi
        private readonly string abi = @"[{'constant':true,'inputs':[],'name':'getOrginalMessage','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_orginalMessage','type':'string'}],'name':'setOrginalMessage','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[{'name':'_Message','type':'string'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";

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
                var contractBody = contract.GetFunction("getOrginalMessage");
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
