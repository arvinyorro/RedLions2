namespace RedLions.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using RedLions.CrossCutting;
    using RedLions.Application.DTO;
    using RedLions.Business;
    using AutoMapper;

    public class InquiryChatService
    {
        private IInquiryChatRepository inquiryChatRepository;

        public InquiryChatService(IInquiryChatRepository inquiryChatRepository)
        {
            this.inquiryChatRepository = inquiryChatRepository;
        }

        public DTO.InquiryChatSession GetSessionByID(int id)
        {
            Business.InquiryChatSession chatSession = this.inquiryChatRepository.GetSessionByID(id);

            DTO.InquiryChatSession chatSessionDto = Mapper.Map<DTO.InquiryChatSession>(chatSession);

            return chatSessionDto;
        }

        // Search CHECKPOINT1
    }
}
