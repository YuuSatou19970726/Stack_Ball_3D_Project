using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class SecurePlayerPrefs
{
    private string secret;
    private List<string> properties = new List<string>();

    // params: pass a variable number of arguments to a method.
    // This can be particularly useful when you don’t know in advance how many arguments you will need to pass.
    public SecurePlayerPrefs(string _secret, params string[] _properties)
    {
        secret = _secret;
        foreach (string property in _properties)
            properties.Add(property);

        Save();
    }

    private string GenerateCheckSum()
    {
        string hashKey = "";
        foreach (string property in properties)
        {
            hashKey += property + ":";
            // PlayerPrefs.HasKey: used to check if a specific key exists in the PlayerPrefs.
            // It returns true if the key exists and false if it doesn't
            if (PlayerPrefs.HasKey(property))
                hashKey += PlayerPrefs.GetInt(property);
        }

        return Md5Sum(hashKey + secret);
    }

    public void Save()
    {
        string checkSum = GenerateCheckSum();
        PlayerPrefs.SetString(PlayerPrefsTags.CHECKSUM + secret, checkSum);
        PlayerPrefs.Save();
    }

    public bool HasBeenEdited()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsTags.CHECKSUM + secret))
            return true;

        string checksumSaved = PlayerPrefs.GetString(PlayerPrefsTags.CHECKSUM + secret);
        string checksumReal = GenerateCheckSum();
        return !checksumSaved.Equals(checksumReal);
    }

    private string Md5Sum(string _strToEncrypt)
    {
        // System.Text.UTF8Encoding: used to encode and decode strings in UTF-8 format
        // UTF8Encoding.GetBytes: used to encode a string into a sequence of bytes using UTF-8 encoding
        System.Text.UTF8Encoding uTF8 = new System.Text.UTF8Encoding();
        byte[] bytes = uTF8.GetBytes(_strToEncrypt);

        // encrypt bytes
        // MD5CryptoServiceProvider: used to compute the MD5 hash value for input data
        // MD5CryptoServiceProvider.ComputeHash: method converts the string into a byte array, computes the hash, and then converts the hash back into a readable hexadecimal string.
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');

        // used to right-align the characters in a string by padding them on the left with a specified Unicode character, up to a specified total length.
        return hashString.PadLeft(32, '0');
    }
}
