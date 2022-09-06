﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamKing.Data.Media;

namespace StreamKing.MainApplication.Models
{
    public class DataTemplateModel
    {
        public string? Title;
        public string? Provider;
        public string? AmountSeasons;
        public string? Tag;
        public float? Rating;
        public Media Media;

        public DataTemplateModel()
        {

        }
    }
}
