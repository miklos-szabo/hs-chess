﻿namespace HSC.Common.RequestContext
{
    public interface IRequestContext
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
