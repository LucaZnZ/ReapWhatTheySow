using System;
using System.Collections.Generic;

namespace ZnZUtil.Settings
{
    public abstract class SettingHandle
    {
        public string name { get; private set; }
        public abstract SettingValueType type { get; }

        protected SettingHandle(string name)
        {
            this.name = name;
        }
    }

    public abstract class SettingHandle<T> : SettingHandle
    {
        private T value;
        public event Action<T> onValueChanged;

        protected SettingHandle(string name, T value = default) : base(name)
        {
            this.value = value;
        }

        public void SetValue(T value)
        {
            this.value = value;
            onValueChanged?.Invoke(value);
        }

        public T GetValue() => value;
    }

    public class SettingFloatHandle : SettingHandle<float>
    {
        public override SettingValueType type => SettingValueType.Float;
        public float min, max;

        public SettingFloatHandle(string name, float value = default) : base(name, value)
        {
        }

        public SettingFloatHandle SetBoundaries(float min, float max)
        {
            this.min = min;
            this.max = max;
            return this;
        }
    }

    public class SettingIntHandle : SettingHandle<int>
    {
        public override SettingValueType type => SettingValueType.Float;
        public int min, max;

        public SettingIntHandle(string name, int value = default) : base(name, value)
        {
        }

        public SettingIntHandle SetBoundaries(int min, int max)
        {
            this.min = min;
            this.max = max;
            return this;
        }
    }

    public class SettingStringHandle : SettingHandle<string>
    {
        public override SettingValueType type => SettingValueType.String;
        public int maxLength;

        public SettingStringHandle(string name, string value = default) : base(name, value)
        {
        }

        public SettingStringHandle SetBoundaries(int max)
        {
            maxLength = max;
            return this;
        }
    }

    public class SettingTextHandle : SettingStringHandle
    {
        public override SettingValueType type => SettingValueType.Text;

        public SettingTextHandle(string name, string value = default) : base(name, value)
        {
        }
    }

    public class SettingBoolHandle : SettingHandle<bool>
    {
        public override SettingValueType type => SettingValueType.Bool;

        public SettingBoolHandle(string name, bool value = default) : base(name, value)
        {
        }
    }

    public class SettingPageHandle
    {
        public readonly string name;
        public readonly List<SettingHandle> settings = new();

        public SettingPageHandle(string name)
        {
            this.name = name;
        }

        public void AddSetting(SettingHandle setting) => settings.Add(setting);
    }
}