| Windows | MacOs | Ubuntu64
| :---- | :------ | :---- |
| [![Build Status](https://dev.azure.com/AmsterdamCoin/AmsterdamCoinBitcoinFullNode/_apis/build/status/HostedWindowsContainer-CI)](https://dev.azure.com/AmsterdamCoin/AmsterdamCoinBitcoinFullNode/_build/latest?definitionId=4) | [![Build Status](https://dev.azure.com/AmsterdamCoin/AmsterdamCoinBitcoinFullNode/_apis/build/status/HostedmacOS-CI)](https://dev.azure.com/AmsterdamCoin/AmsterdamCoinBitcoinFullNode/_build/latest?definitionId=6) | [![Build Status](https://dev.azure.com/AmsterdamCoin/AmsterdamCoinBitcoinFullNode/_apis/build/status/HostedUbuntu1604-CI)](https://dev.azure.com/AmsterdamCoin/AmsterdamCoinBitcoinFullNode/_build/latest?definitionId=5)

AmsterdamCoin Bitcoin
===============

https://amsterdamcoin.com

Bitcoin Implementation in C#
----------------------------

AmsterdamCoin is an implementation of the Bitcoin protocol in C# on the [.NET Core](https://dotnet.github.io/) platform.  
The node can run on the Bitcoin and AmsterdamCoin networks.  
AmsterdamCoin Bitcoin is based on the [NBitcoin](https://github.com/MetacoSA/NBitcoin) project.  

For Proof of Stake support on the AmsterdamCoin token the node is using [NStratis](https://github.com/stratisproject/NStratis) which is a POS implementation of NBitcoin.  

[.NET Core](https://dotnet.github.io/) is an open source cross platform framework and enables the development of applications and services on Windows, macOS and Linux.   

The design
----------

**A Modular Approach**

A Blockchain is made of many components, from a FullNode that validates blocks to a Simple Wallet that track addresses.
The end goal is to develop a set of [Nuget](https://en.wikipedia.org/wiki/NuGet) packages from which an implementer can cherry pick what he needs.

* **NBitcoin**
* **Stratis.Bitcoin.Core**  - The bare minimum to run a pruned node.
* **Stratis.Bitcoin.Store** - Store and relay blocks to peers.
* **Stratis.Bitcoin.MemoryPool** - Track pending transaction.
* **Stratis.Bitcoin.Wallet** - Send and Receive coins
* **Stratis.Bitcoin.Miner** - POS or POW
* **Stratis.Bitcoin.Explorer**


Create a Blockchain in a .NET Core style programming
```
  var node = new FullNodeBuilder()
   .UseNodeSettings(nodeSettings)
   .UseConsensus()
   .UseBlockStore()
   .UseMempool()
   .AddMining()
   .AddRPC()
   .Build();

  node.Run();
```

What's Next
----------

We plan to add many more features on top of the AmsterdamCoin Bitcoin blockchain:
Sidechains, Private/Permissioned blockchain, Compiled Smart Contracts, NTumbleBit/Breeze wallet and more...

Running a FullNode
------------------

The master branch is actively developed and regularly committed to, and it can become unstable.  
To compile a stable (production) release use any of the release tags form version 3.0.x.x and upwards.  

```
git clone https://github.com/AmsterdamCoinPlatform/AmsterdamCoinBitcoinFullNode.git  
cd AmsterdamCoinBitcoinFullNode\src

dotnet build

```

To run on the Bitcoin network:
```
cd Stratis.BitcoinD
dotnet run
```  

To run on the AmsterdamCoin network:
```
cd Stratis.AmsterdamCoinD
dotnet run
```  

Getting Started Guide
-----------
More details on getting started are available [here](https://github.com/AmsterdamCoin/AmsterdamCoinBitcoinFullNode/blob/master/Documentation/getting-started.md)

Development
-----------
Up for some blockchain development?

Check this guides for more info:
* [Contributing Guide](Documentation/contributing.md)
* [Coding Style](Documentation/coding-style.md)
* [Wiki Page](https://stratisplatform.atlassian.net/wiki/spaces/WIKI/overview)

There is a lot to do and we welcome contributers developers and testers who want to get some Blockchain experience.

Testing
-------
* [Testing Guidelines](Documentation/testing-guidelines.md)
