using System;
using System.Linq;
using System.Reflection;

namespace Bimacad.Sys
{
	public static class Mapper
	{
		public static T Map<T>(this T toItem, object onItem)
		{
			Type t = onItem.GetType();
			foreach (var field in toItem.GetType().GetProperties().Where(x => x.CanWrite == true).AsParallel())
			{
				try
				{
					PropertyInfo info = t.GetProperty(field.Name);
					if (info != null && info.GetCustomAttribute(typeof(NotMapingAttribute)) == null)
						field.SetValue(toItem, info.GetValue(onItem));
				}
				catch { }
			}
			return toItem;
		}
	}
}
