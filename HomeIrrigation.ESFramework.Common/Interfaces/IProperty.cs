﻿namespace HomeIrrigation.ESFramework.Common.Interfaces
{
    // Simple abstraction to make field and property access consistent
    public interface IProperty
    {
        string Name { get; }
        object GetValue(object obj, object[] index);
        void SetValue(object obj, object val, object[] index);
    }
}
