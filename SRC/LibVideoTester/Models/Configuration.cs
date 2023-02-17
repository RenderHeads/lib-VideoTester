﻿using System;
namespace LibVideoTester.Models
{
public struct Configuration
{
    public readonly string[] ValidCodecs;
    public readonly int MaxWidth;
    public readonly int MaxHeight;
    public readonly int[] FrameRates;
    public readonly int MaxBitRate;
    public readonly string Name;

    //NOTE: If you add an attribute, be sure to include it in custom Equals
    public Configuration(string name, string[] validCodecs, int maxWidth, int maxHeight, int[] frameRates, int maxBitrate)
    {
        Name = name;
        ValidCodecs = validCodecs;
        MaxWidth = maxWidth;
        MaxHeight = maxHeight;
        FrameRates = frameRates;
        MaxBitRate = maxBitrate;
    }


    public override string ToString()
    {
        return $"Configuration: Name: {Name} ValidCodecs: {string.Join(',', ValidCodecs)} FrameRates: {string.Join(',', FrameRates)} MaxWidth:{MaxWidth} MaxHeight: {MaxHeight} Bitrate: {MaxBitRate}";
    }

    //NOTE: If you add an attribute, be sure to include it in custom Equals
    public override bool Equals(object obj)
    {
        if (obj is Configuration)
        {

            Configuration compareTo = (Configuration)obj;
            bool frameRatesEqual = FrameRates.Length == compareTo.FrameRates.Length;
            if (!frameRatesEqual)
            {
                return false;
            }
            for (int i = 0; i < FrameRates.Length; i++)
            {
                if (FrameRates[i] != compareTo.FrameRates[i])
                {
                    return false;
                }
            }

            return Name == compareTo.Name && MaxWidth == compareTo.MaxWidth &&
                   MaxHeight == compareTo.MaxHeight &&
                   MaxBitRate == compareTo.MaxBitRate &&
                   String.Join(' ', ValidCodecs) == String.Join(' ', compareTo.ValidCodecs);

        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ValidCodecs, MaxWidth, MaxHeight, FrameRates, MaxBitRate);
    }
}
}

