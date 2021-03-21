# smart-docs
The project about integration .net core web app with Ethereum blokchain based on the Solidity language.

***** LOCALHOST SETUP *****

1.Running the .net core web app requires .net core SDK 3.1. It was created in Visual Studio 2019, and not sure if the project is fully compatible with the older VS version.
2.
3.Database stuff is managed via the EF Core, and to fasten the configuration process the Database creation and the run migrations happen just after running the project.


3.The mocked Ethereum blockchain environment:
https://github.com/nethereum/TestChains

a) To setup the test blockchain on Windows need to download the source code and then run the startgeth.bat from the https://github.com/Nethereum/TestChains/tree/master/geth-clique-windows

b) The pass.txt located in the same directory stores password of the test blockchain. In general please read the nethereum readme file and notice that:
"The preconfigured account used across every Testchain is 0x12890d2cce102216644c59daE5baed380d84830c with private key 0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7.
The account key store file password is : password"

c) Bear in mind you need to have jdk installed on your machine. It can be downloaded from: https://www.oracle.com/java/technologies/javase-jdk16-downloads.html

4.Extra info about smart contracts
a) to generate Ethereum smart contract via VS Code need to install Solidity and truffle extensions;
b) to compile the Solidity smart contract just run "truffle compile" in the powershell terminal.


