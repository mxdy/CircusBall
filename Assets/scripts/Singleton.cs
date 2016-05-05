using UnityEngine;
using System.Collections;
using System;

public class Singleton<T> :IDisposable where T : class, new()
{
	private static T _instance;
	private static readonly object syslock = new object();

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (syslock)
				{
					if (_instance == null)
					{
						_instance = new T();
					}
				}
			}
			return _instance;
		}
	}

	public virtual void Dispose()
	{
	}
}

// 所有需要单例化的类的父类
// mxdy
// 16/5/4
