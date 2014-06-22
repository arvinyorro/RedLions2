namespace RedLions.Application
{
    using System;
    using System.Collections.Generic;
    using RedLions.Application.DTO;
    using RedLions.Business;
    using RedLions.CrossCutting;
    using AutoMapper;

    public class SubscriptionService
    {
        private IUnitOfWork unitOfWork;
        private ISubscriptionRepository subscriptionRepository;
        private IMemberRepository memberRepository;

        public SubscriptionService(
            IUnitOfWork unitOfWork,
            ISubscriptionRepository subscriptionRepository,
            IMemberRepository memberRepository)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (subscriptionRepository == null)
            {
                throw new ArgumentNullException("subscriptionRepository");
            }

            if (memberRepository == null)
            {
                throw new ArgumentNullException("memberRepository");
            }

            this.unitOfWork = unitOfWork;
            this.subscriptionRepository = subscriptionRepository;
            this.memberRepository = memberRepository;
        }

        public IEnumerable<DTO.Subscription> GetSubscriptions()
        {
            IEnumerable<Business.Subscription> subscriptions = this.subscriptionRepository.GetAll();

            return Mapper.Map<IEnumerable<DTO.Subscription>>(subscriptions);
        }

        public void ExtendSubscription(int userID, int subscriptionID)
        {
            Business.Member member = this.GetMemberByID(userID);

            Business.Subscription subscription = this.subscriptionRepository
                .GetByID(subscriptionID);

            if (subscription == null)
            {
                throw new Exception("Unable to extend subscription, subscription not found.");
            }

            member.ExtendSubscription(subscription);
            this.unitOfWork.Commit();
        }

        public bool HasSubscriptionExpired(int userID)
        {
            Business.Member member = this.GetMemberByID(userID);

            return member.SubscriptionExpired;
        }

        private Business.Member GetMemberByID(int userID)
        {
            Business.Member member = this.memberRepository.GetMemberByID(userID);

            if (member == null)
            {
                throw new Exception("Unable to extend subscription, member not found.");
            }

            return member;
        }
    }
}
