namespace RedLions.Application.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RedLions.Business;

    public class Inquiry
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CellphoneNumber { get; set; }
        public string Email { get; set; }
        public int? ReferrerID { get; set; }
        public string ReferrerUsername { get; set; }
    }

    internal static class InquiryAssembler
    {
        internal static DTO.Inquiry ToDTO(Business.Inquiry inquiry)
        {
            var inquiryDTO = new DTO.Inquiry()
            {
                FirstName = inquiry.FirstName,
                LastName = inquiry.LastName,
                CellphoneNumber = inquiry.CellphoneNumber,
                Email = inquiry.Email,
                ReferrerID = inquiry.Referrer.ID,
                ReferrerUsername = inquiry.Referrer.Username
            };

            return inquiryDTO;
        }

        internal static IEnumerable<DTO.Inquiry> ToDTOList(IEnumerable<Business.Inquiry> inquiries)
        {
            return inquiries.Select(x => InquiryAssembler.ToDTO(x));
        }
    }
}
