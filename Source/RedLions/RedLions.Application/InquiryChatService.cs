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
        private IUnitOfWork unitOfWork;
        private IInquiryChatRepository inquiryChatRepository;
        private IMemberRepository memberRepository;

        public InquiryChatService(
            IUnitOfWork unitOfWork,
            IInquiryChatRepository inquiryChatRepository,
            IMemberRepository memberRepository)
        {
            this.inquiryChatRepository = inquiryChatRepository;
            this.memberRepository = memberRepository;
            this.unitOfWork = unitOfWork;
        }

        public DTO.InquiryChatSession GetSessionByID(int id)
        {
            Business.InquiryChatSession chatSession = this.inquiryChatRepository.GetSessionByID(id);

            DTO.InquiryChatSession chatSessionDto = Mapper.Map<DTO.InquiryChatSession>(chatSession);

            return chatSessionDto;
        }

        public IEnumerable<DTO.InquiryChatSession> GetSessionsByMember(int memberUserID)
        {
            Business.Member member = this.memberRepository.GetMemberByID(memberUserID);
            IEnumerable<Business.InquiryChatSession> chatSessions = this.inquiryChatRepository
                .GetSessionsByMember(member)
                .OrderByDescending(x => x.LastMessageDateTime);

            return Mapper.Map<IEnumerable<DTO.InquiryChatSession>>(chatSessions);
        }

        public DTO.InquiryChatSession CreateSession(string inquirerName, int memberUserID)
        {
            var member = this.memberRepository.GetMemberByID(memberUserID);
            var chatSession = new Business.InquiryChatSession(member, inquirerName);

            // Add server message.
            var chatMessage = new Business.InquiryChatMessage(
                "[Server]",
                string.Format("* {0} has initiated a chat *", inquirerName));

            chatSession.AddMessage(chatMessage);
            
            this.inquiryChatRepository.CreateSession(chatSession);
            this.unitOfWork.Commit();

            return Mapper.Map<DTO.InquiryChatSession>(chatSession);
        }

        public IEnumerable<DTO.InquiryChatMessage> GetChatMessagesBySession(int chatSessionID)
        {
            Business.InquiryChatSession chatSession = this.inquiryChatRepository
                .GetSessionByID(chatSessionID);

            IEnumerable<DTO.InquiryChatMessage> chatMessageDtoList = this.MapToDto(chatSession.ChatMessages);

            return chatMessageDtoList;
        }

        public void SaveMessage(DTO.InquiryChatMessage chatMessageDTO)
        {
            Business.InquiryChatSession chatSession = this.inquiryChatRepository
                .GetSessionByID(chatMessageDTO.InquiryChatSessionID);

            var chatMessage = new Business.InquiryChatMessage(
                chatMessageDTO.SenderUsername, 
                chatMessageDTO.Message);

            chatSession.AddMessage(chatMessage);

            this.unitOfWork.Commit();
        }

        #region Private Helpers
        private IEnumerable<DTO.InquiryChatMessage> MapToDto(IEnumerable<Business.InquiryChatMessage> chatMessages)
        {
            return Mapper.Map<IEnumerable<DTO.InquiryChatMessage>>(chatMessages);
        }
        #endregion  
    }
}
