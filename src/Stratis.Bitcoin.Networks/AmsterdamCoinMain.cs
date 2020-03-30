using System;
using System.Collections.Generic;
using System.Net;
using NBitcoin;
using NBitcoin.BouncyCastle.Math;
using NBitcoin.DataEncoders;
using NBitcoin.Protocol;
using Stratis.Bitcoin.Networks.Deployments;
using Stratis.Bitcoin.Networks.Policies;

namespace Stratis.Bitcoin.Networks
{
    public class AmsterdamCoinMain : Network
    {
        /// <summary> The name of the root folder containing the different AmsterdamCoin blockchains (AmsterdamCoinMain, AmsterdamCoinTest, AmsterdamCoinRegTest). </summary>
        public const string AmsterdamCoinRootFolderName = "amsterdamcoin";

        /// <summary> The default name used for the AmsterdamCoin configuration file. </summary>
        public const string AmsterdamCoinDefaultConfigFilename = "amsterdamcoin.conf";

        public AmsterdamCoinMain()
        {
            // The message start string is designed to be unlikely to occur in normal data.
            // The characters are rarely used upper ASCII, not valid as UTF-8, and produce
            // a large 4-byte int at any alignment.
            var messageStart = new byte[4];
            messageStart[0] = 0x82;
            messageStart[1] = 0x81;
            messageStart[2] = 0x80;
            messageStart[3] = 0x11;
            uint magic = BitConverter.ToUInt32(messageStart, 0); //0x6233671;

            this.Name = "AmsterdamCoinMain";
            this.NetworkType = NetworkType.Mainnet;
            this.Magic = magic;
            this.DefaultPort = 50000;
            this.DefaultMaxOutboundConnections = 16;
            this.DefaultMaxInboundConnections = 109;
            this.DefaultRPCPort = 51000;
            this.DefaultAPIPort = 63000;
            this.MaxTipAge = 2 * 60 * 60;
            this.MinTxFee = 10000;
            this.FallbackFee = 10000;
            this.MinRelayTxFee = 10000;
            this.RootFolderName = AmsterdamCoinRootFolderName;
            this.DefaultConfigFilename = AmsterdamCoinDefaultConfigFilename;
            this.MaxTimeOffsetSeconds = 25 * 60;
            this.CoinTicker = "AMS";
            
            var consensusFactory = new PosConsensusFactory();

            // Create the genesis block.
            this.GenesisTime = 1585312300;
            this.GenesisNonce = 1904723;
            this.GenesisBits = 0x1e0fffff;
            this.GenesisVersion = 1;
            this.GenesisReward = Money.Zero;

            Block genesisBlock = CreateAmsterdamCoinGenesisBlock(consensusFactory, this.GenesisTime, this.GenesisNonce, this.GenesisBits, this.GenesisVersion, this.GenesisReward);

            this.Genesis = genesisBlock;

            // Taken from SolarisX.
            var consensusOptions = new PosConsensusOptions(
                maxBlockBaseSize: 1_000_000,
                maxStandardVersion: 2,
                maxStandardTxWeight: 100_000,
                maxBlockSigopsCost: 20_000,
                maxStandardTxSigopsCost: 20_000 / 5
            );

            var buriedDeployments = new BuriedDeploymentsArray
            {
                [BuriedDeployments.BIP34] = 0,
                [BuriedDeployments.BIP65] = 0,
                [BuriedDeployments.BIP66] = 0
            };

            var bip9Deployments = new StratisBIP9Deployments
            {
                [StratisBIP9Deployments.ColdStaking] = new BIP9DeploymentsParameters(2, BIP9DeploymentsParameters.AlwaysActive, 999999999)
            };

            this.Consensus = new Consensus(
                consensusFactory: consensusFactory,
                consensusOptions: consensusOptions,
                coinType: 524,
                hashGenesisBlock: genesisBlock.GetHash(),
                subsidyHalvingInterval: 210000,
                majorityEnforceBlockUpgrade: 750,
                majorityRejectBlockOutdated: 950,
                majorityWindow: 1000,
                buriedDeployments: buriedDeployments,
                bip9Deployments: bip9Deployments,
                bip34Hash: new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"),
                ruleChangeActivationThreshold: 1916, // 95% of 2016
                minerConfirmationWindow: 2016, // nPowTargetTimespan / nPowTargetSpacing
                maxReorgLength: 500,
                defaultAssumeValid: new uint256("0x0000000000000000000000000000000000000000000000000000000000000000"), // 0
                maxMoney: long.MaxValue,
                coinbaseMaturity: 50,
                premineHeight: 1,
                premineReward: Money.Coins(144754435m),
                proofOfWorkReward: Money.Coins(2m),
                powTargetTimespan: TimeSpan.FromSeconds(14 * 24 * 60 * 60), // two weeks
                powTargetSpacing: TimeSpan.FromSeconds(10 * 60),
                powAllowMinDifficultyBlocks: false,
                posNoRetargeting: false,
                powNoRetargeting: false,
                powLimit: new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
                minimumChainWork: null,
                isProofOfStake: true,
                lastPowBlock: 2500,
                proofOfStakeLimit: new BigInteger(uint256.Parse("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeLimitV2: new BigInteger(uint256.Parse("000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffff").ToBytes(false)),
                proofOfStakeReward: Money.Coins(2m)
            );

            this.Base58Prefixes = new byte[12][];
            this.Base58Prefixes[(int)Base58Type.PUBKEY_ADDRESS] = new byte[] { (83) };
            this.Base58Prefixes[(int)Base58Type.SCRIPT_ADDRESS] = new byte[] { (125) };
            this.Base58Prefixes[(int)Base58Type.SECRET_KEY] = new byte[] { (63 + 128) };
            this.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_NO_EC] = new byte[] { 0x01, 0x42 };
            this.Base58Prefixes[(int)Base58Type.ENCRYPTED_SECRET_KEY_EC] = new byte[] { 0x01, 0x43 };
            this.Base58Prefixes[(int)Base58Type.EXT_PUBLIC_KEY] = new byte[] { (0x04), (0x88), (0xB2), (0x1E) };
            this.Base58Prefixes[(int)Base58Type.EXT_SECRET_KEY] = new byte[] { (0x04), (0x88), (0xAD), (0xE4) };
            this.Base58Prefixes[(int)Base58Type.PASSPHRASE_CODE] = new byte[] { 0x2C, 0xE9, 0xB3, 0xE1, 0xFF, 0x39, 0xE2 };
            this.Base58Prefixes[(int)Base58Type.CONFIRMATION_CODE] = new byte[] { 0x64, 0x3B, 0xF6, 0xA8, 0x9A };
            this.Base58Prefixes[(int)Base58Type.STEALTH_ADDRESS] = new byte[] { 0x2a };
            this.Base58Prefixes[(int)Base58Type.ASSET_ID] = new byte[] { 23 };
            this.Base58Prefixes[(int)Base58Type.COLORED_ADDRESS] = new byte[] { 0x13 };

            this.Checkpoints = new Dictionary<int, CheckpointInfo>
            {
                { 0, new CheckpointInfo(new uint256("0x18fccdeafad47d3e10a391d761881fca81c8ad32e3e6fa9576363712ab88982e"), new uint256("0x0000000000000000000000000000000000000000000000000000000000000000")) },
                { 2, new CheckpointInfo(new uint256("0x862e8e39013620d1f12b939d3036d1b29205858f159db0c97ee68604ebb72751"), new uint256("0x7889a02d48988af51ab85035d17185968bd403815d737638597234edb11ad7c2")) }
            };

            this.Bech32Encoders = new Bech32Encoder[2];
            this.Bech32Encoders[(int)Bech32Type.WITNESS_PUBKEY_ADDRESS] = null;
            this.Bech32Encoders[(int)Bech32Type.WITNESS_SCRIPT_ADDRESS] = null;

            this.DNSSeeds = new List<DNSSeedData>
            {
                new DNSSeedData("node1.amsterdamcoin.network", "node1.amsterdamcoin.network"),
                new DNSSeedData("node2.amsterdamcoin.network", "node2.amsterdamcoin.network"),
                new DNSSeedData("node3.amsterdamcoin.network", "node3.amsterdamcoin.network"),
                new DNSSeedData("node4.amsterdamcoin.network", "node4.amsterdamcoin.network"),
                new DNSSeedData("node5.amsterdamcoin.network", "node5.amsterdamcoin.network"),
                new DNSSeedData("node6.amsterdamcoin.network", "node6.amsterdamcoin.network")
            };

            this.SeedNodes = new List<NetworkAddress>
            {
                new NetworkAddress(IPAddress.Parse("176.223.131.60"), 50000), //Official node 1
                new NetworkAddress(IPAddress.Parse("85.214.223.236"), 50000), //Official node 2
                new NetworkAddress(IPAddress.Parse("85.214.241.80"), 50000), //Official node 3
                new NetworkAddress(IPAddress.Parse("85.214.130.77"), 50000), //Official node 4
                new NetworkAddress(IPAddress.Parse("81.169.238.113"), 50000), //Official node 5
                new NetworkAddress(IPAddress.Parse("81.169.234.147"), 50000), //Official node 6
            };

            this.StandardScriptsRegistry = new StratisStandardScriptsRegistry();
            
            Assert(this.Consensus.HashGenesisBlock == uint256.Parse("0x18fccdeafad47d3e10a391d761881fca81c8ad32e3e6fa9576363712ab88982e"));
            Assert(this.Genesis.Header.HashMerkleRoot == uint256.Parse("0x739b30bbbf51bc06f54c049db4bbf93747800dc6efcdcf29249c1cd6e19f2a36"));
        }

        protected static Block CreateAmsterdamCoinGenesisBlock(ConsensusFactory consensusFactory, uint nTime, uint nNonce, uint nBits, int nVersion, Money genesisReward)
        {
            const string pszTimestamp = "https://www.amsterdamcoin.com";

            Transaction txNew = consensusFactory.CreateTransaction();
            txNew.Version = 1;
            txNew.Time = nTime;
            txNew.AddInput(new TxIn
            {
                ScriptSig = new Script(Op.GetPushOp(0), new Op
                {
                    Code = (OpcodeType)0x1,
                    PushData = new[] { (byte)42 }
                }, Op.GetPushOp(Encoders.ASCII.DecodeData(pszTimestamp)))
            });
            txNew.AddOutput(new TxOut()
            {
                Value = genesisReward,
            });

            Block genesis = consensusFactory.CreateBlock();
            genesis.Header.BlockTime = Utils.UnixTimeToDateTime(nTime);
            genesis.Header.Bits = nBits;
            genesis.Header.Nonce = nNonce;
            genesis.Header.Version = nVersion;
            genesis.Transactions.Add(txNew);
            genesis.Header.HashPrevBlock = uint256.Zero;
            genesis.UpdateMerkleRoot();
            return genesis;
        }
    }
}
