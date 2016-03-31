using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace ZimmerBot.Core.Utilities
{
  public delegate void AppSettingParseFailedHandler(string appSettingKey, string value, Exception parseException);

  public delegate void AppSettingMissingHandler(string appSettingKey);


  public class AppSetting
  {
    public enum TrimSetting { NoTrim, EnableTrim }
  }


  public class AppSetting<T, R> : AppSetting
  {
    public string AppSettingKey { get; protected set; }

    public bool IsRequired { get; protected set; }

    public bool EmptyIsDefault { get; protected set; }

    public TrimSetting Trim { get; protected set; }

    public R DefaultValue { get; protected set; }

    public R OverrideValue { get; protected set; }

    private bool HasOverrideValue { get; set; }

    public Func<T, R> Transform { get; protected set; }

    protected AppSettingParseFailedHandler onParseFailed;
    protected AppSettingMissingHandler onMissing;


    /// <summary>
    /// Construct required application setting (with no default value).
    /// </summary>
    /// <param name="appSettingKey"></param>
    /// <param name="onMissing"></param>
    /// <param name="onParseFailed"></param>
    public AppSetting(string appSettingKey, Func<T, R> transform, AppSettingMissingHandler onMissing = null, AppSettingParseFailedHandler onParseFailed = null, TrimSetting trim = TrimSetting.EnableTrim)
      : this(appSettingKey, transform, default(R), true, false, onMissing, onParseFailed, trim)
    {
    }


    /// <summary>
    /// Construct optional application setting (with default value)
    /// </summary>
    /// <param name="appSettingKey"></param>
    /// <param name="defaultValue"></param>
    /// <param name="emptyIsDefault"></param>
    /// <param name="onParseFailed"></param>
    public AppSetting(string appSettingKey, Func<T, R> transform, R defaultValue, bool emptyIsDefault = false, AppSettingParseFailedHandler onParseFailed = null, TrimSetting trim = TrimSetting.EnableTrim)
      : this(appSettingKey, transform, defaultValue, false, emptyIsDefault, null, onParseFailed, trim)
    {
    }


    protected AppSetting(string appSettingKey, Func<T, R> transform, R defaultValue, bool isRequired, bool emptyIsDefault, AppSettingMissingHandler onMissing, AppSettingParseFailedHandler onParseFailed,
      TrimSetting trim = TrimSetting.NoTrim)
    {
      if (appSettingKey == null)
        throw new ArgumentNullException("appSettingKey");
      if (transform == null)
        throw new ArgumentNullException("transform");
      AppSettingKey = appSettingKey;
      Transform = transform;
      Trim = trim;
      DefaultValue = defaultValue;
      IsRequired = isRequired;
      EmptyIsDefault = emptyIsDefault;
      this.onMissing = onMissing;
      this.onParseFailed = onParseFailed;
    }


    public void OverrideWithValue(R v)
    {
      OverrideValue = v;
      HasOverrideValue = true;
    }


    public void RemoveOverride()
    {
      HasOverrideValue = false;
    }


    public static implicit operator R(AppSetting<T, R> x)
    {
      return x.Value;
    }

    protected virtual T LoadFromValue(string value)
    {
      return (T)LoadFromValue(value, typeof(T));
    }


    protected virtual object LoadFromValue(string value, Type t)
    {
      if (value == null)
        return null;

      if (t == typeof(Type)) // No TypeConverter for Type apparently...
      {
        if (Trim == TrimSetting.EnableTrim)
          value = value.Trim();
        return Type.GetType(value, true);
      }
      else if (t.IsArray)
      {
        return LoadFromArray(value);
      }
      else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
      {
        return LoadFromDictionary(value);
      }
      else if (t == typeof(TimeSpan) || t == typeof(TimeSpan?))
      {
        return TimeSpanUtility.Parse(value);
      }
      else
      {
        if (Trim == TrimSetting.EnableTrim)
          value = value.Trim();
        TypeConverter converter = TypeDescriptor.GetConverter(t);
        return converter.ConvertFromInvariantString(value);
      }
    }


    protected virtual object LoadFromArray(string value)
    {
      Type elementType = typeof(T).GetElementType();

      string[] values = value.Split(',');

      Array result = Array.CreateInstance(elementType, values.Length);
      for (int i = 0; i < values.Length; ++i)
      {
        object v = LoadFromValue(values[i], elementType);
        result.SetValue(v, i);
      }

      return result;
    }


    private string[] SplitKeyValuePair(string s)
    {
      string[] result = s.Split('#');
      if (result.Length != 2)
        throw new FormatException("We only accept a key-value pair, i.e. only one 'key#value' combination");

      return result;
    }

    protected virtual object LoadFromDictionary(string value)
    {
      Type[] keyValueTypes = typeof(T).GetGenericArguments();
      Type dictType = typeof(Dictionary<,>).MakeGenericType(keyValueTypes[0], keyValueTypes[1]);

      string[] values = value.Split(',');
      string[] keyValuePair = null;

      IDictionary result = Activator.CreateInstance(dictType) as IDictionary;
      foreach (string v in values)
      {
        if (!string.IsNullOrEmpty(v.Trim()))
        {
          keyValuePair = SplitKeyValuePair(v);
          result.Add(LoadFromValue(keyValuePair[0], keyValueTypes[0]), LoadFromValue(keyValuePair[1], keyValueTypes[1]));
        }
      }

      return result;
    }


    public R Value
    {
      get
      {
        if (HasOverrideValue)
          return OverrideValue;

        string value = ConfigurationManager.AppSettings[AppSettingKey];

        if (value == null || (EmptyIsDefault && value == ""))
        {
          if (IsRequired)
          {
            if (onMissing != null)
              onMissing(AppSettingKey);
            else
              throw new InvalidOperationException(string.Format("Missing application setting '{0}'.", AppSettingKey));
            return default(R);
          }
          return DefaultValue;
        }

        try
        {
          return Transform(LoadFromValue(value));
        }
        catch (Exception ex)
        {
          if (onParseFailed != null)
            onParseFailed(AppSettingKey, value, ex);
          else if (typeof(T).IsEnum)
            throw new FormatException(string.Format("Failed to read application setting '{0}': {1} Valid values are: {2}.", AppSettingKey, ex.Message, string.Join(", ", Enum.GetNames(typeof(T)))), ex);
          else
            throw new FormatException(string.Format("Failed to read application setting '{0}': {1}", AppSettingKey, ex.Message), ex);
          return default(R);
        }
      }
    }


    public void Modify(string s)
    {
      ConfigurationManager.AppSettings[AppSettingKey] = s;
    }

    public string Raw()
    {
      return ConfigurationManager.AppSettings[AppSettingKey];
    }

    public override string ToString()
    {
      R value = Value;
      return value == null ? "" : value.ToString();
    }
  }

  /// <summary>
  /// AppSetting with identity transform
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class AppSetting<T> : AppSetting<T, T>
  {
    /// <summary>
    /// Construct required application setting (with no default value).
    /// </summary>
    /// <param name="appSettingKey"></param>
    /// <param name="onMissing"></param>
    /// <param name="onParseFailed"></param>
    public AppSetting(string appSettingKey, AppSettingMissingHandler onMissing = null, AppSettingParseFailedHandler onParseFailed = null, TrimSetting trim = TrimSetting.EnableTrim)
      : base(appSettingKey, x => x, default(T), true, false, onMissing, onParseFailed, trim)
    {
    }


    /// <summary>
    /// Construct optional application setting (with default value)
    /// </summary>
    /// <param name="appSettingKey"></param>
    /// <param name="defaultValue"></param>
    /// <param name="emptyIsDefault"></param>
    /// <param name="onParseFailed"></param>
    public AppSetting(string appSettingKey, T defaultValue, bool emptyIsDefault = false, AppSettingParseFailedHandler onParseFailed = null, TrimSetting trim = TrimSetting.EnableTrim)
      : base(appSettingKey, x => x, defaultValue, false, emptyIsDefault, null, onParseFailed, trim)
    {
    }
  }
}
