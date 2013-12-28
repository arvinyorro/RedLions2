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
        public DateTime InquiredDateTime { get; set; }
    }

    internal static class InquiryAssembler
    {
        internal static DTO.Inquiry ToDTO(this Business.Inquiry inquiry)
        {
            var inquiryDTO = new DTO.Inquiry()
            {
                ID = inquiry.ID,
                FirstName = inquiry.FirstName,
                LastName = inquiry.LastName,
                CellphoneNumber = inquiry.CellphoneNumber,
                Email = inquiry.Email,
                ReferrerID = inquiry.Referrer.ID,
                ReferrerUsername = inquiry.Referrer.Username,
                InquiredDateTime = inquiry.InquiredDataTime
            };

            return inquiryDTO;
        }

        internal static IEnumerable<DTO.Inquiry> ToDTOList(IEnumerable<Business.Inquiry> inquiries)
        {
            return inquiries.Select(x => x.ToDTO());
        }
    }
}
