//using System;
//using System.Collections.Generic;
//using SharedZone.DTO;
//using Bimacad.Sys;

//namespace SharedZone.Client.Model
//{
//	public class CollectionViewModel
//	{
//		public int Id { get; set; }

//		public string Name { get; set; }

//		public int ModelsCount { get; set; }

//		public DateTime? LastDateTime { get; set; }

//		public DateTime? NextDateTime { get; set; }

//		public int ClientId { get; set; }

//		public int HourId { get; set; }

//		public int MinuteId { get; set; }

//		public CollectionViewModel(CollectionDTO item)
//		{
//			this.Map(item);
//		}

//		public static implicit operator CollectionViewModel(CollectionDTO item)
//		{
//			return new CollectionViewModel(item);
//		}
//	}
//}
