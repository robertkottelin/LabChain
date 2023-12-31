using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public class Blockchain
{
    public IList<Block> Chain { get; private set; } // Ensuring that Chain is always initialized

    public Blockchain()
    {
        Chain = new List<Block>(); // Initialize the Chain here
        AddGenesisBlock();
    }

    private void AddGenesisBlock()
    {
        if (Chain.Count == 0)
        {
            Chain.Add(CreateGenesisBlock());
        }
    }

    private Block CreateGenesisBlock()
    {
        // Using string.Empty for the genesis block's PreviousHash
        return new Block(DateTime.Now, string.Empty, "{}");
    }

    public Block GetLatestBlock()
    {
        return Chain[Chain.Count - 1];
    }

    public void AddBlock(Block block)
    {
        Block latestBlock = GetLatestBlock();
        block.Index = latestBlock.Index + 1;
        block.PreviousHash = latestBlock.Hash;
        block.Hash = block.CalculateHash();
        Chain.Add(block);
    }

    public bool IsValid()
    {
        for (int i = 1; i < Chain.Count; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }
        return true;
    }
}

public class Block
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    public string Data { get; set; }

    public Block(DateTime timestamp, string previousHash, string data)
    {
        Index = 0;
        Timestamp = timestamp;
        PreviousHash = previousHash ?? string.Empty; // Safeguard against null values
        Data = data;
        Hash = CalculateHash();
    }

    public string CalculateHash()
    {
        SHA256 sha256 = SHA256.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes($"{Timestamp}-{PreviousHash}-{Data}");
        byte[] outputBytes = sha256.ComputeHash(inputBytes);

        return Convert.ToBase64String(outputBytes);
    }
}
