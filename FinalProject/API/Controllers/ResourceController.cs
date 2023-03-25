using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ResourceController : ControllerBase
	{
		private Prn231dbContext db = new Prn231dbContext();

		[HttpPost]
		[Route("{classId}")]
		public IActionResult AddResource(int classId, [FromForm]AddResourceModel data)
		{
			var clss = db.Classes.SingleOrDefault(c => c.Id == classId);
			if (clss == null)
				return NotFound();

			Resource resource = new Resource()
			{
				UploadDate = DateTime.Now,
				Path = "",
				Name = data.Name,
				ContentType = data.File.ContentType,
				ClassId = classId
			};
			db.Resources.Add(resource);
			db.SaveChanges();

			string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string oldName = data.File.FileName;
			int insertPosi = oldName.LastIndexOf(".");
			if (insertPosi == -1) insertPosi = oldName.Length;
			string fileName = oldName.Insert(insertPosi, "_" + resource.Id);
			string fileNameWithPath = Path.Combine(path, fileName);
			using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
			{
				data.File.CopyTo(stream);
			}

			resource.Path = fileName;
			db.SaveChanges();

			return Ok();
		}
	}
}
