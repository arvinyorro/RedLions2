﻿namespace RedLions.Presentation.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;


    public class Membership
    {
        public int LocationID { get; set; }

        [Display(Name = "Location")]
        public ICollection<SelectListItem> LocationSelectListItems { get; set; }
    }
}