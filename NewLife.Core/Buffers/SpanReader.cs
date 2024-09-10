﻿using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace NewLife.Buffers;

/// <summary>Span读取器</summary>
/// <param name="span"></param>
public ref struct SpanReader(Span<Byte> span)
{
    #region 属性
    private readonly Span<Byte> _span = span;

    private Int32 _index;
    /// <summary>已读取字节数</summary>
    public Int32 Position => _index;

    /// <summary>总容量</summary>
    public Int32 Capacity => _span.Length;

    /// <summary>空闲容量</summary>
    public Int32 FreeCapacity => _span.Length - _index;

    /// <summary>是否小端字节序。默认true</summary>
    public Boolean IsLittleEndian { get; set; } = true;
    #endregion

    #region 基础方法
    /// <summary>告知有多少数据已从缓冲区读取</summary>
    /// <param name="count"></param>
    public void Advance(Int32 count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (_index + count > _span.Length) throw new ArgumentOutOfRangeException(nameof(count));

        _index += count;
    }

    /// <summary>返回要写入到的Span，其大小按 sizeHint 参数指定至少为所请求的大小</summary>
    /// <param name="sizeHint"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Span<Byte> GetSpan(Int32 sizeHint = 0)
    {
        if (sizeHint > FreeCapacity) throw new ArgumentOutOfRangeException(nameof(sizeHint));

        return _span[_index..];
    }
    #endregion

    #region 读取方法
    /// <summary>确保缓冲区中有足够的空间。</summary>
    /// <param name="size">需要的字节数。</param>
    private void EnsureSpace(Int32 size)
    {
        if (_index + size > _span.Length)
            throw new InvalidOperationException("Not enough data to read.");
    }

    /// <summary>读取单个字节</summary>
    /// <returns></returns>
    public Byte ReadByte()
    {
        var size = sizeof(Byte);
        EnsureSpace(size);
        var result = _span[_index];
        _index += size;
        return result;
    }

    /// <summary>读取Int16整数</summary>
    /// <returns></returns>
    public Int16 ReadInt16()
    {
        var size = sizeof(Int16);
        EnsureSpace(size);
        var result = IsLittleEndian ?
            BinaryPrimitives.ReadInt16LittleEndian(_span.Slice(_index, size)) :
            BinaryPrimitives.ReadInt16BigEndian(_span.Slice(_index, size));
        _index += size;
        return result;
    }

    /// <summary>读取UInt16整数</summary>
    /// <returns></returns>
    public UInt16 ReadUInt16()
    {
        var size = sizeof(UInt16);
        EnsureSpace(size);
        var result = IsLittleEndian ?
            BinaryPrimitives.ReadUInt16LittleEndian(_span.Slice(_index, size)) :
            BinaryPrimitives.ReadUInt16BigEndian(_span.Slice(_index, size));
        _index += size;
        return result;
    }

    /// <summary>读取Int32整数</summary>
    /// <returns></returns>
    public Int32 ReadInt32()
    {
        var size = sizeof(Int32);
        EnsureSpace(size);
        var result = IsLittleEndian ?
            BinaryPrimitives.ReadInt32LittleEndian(_span.Slice(_index, size)) :
            BinaryPrimitives.ReadInt32BigEndian(_span.Slice(_index, size));
        _index += size;
        return result;
    }

    /// <summary>读取UInt32整数</summary>
    /// <returns></returns>
    public UInt32 ReadUInt32()
    {
        var size = sizeof(UInt32);
        EnsureSpace(size);
        var result = IsLittleEndian ?
            BinaryPrimitives.ReadUInt32LittleEndian(_span.Slice(_index, size)) :
            BinaryPrimitives.ReadUInt32BigEndian(_span.Slice(_index, size));
        _index += size;
        return result;
    }

    /// <summary>以压缩格式读取32位整数</summary>
    /// <returns></returns>
    public Int32 ReadEncodedInt()
    {
        Byte b;
        UInt32 rs = 0;
        Byte n = 0;
        while (true)
        {
            var bt = ReadByte();
            if (bt < 0) throw new Exception($"The data stream is out of range! The integer read is {rs: n0}");
            b = (Byte)bt;

            // 必须转为Int32，否则可能溢出
            rs |= (UInt32)((b & 0x7f) << n);
            if ((b & 0x80) == 0) break;

            n += 7;
            if (n >= 32) throw new FormatException("The number value is too large to read in compressed format!");
        }
        return (Int32)rs;
    }

    /// <summary>读取Int64整数</summary>
    /// <returns></returns>
    public Int64 ReadInt64()
    {
        var size = sizeof(Int64);
        EnsureSpace(size);
        var result = IsLittleEndian ?
            BinaryPrimitives.ReadInt64LittleEndian(_span.Slice(_index, size)) :
            BinaryPrimitives.ReadInt64BigEndian(_span.Slice(_index, size));
        _index += size;
        return result;
    }

    /// <summary>读取UInt64整数</summary>
    /// <returns></returns>
    public UInt64 ReadUInt64()
    {
        var size = sizeof(UInt64);
        EnsureSpace(size);
        var result = IsLittleEndian ?
            BinaryPrimitives.ReadUInt64LittleEndian(_span.Slice(_index, size)) :
            BinaryPrimitives.ReadUInt64BigEndian(_span.Slice(_index, size));
        _index += size;
        return result;
    }

    /// <summary>读取单精度浮点数</summary>
    /// <returns></returns>
    public unsafe Single ReadSingle()
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
        return BitConverter.Int32BitsToSingle(ReadInt32());
#else
        var result = ReadInt32();
        return Unsafe.ReadUnaligned<Single>(ref Unsafe.As<Int32, Byte>(ref result));
#endif
    }

    /// <summary>读取双精度浮点数</summary>
    /// <returns></returns>
    public Double ReadDouble()
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP
        return BitConverter.Int64BitsToDouble(ReadInt64());
#else
        var result = ReadInt64();
        return Unsafe.ReadUnaligned<Double>(ref Unsafe.As<Int64, Byte>(ref result));
#endif
    }

    /// <summary>读取字符串</summary>
    /// <param name="length"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public String ReadString(Int32 length, Encoding encoding)
    {
        EnsureSpace(length);

        var result = encoding.GetString(_span.Slice(_index, length));
        _index += length;
        return result;
    }

    /// <summary>读取字节数组</summary>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Span<Byte> ReadBytes(Int32 length)
    {
        EnsureSpace(length);

        var result = _span.Slice(_index, length);
        _index += length;
        return result;
    }

    /// <summary>读取结构体</summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Read<T>() where T : struct
    {
        var size = Unsafe.SizeOf<T>();
        EnsureSpace(size);

        var result = MemoryMarshal.Read<T>(_span.Slice(_index));
        _index += size;
        return result;
    }
    #endregion
}
