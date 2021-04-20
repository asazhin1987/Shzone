namespace SharedZone.Web.Models
{
	public class MenuViewModel
	{
		public string EditActionName { get; set; }
		public string RemoveActionName { get; set; }
		public string CloneActionName { get; set; }
		public string ControllerName { get; set; }
		public string RemoveConfirmMessage { get; set; } = Resources.Global.RemoveConfirmMsg;
		public int Id { get; set; }

		public MenuViewModel(int Id)
		{
			this.Id = Id;
		}
	}

	public class ServerMenuViewModel : MenuViewModel
	{
		public ServerMenuViewModel(int Id) : base(Id)
		{
			ControllerName = "Server";
			EditActionName = "EditServer";
			CloneActionName = "CloneServer";
			RemoveActionName = "RemoveServer";
		}
	}

	public class CollectionMenuViewModel : MenuViewModel
	{
		public CollectionMenuViewModel(int Id) : base(Id)
		{
			ControllerName = "Collection";
			EditActionName = "EditCollection";
			CloneActionName = "CloneCollection";
			RemoveActionName = "RemoveCollection";
		}
	}
}