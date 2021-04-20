using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SharedZone.Web.Models;
using SharedZone.Web.Filters;
using SharedZone.DTO;
using System.Web.UI.WebControls.WebParts;
using SharedZone.IWebService;
using Bimacad.Sys;

namespace SharedZone.Web.Controllers
{
	[Licensing]
    public class CollectionController : SharedZoneController
    {
		public CollectionController(IWebSrc service) : base(service) { }

		[HttpGet]
		[ActiveItem(MenuItem.Collections)]
		//[Licensing]
		public async Task<ActionResult> Collections()
		{
			try
			{
				Application.UI.ViewType vtype = Application.UI.ViewType.Table; ;
				if (Session["ViewType"] != null)
					vtype = (Application.UI.ViewType)Session["ViewType"];

				return View(new CollectionsViewModel(await src.GetAllCollectionsAsync()) {ViewType = vtype });
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		[HttpGet]
		public async Task<PartialViewResult> CollectionReport(CollectionsViewModel model)
		{
			Session["ViewType"] = model.ViewType;
			if (model.ViewType == Application.UI.ViewType.Calendar)
				return await CollectionCalendar(model);
			else if (model.ViewType == Application.UI.ViewType.Chart)
				return await CollectionChart(model);
			else 
				return await CollectionTable(model);
		}

		[HttpGet]
		public async Task<PartialViewResult> CollectionTable(CollectionsViewModel model)
		{
			return PartialView("_CollectionTable", await src.GetAllCollectionsAsync(model.Search));
		}

		[HttpGet]
		public async Task<PartialViewResult> CollectionChart(CollectionsViewModel model)
		{
			return PartialView("_CollectionChart", new ChartViewModel()
			{
				WeekDays = (await src.GetAllWeekDaysAsync()).Select(x => new WeekDayViewModel(x.Id)),

			});
		}

		[HttpGet]
		public async Task<PartialViewResult> CollectionCalendar(CollectionsViewModel model)
		{
			return PartialView("_CollectionCalendar", new CollectionCalendarViewModel()
			{
				 WeekDays = (await src.GetAllWeekDaysAsync()).Select(x => new WeekDayViewModel(x.Id)), 
				 Hours = (await src.GetAllHoursAsync()).Select(x => (HourViewModel)x), 
				 Collections = await  src.GetAllCollectionsAsync(model.Search), 
				 Minutes = (await src.GetAllMinutesAsync()).Select(x => (MinuteViewModel)x)
			});
		}


		#region CRUD Collection


		[HttpGet]
		public PartialViewResult CreateCollection()
		{
			return PartialView("_CollectionCreate", new EditCollectionViewModel());
		}

		[HttpGet]
		public async Task<PartialViewResult> EditCollection(int Id)
		{
			return PartialView("_CollectionEdit", new CollectionViewModel(await src.GetCollectionAsync(Id)));
		}


		[HttpPost]
		public async Task<PartialViewResult> CreateCollection(EditCollectionViewModel item)
		{
			if (ModelState.IsValid)
			{
				int Id = await src.CreateCollectionAsync(new CollectionDTO() { Name = item.Name, Description = item.Description });
				//ViewBag[""] = "U";
				return await EditCollection(Id);
			}
			else
			{
				return PartialView("_CollectionCreate", item);
			}
		}


		[HttpPost]
		public async Task<string> RemoveCollection(int Id)
		{
			try
			{
				await src.RemoveCollectionAsync(Id);
				return Resources.Global.SuccessRemoveMsg;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		#endregion CRUD Collection

		#region CONTROLS

		public PartialViewResult WeekDays(IEnumerable<int> selected)
		{
			return PartialView("_WeekDays",
				src.GetAllWeekDays().Select(x => new CheckBoxViewModel(new WeekDayViewModel(x))
				{
					Checked = selected?.Contains(x.Id) ?? false,
					Name = "WeekDays",
					Value = x.Id,
					Id = $"WeekDays{x.Id}"
				}));
		}

		public PartialViewResult RevitVersions(int? selected) =>
			DropDown(new RevitVersionsDropDownViewModel(GetNamedUnitViewModels(src.GetAllRevitVersions), selected));

		public PartialViewResult Clients(int? selected) =>
			DropDown(new ClientsDropDownViewModel(src.GetAllClients().Select(x => (NamedUnitViewModel)x), selected));

		public PartialViewResult Hours(int? selected) =>
			DropDown(new HoursDropDownViewModel(src.GetAllHours().Select(x =>(HourViewModel)x), selected));

		public PartialViewResult Minutes(int? selected) =>
			DropDown(new MinutesDropDownViewModel(GetNamedUnitViewModels(src.GetAllMinutes), selected));

		public PartialViewResult NavisConvertElementsProperties(int? selected) =>
			DropDown(new NavisConvertElementsPropertiesViewModel(GetNamedUnitViewModels(src.GetAllNavisConvertElementsProperties), selected));

		public PartialViewResult NavisCoordinates(int? selected) =>
			DropDown(new NavisCoordinatesViewModel(GetNamedUnitViewModels(src.GetAllNavisCoordinates), selected));

		public PartialViewResult NavisViews(int? selected) =>
			DropDown(new NavisViewsViewModel(GetNamedUnitViewModels(src.GetAllNavisViews), selected));

		public PartialViewResult IFCIncludeBoundaties(int? selected) =>
			DropDown(new IFCIncludeBoundatiesViewModel(GetNamedUnitViewModels(src.GetAllIFCIncludeBoundaties), selected));

		public PartialViewResult IFCFileTypes(int? selected) =>
			DropDown(new IFCFileTypesViewModel(GetNamedUnitViewModels(src.GetAllIFCFileTypes), selected));


		private IEnumerable<NamedUnitViewModel> GetNamedUnitViewModels(Func<IEnumerable<NamedUnitDTO>> func) =>
			func.Invoke().Select(x => (NamedUnitViewModel)x);

		[NonAction]
		public PartialViewResult DropDown(DropDownsViewModel model) =>
			PartialView("_DropDown", model);

		#endregion CONTROLS

		#region Setting Files
		[HttpGet]
		public async Task<PartialViewResult> CollectionFiles(int Id)
		{
			return PartialView("_SettingFiles",
				new CollectionFilesViewModel(Id, await src.GetFilesAsync(Id)));
		}


		[HttpGet]
		public async Task<PartialViewResult> EditFiles(int Id)
		{
			return PartialView("_EditFiles",
				new CollectionFilesViewModel(Id, await src.GetAllServersFullDataAsync(Id)));
		}

		[HttpPost]
		public async Task<PartialViewResult> EditFiles(int Id, IEnumerable<int> files)
		{
			await src.UpdateModelsAsync(Id, files);
			return PartialView("_SettingFiles",
				new CollectionFilesViewModel(Id, await src.GetFilesAsync(Id)));
		}

		[HttpPost]
		public async Task<PartialViewResult> ClearModels(int Id)
		{
			await src.ClearModels(Id);
			return PartialView("_SettingFiles",
				new CollectionFilesViewModel(Id, await src.GetFilesAsync(Id)));
		}

		#endregion Setting Files

		#region Setting Params

		[HttpGet]
		public async Task<PartialViewResult> EditParams(int Id)
		{
			return PartialView("_EditParams",
				new EditCollectionViewModel(await src.GetCollectionAsync(Id)));
		}

		[HttpPost]
		public async Task<PartialViewResult> EditParams(EditCollectionViewModel item)
		{
			if (ModelState.IsValid)
			{
				CollectionDTO dto = Mapper.Map(new CollectionDTO(), item);
				dto.Worksets = item.Worksets == 1;
				await src.UpdateCollectionAsync(dto);
				return PartialView("_SettingParams",
					new ParamsCollectionViewModel(await src.GetCollectionAsync(item.Id)));
			}
			else
			{
				return PartialView("_EditParams", item);
			}
		}


		#endregion Setting Params

		#region Jobs
		private string EditJobViewName { get { return "_EditJob"; } }

		[HttpGet]
		public async Task<PartialViewResult> SettingParams(int Id)
		{
			return PartialView("_SettingParams",
					new ParamsCollectionViewModel(await src.GetCollectionAsync(Id)));
		}

		[HttpGet]
		public async Task<PartialViewResult> SettingJobs(int Id)
		{
			return PartialView("_SettingJobs", await GetCollectionJobs(Id));
		}

		[HttpGet]
		public PartialViewResult CreateRevitJob(int collectionId)
		{
			return PartialView(EditJobViewName, new RevitJobViewModel()
			{
				CollectionId = collectionId
			});
		}

		[HttpGet]
		public PartialViewResult CreateNavisJob(int collectionId)
		{
			return PartialView(EditJobViewName, new NavisJobViewModel()
			{
				CollectionId = collectionId,
				ConvertIdentif = true,
				TransformConstructionElements = true,
				TransformElementsProperty = true,
				TransformRoom = true,
				ExportGeometryRoom = true,
				TransformURL = true, 
				NavisViewId = 1
			});
		}

		[HttpGet]
		public PartialViewResult CreateIFCJob(int collectionId)
		{
			return PartialView(EditJobViewName, new IFCJobViewModel()
			{
				CollectionId = collectionId
			});
		}

		[HttpGet]
		public async Task<PartialViewResult> EditJob(int Id, string typeName)
		{
			if (typeName == nameof(RevitJobViewModel))
				return await EditRevitJob(Id);
			else if (typeName == nameof(NavisJobViewModel))
				return await EditNavisJob(Id);
			else if (typeName == nameof(IFCJobViewModel))
				return await EditIFCJob(Id);
			throw new Exception($"{Resources.Global.UnknownTypeMsg}: {typeName}");
		}

		[HttpPost]
		public async Task<PartialViewResult> DeleteJob(int Id, int collectionId, string typeName)
		{
			if (typeName == nameof(RevitJobViewModel))
				await DeleteRevitJob(Id);
			else if (typeName == nameof(NavisJobViewModel))
				await DeleteNavisJob(Id);
			else if (typeName == nameof(IFCJobViewModel))
				await DeleteIFCJob(Id);
			else 
				throw new Exception($"{Resources.Global.UnknownTypeMsg}: {typeName}");
			return PartialView("_SettingJobs", new JobsViewModel(await src.GetCollectionAsync(collectionId)));
		}

		[HttpPost]
		public async Task DeleteRevitJob(int Id)
		{
			await src.RemoveRevitJobAsync(Id);
		}

		[HttpPost]
		public async Task DeleteNavisJob(int Id)
		{
			await src.RemoveNavisJobAsync(Id);
		}

		[HttpPost]
		public async Task DeleteIFCJob(int Id)
		{
			await src.RemoveIFCJobAsync(Id);
		}


		[HttpGet]
		public async Task<PartialViewResult> EditRevitJob(int Id)
		{
			return PartialView(EditJobViewName,
				new RevitJobViewModel(await src.GetRevitJob(Id)));
		}

		[HttpGet]
		public async Task<PartialViewResult> EditNavisJob(int Id)
		{
			return PartialView(EditJobViewName,
				new NavisJobViewModel(await src.GetNavisJob(Id)));
		}

		[HttpGet]
		public async Task<PartialViewResult> EditIFCJob(int Id)
		{
			return PartialView(EditJobViewName,
				new IFCJobViewModel(await src.GetIFCJob(Id)));
		}

		[HttpPost]
		public async Task<PartialViewResult> EditRevitJob(RevitJobViewModel model)
		{
			if (ModelState.IsValid)
			{
				await src.MergeRevitJobAsync(Mapper.Map(new RevitJobDTO(), model));
				return PartialView("_SettingJobs", await GetCollectionJobs(model.CollectionId));
			}
			return PartialView(EditJobViewName, model);
		}

		[HttpPost]
		public async Task<PartialViewResult> EditNavisJob(NavisJobViewModel model)
		{
			if (ModelState.IsValid)
			{
				await src.MergeNavisJobAsync(Mapper.Map(new NavisJobDTO(), model));
				return PartialView("_SettingJobs", await GetCollectionJobs(model.CollectionId));
			}
			return PartialView(EditJobViewName, model);
		}

		[HttpPost]
		public async Task<PartialViewResult> EditIFCJob(IFCJobViewModel model)
		{
			if (ModelState.IsValid)
			{
				await src.MergeIFCJobAsync(Mapper.Map(new IFCJobDTO(), model));
				return PartialView("_SettingJobs", await GetCollectionJobs(model.CollectionId));
			}
			return PartialView(EditJobViewName, model);
		}


		[NonAction]
		private async Task<JobsViewModel> GetCollectionJobs(int collectionId) =>
			new JobsViewModel(await src.GetCollectionAsync(collectionId));


		#endregion Jobs
	}
}