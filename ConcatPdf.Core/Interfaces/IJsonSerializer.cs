﻿
namespace ConcatPdf.Core.Interfaces
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T value);

        T Deserialize<T>(string value);
    }
}
