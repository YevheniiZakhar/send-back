﻿namespace Land.Models
{
    public class GetAdRequest
    {
        public int? Page { get; set; } = 1;

        public int? PageSize { get; set; } = 16;

        public string? SearchInput { get; set; }

        public int SearchLocality { get; set; }
    }
}
