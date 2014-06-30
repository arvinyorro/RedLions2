namespace RedLions.Presentation.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DTO = RedLions.Application.DTO;
    using RedLions.Application;
    using PagedList;
    using AutoMapper;

    [Authorize(Roles = "Admin")]
    public class AnnouncementsController : Controller
    {
        private AnnouncementService announcementService;
        private UserService userService;

        public AnnouncementsController(
            AnnouncementService announcementService,
            UserService userService)
        {
            if (announcementService == null)
            {
                throw new ArgumentNullException("announcementService");
            }

            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            this.announcementService = announcementService;
            this.userService = userService;
        }

        //
        // GET: /Admin/Announcement/
        public ViewResult Index(int? page)
        {
            int currentPage = (page ?? 1);

            // Fix negative page
            currentPage = currentPage < 0 ? 1 : currentPage;

            int pageSize = 20;
            int totalItems = 0;

            IEnumerable<DTO.Announcement> announcementDtoList = this.announcementService
                .GetPagedAnnouncements(
                    currentPage, 
                    out totalItems, 
                    pageSize);

            IEnumerable<Models.Announcement> announcementModels = Mapper.Map<IEnumerable<Models.Announcement>>(announcementDtoList);

            var pagedList = new StaticPagedList<Models.Announcement>(announcementModels, currentPage, pageSize, totalItems);

            return View(pagedList);
        }

        //
        // GET: /Admin/Announcement/Create
        public ViewResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Announcement/Create
        [HttpPost]
        public ActionResult Create(Models.Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return View(announcement);
            }

            DTO.User userDTO = this.userService.GetUserByUsername(HttpContext.User.Identity.Name);

            var announcementDTO = new DTO.Announcement()
            {
                Title = announcement.Title,
                Message = announcement.Message,
                Poster = userDTO,
            };

            this.announcementService.PostAnnouncement(announcementDTO);

            return RedirectToAction("Index");
        }

        //
        // GET: /Admin/Announcement/Details/{id}
        public ViewResult Details(int id)
        {
            DTO.Announcement announcementDTO = this.announcementService.GetAnnouncementByID(id);
            Models.Announcement announcement = Mapper.Map<Models.Announcement>(announcementDTO);

            return View(announcement);
        }

        //
        // GET: /Admin/Announcement/Edit/{id}
        public ViewResult Edit(int id)
        {
            DTO.Announcement announcementDTO = this.announcementService.GetAnnouncementByID(id);
            ViewModels.EditAnnouncement editAnnouncement = Mapper.Map<ViewModels.EditAnnouncement>(announcementDTO);

            return View(editAnnouncement);
        }

        //
        // POST: /Admin/Announcement/Edit
        [HttpPost]
        public ActionResult Edit(ViewModels.EditAnnouncement editAnnouncement)
        {
            if (!ModelState.IsValid)
            {
                return View(editAnnouncement);
            }

            DTO.Announcement announcementDTO = this.announcementService.GetAnnouncementByID(editAnnouncement.ID);

            announcementDTO.Title = editAnnouncement.Title;
            announcementDTO.Message = editAnnouncement.Message;

            this.announcementService.UpdateAnnouncement(announcementDTO);

            return RedirectToAction("Details", new { id = editAnnouncement.ID });
        }

        //
        // GET: /Admin/Announcement/Delete/{id}
        public ViewResult Delete(int id)
        {
            DTO.Announcement announcementDTO = this.announcementService.GetAnnouncementByID(id);
            Models.Announcement announcement = Mapper.Map<Models.Announcement>(announcementDTO);
            return View(announcement);
        }

        //
        // POST: /Admin/Announcement/Delete
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            this.announcementService.DeleteAnnouncementByID(id);
            return RedirectToAction("Index");
        }
	}
}