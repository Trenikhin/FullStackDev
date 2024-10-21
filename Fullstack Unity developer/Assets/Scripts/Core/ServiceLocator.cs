namespace ShootEmUp
{
	using System;
	using System.Collections.Generic;
	
	public class ServiceLocator
	{
		static Dictionary<Type, object> services = new Dictionary<Type, object>();
		static ServiceLocator _instance;
		
		public static ServiceLocator Instance => _instance ??= new ServiceLocator();
        
		public void Register<T>(T service)
		{
			services[typeof(T)] = service;
		}

		public T Get<T>()
		{
			if (services.TryGetValue(typeof(T), out var service))
			{
				return (T)service;
			}
			throw new KeyNotFoundException($"Service of type {typeof(T)} not found.");
		}

		public void Unregister<T>()
		{
			services.Remove(typeof(T));
		}
	}
}