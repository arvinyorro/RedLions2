namespace RedLions.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using RedLions.Application.DTO;
    using RedLions.Business;
    using RedLions.CrossCutting;
    using AutoMapper;

    public class AnnouncementService
    {
        private IUnitOfWork unitOfWork;
        private IAnnouncementRepository announcementRepository;
        private IUserRepository userRepository;

        public AnnouncementService(
            IUnitOfWork unitOfWork,
            IAnnouncementRepository announcementRepository,
            IUserRepository userRepository)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (announcementRepository == null)
            {
                throw new ArgumentNullException("announcementRepository");
            }

            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }

            this.unitOfWork = unitOfWork;
            this.announcementRepository = announcementRepository;
            this.userRepository = userRepository;
        }

        public DTO.Announcement GetAnnouncementByID(int id)
        {
            Business.Announcement announcement = this.announcementRepository.GetByID(id);
            return Mapper.Map<DTO.Announcement>(announcement);
        }

        public IEnumerable<DTO.Announcement> GetPagedAnnouncements(
            int pageIndex,
            out int totalCount,
            int pageSize)
        {
            IEnumerable<Business.Announcement> announcements = this.announcementRepository
                .GetPagedListOrderedByRecent(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    totalCount: out totalCount);

            return Mapper.Map<IEnumerable<DTO.Announcement>>(announcements);
        }

        public void PostAnnouncement(DTO.Announcement announcementDto)
        {
            var poster = this.userRepository.GetUserByID(announcementDto.Poster.ID);
            var announcement = new Business.Announcement(announcementDto.Title, announcementDto.Message, poster);

            this.announcementRepository.Create(announcement);
            this.unitOfWork.Commit();
        }

        public void UpdateAnnouncement(DTO.Announcement announcementDto)
        {
            Business.Announcement announcement = this.announcementRepository.GetByID(announcementDto.ID);

            announcement.Title = announcementDto.Title;
            announcement.Message = announcementDto.Message;

            this.announcementRepository.Update(announcement);
            this.unitOfWork.Commit();
        }

        public void DeleteAnnouncementByID(int id)
        {
            Business.Announcement announcement = this.announcementRepository.GetByID(id);

            this.announcementRepository.Delete(announcement);
            this.unitOfWork.Commit();
        }
    }
}
