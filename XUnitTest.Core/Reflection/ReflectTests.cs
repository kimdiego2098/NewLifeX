﻿using System;
using System.Collections;
using System.Collections.Generic;
using NewLife.Data;
using NewLife.IoT.ThingModels;
using NewLife.Reflection;
using Xunit;

namespace XUnitTest.Reflection;

public class ReflectTests
{
    [Theory]
    [InlineData(typeof(Boolean))]
    [InlineData(typeof(Char))]
    [InlineData(typeof(SByte))]
    [InlineData(typeof(Byte))]
    [InlineData(typeof(Int16))]
    [InlineData(typeof(UInt16))]
    [InlineData(typeof(Int32))]
    [InlineData(typeof(UInt32))]
    [InlineData(typeof(Int64))]
    [InlineData(typeof(UInt64))]
    [InlineData(typeof(Single))]
    [InlineData(typeof(Double))]
    [InlineData(typeof(Decimal))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(String))]
    [InlineData(typeof(Guid))]
    [InlineData(typeof(Byte[]))]
    public void GetTypeExTest(Type type)
    {
        var name = type.Name;
        var t2 = name.GetTypeEx();
        Assert.Equal(type, t2);
    }

    [Theory]
    [InlineData(typeof(Boolean), TypeCode.Boolean)]
    [InlineData(typeof(Char), TypeCode.Char)]
    [InlineData(typeof(SByte), TypeCode.SByte)]
    [InlineData(typeof(Byte), TypeCode.Byte)]
    [InlineData(typeof(Int16), TypeCode.Int16)]
    [InlineData(typeof(UInt16), TypeCode.UInt16)]
    [InlineData(typeof(Int32), TypeCode.Int32)]
    [InlineData(typeof(UInt32), TypeCode.UInt32)]
    [InlineData(typeof(Int64), TypeCode.Int64)]
    [InlineData(typeof(UInt64), TypeCode.UInt64)]
    [InlineData(typeof(Single), TypeCode.Single)]
    [InlineData(typeof(Double), TypeCode.Double)]
    [InlineData(typeof(Decimal), TypeCode.Decimal)]
    [InlineData(typeof(DateTime), TypeCode.DateTime)]
    [InlineData(typeof(String), TypeCode.String)]
    [InlineData(typeof(Guid), TypeCode.Object)]
    [InlineData(typeof(Byte[]), TypeCode.Object)]
    [InlineData(typeof(TimeSpan), TypeCode.Object)]
    [InlineData(typeof(Enum), TypeCode.Object)]
    [InlineData(typeof(ServiceStatus), TypeCode.Int32)]
    public void GetTypeCode(Type type, TypeCode code)
    {
        Assert.Equal(code, Type.GetTypeCode(type));
    }

    [Theory]
    [InlineData(typeof(Boolean))]
    [InlineData(typeof(Char))]
    [InlineData(typeof(SByte))]
    [InlineData(typeof(Byte))]
    [InlineData(typeof(Int16))]
    [InlineData(typeof(UInt16))]
    [InlineData(typeof(Int32))]
    [InlineData(typeof(UInt32))]
    [InlineData(typeof(Int64))]
    [InlineData(typeof(UInt64))]
    [InlineData(typeof(Single))]
    [InlineData(typeof(Double))]
    [InlineData(typeof(Decimal))]
    [InlineData(typeof(DateTime))]
    //[InlineData(typeof(String))]
    [InlineData(typeof(Guid))]
    //[InlineData(typeof(Byte[]))]
    [InlineData(typeof(TimeSpan))]
    //[InlineData(typeof(Enum))]
    [InlineData(typeof(ServiceStatus))]
    public void IsValueType(Type type)
    {
        Assert.True(type.IsValueType);
    }

    [Theory]
    [InlineData(typeof(Boolean))]
    [InlineData(typeof(Char))]
    [InlineData(typeof(SByte))]
    [InlineData(typeof(Byte))]
    [InlineData(typeof(Int16))]
    [InlineData(typeof(UInt16))]
    [InlineData(typeof(Int32))]
    [InlineData(typeof(UInt32))]
    [InlineData(typeof(Int64))]
    [InlineData(typeof(UInt64))]
    [InlineData(typeof(Single))]
    [InlineData(typeof(Double))]
    //[InlineData(typeof(Decimal))]
    //[InlineData(typeof(DateTime))]
    //[InlineData(typeof(String))]
    //[InlineData(typeof(Guid))]
    //[InlineData(typeof(Byte[]))]
    //[InlineData(typeof(TimeSpan))]
    //[InlineData(typeof(Enum))]
    //[InlineData(typeof(ServiceStatus))]
    public void IsPrimitive(Type type)
    {
        Assert.True(type.IsPrimitive);
    }

    [Theory]
    [InlineData(typeof(Boolean))]
    [InlineData(typeof(Char))]
    [InlineData(typeof(SByte))]
    [InlineData(typeof(Byte))]
    [InlineData(typeof(Int16))]
    [InlineData(typeof(UInt16))]
    [InlineData(typeof(Int32))]
    [InlineData(typeof(UInt32))]
    [InlineData(typeof(Int64))]
    [InlineData(typeof(UInt64))]
    [InlineData(typeof(Single))]
    [InlineData(typeof(Double))]
    [InlineData(typeof(Decimal))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(String))]
    [InlineData(typeof(Boolean?))]
    [InlineData(typeof(Char?))]
    [InlineData(typeof(SByte?))]
    [InlineData(typeof(Byte?))]
    [InlineData(typeof(Int16?))]
    [InlineData(typeof(UInt16?))]
    [InlineData(typeof(Int32?))]
    [InlineData(typeof(UInt32?))]
    [InlineData(typeof(Int64?))]
    [InlineData(typeof(UInt64?))]
    [InlineData(typeof(Single?))]
    [InlineData(typeof(Double?))]
    [InlineData(typeof(Decimal?))]
    [InlineData(typeof(DateTime?))]
    //[InlineData(typeof(Guid))]
    //[InlineData(typeof(Byte[]))]
    //[InlineData(typeof(TimeSpan))]
    //[InlineData(typeof(Enum))]
    [InlineData(typeof(ServiceStatus))]
    [InlineData(typeof(ServiceStatus?))]
    public void IsBaseType(Type type)
    {
        Assert.True(type.IsBaseType());
    }

    [Theory]
    [InlineData("true", typeof(Boolean), true)]
    [InlineData("1", typeof(Boolean), true)]
    [InlineData("0", typeof(Boolean), false)]
    [InlineData("1234", typeof(Int16), (Int16)1234)]
    [InlineData("1234", typeof(Int32), 1234)]
    [InlineData("12.34", typeof(Double), 12.34)]
    [InlineData("byte[]", typeof(Type), typeof(Byte[]))]
    public void ChangeTypeTest(Object value, Type targetType, Object target)
    {
        var rs = value.ChangeType(targetType);
        Assert.Equal(target, rs);
    }

    [Fact]
    public void DateTimeOffsetChangeTypeTest()
    {
        var value = "2023/4/5 11:32 +08:00";
        var targetType = typeof(DateTimeOffset);
        var target = new DateTimeOffset(2023, 4, 5, 11, 32, 00, TimeSpan.FromHours(8));

        var rs = value.ChangeType(targetType);
        Assert.Equal(target, rs);
    }

    [Fact]
    public void AsListTest()
    {
        var list = new List<Int32>();
        var type = list.GetType();

        Assert.True(type.As<List<Int32>>());
        Assert.True(type.As<IList<Int32>>());
        Assert.True(type.As<IList>());
        Assert.True(type.As(typeof(List<Int32>)));
        Assert.True(type.As(typeof(IList<Int32>)));
        Assert.True(type.As(typeof(IList<>)));
    }

    [Fact]
    public void AsDictionaryTest()
    {
        var dic = new Dictionary<Int32, String>();
        var type = dic.GetType();

        Assert.True(type.As<Dictionary<Int32, String>>());
        Assert.True(type.As<IDictionary<Int32, String>>());
        Assert.True(type.As<IDictionary>());
        Assert.True(type.As(typeof(Dictionary<Int32, String>)));
        Assert.True(type.As(typeof(IDictionary<Int32, String>)));
        Assert.True(type.As(typeof(IDictionary<,>)));
    }

    [Fact]
    public void CreateInstance()
    {
        var type = typeof(PageParameter);
        var obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<PageParameter>(obj);

        type = typeof(DbTable);
        obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<DbTable>(obj);
    }

    //[Fact]
    //public void CreateInstanceForArray()
    //{
    //    var type = typeof(Int32[]);
    //    var obj = type.CreateInstance();
    //    Assert.NotNull(obj);
    //    Assert.IsType<Int32[]>(obj);

    //    type = typeof(PageParameter[]);
    //    obj = type.CreateInstance();
    //    Assert.NotNull(obj);
    //    Assert.IsType<PageParameter[]>(obj);
    //}

    [Fact]
    public void CreateInstanceForList()
    {
        var type = typeof(List<Int32>);
        var obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<List<Int32>>(obj);

        type = typeof(IList<PageParameter>);
        obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<List<PageParameter>>(obj);

        type = typeof(IList);
        obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<List<Object>>(obj);
    }

    [Fact]
    public void CreateInstanceForDictionary()
    {
        var type = typeof(Dictionary<Int32, String>);
        var obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<Dictionary<Int32, String>>(obj);

        type = typeof(IDictionary<String, PageParameter>);
        obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<Dictionary<String, PageParameter>>(obj);

        type = typeof(IDictionary);
        obj = type.CreateInstance();
        Assert.NotNull(obj);
        Assert.IsType<Dictionary<Object, Object>>(obj);
    }
}