﻿using e_crime.Data;
using System.ComponentModel.DataAnnotations;

namespace e_crime.ViewModels.PoliceStation
{
    public class PoliceStationVM
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        [Display(Name = "Station Name")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Location cannot be empty")]
        public string Location { get; set; } = string.Empty;
        [Required(ErrorMessage = "County cannot be null")]
        public string County { get; set; } = string.Empty;
        [Required(ErrorMessage = "InCharge Email cannot be empty")]
        [Display(Name = "Station InCharge")]
        public string InchargeEmail { get; set; } = string.Empty;
    }
}